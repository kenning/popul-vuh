using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ShopAwardCanvas : MonoBehaviour {

	bool initialized = false;

	Image background;
	Text description;

	public ShopAndGoalParentCanvas SHOPANDGOALPARENTCANVAS;

	void Initialize () {
		if(initialized) return;
		initialized = true;

		background = gameObject.GetComponentInChildren<Image> ();
		description = gameObject.GetComponentInChildren<Text> ();
	}

	//Here's a stupid method for setting one grade/whatever notification
	public void SetGradeInfo (Goal goal, bool highScoreNotification) {

		Initialize();

		gameObject.SetActive(true);

		string[] awards = {
			"You didn't get an award for this goal.", 
			"Getting bronze on this goal just made you $1!", 
			"Getting silver on this goal just made you $2!", 
			"Getting gold on this goal just made you $3!", 
		};
		string grade = "You didn't get an award for this goal.";

		if(goal.HigherScoreIsGood) {
			if(goal.HighScore >= goal.GoalScore[2]) grade = awards[3];
			else if (goal.HighScore >= goal.GoalScore[1]) grade = awards[2];
			else if(goal.HighScore >= goal.GoalScore[0]) grade = awards[1];
		} else {
			if(goal.HighScore <= goal.GoalScore[2]) grade = awards[3];
			else if(goal.HighScore <= goal.GoalScore[1]) grade = awards[2];
			else if(goal.HighScore <= goal.GoalScore[0]) grade = awards[1];
		}

		int gradeQuality = 0;

		if (grade == awards [3]) gradeQuality = 3;
		else if (grade == awards [2]) gradeQuality = 2;
		else if (grade == awards [1]) gradeQuality = 1;

		if (highScoreNotification) {
			string scoreText = "New highest score: ";
			if(!goal.HigherScoreIsGood) scoreText = "New lowest score: ";
			scoreText += goal.CurrentScore.ToString();
			scoreText += "\n\n";
			grade = scoreText + grade;

			Invoke("TurnOff", 2.0f);
			//TODO interact with SaveDataControl or something here when i implement high scores.
		}

		description.text = grade;
		Debug.Log(gradeQuality);
		background.sprite = SHOPANDGOALPARENTCANVAS.GRADEBACKGROUNDS[gradeQuality];
	}

	public void TurnOff() {
		gameObject.SetActive(false);
	}
}
