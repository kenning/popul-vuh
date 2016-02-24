using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EventControl : MonoBehaviour {

	static bool initialized = false;
	static List<Card> TriggerList;
	static GameControl gameControl = null;
	//keyword == "Enemy Death"
	//keyword == "Punch"
	//keyword == "Burn"

	static void Initialize() {
		if (initialized) return;
		initialized = true;
		gameControl = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameControl>();
	}

	public static void NewLevelReset () {
		TriggerList = new List<Card> ();
		StateSavingControl.ResetTriggerList();
//		StateSavingControl.Save();
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

    public static void RemoveFromLists(Card removedCard) {
        TriggerList.Remove(removedCard);
		StateSavingControl.RemoveFromTriggerList(removedCard.CardName);
		StateSavingControl.Save();
    }

    public static void AddToTriggerList(Card addedCard) {
        TriggerList.Add(addedCard);
		StateSavingControl.AddToTriggerList(addedCard.CardName);
		StateSavingControl.Save();
    }

	public static void LoadTriggerListState(List<string> stringList) {
        foreach (string s in stringList) {
        	foreach (GameObject tempGO in gameControl.Discard) {
				Debug.Log("I should eventually test if these event things actually persist because i'll never run into this in the wild");
				Debug.Log("Also this is pretty shitty but if the event trigger list is [SpymasterStyle, SpymasterStyle] it will trigger" + 
					"the first card twice instead of triggering each individually. Who cares though seriously");
				if (stringList.Contains(tempGO.name)) {
					TriggerList.Add(tempGO.GetComponent<Card>());					
				}
			}
		}
	}
}
