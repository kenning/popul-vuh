using UnityEngine;
using System.Collections;

public class ClickBlocker : MonoBehaviour {

	void Start() {
		MoveToSpot (-1);
	}

	public void MoveToSpot(int spot) {

		if(spot < -1 | spot > 3) {
			throw new System.Exception("Invalid spot!");
		}
		transform.localPosition = new Vector3 (((float)spot - 1f) * 2.2f, -1.7f, 0);
	}
}
