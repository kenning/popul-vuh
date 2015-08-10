using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class OptionControl : MonoBehaviour {

	Card returnToThisCard;
	ClickControl clickControl;
	GameControlGUI GameControlGUI;

	public List<string> options;
	public bool optionYesNo = false;

	GUIStyleLibrary styleLibrary;
	
	void Awake () {
		clickControl = gameObject.GetComponent<ClickControl> ();
		styleLibrary = gameObject.GetComponent<GUIStyleLibrary> ();
		GameControlGUI = gameObject.GetComponent<GameControlGUI> ();
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
		GameControlGUI.Dim (false);
	}

	void BaseOptionSet() {
		GameControlGUI.Dim (true);		
		clickControl.DisallowEveryInput ();
		clickControl.AllowInfoInput = true;
	}

	void OnGUI () {
		if(optionYesNo) {
			GUI.BeginGroup(new Rect(Screen.width*.2f, Screen.height*.3f, Screen.width*.6f, Screen.height*.4f), 
			               styleLibrary.OptionControlStyles.Box);
				if(GUI.Button(new Rect(Screen.width*.1f, Screen.height*.05f, Screen.width*4f, Screen.height*.1f), "Yes")) {
					returnToThisCard.OptionsCalledThis(true);
					GameControlGUI.Dim(false);
				}
				if(GUI.Button(new Rect(Screen.width*.1f, Screen.height*.25f, Screen.width*4f, Screen.height*.1f), "No")) {
					returnToThisCard.OptionsCalledThis(false);
					GameControlGUI.Dim(false);
				}
			GUI.EndGroup();
		}
		if(options.Count > 1) {
			//when i have internet, this needs to be a scrollview
			GUI.BeginGroup(new Rect(Screen.width*.2f, Screen.height*.3f, Screen.width*.6f, Screen.height*.4f), styleLibrary.OptionControlStyles.Box);
				for(int i = 0; i < options.Count; i++) {
				if(GUI.Button(new Rect(Screen.width*.1f, Screen.height*.1f*i, Screen.width*.4f, Screen.height*.1f), 
				              options[i], styleLibrary.OptionControlStyles.Button)){
						returnToThisCard.OptionsCalledThis(i);
						GameControlGUI.Dim(false);
					}
				}
			GUI.EndGroup();
		}
	}
}
