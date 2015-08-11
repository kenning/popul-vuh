﻿using UnityEngine;
using System.Collections;

public class ClickBlocker : MonoBehaviour {

	Vector3 cameraPosition;

	void Start() {
		MoveToSpot (-1);
	}

	public void MoveToSpot(int spot) {

		cameraPosition = Camera.main.transform.position;

		if(spot < -1 | spot > 3) {
			throw new System.Exception("Invalid spot!");
		}
		transform.position = new Vector3 (((float)spot - 1f) * 2.1f + cameraPosition.x, transform.position.y, 0);
	}
}
