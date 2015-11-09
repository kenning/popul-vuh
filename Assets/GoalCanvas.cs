using UnityEngine;
using System.Collections;

public class GoalCanvas : MonoBehaviour {
		
	public void SetInitialGoalInfo(Goal goal) {
		int godNumber;
		if(!SaveData.UnlockedGods.Contains(goal.God)) godNumber = 7;
		else godNumber = ShopControl.AllGods.IndexOf (goal.God);
		// do something with this number please

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

		//then set the canvas pictures to the pictures of the god
	}

	public void UpdateGoalInfo() {
		// updates and sets text for this goal. gets called when goals are triggered

	}

	public void ExpandGoalDisplay () {

	}

	public void ContractGoalDisplay () {

	}
}
