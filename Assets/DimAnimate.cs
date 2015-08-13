using UnityEngine;
using System.Collections;

public class DimAnimate : MonoBehaviour {

	SpriteRenderer dimSprite;
	float dimTime = 0f;
	float dimLength = .35f;
	public float dimPercent = 0f;
	public bool dimSetting = false;
	bool dimAnimating = false;

	void Start() {
		dimSprite = gameObject.GetComponent<SpriteRenderer> ();
	}

	public void Dim() {
		dimTime = Time.time;
		dimSetting = true;
		dimAnimating = true;
	}

	public void Undim() {
		dimTime = Time.time;
		dimSetting = false;
		dimAnimating = true;
	}

	void Update() {
		if(!dimAnimating) return;

		if (dimPercent > 99 && dimSetting == true) {
			dimPercent = 100;
			dimAnimating = false;
		}
		if (dimPercent < 0 && dimSetting == false) {
			dimPercent = 0;
			dimAnimating = false;
		}

		dimPercent = (Time.time - dimTime) / dimLength;
		if(dimSetting == false) dimPercent = 1 - dimPercent;

		Color newColor = new Color(1, 1, 1, dimPercent);

		dimSprite.color = newColor;
	}
}
