using UnityEngine;
using System.Collections;

public class GridCursorControl : MonoBehaviour {

	public static bool cursorActionSet = false;
	public static bool GridCursorIsActive = false;
	public static bool ClickedOffScreen = false;

	GameObject playerObject;

	public enum CursorActions {Punch, TargetSquare, Move, StairMove, Poke, Info, None };
	public enum CursorInfoTypes {EnemyInfo, PlayerInfo, ObstacleInfo};

	CursorActions currentCursorAction = CursorActions.None;
    CursorInfoTypes currentCursorInfoType;
	GameObject currentCursorTarget = null;
	GameObject cursorInfoTarget = null;
	int currentCursorXPosition = 0;
	int currentCursorYPosition = 0;
	Obstacle walkableObstacleToWalkInto = null;
	string moveDirection = null;

	void Start() {
		playerObject = GameObject.FindGameObjectWithTag ("Player");
	}

	/// <summary>
	/// Main method 1; sets the cursor action
	/// </summary>
	/// <param name="action">Action.</param>
	public void PresentCursor(CursorActions action) {
		GridCursorControl.GridCursorIsActive = true;
		Vector3 clickPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		if(clickPosition.x > 0) clickPosition.x += .5f;
		if(clickPosition.y > 0) clickPosition.y += .5f;
		if(clickPosition.x < 0) clickPosition.x -= .5f;
		if(clickPosition.y < 0) clickPosition.y -= .5f;
		if (cursorActionSet == false |
		    action != currentCursorAction |
			(int)(clickPosition.x) != currentCursorXPosition |
			(int)(clickPosition.y) != currentCursorYPosition) {
			ResetInfoTarget ();
			S.ClickControlInst.lastCursorSetTime = Time.time;
			cursorActionSet = true;
			currentCursorXPosition = (int)clickPosition.x;
			currentCursorYPosition = (int)clickPosition.y;
			currentCursorAction = action;
			S.GridCursorControlGUIInst.PresentCursor (action, currentCursorXPosition, currentCursorYPosition);
		}
		
	}

	public void UnpresentCursor() {
		S.GridCursorControlGUIInst.UnpresentCursor();
		GridCursorControl.GridCursorIsActive = false;
		// Set them off camera so they get retriggered
		currentCursorXPosition = 500;
		currentCursorYPosition = 500;
		S.GameControlGUIInst.SetTooltip ("");
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
			if(S.GameControlInst.MovesLeft > 0){
				S.GameControlInst.AddMoves(-1);
				S.ShopControlInst.GoalCheck("Move X times in one turn");
				S.ShopControlInst.GoalCheck("Don't move X turns in a row");
				S.ShopControlInst.GoalCheck("Don't deal damage or move X turns in a row");
				S.ShopControlInst.GoalCheck("Don't move X turns in a row");
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
					S.GameControlInst.gameObject.GetComponent<Tutorial>().TutorialTrigger(5);
				}
				else 
				{
					S.GameControlInst.gameObject.GetComponent<Tutorial>().TutorialMessage = "Just tap the enemy. Kill it!";
					return;
				}
			}
			S.GameControlInst.TargetSquareCallback.TargetSquareCalledThis(square.XCoor, square.YCoor);
			break;
		case CursorActions.None:
			break;
		default:
			Debug.Log("shouldn't be showing info! bug!");
			break;
		}

		UnpresentCursor();

		cursorActionSet = false;
	}

	public void ShowInfo() {
        switch (currentCursorInfoType) {
        case CursorInfoTypes.EnemyInfo:
            GridUnit tempGU = currentCursorTarget.GetComponent<GridUnit> ();
            Enemy tempEnemy = currentCursorTarget.GetComponent<Enemy> ();
            S.GameControlGUIInst.SetTooltip (tempEnemy.Tooltip);
            S.GridControlInst.MakeSquares (tempEnemy.AttackTargetType, tempEnemy.AttackMinRange, 
                                    tempEnemy.AttackMaxRange, tempGU.xPosition, tempGU.yPosition, false);
            break;
        case CursorInfoTypes.ObstacleInfo:
            Obstacle hitObstacle = currentCursorTarget.GetComponent<Obstacle> ();
            hitObstacle.ShowTooltip ();
            break;
        case CursorInfoTypes.PlayerInfo:
            S.GameControlGUIInst.SetTooltip ("That's you! You're Xbalanque, one of the twin sons of Hunapu.");
            S.GridControlInst.MakeSquares (GridControl.TargetTypes.diamond, 1, 1, false);
            break;
        }
	}

	public void ResetInfoTarget() {
		cursorInfoTarget = null;
	}
}
