using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class OptionControl : MonoBehaviour {

	Card returnToThisCard;

	public List<string> options;
	public bool optionYesNo = false;

	GUIStyleLibrary S.GUIStyleLibraryInst;
	
	void Awake () {
		useGUILayout = false;
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
		S.GameControlGUIInst.Dim (false);
	}

	void BaseOptionSet() {
		S.GameControlGUIInst.Dim (true);		
		S.ClickControlInst.DisallowEveryInput ();
		S.ClickControlInst.AllowInfoInput = true;
	}

	void OnGUI () {
		if(optionYesNo) {
			GUI.BeginGroup(new Rect(Screen.width*.2f, Screen.height*.3f, Screen.width*.6f, Screen.height*.4f), 
			               S.GUIStyleLibraryInst.OptionControlStyles.Box);
				if(GUI.Button(new Rect(Screen.width*.1f, Screen.height*.05f, Screen.width*4f, Screen.height*.1f), "Yes")) {
					returnToThisCard.OptionsCalledThis(true);
					S.GameControlGUIInst.Dim(false);
				}
				if(GUI.Button(new Rect(Screen.width*.1f, Screen.height*.25f, Screen.width*4f, Screen.height*.1f), "No")) {
					returnToThisCard.OptionsCalledThis(false);
					S.GameControlGUIInst.Dim(false);
				}
			GUI.EndGroup();
		}
		if(options.Count > 1) {
			//when i have internet, this needs to be a scrollview
			GUI.BeginGroup(new Rect(Screen.width*.2f, Screen.height*.3f, Screen.width*.6f, Screen.height*.4f), S.GUIStyleLibraryInst.OptionControlStyles.Box);
				for(int i = 0; i < options.Count; i++) {
				if(GUI.Button(new Rect(Screen.width*.1f, Screen.height*.1f*i, Screen.width*.4f, Screen.height*.1f), 
				              options[i], S.GUIStyleLibraryInst.OptionControlStyles.Button)){
						returnToThisCard.OptionsCalledThis(i);
						S.GameControlGUIInst.Dim(false);
					}
				}
			GUI.EndGroup();
		}
	}
}
