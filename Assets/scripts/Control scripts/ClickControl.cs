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

	//gridcursorcontrol variables
	public float lastCursorSetTime = 0;

	//card clicking and display variables
	public bool cardHasBeenClickedOn = false;
	public Card cardScriptClickedOn;
    Vector3 cardClickOrigin;
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
    float currentCursorXPosition = 0;
    float currentCursorYPosition = 0;

    EventSystem eventSystem;
	
	void Start(){
		playerObject = GameObject.FindGameObjectWithTag ("Player");
        player = playerObject.GetComponent<Player>();

		playButton = GameObject.Find ("play end button").GetComponent<ButtonAnimate> ();

		displayCardCanvas = GameObject.FindGameObjectWithTag("displaycard").GetComponent<DisplayCardCanvas>(); 
        
        eventSystem = GameObject.FindGameObjectWithTag("eventsystem").GetComponent<EventSystem>();
	}

	void Update () {

		if (Input.GetMouseButtonDown (0)) {
			S.ShopControlInst.GoalCheck("Touch the screen no more than than X times");
		}

		if (!Input.GetMouseButton (0) && GridCursorControl.ClickedOffScreen) {
			GridCursorControl.ClickedOffScreen = false;
		}

		// Resetting things
		
		if( S.MenuControlInst.AnyMenuIsUp() | 
            S.ShopControlGUIInst.IgnoreClicking | 
            GridCursorControl.ClickedOffScreen | 
            !AllowInputUmbrella | 
            !AllowInfoInput) {
			return;
		}
		
        if(cardScriptClickedOn != null) {
            if (!Input.GetMouseButton(0))
            {
				cardScriptClickedOn.cardUI.GlowAnimate(false);
            } 
        }

		if(displayCardCanvas.CardDisplay && (!Input.GetMouseButton(0) || !AllowInputUmbrella || !AllowInfoInput)) {
			S.GameControlGUIInst.Undisplay();
			cardHasBeenClickedOn = false;
            return;
		}

		// This replaces the clickblocker system. Prevents clicking on stuff under canvas elements
		if (eventSystem.IsPointerOverGameObject()) {
			return;
		}

		if (GridCursorControl.cursorActionSet) {
			if(!Input.GetMouseButton(0)) {
                if(Time.time > lastCursorSetTime + 1.0f) {
                    // S.GridCursorControlInst.ShowInfo();
				    return;
                } else {
                    S.GridCursorControlInst.ReleaseCursor();
                    return;
                }
			}
		}

		//////////////////////////////////////
		/// CARDCLICKS
		//////////////////////////////////////

		if(cardHasBeenClickedOn && cardScriptClickedOn != null && !S.DragControlInst.DraggingHand){
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

				if(Mathf.Abs(Input.mousePosition.x - cardClickOrigin.x) > .2f 
				   && !cardScriptClickedOn.Discarded && AllowInfoInput) {
					S.DragControlInst.HandDrag(cardScriptClickedOn, cardClickOrigin);
            		cardHasBeenClickedOn = false;
					return;
				}
				else if(Mathf.Abs(Input.mousePosition.y - cardClickOrigin.y) > .2f 
				        && cardScriptClickedOn.Discarded && AllowInfoInput) {
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

		if(Input.GetMouseButtonDown(0)){
            
            Vector3 newClickPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            currentCursorXPosition = newClickPosition.x;
            currentCursorYPosition = newClickPosition.y;

			// S.GameControlGUIInst.ShowDeck(false);

			float dist = transform.position.z - Camera.main.transform.position.z;
			var pos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, dist);
			pos = Camera.main.ScreenToWorldPoint(pos);
			RaycastHit2D[] hits = Physics2D.RaycastAll (pos, Vector2.zero);

			if(Input.GetMouseButtonDown(0)) {
                registerClick();
                
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
					// foreach(RaycastHit2D hit in hits) {
					// 	if(hit.collider.gameObject.tag == "click blocker" ) { 
					// 		return;
					// 	}
					// }
					// foreach(RaycastHit2D hit in hits) {
					// 	if(hit.collider.gameObject.name == "Deck" && AllowInfoInput) { 
					// 		S.GameControlGUIInst.ShowDeck(true);
					// 		return;
					// 	}
					// }
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
            // if(S.DragControlInst.DraggingHand) return;
            // if(S.DragControlInst.DraggingGameboard) return;

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
		}
        
        if (Input.GetMouseButton(0) && 
            CursorHasMovedCheck() && 
            !S.DragControlInst.DraggingGameboard && 
            !S.DragControlInst.DraggingHand) {
            S.DragControlInst.GameBoardDrag();        
            S.GridCursorControlInst.UnpresentCursor();
        }
        
	}

	void clickOnCard(GameObject tempGO) {
		cardScriptClickedOn = tempGO.GetComponent<Card>();
		cardHasBeenClickedOn = true;
		cardScriptClickedOn.cardUI.GlowAnimate(true);
		
		lastCardClick = Time.time;
		cardClickOrigin = Input.mousePosition;
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
    
    bool CursorHasMovedCheck() {
        Vector3 clickPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if(clickPosition.x > 0) clickPosition.x += .5f;
        if(clickPosition.y > 0) clickPosition.y += .5f;
        if(clickPosition.x < 0) clickPosition.x -= .5f;
        if(clickPosition.y < 0) clickPosition.y -= .5f;
        return ((int)clickPosition.x != currentCursorXPosition | 
            (int)clickPosition.y != currentCursorYPosition); 
    }
    
    void registerClick() {
        Vector3 clickPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if(clickPosition.x > 0) clickPosition.x += .5f;
        if(clickPosition.y > 0) clickPosition.y += .5f;
        if(clickPosition.x < 0) clickPosition.x -= .5f;
        if(clickPosition.y < 0) clickPosition.y -= .5f;
        currentCursorXPosition = (int)clickPosition.x;
        currentCursorYPosition = (int)clickPosition.y;
    }
}
