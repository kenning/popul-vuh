using UnityEngine;
using System.Collections;

public class DimAnimate : MonoBehaviour {

	SpriteRenderer dimSprite;
	bool dimLock = false;
	float dimSpeed = .035f;
	public float dimPercent = 0f;
	public bool dimSetting = false;
	bool dimAnimating = false;

	void Start() {
		dimSprite = gameObject.GetComponent<SpriteRenderer> ();
	}

	public void Dim() {
		if(dimLock) return;
		dimSetting = true;
		dimAnimating = true;
	}

	public void Undim() {
		if(dimLock) return;
		dimSetting = false;
		dimAnimating = true;
	}

	void Update() {
		if(!dimAnimating) return;

		if (dimPercent > 1 && dimSetting == true) {
			dimPercent = 1;
			dimAnimating = false;
		}
		if (dimPercent < 0 && dimSetting == false) {
			dimPercent = 0;
			dimAnimating = false;
		}

		if(dimSetting == true) dimPercent += dimSpeed;
		else dimPercent -= dimSpeed;

		Color newColor = new Color(1, 1, 1, dimPercent);

		dimSprite.color = newColor;
	}

	public void ForceDim() {
		dimLock = false;
		Dim ();
		dimLock = true;
	}

	public void ForceUndim() {
		dimLock = false;
		Undim ();
		dimLock = true;
	}

	public void UnlockDim() {
		dimLock = false;
	}
}
