using UnityEngine;
using System.Collections;

public class TargetSquare : MonoBehaviour {

	public int XCoor = 0;
	public int YCoor = 0;
    
    void Start() {
        useGUILayout = false;
    }
    
	public void MoveToPoint(int x, int y){
		Vector3 placement = new Vector3 (x, y, 0);
		XCoor = x;
		YCoor = y;
		transform.position = placement;
	}
}
