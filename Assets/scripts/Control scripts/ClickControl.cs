using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class ClickControl : MonoBehaviour {
	//Game control scripts
	GameControl gameControl;
	GridControl gridBoss;
	ShopControl shopControl;
	GameObject handObj;
	GameControlGUI gameControlGUI;
	ShopControlGUI shopControlGUI;
	ButtonAnimate playButton;
	GridCursorControl gridCursorControl;
	DisplayCardCanvas displayCardCanvas;
	MenuControl menuControl;
    Player player;
	EventSystem eventSystem;

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

	Texture2D SideArrows;
	
	void Start(){
		GameObject tempGO = GameObject.FindGameObjectWithTag ("GameController");
		gameControl = tempGO.GetComponent<GameControl> ();
		handObj = GameObject.Find ("Hand");
		gridBoss = tempGO.GetComponent<GridControl>();
		shopControl = tempGO.GetComponent<ShopControl> ();
		menuControl = tempGO.GetComponent<MenuControl> ();
		gridCursorControl = tempGO.GetComponent<GridCursorControl> ();
		gameControlGUI = tempGO.GetComponent<GameControlGUI> ();
		shopControlGUI = tempGO.GetComponent<ShopControlGUI> ();

		playerObject = GameObject.FindGameObjectWithTag ("Player");
        player = playerObject.GetComponent<Player>();

		playButton = GameObject.Find ("play end button").GetComponent<ButtonAnimate> ();

		arrows =  GameObject.FindGameObjectWithTag("Camera Arrow");
		SideArrows = (Texture2D)Resources.Load ("sprites/ui/side arrows");

		displayCardCanvas = GameObject.FindGameObjectWithTag("displaycard").GetComponent<DisplayCardCanvas>(); 

		eventSystem = GameObject.FindGameObjectWithTag("eventsystem").GetComponent<EventSystem>();
	}

	void Update () {

		// Resetting things
		
		if (Input.GetMouseButtonDown (0)) {
			shopControl.GoalCheck("Touch the screen no more than than X times");
		}
		if (eventSystem.IsPointerOverGameObject()) {
			return;
		}
		if (!Input.GetMouseButton (0) && GridCursorControl.ClickedOffScreen) {
			GridCursorControl.ClickedOffScreen = false;
		}

		if(menuControl.AnyMenuIsUp() | shopControlGUI.IgnoreClicking | GridCursorControl.ClickedOffScreen) {
			return;
		}

		if (!Input.GetMouseButton (0)) {
			gameControlGUI.SetTooltip("");
		}

        if(cardScriptClickedOn != null) {
            if (!Input.GetMouseButton(0))
            {
				cardScriptClickedOn.cardUI.GlowAnimate(false);
            } 
        }

		if(!AllowInputUmbrella) {
			if(displayCardCanvas.CardDisplay) {
				gameControlGUI.Undisplay();
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
			gameControlGUI.Undisplay();
			cardHasBeenClickedOn = false;
		}

		if (GridCursorControl.cursorActionSet) {
			if(!Input.GetMouseButton(0)) {
				gridCursorControl.ReleaseCursor();
				return;
			} else if(Time.time > lastCursorSetTime + 1.0f) {
				GameBoardDrag();	
				gridCursorControl.UnpresentCursor();
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
				if(gameControl.Hand.Count < 4) { 
					handObj.transform.localPosition = new Vector3(((3) * -1.48f) + 3.7f, 0, 0);
					return;
				}
				Vector3 pos = Camera.main.ScreenToViewportPoint (Input.mousePosition - dragOrigin);

				if(handObj.transform.localPosition.x >= -.75f && pos.x > 0) {
					handObj.transform.localPosition = new Vector3(-.73f, 0, 0f);
					return;
				}
				else if((handObj.transform.localPosition.x <= ((gameControl.Hand.Count) * -1.55f) + 5.35f) &&  pos.x < 0) {
					//this is for after the exact position has gotten nailed down, purpose is to lock it to the edge. 
					//the key numbers are: 3.95 one line above and .75 six lines above.
					handObj.transform.localPosition = new Vector3(((gameControl.Hand.Count) * -1.55f) + 5.3f, 0, 0);
				} else {
					Vector3 move = new Vector3(pos.x, 0, 0);
					handObj.transform.Translate(move);  
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
				if(gameControl.CardsToTarget != 0 
				   && AllowCardTargetInput 
				   && (cardScriptClickedOn.Peeked == gameControl.CardsToTargetArePeeked)
				   && (gameControl.CardsToTargetAreDiscarded == cardScriptClickedOn.Discarded)) {
					if(gameControl.TargetedCards.Contains(cardScriptClickedOn.gameObject))
					{
						cardScriptClickedOn.Untarget();
						gameControl.TargetedCards.Remove(cardScriptClickedOn.gameObject);
						cardHasBeenClickedOn = false;
					}
					else if(cardScriptClickedOn != gameControl.TargetCardCallback) 
					{
						cardScriptClickedOn.Target();
						gameControl.TargetedCards.Add(cardScriptClickedOn.gameObject);
						if(gameControl.TargetedCards.Count == gameControl.CardsToTarget) 
							gameControl.TargetCardCallback.AfterCardTargetingCallback();
						cardHasBeenClickedOn = false;
					}
				}
				else if (!cardScriptClickedOn.Discarded 
				         && gameControl.PlaysLeft > 0 
				         && AllowNewPlayInput 
				         && gameControl.CardsToTarget == 0) {
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
					gameControlGUI.Display(cardScriptClickedOn);
					cardHasBeenClickedOn = false;
				}
				else if(Time.time - .03f > lastCardClick && AllowInfoInput) 
                {
                    cardScriptClickedOn.cardUI.ShineAnimate();
					gameControlGUI.DisplayDim();
                }
			}
		}

		//////////////////////////////////////
		/// NEW CLICKS
		//////////////////////////////////////

		if(Input.GetMouseButton(0)){

			gameControlGUI.ShowDeck(false);

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
							gameControl.ButtonSpritesLookClicked();
							gameControl.DeselectCards();
							gridBoss.DestroyAllTargetSquares();
	                        if (Tutorial.TutorialLevel != 0)
	                        {
	                            gameControl.StartNewTurn();
	                        }
	                        else
	                        {
	                            gameControl.EnemyTurn(true);
	                        }
							return;
						}
					}
					foreach(RaycastHit2D hit in hits) {
						if(hit.collider.gameObject.name == "play end button" && AllowForfeitButtonInput) { 
							if(gameControl.PlaysLeft == 1) {
								gameControlGUI.SetTooltip("You can punch or play a card one more time this turn.");
							} else {
								gameControlGUI.SetTooltip("You can punch or play a card " + 
								                          gameControl.PlaysLeft.ToString() + 
								                          " more times this turn.");
							}
							foreach(GameObject card in gameControl.Hand) {
								card.GetComponent<CardUI>().ShineAnimate();
							}
							return;
						}
					}
					foreach(RaycastHit2D hit in hits) {
						if(hit.collider.gameObject.name == "move end button" && AllowForfeitButtonInput) { 
							if(gameControl.MovesLeft == 1) {
								gameControlGUI.SetTooltip("You can move one more time this turn.");
							} else {
								gameControlGUI.SetTooltip("You can move " + 
								                          gameControl.MovesLeft.ToString() + 
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
							gameControlGUI.ShowDeck(true);
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
						if(hit.collider.gameObject.name == "Discard pile" && AllowInfoInput) {
							gameControlGUI.FlipDiscard();
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
					gridCursorControl.UnpresentCursor();
				}
				return;
			}

			foreach(RaycastHit2D hit in hits){
				if(hit.collider.gameObject.tag == "Target square" && AllowSquareTargetInput){
					gridCursorControl.PresentCursor(GridCursorControl.CursorActions.TargetSquare);
					gridCursorControl.SetCurrentCursorTarget(hit.collider.gameObject);
					return;
				}
			}

			gameControl.DeselectCards();

			gridBoss.DestroyAllTargetSquares();
			
			foreach(RaycastHit2D hit in hits){
				if(hit.collider.gameObject.tag == "Enemy"){
					if(AllowInfoInput) {
						willShowEnemyInfo = true;
						willShowPlayerInfo = false;
						willShowObstacleInfo = false;
						infoTarget = hit.collider.gameObject;
					}
					if(adjCheck(hit.collider.gameObject.transform.position) != "none" 
					   && gameControl.PlaysLeft > 0 && AllowNewPlayInput){
						gridCursorControl.PresentCursor(GridCursorControl.CursorActions.Punch);
						gridCursorControl.SetCurrentCursorTarget(hit.collider.gameObject);
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
					GridUnit playerGU = gameControl.playerObj.GetComponent<GridUnit>();
					
					
                    if (obstacleGU.IsAdjacent(playerGU))
                    {
                        Obstacle hitObstacle = hit.collider.gameObject.GetComponent<Obstacle>();
                        if (hitObstacle.Walkable)
                        {
							gridCursorControl.PresentCursor(GridCursorControl.CursorActions.Move);
							gridCursorControl.SetCurrentCursorTarget(hit.collider.gameObject);
						}
                        else {
							gridCursorControl.PresentCursor(GridCursorControl.CursorActions.Poke);
							gridCursorControl.SetCurrentCursorTarget(hit.collider.gameObject);
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
						gridCursorControl.PresentCursor(GridCursorControl.CursorActions.StairMove);
					}
					return;
				}
			}
			foreach(RaycastHit2D hit in hits) {
				if(hit.collider.gameObject.tag == "Gameboard") {
					if(adjCheck(clickPosition) != "none" && AllowMoveInput) {
						gridCursorControl.PresentCursor(GridCursorControl.CursorActions.Move);
						gridCursorControl.SetMoveDirection(adjCheck(clickPosition));
					}
					else if (AllowInfoInput) {
						gridCursorControl.PresentCursor(GridCursorControl.CursorActions.None);
					}
				}
			}

			tryToShowInfo();
		}
	}

	void tryToShowInfo() {
		if((Time.time - .15f) > lastCursorSetTime) {
			if(willShowEnemyInfo) {
				gridCursorControl.ShowInfo(GridCursorControl.CursorInfoTypes.EnemyInfo, infoTarget);
				willShowEnemyInfo = false;
				willShowPlayerInfo = false;
				willShowObstacleInfo = false;
			} else if (willShowPlayerInfo) {
				gridCursorControl.ShowInfo(GridCursorControl.CursorInfoTypes.PlayerInfo, infoTarget);
				willShowEnemyInfo = false;
				willShowPlayerInfo = false;
				willShowObstacleInfo = false;
			} else if (willShowObstacleInfo) {
				gridCursorControl.ShowInfo(GridCursorControl.CursorInfoTypes.ObstacleInfo, infoTarget);
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
		gameControlGUI.Dim (false);
        draggingGameboard = true;
        Vector3 worldPointVector = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        arrows.transform.position = new Vector3(worldPointVector.x, worldPointVector.y, 0);
        arrows.GetComponent<SpriteRenderer>().enabled = true;
    }

	void handDrag() {
		draggingHand = true;
		gameControlGUI.Dim(false);
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
