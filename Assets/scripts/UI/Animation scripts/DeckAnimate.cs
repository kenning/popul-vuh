﻿using UnityEngine;
using System.Collections;

public class DeckAnimate : MonoBehaviour {

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
	GameObject anotherCardUnderDeck;

	public void Start () {
		cardUnderDeck = GameObject.Find ("Card under deck");
		anotherCardUnderDeck = GameObject.Find ("another card under deck");
		originalPosition = transform.localPosition;
		sprite = gameObject.GetComponent<SpriteRenderer>();
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
				S.GameControlGUIInst.SetTooltip("");

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
        // Makes deck sprites all reappear
        CancelInvoke();
        DisplayDeckSize(6);
        sprite.enabled = true;
        
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
            
            // If cards left in the deck, reset the deck sprite to off
            if (S.GameControlInst.Deck.Count < 1) {
        		Invoke ("errorAnimateOff", .4f);            
            }
    		Invoke ("errorAnimateBoolOff", .4f);            
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
    
    public void DisplayDeckSize(int decksize) {
        anotherCardUnderDeck.GetComponent<SpriteRenderer>().enabled = (decksize > 5);
        cardUnderDeck.GetComponent<SpriteRenderer>().enabled = decksize > 3;
        gameObject.GetComponent<SpriteRenderer>().enabled = decksize > 0;
    }

    bool hungerCheck()
    {
        return (S.GameControlInst.HungerTurns > 0);
    }
}
