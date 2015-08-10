﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class QControl {

	public enum QMethodType {Activate, FreeActivate, Option, Special, FreeSpecial, Discard};
	static Queue<Card> CardQ;
	static Queue<QMethodType> MethodQ;
	static GameControl gameControl;

	public static void Initialize () {
		CardQ = new Queue<Card> ();
		MethodQ = new Queue<QMethodType> ();
		gameControl = GameObject.FindGameObjectWithTag ("GameController").GetComponent<GameControl> ();
	}

	public static void AddToQ(Card card, QMethodType methodType) {
		CardQ.Enqueue (card);
		MethodQ.Enqueue (methodType);
	}

	public static void CheckQ() {

		if(CardQ.Count != MethodQ.Count) {
			Debug.Log("YOU FUCKED UP BIGTIME");
			return;
		}

		if(CardQ.Count == 0) {

			ClickControl clickControl = GameObject.FindGameObjectWithTag("GameController").GetComponent<ClickControl>();
			clickControl.AllowEveryInput();
			Debug.Log("Checked Q and allowed every input! This is where card effects terminate and EndTurnCheck() is called.");
			gameControl.Invoke ("AnimateCardsToCorrectPosition", .05f);
			gameControl.CheckDeckCount();
            gameControl.EndTurnCheck();
			return;
		}

		Card deQCard = CardQ.Dequeue ();
		QMethodType deQMethod = MethodQ.Dequeue ();

		switch(deQMethod) {
		case QMethodType.Activate:
			deQCard.Activate();
			break;
		case QMethodType.FreeActivate:
			if(deQCard.CardAction == Card.CardActionTypes.TargetGridSquare) {
				deQCard.FreeTargetSquare = true;
				deQCard.Activate();
			}
			else {
				deQCard.Activate(true);
			}
			break;
		case QMethodType.Option:
			deQCard.OptionQCall();
			break;
		case QMethodType.Special:
			deQCard.SpecialQCall();
			break;
		case QMethodType.FreeSpecial:
			deQCard.SpecialQCall();
			break;
		case QMethodType.Discard:
			deQCard.DiscardAnimate();
			break;
		}
	}

	public static bool QContains(Card card) {
		if (CardQ.Contains (card))
						return true;
		return false;
	}
}
