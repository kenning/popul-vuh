using UnityEngine;
using System.Collections;

public class DragControl : MonoBehaviour {
    public bool DraggingGameboard = false;
    public bool DraggingHand = false;
	Vector3 dragOrigin;
    Vector3 cameraOrigin;
	GameObject arrows;
	Texture2D SideArrows;
	//gameboard limits
	float leftLimit = -1.6f;
	float rightLimit = 1.6f;
	float topLimit = -.6f;
	float bottomLimit = -2f;
    Card cardScriptClickedOn;
    void Start() {
        arrows =  GameObject.FindGameObjectWithTag("Camera Arrow");
		SideArrows = (Texture2D)Resources.Load ("sprites/ui/side arrows");
    }
	public void GameBoardDrag()
    {
        dragOrigin = Camera.main.ScreenToViewportPoint(Input.mousePosition);
		S.GameControlGUIInst.Dim (false);
        S.DragControlInst.DraggingGameboard = true;
        cameraOrigin = Camera.main.transform.position;
        Vector3 arrowsOrigin = Camera.main.ScreenToViewportPoint(Input.mousePosition);
        arrows.GetComponent<SpriteRenderer>().enabled = true;
    }
	public void HandDrag(Card clickedCard, Vector3 clickOrigin) {
        cardScriptClickedOn = clickedCard;
		S.DragControlInst.DraggingHand = true;
		S.GameControlGUIInst.Dim(false);
		Cursor.SetCursor(SideArrows, Vector2.zero, CursorMode.Auto);
	}
    public void StopDragging() {
        DraggingGameboard = false;
        DraggingHand = false;
        arrows.GetComponent<SpriteRenderer>().enabled = false;
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
    }
    
    void Update() {
        if(DraggingHand){
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
			} else {
				S.DragControlInst.DraggingHand = false;
				Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
			}
			return;
		} else if (DraggingGameboard){
			if(Input.GetMouseButton(0)){
                arrows.transform.position = Input.mousePosition;
				Vector3 pos = Camera.main.transform.position;
				Vector3 move = Camera.main.ScreenToViewportPoint (Input.mousePosition);
				// stops at the end of the screen
                float multiplierx = 8.5f;
                float multipliery = 15f;
				if(pos.x < leftLimit && move.x > 0) {
					multiplierx = 0;
					Camera.main.transform.position = new Vector3(
                        leftLimit -.05f,
                        Camera.main.transform.position.y,
                        0
                    );
				}
				if(pos.y < bottomLimit && move.y > 0) {
					multipliery = 0;
					Camera.main.transform.position = new Vector3(
                        Camera.main.transform.position.x,
                        topLimit + .05f,
                        0
                    );
				}
				if(pos.x > rightLimit && move.x < 0) {
					multiplierx = 0;
					Camera.main.transform.position = new Vector3(
                        rightLimit + .05f,
                        Camera.main.transform.position.y,
                        0
                    );
				}
				if(pos.y > topLimit && (dragOrigin.y - move.y) < 0) {
					multipliery = 0;
					Camera.main.transform.position = new Vector3(
                        Camera.main.transform.position.x,
                        bottomLimit - .05f,
                        0
                    );
				}
				Camera.main.transform.position = cameraOrigin + 
                    new Vector3((dragOrigin.x - move.x) * multiplierx, 
                                (dragOrigin.y - move.y) * multipliery, 0);
				return;
			} else {
				S.DragControlInst.DraggingGameboard = false;
				arrows.GetComponent<SpriteRenderer>().enabled = false;
			}
			return;
		}
    }
}