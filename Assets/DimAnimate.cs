using UnityEngine;
using System.Collections;

public class DimAnimate : MonoBehaviour {

	SpriteRenderer dimSprite;
	float dimTime = 0f;
	float dimLength = 1f;
	bool dimSetting;

	void Start() {
		dimSprite = gameObject.GetComponent<SpriteRenderer> ();
	}

	public void Dim() {

	}

	public void Undim() {

	}

	void Update() {

	}
}
