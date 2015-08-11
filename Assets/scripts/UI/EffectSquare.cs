using UnityEngine;
using System.Collections;

public class EffectSquare : MonoBehaviour {
	
//	int XCoor = 0;
//	int YCoor = 0;
	float creationTime;
	bool fadeOut;

	SpriteRenderer sprite;
	
	public void MoveToPointAndFadeOut(int x, int y){
		creationTime = Time.time;
		fadeOut = true;

		Vector3 placement = new Vector3 (x, y, 0);
//		XCoor = x;
//		YCoor = y;
		transform.position = placement;

		sprite = GetComponent<SpriteRenderer> ();
	}

	void Update() {

		if(fadeOut) {
			float fade = Time.time - creationTime;
			sprite.color = new Color(1f, 1f, 1f, 1-fade*4);
		}

		if(creationTime + .25f < Time.time) {
			Destroy(gameObject);
		}
	}
}
