using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EventControl {

	static List<Card> TriggerList;
	//keyword == "Enemy Death"
	//keyword == "Punch"
	//keyword == "Burn"

	public static void NewLevelReset () {
		TriggerList = new List<Card> ();
	}

	//////////////////////
	/// Actual event calling methods 
	//////////////////////

	public static void EventCheck (string s) {
		for(int i = 0; i < TriggerList.Count; i++) {
			TriggerList[i].EventCall(s);
		}
	}


    public static void NewTurnReset()
    {
		for(int i = TriggerList.Count-1; i >= 0; i--) {
			if(TriggerList[i].TriggerResetsOnNewTurn) {
				TriggerList.RemoveAt(i);
            }
		}
    }

	//////////////////////
	/// utilities
	//////////////////////

    public static void RemoveFromLists(Card removedCard)
    {
        TriggerList.Remove(removedCard);
    }

    public static void AddToTriggerList(Card addedCard)
    {
        TriggerList.Add(addedCard);
    }
}
