using UnityEngine;
using System.Collections;

public class GridCursorControlGUI : MonoBehaviour {

	public SpriteRenderer cursorSpriteRenderer;
	public SpriteRenderer childSpriteRenderer;
	public SpriteRenderer iconSpriteRenderer;
	public Sprite NONESPRITE;
	public Sprite MOVESPRITE;
	public Sprite PUNCHSPRITE;
	public Sprite TARGETSQUARESPRITE;
	public Sprite INFOSPRITE;
	public Sprite MOVEICONSPRITE;
	public Sprite PUNCHICONSPRITE;
	public Sprite TARGETSQUAREICONSPRITE;
	public Sprite INFOICONSPRITE;

	void Start() {
		cursorSpriteRenderer = gameObject.GetComponent<SpriteRenderer> ();
		SpriteRenderer[] sprites = gameObject.GetComponentsInChildren<SpriteRenderer> ();
		foreach(SpriteRenderer sprite in sprites) {
			if(sprite.gameObject.name == "Grid Cursor Box") {
				childSpriteRenderer = sprite;
			}
			if(sprite.gameObject.name == "Grid Cursor Icon") {
				iconSpriteRenderer = sprite;
			}
		}
	}

	public void PresentCursor(GridCursorControl.CursorActions action, int x, int y) {
		transform.position = new Vector3 (x, y, 0);
		cursorSpriteRenderer.enabled = true;
		childSpriteRenderer.enabled = true;
		iconSpriteRenderer.enabled = true;
		switch (action) {
		case GridCursorControl.CursorActions.StairMove:
			cursorSpriteRenderer.sprite = MOVESPRITE;
			iconSpriteRenderer.sprite = MOVEICONSPRITE;
			break;
		case GridCursorControl.CursorActions.Move:
			cursorSpriteRenderer.sprite = MOVESPRITE;
			iconSpriteRenderer.sprite = MOVEICONSPRITE;
			break;
		case GridCursorControl.CursorActions.Poke:
			cursorSpriteRenderer.sprite = MOVESPRITE;
			iconSpriteRenderer.sprite = MOVEICONSPRITE;
			break;
		case GridCursorControl.CursorActions.Punch:
			cursorSpriteRenderer.sprite = PUNCHSPRITE;
			iconSpriteRenderer.sprite = PUNCHICONSPRITE;
			break;
		case GridCursorControl.CursorActions.TargetSquare:
			cursorSpriteRenderer.sprite = TARGETSQUARESPRITE;
			iconSpriteRenderer.sprite = TARGETSQUAREICONSPRITE;
			break;
		case GridCursorControl.CursorActions.EnemyInfo:
			cursorSpriteRenderer.sprite = INFOSPRITE;
			iconSpriteRenderer.sprite = INFOICONSPRITE;
			break;
		case GridCursorControl.CursorActions.ObstacleInfo:
			cursorSpriteRenderer.sprite = INFOSPRITE;
			iconSpriteRenderer.sprite = INFOICONSPRITE;
			break;
		case GridCursorControl.CursorActions.PlayerInfo:
			cursorSpriteRenderer.sprite = INFOSPRITE;
			iconSpriteRenderer.sprite = INFOICONSPRITE;
			break;
		case GridCursorControl.CursorActions.None:
			cursorSpriteRenderer.sprite = NONESPRITE;
			iconSpriteRenderer.sprite = null;
			break;
		}
	}

	public void UnpresentCursor() {
		cursorSpriteRenderer.enabled = false;
		childSpriteRenderer.enabled = false;
		iconSpriteRenderer.enabled = false;
	}
}
