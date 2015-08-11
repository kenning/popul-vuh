using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ClickControl : MonoBehaviour {
	//Game control scripts
	GameControl battleBoss;
	GridControl gridBoss;
	ShopControl shopBoss;
	GameObject hand;
	GameControlGUI gameControlGUI;
	ButtonAnimate playButton;
    //player script
    Player player;

	//units
	List<GridUnit> UnitList;
	GameObject playerObject;

	//dragging variables
	Vector3 dragOrigin;
	bool draggingGameboard = false;
	bool draggingHand = false;
	bool draggingDiscard = false;
	GameObject arrows;

	//gameboard limits
	float leftLimit = -2.0f;
	float rightLimit = 2.0f;
	float topLimit = 0.9f;
	float bottomLimit = -2.95f;

	Texture2D SideArrows;

	//card clicking and display variables
	public bool cardHasBeenClickedOn = false;
	public Card cardScriptClickedOn;
	float lastCardClick = 0f;

	//allow bools
	public bool AllowInputUmbrella = false;
	public bool AllowInfoInput = true;
	public bool AllowForfeitButtonInput = true;
	public bool AllowNewPlayInput = true;
		//stab, play
	public bool AllowMoveInput = true;
	public bool AllowCardTargetInput = false;
	public bool AllowSquareTargetInput = true;

	
	//i'm impatient and want to end the damn turn already bool
	public bool turnEndedAlready = false;

	void Start(){
		GameObject tempGO = GameObject.FindGameObjectWithTag ("GameController");
		battleBoss = tempGO.GetComponent<GameControl> ();
		gridBoss = tempGO.GetComponent<GridControl>();
		shopBoss = tempGO.GetComponent<ShopControl> ();
		gameControlGUI = tempGO.GetComponent<GameControlGUI> ();
		playerObject = GameObject.FindGameObjectWithTag ("Player");
        player = playerObject.GetComponent<Player>();
		hand = GameObject.Find ("Hand");
//		discard = GameObject.Find ("Discard pile");
		playButton = GameObject.Find ("play end button").GetComponent<ButtonAnimate> ();
		arrows =  GameObject.FindGameObjectWithTag("Camera Arrow");
		SideArrows = (Texture2D)Resources.Load ("sprites/ui/side arrows");
	}

	void Update () {

	//MainMenu
		if(MainMenu.MainMenuUp | MainMenu.DeleteDataMenuUp | CustomizeMenu.CustomizeMenuUp | GodChoiceMenu.GodChoiceMenuUp ) {
			return;
		}

        if(cardScriptClickedOn != null) {
            if (cardScriptClickedOn.Glow.enabled && !Input.GetMouseButton(0))
            {
                cardScriptClickedOn.GlowAnimate(false);
            } 
        }

	//AllowInputUmbrella
		if(!AllowInputUmbrella) {
			if(gameControlGUI.CardDisplay) {
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
		
		//////////////////////////////////////
		/// DRAGGING AND DISPLAY -- InfoInput
		//////////////////////////////////////

		if(gameControlGUI.CardDisplay && !Input.GetMouseButton(0)) {
			gameControlGUI.Undisplay();
			cardHasBeenClickedOn = false;
		}

	//AllowInfoInput
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
			}
			else {
				draggingGameboard = false;
				arrows.GetComponent<SpriteRenderer>().enabled = false;
			}
		}
	//AllowInfoInput
		if(draggingHand && AllowInfoInput){
			if(Input.GetMouseButton(0)){

				if(battleBoss.Hand.Count < 3) { 
					hand.transform.localPosition = new Vector3(((3) * -1.48f) + 3.7f, 0, 0);
					return;
				}
				Vector3 pos = Camera.main.ScreenToViewportPoint (Input.mousePosition - dragOrigin);

				if(hand.transform.localPosition.x >= -.75f &&  pos.x > 0) {
					hand.transform.localPosition = new Vector3(-.73f, 0, 0f);
					return;
				}
				else if((hand.transform.localPosition.x <= ((battleBoss.Hand.Count) * -1.55f) + 3.75f) &&  pos.x < 0) {
					//this is for after the exact position has gotten nailed down, purpose is to lock it to the edge. 
					//the key numbers are: 3.95 one line above and .75 six lines above.
					hand.transform.localPosition = new Vector3(((battleBoss.Hand.Count) * -1.55f) + 3.7f, 0, 0);
					return;
				}
				else {
					Vector3 move = new Vector3(pos.x, 0, 0);
					hand.transform.Translate(move);  
					return;
				}
			}
			else {
				draggingHand = false;
				Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
			}
		}

		//////////////////////////////////////
		/// CARDCLICKS
		//////////////////////////////////////

		if(cardHasBeenClickedOn && cardScriptClickedOn != null && !draggingHand && !draggingDiscard){
			if(!Input.GetMouseButton(0)){
         //AllowCardTargetInput
				if(battleBoss.CardsToTarget != 0 && AllowCardTargetInput && (cardScriptClickedOn.Peeked == battleBoss.CardsToTargetArePeeked)
				   && (battleBoss.CardsToTargetAreDiscarded == cardScriptClickedOn.Discarded)) {
					if(battleBoss.TargetedCards.Contains(cardScriptClickedOn.gameObject))
					{
						cardScriptClickedOn.Untarget();
						battleBoss.TargetedCards.Remove(cardScriptClickedOn.gameObject);
						cardHasBeenClickedOn = false;
					}
					else if(cardScriptClickedOn != battleBoss.TargetCardCallback) 
					{
						cardScriptClickedOn.Target();
						battleBoss.TargetedCards.Add(cardScriptClickedOn.gameObject);
						if(battleBoss.TargetedCards.Count == battleBoss.CardsToTarget) 
							battleBoss.TargetCardCallback.AfterCardTargetingCallback();
						cardHasBeenClickedOn = false;
					}
				}
	//AllowNewPlayInput
				else if (!cardScriptClickedOn.Discarded && battleBoss.PlaysLeft > 0 && AllowNewPlayInput && battleBoss.CardsToTarget == 0) {
					cardScriptClickedOn.Click ();
					cardHasBeenClickedOn = false;
				}
				else {
					playButton.ErrorAnimation();
					cardHasBeenClickedOn = false;
				}
				cardHasBeenClickedOn = false;
				//return?
			}
			else {
	//AllowInfoInput
				if(Mathf.Abs(Input.mousePosition.x - dragOrigin.x) > .2f && !cardScriptClickedOn.Discarded && AllowInfoInput) {
					draggingHand = true;
					Cursor.SetCursor(SideArrows, Vector2.zero, CursorMode.Auto);
					cardHasBeenClickedOn = false;
				}
	//AllowInfoInput
				else if(Mathf.Abs(Input.mousePosition.y - dragOrigin.y) > .2f && cardScriptClickedOn.Discarded && AllowInfoInput) {
					draggingDiscard = true;
					cardHasBeenClickedOn = false;
				}
	//AllowInfoInput
				else if(Time.time - 0.2f > lastCardClick && !gameControlGUI.CardDisplay && AllowInfoInput) { 
					gameControlGUI.Display(cardScriptClickedOn);
					cardHasBeenClickedOn = false;
				}
				else if(Time.time - .1f > lastCardClick && AllowInfoInput) 
                {
                    cardScriptClickedOn.ShineAnimate();
                }
			}
		}

		//////////////////////////////////////
		/// CLICKS (CLICKS ON CARD GO IN CARDCLICKS HEADING)
		//////////////////////////////////////

		if(Input.GetMouseButtonDown(0)){

			shopBoss.GoalCheck("Touch the screen no more than than X times");

			gameControlGUI.ShowDeck(false);
			battleBoss.Tooltip = "";

			float dist = transform.position.z - Camera.main.transform.position.z;
			var pos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, dist);
			pos = Camera.main.ScreenToWorldPoint(pos);
			RaycastHit2D[] hits = Physics2D.RaycastAll (pos, Vector2.zero);

			if(hits != null ){
				foreach(RaycastHit2D hit in hits) {
	//AllowForfeitButtonInput
					if(hit.collider.gameObject.name == "end turn button" && AllowForfeitButtonInput) {
						if(turnEndedAlready) 
							return;
						turnEndedAlready = true;
						battleBoss.ButtonSpritesLookClicked();
						battleBoss.DeselectCards();
						gridBoss.DestroyAllTargetSquares();
                        if (Tutorial.TutorialLevel != 0)
                        {
                            battleBoss.StartNewTurn();
                        }
                        else
                        {
                            battleBoss.EnemyTurn(true);
                        }
						return;
					}
				}
	//AllowForfeitButtonInput
				foreach(RaycastHit2D hit in hits) {
					if(hit.collider.gameObject.name == "play end button" && AllowForfeitButtonInput) { 
						if(battleBoss.PlaysLeft > 0) battleBoss.AddPlays(-battleBoss.PlaysLeft);
						return;
					}
				}
	//AllowForfeitButtonInput
				foreach(RaycastHit2D hit in hits) {
					if(hit.collider.gameObject.name == "move end button" && AllowForfeitButtonInput) { 
						if(battleBoss.MovesLeft > 0) battleBoss.AddMoves(-battleBoss.MovesLeft);
						return;
					}
				}
    //click blocker
				foreach(RaycastHit2D hit in hits) {
					if(hit.collider.gameObject.tag == "click blocker" ) { 
						return;
					}
				}
	//AllowInfoInput
				foreach(RaycastHit2D hit in hits) {
					if(hit.collider.gameObject.name == "Deck" && AllowInfoInput) { 
						gameControlGUI.ShowDeck(true);
						return;
					}
				}
	//n/a, because cardclicks are handled elsewhere, right? (not sure when i wrote this comment)
				foreach(RaycastHit2D hit in hits){
					if(hit.collider.gameObject.tag == "Card") {
						cardScriptClickedOn = hit.collider.gameObject.GetComponent<Card>();
						cardHasBeenClickedOn = true;
                        cardScriptClickedOn.GlowAnimate(true);

						lastCardClick = Time.time;
						dragOrigin = Input.mousePosition;
						return;
					}
				}
	//AllowSquareTargetInput
				foreach(RaycastHit2D hit in hits){
					if(hit.collider.gameObject.tag == "Target square" && AllowSquareTargetInput){
						TargetSquare square = hit.collider.gameObject.GetComponent<TargetSquare>();
                        if (Tutorial.TutorialLevel != 0)
                        {
                            if(square.XCoor == 0 && square.YCoor == -2) 
                            {
                                battleBoss.gameObject.GetComponent<Tutorial>().TutorialTrigger(5);
                            }
                            else 
                            {
                                battleBoss.gameObject.GetComponent<Tutorial>().TutorialMessage = "Just tap the enemy. Kill it!";
                                return;
                            }
                        }
						battleBoss.TargetSquareCallback.TargetSquareCalledThis(square.XCoor, square.YCoor);
						return;
					}
				}
				foreach(RaycastHit2D hit in hits){
					if(hit.collider.gameObject.tag == "Enemy"){
	//AllowNewPlayInput
						if(adjCheck(hit.collider.gameObject.transform.position) != "none" && battleBoss.PlaysLeft > 0 && AllowNewPlayInput){
							playerObject.GetComponent<Player>().Punch(hit.collider.gameObject);
						}
	//AllowInfoInput
						else if(AllowInfoInput) {
							GridUnit tempGU = hit.collider.gameObject.GetComponent<GridUnit>();
							Enemy tempEnemy = hit.collider.gameObject.GetComponent<Enemy>();
							battleBoss.Tooltip = tempEnemy.Tooltip;
							gridBoss.MakeSquares(tempEnemy.AttackTargetType, tempEnemy.AttackMinRange, tempEnemy.AttackMaxRange, tempGU.xPosition, tempGU.yPosition, false);
						}
						return;
					}
				}

	//Deselect card
				battleBoss.DeselectCards();
				gridBoss.DestroyAllTargetSquares();

				foreach(RaycastHit2D hit in hits) 
	//return
					if(hit.collider.gameObject.tag == "Play board") 
						return;
				foreach(RaycastHit2D hit in hits) {
	//AllowInfoInput
					if(hit.collider.gameObject.name == "Discard pile" && AllowInfoInput) {
						gameControlGUI.FlipDiscard();
						return;
					}
				}
				foreach(RaycastHit2D hit in hits){
	//AllowInfoInput
					if(hit.collider.gameObject.tag == "Player" && AllowInfoInput){
						battleBoss.Tooltip = "That's you!\nTap adjacent squares to move there or punch enemies.";
						gridBoss.MakeSquares(GridControl.TargetTypes.diamond, 1, 1, false);
						return;
					}
				}
                foreach (RaycastHit2D hit in hits)
                {
                    if (hit.collider.gameObject.tag == "obstacle")
                    {
                        GridUnit obstacleGU = hit.collider.gameObject.GetComponent<GridUnit>();
                        //will this work???
                        Obstacle hitObstacle = hit.collider.gameObject.transform.parent.gameObject.GetComponent<Obstacle>();
                        GridUnit playerGU = battleBoss.playerObj.GetComponent<GridUnit>();

                        if (!obstacleGU.IsAdjacent(playerGU))
                        {
                            hitObstacle.ShowTooltip();

                            gameBoardDrag();
                        }
                        else
                        {
                            if (hitObstacle.Walkable)
                            {
                                player.MoveClick(playerGU.AdjacentPosition(obstacleGU));
                                hitObstacle.StepIn();
                            }
                            else
                                playerGU.PokeTowards(playerGU.AdjacentPosition(obstacleGU));
                        }
                        return;
                    }
                }
                foreach (RaycastHit2D hit in hits)
                {
	//AllowMoveInput
					if(hit.collider.gameObject.name == "stairs" && AllowMoveInput) {
						if(adjCheck(hit.collider.gameObject.transform.position) != "none") {
							if(battleBoss.MovesLeft > 0){
								battleBoss.AddMoves(-1);
								shopBoss.GoalCheck("Move X times in one turn");
								shopBoss.GoalCheck("Don't move X turns in a row");
								shopBoss.GoalCheck("Don't deal damage or move X turns in a row");
								shopBoss.GoalCheck("Don't move X turns in a row");
							}
							else {
								ButtonAnimate moveButton = GameObject.Find("move end button").GetComponent<ButtonAnimate>();
								moveButton.ErrorAnimation();
							}
						}
						return;
					}
				}
				foreach(RaycastHit2D hit in hits) {
					if(hit.collider.gameObject.tag == "Gameboard") {
						Vector3 clickPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
	//AllowMoveInput
						if(adjCheck(clickPosition) != "none" && AllowMoveInput) {
                            player.MoveClick(adjCheck(clickPosition));
						}
	//AllowInfoInput
						else if (AllowInfoInput) {
                            gameBoardDrag();
						}
						return;
					}
				}
			}
		}
	}

    void gameBoardDrag()
    {
        dragOrigin = Input.mousePosition;
        draggingGameboard = true;
        Vector3 worldPointVector = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        arrows.transform.position = new Vector3(worldPointVector.x, worldPointVector.y, 0);
        arrows.GetComponent<SpriteRenderer>().enabled = true;
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
