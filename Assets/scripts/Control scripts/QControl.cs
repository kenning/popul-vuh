﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class QControl {

	public enum QMethodType {Activate, FreeActivate, Option, Special, FreeSpecial, Discard};
	static Queue<Card> CardQ;
	static Queue<QMethodType> MethodQ;

	public static void Initialize () {
		CardQ = new Queue<Card> ();
		MethodQ = new Queue<QMethodType> ();
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
			S.ClickControlInst.AllowEveryInput();
			Debug.Log("Checked Q and allowed every input! This is where card effects terminate and EndTurnCheck() is called.");
			S.GameControlGUIInst.AnimateCardsToCorrectPositionInSeconds(.15f);
			S.GameControlInst.CheckDeckCount();

			// Checks if the turn is over, and if it is, takes the enemy's turn. 
			if (!S.GameControlInst.EndTurnCheck()) {
				// The player's turn is not over yet
				StateSavingControl.Save();
			}
			return;
		}

		Card deQCard = CardQ.Dequeue ();
		QMethodType deQMethod = MethodQ.Dequeue ();

		switch(deQMethod) {
		case QMethodType.Activate:
			deQCard.Activate(false);
			break;
		case QMethodType.FreeActivate:
			if(deQCard.CardAction == Card.CardActionTypes.TargetGridSquare) {
				deQCard.FreeTargetSquare = true;
				deQCard.Activate(true);
			}
			else if(deQCard.CardAction == Card.CardActionTypes.Armor) {
				deQCard.Activate(true);
			} else {
				Debug.Log ("what is happening here?");
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
			deQCard.Discard();
			break;
		}
	}

	public static bool QContains(Card card) {
		if (CardQ.Contains (card))
						return true;
		return false;
	}
}
