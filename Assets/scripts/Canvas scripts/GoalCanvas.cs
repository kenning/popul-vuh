using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GoalCanvas : MonoBehaviour {

	bool initialized = false;

	Goal goal = null;

	GameObject clickedBox;
	GameObject unclickedBox;
	Image godIcon;
	Text score;
	Text unclickedGoalDescription;
	Text clickedGoalDescription;
	Text clickedHighScore;
	Text clickedScoreList;

	void Initialize () {
		if(initialized) return;
		initialized = true;

		Image[] images = gameObject.GetComponentsInChildren<Image> ();
		foreach(Image img in images) {
			if(img.gameObject.name == "Goal god icon") godIcon = img;
		}
		Text[] texts = gameObject.GetComponentsInChildren<Text> ();
		foreach (Text text in texts) {
			if(text.gameObject.name == "Unclicked goal description") unclickedGoalDescription = text;
			if(text.gameObject.name == "Clicked goal description") clickedGoalDescription = text;
			if(text.gameObject.name == "Goal high score text") clickedHighScore = text;
			if(text.gameObject.name == "Goal score list text") clickedScoreList = text;
			if(text.gameObject.name == "Goal score") score = text;
		}
		unclickedBox = gameObject.transform.FindChild ("Unclicked goal info box").gameObject;
		clickedBox = gameObject.transform.FindChild ("Clicked goal info box").gameObject;
	}
		
	public void SetInitialGoalInfo(Goal thisGoal) {
		Initialize();

		goal = thisGoal;

		int godNumber;
		if(!SaveDataControl.UnlockedGods.Contains(goal.God)) godNumber = 7;
		else godNumber = ShopControl.AllGods.IndexOf (goal.God);
		gameObject.SetActive(true);
		godIcon.sprite = S.ShopControlGUIInst.SpriteGodIcons [godNumber];

		unclickedGoalDescription.text = goal.MiniDescription;
		clickedGoalDescription.text = goal.God.ToString() + goal.Description;

		ContractGoalDisplay();

		UpdateGoalInfo ();
	}

	public void UpdateGoalInfo() {
		string tempString = "";
		for(int j = 0; j < goal.GoalScore.Length; j++){
			
			if(goal.HigherScoreIsGood) {
				if(goal.HighScore >= goal.GoalScore[j]) tempString += "X " + goal.GoalScore[j].ToString();
				else tempString += "  " + goal.GoalScore[j].ToString();
				if(j+1 != goal.GoalScore.Length) tempString += "\n";
			}
			else {
				if(goal.HighScore <= goal.GoalScore[j]) tempString += "X " + goal.GoalScore[j].ToString();
				else tempString += "  " + goal.GoalScore[j].ToString();
				if(j+1 != goal.GoalScore.Length) tempString += "\n";
			}
		}

		clickedScoreList.text = tempString;

		clickedHighScore.text = "High score:\n" + goal.HighScore.ToString();

		score.text = goal.TheScore ();
	}

	public void ExpandGoalDisplay () {
		clickedBox.SetActive (true);
		unclickedBox.SetActive (false);
	}

	public void ContractGoalDisplay () {
		clickedBox.SetActive (false);
		unclickedBox.SetActive (true);
	}
}
