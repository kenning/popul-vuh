using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class OptionControl : MonoBehaviour {

	Card returnToThisCard;
	GameControl battleBoss;
	ClickControl clickBoss;
	GameControlUI gameControlUI;


	public List<string> options;
	public bool optionYesNo = false;

	GUISkin gooeyskin;
	
	void Awake () {
		GameObject tempGO = GameObject.FindGameObjectWithTag ("GameController");
		battleBoss = tempGO.GetComponent<GameControl> ();
		clickBoss = tempGO.GetComponent<ClickControl> ();
		gooeyskin = (GUISkin)Resources.Load ("GUISkins/options guiskin");
	}

	public void SetYesNoOption (Card ReturnCard) {
		returnToThisCard = ReturnCard;
		optionYesNo = true;

		BaseOptionSet ();
	}

	public void SetOptions (string[] OptionArray, Card ReturnCard) {
		returnToThisCard = ReturnCard;
		options = new List<string> ();
		foreach(string s in OptionArray) {
			options.Add(s);
		}

		BaseOptionSet ();
	}

	public void TurnOffOptions () {
		options = new List<string> ();
		optionYesNo = false;
		gameControlUI.Dim (false);
	}

	void BaseOptionSet() {
		gameControlUI.Dim (true);		
		clickBoss.DisallowEveryInput ();
		clickBoss.AllowInfoInput = true;
	}

	void OnGUI () {
		if(optionYesNo) {
			GUI.BeginGroup(new Rect(Screen.width*.2f, Screen.height*.3f, Screen.width*.6f, Screen.height*.4f), gooeyskin.box);
				if(GUI.Button(new Rect(Screen.width*.1f, Screen.height*.05f, Screen.width*4f, Screen.height*.1f), "Yes")) {
					returnToThisCard.OptionsCalledThis(true);
					gameControlUI.Dim(false);
				}
				if(GUI.Button(new Rect(Screen.width*.1f, Screen.height*.25f, Screen.width*4f, Screen.height*.1f), "No")) {
					returnToThisCard.OptionsCalledThis(false);
					gameControlUI.Dim(false);
				}
			GUI.EndGroup();
		}
		if(options.Count > 1) {
			//when i have internet, this needs to be a scrollview
			GUI.BeginGroup(new Rect(Screen.width*.2f, Screen.height*.3f, Screen.width*.6f, Screen.height*.4f), gooeyskin.box);
				for(int i = 0; i < options.Count; i++) {
					if(GUI.Button(new Rect(Screen.width*.1f, Screen.height*.1f*i, Screen.width*.4f, Screen.height*.1f), options[i], gooeyskin.button)){
						returnToThisCard.OptionsCalledThis(i);
						gameControlUI.Dim(false);
					}
				}
			GUI.EndGroup();
		}
	}
}
