using UnityEngine;
using System.Collections;

public class DeckAnimate : MonoBehaviour {

	GameControlGUI gameControlGUI;

	SpriteRenderer sprite;
    public Sprite NORMALSPRITE;
    public Sprite SICKSPRITE;

	bool Animating;
	bool ShuffleAnimating;
	bool ErrorAnimating;
	float AnimateStartTime;
	Vector3 EndPosition;
	Vector3 originalPosition;
	GameObject cardUnderDeck;

	public void Start () {
		cardUnderDeck = GameObject.Find ("Card under deck");
		originalPosition = transform.localPosition;
		sprite = gameObject.GetComponent<SpriteRenderer>();

		GameObject tempGO = GameObject.FindGameObjectWithTag ("GameController");
		gameControlGUI = tempGO.GetComponent<GameControlGUI> ();
	}

	public void Update() {
		if(Animating) {
			if(Time.time > AnimateStartTime + .5f) {
				Animating = false;
				transform.localPosition = EndPosition;
				cardUnderDeck.transform.localPosition = Vector3.zero;
				Shuffle();
				return;
			}
			else {
				float time = Time.time - AnimateStartTime;
				transform.localPosition = Vector3.Lerp(transform.localPosition, EndPosition, time);
				cardUnderDeck.transform.position = Vector3.Lerp(cardUnderDeck.transform.position, transform.position, time);
			}
		}
		if(ShuffleAnimating) {
			if(Time.time > AnimateStartTime + 1f) {
				ShuffleAnimating = false;
				transform.localPosition = originalPosition;
				cardUnderDeck.transform.parent = gameObject.transform;
				cardUnderDeck.transform.localPosition = Vector3.zero;

                S.GameControlInst.gameObject.GetComponent<ShopControlGUI>().TurnOnNormalGUI();
				gameControlGUI.SetTooltip("");

				//VVV actually really important part. this is where the new level starts!
				if(SaveDataControl.UnlockedGods.Count == 7) S.GameControlInst.ReturnToGodChoiceMenu ();
				else S.GameControlInst.StartNewLevel();
				return;
			}
			else {
				transform.localPosition = new Vector3(Mathf.Abs(Mathf.Sin(Time.time*5)), transform.localPosition.y,0);
//				cardUnderDeck.transform.localPosition = new Vector3(Mathf.Abs(Mathf.Cos(Time.time*2)), 2,0);
			}	
		}
	}

	public void ShuffleMoveAnimate () {
		EndPosition = new Vector2 (0, 4f);
		Animating = true;
		AnimateStartTime = Time.time;
		//leads to Shuffle()
		GameObject[] handObjs = GameObject.FindGameObjectsWithTag ("Card");
		foreach(GameObject handObj in handObjs) {
			handObj.GetComponent<CardUI>().ShuffleMoveAnimate(transform);
		}
	}
	void Shuffle () {
		S.GameControlInst.ShuffleInHandAndDiscard();

		EndPosition = new Vector2(0, 4f);
		ShuffleAnimating = true;
		AnimateStartTime = Time.time;
		GameObject playBoard = GameObject.FindGameObjectWithTag("Play board");
		Transform parent = playBoard.GetComponent<Transform>();
		cardUnderDeck.transform.parent = parent;
	}

    #region ErrorAnimate methods
    public void ErrorAnimate() {
		if(!ErrorAnimating) {
			sprite = gameObject.GetComponent<SpriteRenderer> ();
			sprite.enabled = false;
			ErrorAnimating = true;
			Invoke ("errorAnimateOn", .05f);
			Invoke ("errorAnimateOff", .1f);
			Invoke ("errorAnimateOn", .15f);
			Invoke ("errorAnimateOff", .2f);
			Invoke ("errorAnimateOn", .25f);
			Invoke ("errorAnimateOff", .3f);
			Invoke ("errorAnimateOn", .35f);
			Invoke ("errorAnimateBoolOff", .36f);
		}
	}
	void errorAnimateOn () {
        sprite.enabled = true;
	}
	void errorAnimateOff () {
        sprite.enabled = false;
	}
	void errorAnimateBoolOff() {
		ErrorAnimating = false;
	}
    #endregion
 
    public void SetSprite() {
        bool hunger = hungerCheck();

        if (hunger)
            sprite.sprite = SICKSPRITE;
        else 
            sprite.sprite = NORMALSPRITE;
    }

    bool hungerCheck()
    {
        return (S.GameControlInst.HungerTurns > 0);
    }
}
