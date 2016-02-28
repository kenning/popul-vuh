using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class ClickControl : MonoBehaviour {
	//Game control scripts
	ButtonAnimate playButton;
	DisplayCardCanvas displayCardCanvas;
    Player player;

	//units
	List<GridUnit> UnitList;
	GameObject playerObject;

	//dragging variables
	Vector3 dragOrigin;
	public bool draggingGameboard = false;
	public bool draggingHand = false;
	bool draggingDiscard = false;
	GameObject arrows;

	//gameboard limits
	float leftLimit = -1.6f;
	float rightLimit = 1.6f;
	float topLimit = -.6f;
	float bottomLimit = -2f;

	//gridcursorcontrol variables
	public float lastCursorSetTime = 0;

	//card clicking and display variables
	public bool cardHasBeenClickedOn = false;
	public Card cardScriptClickedOn;
	float lastCardClick = 0f;

	//allow bools
	public bool AllowInputUmbrella = false;
	public bool AllowInfoInput = true;
	public bool AllowForfeitButtonInput = true;
	public bool AllowNewPlayInput = true;
	public bool AllowMoveInput = true; 			// punch, play cards
	public bool AllowCardTargetInput = false;
	public bool AllowSquareTargetInput = true;

	// Info will be displayed after cursor is set 
		// These bools track what info will be displayed so that after the 'none' cursor is set,
		// the proper call of ShowInfo will be sent to GridCursorControl.
	bool willShowPlayerInfo = false;
	bool willShowEnemyInfo = false;
	bool willShowObstacleInfo = false;
	GameObject infoTarget;
	
	//i'm impatient and want to end the damn turn already bool
	public bool turnEndedAlready = false;

	// This gets set when the shop menu is up so that the behavior for undisplaying a card
	// is different from normal. Pretty ugly i know
	public bool undisplayCardOnClick = false;

	Texture2D SideArrows;
    EventSystem eventSystem = GameObject.FindGameObjectWithTag("eventsystem").GetComponent<EventSystem>();
	
	void Start(){
		playerObject = GameObject.FindGameObjectWithTag ("Player");
        player = playerObject.GetComponent<Player>();

		playButton = GameObject.Find ("play end button").GetComponent<ButtonAnimate> ();

		arrows =  GameObject.FindGameObjectWithTag("Camera Arrow");
		SideArrows = (Texture2D)Resources.Load ("sprites/ui/side arrows");

		displayCardCanvas = GameObject.FindGameObjectWithTag("displaycard").GetComponent<DisplayCardCanvas>(); 
	}

	void Update () {

		// Resetting things
		
		if (Input.GetMouseButtonDown (0)) {
			S.ShopControlInst.GoalCheck("Touch the screen no more than than X times");
		}

		if (!Input.GetMouseButton (0) && GridCursorControl.ClickedOffScreen) {
			GridCursorControl.ClickedOffScreen = false;
		}

		if(S.MenuControlInst.AnyMenuIsUp() | S.ShopControlGUIInst.IgnoreClicking | GridCursorControl.ClickedOffScreen) {
			return;
		}

        if(cardScriptClickedOn != null) {
            if (!Input.GetMouseButton(0))
            {
				cardScriptClickedOn.cardUI.GlowAnimate(false);
            } 
        }

		if(!AllowInputUmbrella) {
			if(displayCardCanvas.CardDisplay) {
				S.GameControlGUIInst.Undisplay();
				cardHasBeenClickedOn = false;
			}
			if(draggingGameboard) {
				draggingGameboard = false;
				arrows.GetComponent<SpriteRenderer>().enabled = false;
			}
			if(draggingHand) {
				draggingHand = false;
				Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
			}
			return;
		}
		
		if(displayCardCanvas.CardDisplay && !Input.GetMouseButton(0)) {
			S.GameControlGUIInst.Undisplay();
			cardHasBeenClickedOn = false;
		}

		// This replaces the clickblocker system. Prevents clicking on stuff under canvas elements
		if (eventSystem.IsPointerOverGameObject()) {
			return;
		}

		if (GridCursorControl.cursorActionSet) {
			if(!Input.GetMouseButton(0)) {
				S.GridCursorControlInst.ReleaseCursor();
				return;
			} else if(Time.time > lastCursorSetTime + 1.0f) {
				GameBoardDrag();	
				S.GridCursorControlInst.UnpresentCursor();
				return;
			}
		}
		
		//////////////////////////////////////
		/// DRAGGING AND DISPLAY -- InfoInput
		//////////////////////////////////////

		if(draggingGameboard && AllowInfoInput){
			if(Input.GetMouseButton(0)){
				Vector3 pos = Camera.main.transform.position;
				Vector3 move = Camera.main.ScreenToViewportPoint (Input.mousePosition - dragOrigin);
				move = new Vector3(move.x * .5f, move.y * .5f, 0);
				//stops at the end of the screen
				if(pos.x < leftLimit && move.x < 0) {
					move.x = 0;
					pos.x = leftLimit -.05f;
				}
				if(pos.y < bottomLimit && move.y < 0) {
					move.y = 0;
					pos.y = bottomLimit-.05f;
				}
				if(pos.x > rightLimit && move.x > 0) {
					move.x = 0;
					pos.x = rightLimit + .05f;
				}
				if(pos.y > topLimit && move.y > 0) {
					move.y = 0;
					pos.y = topLimit + .05f;
				}

				Camera.main.transform.position = move + pos;
				return;
			}
			else {
				draggingGameboard = false;
				arrows.GetComponent<SpriteRenderer>().enabled = false;
			}
			return;
		}

		if(draggingHand && AllowInfoInput){
			if(Input.GetMouseButton(0)){
				if(S.GameControlInst.Hand.Count < 4) { 
					GameObject.Find("Hand").transform.localPosition = new Vector3(((3) * -1.48f) + 3.7f, 0, 0);
					return;
				}
				Vector3 pos = Camera.main.ScreenToViewportPoint 
					(Input.mousePosition - (cardScriptClickedOn.transform.position*10) - dragOrigin);
				Debug.Log(pos.x);

				if(GameObject.Find("Hand").transform.localPosition.x >= -.75f && pos.x > 0) {
					GameObject.Find("Hand").transform.localPosition = new Vector3(-.73f, 0, 0f);
					return;
				}
				else if((GameObject.Find("Hand").transform.localPosition.x <= ((S.GameControlInst.Hand.Count) * -1.55f) + 5.35f) &&  pos.x < 0) {
					//this is for after the exact position has gotten nailed down, purpose is to lock it to the edge. 
					//the key numbers are: 3.95 one line above and .75 six lines above.
					GameObject.Find("Hand").transform.localPosition = new Vector3(((S.GameControlInst.Hand.Count) * -1.55f) + 5.3f, 0, 0);
				} else {
					Vector3 move = new Vector3(pos.x, 0, 0);
					GameObject.Find("Hand").transform.Translate(move);  
					return;
				}
			}
			else {
				draggingHand = false;
				Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
			}
			return;
		}

		//////////////////////////////////////
		/// CARDCLICKS
		//////////////////////////////////////

		if(cardHasBeenClickedOn && cardScriptClickedOn != null && !draggingHand && !draggingDiscard){
			if(!Input.GetMouseButton(0)){
				if(S.GameControlInst.CardsToTarget != 0 
				   && AllowCardTargetInput 
				   && (cardScriptClickedOn.Peeked == S.GameControlInst.CardsToTargetArePeeked)
				   && (S.GameControlInst.CardsToTargetAreDiscarded == cardScriptClickedOn.Discarded)) {
					if(S.GameControlInst.TargetedCards.Contains(cardScriptClickedOn.gameObject))
					{
						cardScriptClickedOn.Untarget();
						S.GameControlInst.TargetedCards.Remove(cardScriptClickedOn.gameObject);
						cardHasBeenClickedOn = false;
					}
					else if(cardScriptClickedOn != S.GameControlInst.TargetCardCallback) 
					{
						cardScriptClickedOn.Target();
						S.GameControlInst.TargetedCards.Add(cardScriptClickedOn.gameObject);
						if(S.GameControlInst.TargetedCards.Count == S.GameControlInst.CardsToTarget) 
							S.GameControlInst.TargetCardCallback.AfterCardTargetingCallback();
						cardHasBeenClickedOn = false;
					}
				}
				else if (!cardScriptClickedOn.Discarded 
				         && S.GameControlInst.PlaysLeft > 0 
				         && AllowNewPlayInput 
				         && S.GameControlInst.CardsToTarget == 0) {
					cardScriptClickedOn.Click ();
					cardHasBeenClickedOn = false;
				}
				else {
					playButton.ErrorAnimation();
					cardHasBeenClickedOn = false;
				}
				cardHasBeenClickedOn = false;
			}
			else {

				if(Mathf.Abs(Input.mousePosition.x - dragOrigin.x) > .2f 
				   && !cardScriptClickedOn.Discarded && AllowInfoInput) {
					handDrag();
					return;
				}
				else if(Mathf.Abs(Input.mousePosition.y - dragOrigin.y) > .2f 
				        && cardScriptClickedOn.Discarded && AllowInfoInput) {
					draggingDiscard = true;
					cardHasBeenClickedOn = false;
				}
				else if(Time.time - 0.22f > lastCardClick && !displayCardCanvas.CardDisplay && AllowInfoInput) { 
					S.GameControlGUIInst.Display(cardScriptClickedOn);
					cardHasBeenClickedOn = false;
				}
				else if(Time.time - .03f > lastCardClick && AllowInfoInput) 
                {
                    cardScriptClickedOn.cardUI.ShineAnimate();
					S.GameControlGUIInst.DisplayDim();
                }
			}
		}

		//////////////////////////////////////
		/// NEW CLICKS
		//////////////////////////////////////

		if(Input.GetMouseButton(0)){
            
			S.GameControlGUIInst.ShowDeck(false);

			float dist = transform.position.z - Camera.main.transform.position.z;
			var pos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, dist);
			pos = Camera.main.ScreenToWorldPoint(pos);
			RaycastHit2D[] hits = Physics2D.RaycastAll (pos, Vector2.zero);

			if(Input.GetMouseButtonDown(0)) {
				if(hits != null ){
					foreach(RaycastHit2D hit in hits) {
						if(hit.collider.gameObject.name == "end turn button" && AllowForfeitButtonInput) {
							if(turnEndedAlready) 
								return;
							turnEndedAlready = true;
							S.GameControlInst.ButtonSpritesLookClicked();
							S.GameControlInst.DeselectCards();
							S.GridControlInst.DestroyAllTargetSquares();
	                        if (Tutorial.TutorialLevel != 0)
	                        {
								S.GameControlInst.StartNewTurn(false);
	                        }
	                        else
	                        {
	                            S.GameControlInst.EnemyTurn(true);
	                        }
							return;
						}
					}
					foreach(RaycastHit2D hit in hits) {
						if(hit.collider.gameObject.name == "play end button" && AllowForfeitButtonInput) { 
							if(S.GameControlInst.PlaysLeft == 1) {
								S.GameControlGUIInst.SetTooltip("You can punch or play a card one more time this turn.");
							} else {
								S.GameControlGUIInst.SetTooltip("You can punch or play a card " + 
								                          S.GameControlInst.PlaysLeft.ToString() + 
								                          " more times this turn.");
							}
							foreach(GameObject card in S.GameControlInst.Hand) {
								card.GetComponent<CardUI>().ShineAnimate();
							}
							return;
						}
					}
					foreach(RaycastHit2D hit in hits) {
						if(hit.collider.gameObject.name == "move end button" && AllowForfeitButtonInput) { 
							if(S.GameControlInst.MovesLeft == 1) {
								S.GameControlGUIInst.SetTooltip("You can move one more time this turn.");
							} else {
								S.GameControlGUIInst.SetTooltip("You can move " + 
								                          S.GameControlInst.MovesLeft.ToString() + 
								                          " more times this turn.");
							}
							return;
						}
					}
					foreach(RaycastHit2D hit in hits) {
						if(hit.collider.gameObject.tag == "click blocker" ) { 
							return;
						}
					}
					foreach(RaycastHit2D hit in hits) {
						if(hit.collider.gameObject.name == "Deck" && AllowInfoInput) { 
							S.GameControlGUIInst.ShowDeck(true);
							return;
						}
					}
					foreach(RaycastHit2D hit in hits){
						if(hit.collider.gameObject.tag == "Card") {
							clickOnCard(hit.collider.gameObject);
							return;
						}
					}

					foreach(RaycastHit2D hit in hits) 
						if(hit.collider.gameObject.tag == "Play board") 
							return;

					foreach(RaycastHit2D hit in hits) {
						//AllowInfoInput
						if(hit.collider.gameObject.name == "Discard pile marker" && AllowInfoInput) {
							S.GameControlGUIInst.FlipDiscard();
							return;
						}
					}
				}
			}

			if(cardHasBeenClickedOn) return;

			Vector3 clickPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

			if((clickPosition.x > GridControl.GridSize + .5f |
			    clickPosition.x < -GridControl.GridSize - .5f |
				clickPosition.y > GridControl.GridSize + .5f | 
			    clickPosition.y < -GridControl.GridSize - .5f)) {
				if(GridCursorControl.GridCursorIsActive) {
					GridCursorControl.ClickedOffScreen = true;
					S.GridCursorControlInst.UnpresentCursor();
				}
				return;
			}

			foreach(RaycastHit2D hit in hits){
				if(hit.collider.gameObject.tag == "Target square" && AllowSquareTargetInput){
					S.GridCursorControlInst.PresentCursor(GridCursorControl.CursorActions.TargetSquare);
					S.GridCursorControlInst.SetCurrentCursorTarget(hit.collider.gameObject);
					return;
				}
			}

			S.GameControlInst.DeselectCards();

			S.GridControlInst.DestroyAllTargetSquares();
			
			foreach(RaycastHit2D hit in hits){
				if(hit.collider.gameObject.tag == "Enemy"){
					if(AllowInfoInput) {
						willShowEnemyInfo = true;
						willShowPlayerInfo = false;
						willShowObstacleInfo = false;
						infoTarget = hit.collider.gameObject;
					}
					if(adjCheck(hit.collider.gameObject.transform.position) != "none" 
					   && S.GameControlInst.PlaysLeft > 0 && AllowNewPlayInput){
						S.GridCursorControlInst.PresentCursor(GridCursorControl.CursorActions.Punch);
						S.GridCursorControlInst.SetCurrentCursorTarget(hit.collider.gameObject);
						// Even though this should return and end the Update method, it still needs to
							// try to show a tooltip
						tryToShowInfo();
						return;
					}
				}
			}

			foreach(RaycastHit2D hit in hits){
				if(hit.collider.gameObject.tag == "Player" && AllowInfoInput){
					willShowEnemyInfo = false;
					willShowPlayerInfo = true;
					willShowObstacleInfo = false;
					infoTarget = hit.collider.gameObject;
				}
			}
            foreach (RaycastHit2D hit in hits)
            {
                if (hit.collider.gameObject.tag == "obstacle")
                {

					GridUnit obstacleGU = hit.collider.gameObject.GetComponent<GridUnit>();
					GridUnit playerGU = S.GameControlInst.playerObj.GetComponent<GridUnit>();
					
					
                    if (obstacleGU.IsAdjacent(playerGU))
                    {
                        Obstacle hitObstacle = hit.collider.gameObject.GetComponent<Obstacle>();
                        if (hitObstacle.Walkable)
                        {
							S.GridCursorControlInst.PresentCursor(GridCursorControl.CursorActions.Move);
							S.GridCursorControlInst.SetCurrentCursorTarget(hit.collider.gameObject);
						}
                        else {
							S.GridCursorControlInst.PresentCursor(GridCursorControl.CursorActions.Poke);
							S.GridCursorControlInst.SetCurrentCursorTarget(hit.collider.gameObject);
						}
                    	return;
					}

					willShowEnemyInfo = false;
					willShowPlayerInfo = false;
					willShowObstacleInfo = true;
					infoTarget = hit.collider.gameObject;
                }
            }
            foreach (RaycastHit2D hit in hits)
            {
				if(hit.collider.gameObject.name == "stairs" && AllowMoveInput) {
					if(adjCheck(hit.collider.gameObject.transform.position) != "none") {
						S.GridCursorControlInst.PresentCursor(GridCursorControl.CursorActions.StairMove);
					}
					return;
				}
			}
			foreach(RaycastHit2D hit in hits) {
				if(hit.collider.gameObject.tag == "Gameboard") {
					if(adjCheck(clickPosition) != "none" && AllowMoveInput) {
						S.GridCursorControlInst.PresentCursor(GridCursorControl.CursorActions.Move);
						S.GridCursorControlInst.SetMoveDirection(adjCheck(clickPosition));
					}
					else if (AllowInfoInput) {
						S.GridCursorControlInst.PresentCursor(GridCursorControl.CursorActions.None);
					}
				}
			}

			tryToShowInfo();
		}
	}

	void tryToShowInfo() {
		if((Time.time - .15f) > lastCursorSetTime) {
			if(willShowEnemyInfo) {
				S.GridCursorControlInst.ShowInfo(GridCursorControl.CursorInfoTypes.EnemyInfo, infoTarget);
				willShowEnemyInfo = false;
				willShowPlayerInfo = false;
				willShowObstacleInfo = false;
			} else if (willShowPlayerInfo) {
				S.GridCursorControlInst.ShowInfo(GridCursorControl.CursorInfoTypes.PlayerInfo, infoTarget);
				willShowEnemyInfo = false;
				willShowPlayerInfo = false;
				willShowObstacleInfo = false;
			} else if (willShowObstacleInfo) {
				S.GridCursorControlInst.ShowInfo(GridCursorControl.CursorInfoTypes.ObstacleInfo, infoTarget);
				willShowEnemyInfo = false;
				willShowPlayerInfo = false;
				willShowObstacleInfo = false;
			}
		}
	}
	
	public void GameBoardDrag()
    {
		if( draggingGameboard | GridCursorControl.ClickedOffScreen ) return;
        dragOrigin = Input.mousePosition;
		S.GameControlGUIInst.Dim (false);
        draggingGameboard = true;
        Vector3 worldPointVector = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        arrows.transform.position = new Vector3(worldPointVector.x, worldPointVector.y, 0);
        arrows.GetComponent<SpriteRenderer>().enabled = true;
    }

	void handDrag() {
		draggingHand = true;
		S.GameControlGUIInst.Dim(false);
		Cursor.SetCursor(SideArrows, Vector2.zero, CursorMode.Auto);
		cardHasBeenClickedOn = false;
	}
	
	void clickOnCard(GameObject tempGO) {
		cardScriptClickedOn = tempGO.GetComponent<Card>();
		cardHasBeenClickedOn = true;
		cardScriptClickedOn.cardUI.GlowAnimate(true);
		
		lastCardClick = Time.time;
		dragOrigin = Input.mousePosition;
	}

	//returns true and moves Player if you clicked on a square adjacent to Player
	string adjCheck (Vector3 checkedObjPos){
		Vector3[] aboveBelowLeftRight = new Vector3[] { playerObject.transform.position, playerObject.transform.position, 
														playerObject.transform.position, playerObject.transform.position};
		aboveBelowLeftRight[0].y++;
		aboveBelowLeftRight[1].y--;
		aboveBelowLeftRight[2].x--;
		aboveBelowLeftRight[3].x++;

		//check if there is a unit in this square. if not, move there
		if(GridSquareCheck(checkedObjPos, aboveBelowLeftRight[0])) return "up";
		else if(GridSquareCheck(checkedObjPos, aboveBelowLeftRight[1])) return "down";
		else if(GridSquareCheck(checkedObjPos, aboveBelowLeftRight[2])) return "left";
		else if(GridSquareCheck(checkedObjPos, aboveBelowLeftRight[3])) return "right";
		else return "none";
	}

	bool GridSquareCheck(Vector3 clickPos, Vector3 objectPos){
		if(clickPos.x < (objectPos.x + .5f) && clickPos.x > (objectPos.x - .5f) && 
		   clickPos.y < (objectPos.y + .5f) && clickPos.y > (objectPos.y - .5f))
		{
			return true;
		}
		else return false;
	}

	public void AllowEveryInput () {
		AllowInputUmbrella = true;
		AllowInfoInput = true;
		AllowForfeitButtonInput = true;
		AllowNewPlayInput = true;
		AllowMoveInput = true;

		AllowCardTargetInput = false;
		AllowSquareTargetInput = true;
	}

	public void DisallowEveryInput () {
		AllowInputUmbrella = false;
		AllowInfoInput = false;
		AllowForfeitButtonInput = false;
		AllowNewPlayInput = false;
		AllowMoveInput = false;
		AllowCardTargetInput = false;
		AllowSquareTargetInput = false;
	}

	public void ChangeUmbrellaInputAllowToTrue() {
		AllowInputUmbrella = true;
	}
}
