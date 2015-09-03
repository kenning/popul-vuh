using UnityEngine;
using System.Collections;

public class GridCursorControl : MonoBehaviour {

	GameControl gameControl;
	GridControl gridControl;
	ShopControl shopControl;
	ClickControl clickControl;
	GridCursorControlGUI gridCursorControlGUI;
	bool cursorActionSet = false;
	float lastCursorSetTime = 0;

	GameObject playerObject;

	public enum CursorActions {Punch, TargetSquare, PlayerInfo, 
		EnemyInfo, ObstacleInfo, Move, StairMove, Poke, None };

	CursorActions currentCursorAction = CursorActions.None;
	GameObject currentCursorTarget = null;
	int currentCursorXPosition = 0;
	int currentCursorYPosition = 0;
	Obstacle walkableObstacleToWalkInto = null;
	string moveDirection = null;

	void Start() {
		gameControl = gameObject.GetComponent<GameControl> ();
		gridControl = gameObject.GetComponent<GridControl> ();
		shopControl = gameObject.GetComponent<ShopControl> ();
		clickControl = gameObject.GetComponent<ClickControl> ();
		gridCursorControlGUI = GameObject.Find("Grid Cursor").GetComponent<GridCursorControlGUI> ();
		playerObject = GameObject.FindGameObjectWithTag ("Player");
	}

	void Update() {
		if (cursorActionSet) {
			if(!Input.GetMouseButton(0)) {
				ReleaseCursor();
				gridCursorControlGUI.UnpresentCursor();
				cursorActionSet = false;
			} else if(Time.time > lastCursorSetTime + 1.0f) {
				clickControl.GameBoardDrag();	
				UnpresentCursor();
			}
		}
	}

	/// <summary>
	/// Main method 1; sets the cursor action
	/// </summary>
	/// <param name="action">Action.</param>
	public void PresentCursor(CursorActions action) {
		Vector3 clickPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		if(clickPosition.x > 0) clickPosition.x += .5f;
		if(clickPosition.y > 0) clickPosition.y += .5f;
		if(clickPosition.x < 0) clickPosition.x -= .5f;
		if(clickPosition.y < 0) clickPosition.y -= .5f;
		if (cursorActionSet == false |
		    action != currentCursorAction |
			(int)(clickPosition.x) != currentCursorXPosition |
			(int)(clickPosition.y) != currentCursorYPosition) {
			lastCursorSetTime = Time.time;
			cursorActionSet = true;
			currentCursorXPosition = (int)clickPosition.x;
			currentCursorYPosition = (int)clickPosition.y;
			currentCursorAction = action;
			gridCursorControlGUI.PresentCursor (action, currentCursorXPosition, currentCursorYPosition);
		}
		
	}

	public void UnpresentCursor() {
		PresentCursor (CursorActions.None);
		// Set them off camera so they get retriggered
		currentCursorXPosition = 500;
		currentCursorYPosition = 500;
		gridCursorControlGUI.UnpresentCursor ();
	}

	/// <summary>
	/// Supplementary method for PresentCursor; supplies a target for cursor actions that require it
	/// </summary>
	/// <param name="target">Target.</param>
	public void SetCurrentCursorTarget(GameObject target) {
		// Sets movedirection to null so that the move action can check it and move
		// in the direction of stairs if stairs were clicked on.
		moveDirection = null;
		currentCursorTarget = target;
	}

	public void SetWalkableObstacle(GameObject obstacleObject) {
		walkableObstacleToWalkInto = obstacleObject.GetComponent<Obstacle> ();
	}

	public void UnsetWalkableObstacle() {
		walkableObstacleToWalkInto = null;
	}

	public void SetMoveDirection(string direction) {
		moveDirection = direction;
	}

	/// <summary>
	/// Main method 2; Executes whatever action is set to happen
	/// </summary>
	public void ReleaseCursor() {
		switch (currentCursorAction) {
		case CursorActions.StairMove:
			if(gameControl.MovesLeft > 0){
				gameControl.AddMoves(-1);
				shopControl.GoalCheck("Move X times in one turn");
				shopControl.GoalCheck("Don't move X turns in a row");
				shopControl.GoalCheck("Don't deal damage or move X turns in a row");
				shopControl.GoalCheck("Don't move X turns in a row");
			}
			else {
				ButtonAnimate moveButton = 
					GameObject.Find("move end button").GetComponent<ButtonAnimate>();
				moveButton.ErrorAnimation();
			}
			break;
		case CursorActions.Move:
			if(walkableObstacleToWalkInto != null) {
				walkableObstacleToWalkInto.StepIn();
			}
			if(moveDirection != null) {
				playerObject.GetComponent<Player>().MoveClick(moveDirection);
			} 
			else {
				Debug.LogError("there should be a move direction set already. oops");
			}
			break;
		case CursorActions.Poke:
			GridUnit playerGU = playerObject.GetComponent<GridUnit>();
			playerGU.PokeTowards(playerGU.AdjacentPosition(currentCursorTarget.GetComponent<GridUnit>()));
			break;
		case CursorActions.Punch:
			playerObject.GetComponent<Player>().Punch(currentCursorTarget);
			break;
		case CursorActions.TargetSquare:
			TargetSquare square = currentCursorTarget.GetComponent<TargetSquare>();
			if (Tutorial.TutorialLevel != 0)
			{
				if(square.XCoor == 0 && square.YCoor == -2) 
				{
					gameControl.gameObject.GetComponent<Tutorial>().TutorialTrigger(5);
				}
				else 
				{
					gameControl.gameObject.GetComponent<Tutorial>().TutorialMessage = "Just tap the enemy. Kill it!";
					return;
				}
			}
			gameControl.TargetSquareCallback.TargetSquareCalledThis(square.XCoor, square.YCoor);
			break;
		case CursorActions.EnemyInfo:
			GridUnit tempGU = currentCursorTarget.GetComponent<GridUnit>();
			Enemy tempEnemy = currentCursorTarget.GetComponent<Enemy>();
			gameControl.Tooltip = tempEnemy.Tooltip;
			gridControl.MakeSquares(tempEnemy.AttackTargetType, tempEnemy.AttackMinRange, 
			                     tempEnemy.AttackMaxRange, tempGU.xPosition, tempGU.yPosition, false);
			break;
		case CursorActions.ObstacleInfo:
			Obstacle hitObstacle = currentCursorTarget.GetComponent<Obstacle>();
			hitObstacle.ShowTooltip();
			break;
		case CursorActions.PlayerInfo:
			gameControl.Tooltip = "That's you! You're Xbalanque, one of the twin sons of Hunapu.";
			gridControl.MakeSquares(GridControl.TargetTypes.diamond, 1, 1, false);
			break;
		case CursorActions.None:
			break;
		}
	}
}
