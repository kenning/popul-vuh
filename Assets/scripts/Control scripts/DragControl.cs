using UnityEngine;
using System.Collections;

public class DragControl : MonoBehaviour {
    public bool DraggingGameboard = false;
    public bool DraggingHand = false;
	Vector3 dragOrigin;
    GameObject handObj;
	Texture2D SideArrows;
	Texture2D CompassArrows;
	//gameboard limits
	float leftLimit = -1.6f;
	float rightLimit = 1.6f;
	float topLimit = -.6f;
	float bottomLimit = -2f;
    float multiplierx = 8.5f;
    float multipliery = 15f;
    void Start() {
        useGUILayout = false;
		SideArrows = (Texture2D)Resources.Load ("sprites/ui/side arrows");
		CompassArrows = (Texture2D)Resources.Load ("sprites/ui/arrows");
        handObj = GameObject.Find("Hand");
    }
	public void GameBoardDrag() {
        dragOrigin = Camera.main.ScreenToViewportPoint(Input.mousePosition);

		S.GameControlGUIInst.Dim (false);
        S.DragControlInst.DraggingGameboard = true;
        Cursor.SetCursor(CompassArrows, new Vector2(CompassArrows.width/2, CompassArrows.height/2), CursorMode.Auto);
    }
	public void HandDrag(Card clickedCard, Vector3 clickOrigin) {
        dragOrigin = Camera.main.ScreenToViewportPoint(Input.mousePosition);
		S.DragControlInst.DraggingHand = true;
		S.GameControlGUIInst.Dim(false);
        // this should really be set to false already...
		Cursor.SetCursor(SideArrows, new Vector2(SideArrows.width/2, SideArrows.height/2), CursorMode.Auto);
        S.GridCursorControlInst.UnpresentCursor();
	}
    public void StopDragging() {
        DraggingGameboard = false;
        DraggingHand = false;
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
    }
    
    void Update() {
        if(DraggingHand){
			if(Input.GetMouseButton(0)){
				if(S.GameControlInst.Hand.Count < 5) { 
					handObj.transform.localPosition = new Vector3(((3) * -1.48f) + 3.7f, 0, 0);
					return;
				}
				Vector3 pos = Camera.main.ScreenToViewportPoint(Input.mousePosition)  - dragOrigin;

				if(handObj.transform.localPosition.x >= -.75f && pos.x > 0) {
					handObj.transform.localPosition = new Vector3(-.75f, 0, 0f);
					return;
				} else if((handObj.transform.localPosition.x <= ((S.GameControlInst.Hand.Count) * -1.6f) + 5.5f) &&  pos.x < 0) {
					//this is for after the exact position has gotten nailed down, purpose is to lock it to the edge. 
					handObj.transform.localPosition = new Vector3(((S.GameControlInst.Hand.Count) * -1.6f) + 5.4f, 0, 0);
				} else {
					handObj.transform.Translate(new Vector3(pos.x * multiplierx, 0, 0));
                    dragOrigin = Camera.main.ScreenToViewportPoint(Input.mousePosition);
					return;
				}
			} else {
				S.DragControlInst.DraggingHand = false;
				Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
			}
			return;
		} else if (DraggingGameboard){
			if(Input.GetMouseButton(0)){
				Vector3 camPos = Camera.main.transform.position;
				Vector3 move = Camera.main.ScreenToViewportPoint (Input.mousePosition);
				// stops at the end of the screen
                float finalx = 0;
                float finaly = 0;
                float buffer = 0.2f;

                float movex = dragOrigin.x - move.x;
                float movey = dragOrigin.y - move.y;
                
                if (camPos.x < leftLimit + buffer && movex < 0) {
                    finalx = leftLimit;
                } else if (camPos.x > rightLimit - buffer && movex > 0) {
                    finalx = rightLimit;                    
                } else {
                    finalx = camPos.x + movex * multiplierx;
                }
                
                if (camPos.y < bottomLimit + buffer && movey < 0) {
                    finaly = bottomLimit;
                } else if (camPos.y > topLimit - buffer && movey > 0) {
                    finaly = topLimit;                    
                } else {
                    finaly = camPos.y + movey * multipliery;
                }

                Camera.main.transform.position = new Vector3(finalx, finaly, -1);

                dragOrigin = Camera.main.ScreenToViewportPoint(Input.mousePosition);
                
				return;
			} else {
				StopDragging();
			}
			return;
		}
    }
}