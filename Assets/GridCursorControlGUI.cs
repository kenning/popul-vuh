using UnityEngine;
using System.Collections;

public class GridCursorControlGUI : MonoBehaviour {

	public SpriteRenderer cursorSpriteRenderer;
	public Sprite NONESPRITE;
	public Sprite MOVESPRITE;
	public Sprite PUNCHSPRITE;
	public Sprite TARGETSQUARESPRITE;
	public Sprite INFOSPRITE;

	void Start() {
		cursorSpriteRenderer = gameObject.GetComponent<SpriteRenderer> ();
	}

	public void PresentCursor(GridCursorControl.CursorActions action, int x, int y) {
		transform.position = new Vector3 (x, y, 0);
		cursorSpriteRenderer.enabled = true;
		switch (action) {
		case GridCursorControl.CursorActions.StairMove:
			cursorSpriteRenderer.sprite = MOVESPRITE;
			break;
		case GridCursorControl.CursorActions.Move:
			cursorSpriteRenderer.sprite = MOVESPRITE;
			break;
		case GridCursorControl.CursorActions.Poke:
			cursorSpriteRenderer.sprite = MOVESPRITE;
			break;
		case GridCursorControl.CursorActions.Punch:
			cursorSpriteRenderer.sprite = PUNCHSPRITE;
			break;
		case GridCursorControl.CursorActions.TargetSquare:
			cursorSpriteRenderer.sprite = TARGETSQUARESPRITE;
			break;
		case GridCursorControl.CursorActions.EnemyInfo:
			cursorSpriteRenderer.sprite = INFOSPRITE;
			break;
		case GridCursorControl.CursorActions.ObstacleInfo:
			cursorSpriteRenderer.sprite = INFOSPRITE;
			break;
		case GridCursorControl.CursorActions.PlayerInfo:
			cursorSpriteRenderer.sprite = INFOSPRITE;
			break;
		case GridCursorControl.CursorActions.None:
			cursorSpriteRenderer.sprite = NONESPRITE;
			break;
		}
		//set cursor sprite to appropriate cursoraction's sprite
		//Vector3 clickPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		//move cursor to there
	}

	public void UnpresentCursor() {
		cursorSpriteRenderer.enabled = false;
	}
}
