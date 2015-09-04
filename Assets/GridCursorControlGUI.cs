using UnityEngine;
using System.Collections;

public class GridCursorControlGUI : MonoBehaviour {

	public SpriteRenderer cursorSpriteRenderer;
	public SpriteRenderer childSpriteRenderer;
	public Sprite NONESPRITE;
	public Sprite MOVESPRITE;
	public Sprite PUNCHSPRITE;
	public Sprite TARGETSQUARESPRITE;
	public Sprite INFOSPRITE;

	void Start() {
		cursorSpriteRenderer = gameObject.GetComponent<SpriteRenderer> ();
		SpriteRenderer[] sprites = gameObject.GetComponentsInChildren<SpriteRenderer> ();
		foreach(SpriteRenderer sprite in sprites) {
			if(sprite.gameObject.name == "Grid Cursor Box") {
				childSpriteRenderer = sprite;
				break;
			}
		}
	}

	public void PresentCursor(GridCursorControl.CursorActions action, int x, int y) {
		transform.position = new Vector3 (x, y, 0);
		cursorSpriteRenderer.enabled = true;
		childSpriteRenderer.enabled = true;
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
	}

	public void UnpresentCursor() {
		cursorSpriteRenderer.enabled = false;
		childSpriteRenderer.enabled = false;
	}
}
