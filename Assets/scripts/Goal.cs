using UnityEngine;
using System.Collections;

public class Goal  {

	public ShopControl.Gods God;
	public string GodString;
	public Sprite GodPicture;
	public Texture2D GodIcon;
	public Texture2D GodTexture;
	public string Description;
	public string MiniDescription;
	public int CurrentScore;
	public int HighScore = 0;
	public int[] GoalScore;
	public string DisplayScore;

	//only used in some goals
	public bool DidGoalThisTurnTracker = false;

	public bool HigherScoreIsGood;

	public Goal(string Name, string minidescription, string description, ShopControl.Gods god, int[] goalScore, bool higherScoreIsGood) {
		MiniDescription = minidescription;
		Description = description;
		God = god;
		GoalScore = goalScore;
		HigherScoreIsGood = higherScoreIsGood;
	}
	public Goal() { }

	public void MakeCheck() {

		if(MiniDescription == "Kill X enemies in one turn" 		| MiniDescription == "Protect against X attacks" |
		   MiniDescription == "Play X cards in one turn" 		| MiniDescription == "Play X paper cards in one turn" |
		   MiniDescription == "Play less than X cards total"	| MiniDescription == "Draw X cards in one turn" |
		   MiniDescription == "Draw X paper cards in one turn" 	| MiniDescription == "Touch the screen no more than than X times"|
		   MiniDescription == "Move X times in one turn"		| MiniDescription == "Deal X damage in one turn" | 
		   MiniDescription == "Punch X times"
		   ) {
			AddToScore(1);
		}
		else if(MiniDescription == "Kill enemies X turns in a row" ) {
			if(!DidGoalThisTurnTracker) {
				AddToScore(1);
			}
			DidGoalThisTurnTracker = true;
		}
		else if (MiniDescription == "Don't deal damage X turns in a row" | MiniDescription == "Don't deal damage or move X turns in a row"  |
		         MiniDescription == "Don't play a card X turns in a row" | MiniDescription == "Don't move X turns in a row") {
			DidGoalThisTurnTracker = true;
			ChangeScore(0);
		}
		else {
			switch(MiniDescription) {
			case "Discard pile has X cards in it":
				GameControl battleBoss = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameControl>();
				ChangeScore(battleBoss.Discard.Count);
				break;
			case "Discard pile has X cards in a row with the same God":
				GameControl bigBattleBoss = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameControl>();
				int inARow = 1;
				int hiScore = 0;
				ShopControl.Gods lastGod = ShopControl.Gods.none;
				for(int i = 0; i < bigBattleBoss.Discard.Count; i++) {
					Card c = bigBattleBoss.Discard[i].GetComponent<Card>();
					if(lastGod != c.God) {
						inARow = 1;
					}
					else {
						inARow++;
					}

					lastGod = c.God;

					if(inARow > hiScore) {
						hiScore = inARow;
					}

					ChangeScore(hiScore);
				}
				break;
			}
		}
		SetDisplayScore ();
	}

	public void NewTurnCheck() {
		if(MiniDescription == "Protect against X attacks" 		| MiniDescription == "Touch the screen no more than than X times"	|
		   MiniDescription == "Play less than X cards total"	| MiniDescription == "Discard pile has X cards in a row with the same God" | 
		   MiniDescription == "Punch X times") {
			return;
		}
		else if(MiniDescription == "Kill X enemies in one turn" | MiniDescription == "Move X times in one turn"			|
		   MiniDescription == "Play X cards in one turn" 		| MiniDescription == "Play X paper cards in one turn" 	|
		   MiniDescription == "Draw X cards in one turn" 		| MiniDescription == "Draw X paper cards in one turn" 	|
		   MiniDescription == "Deal X damage in one turn"		) {
			ChangeScore(0);
		}
		else if(MiniDescription == "Kill enemies X turns in a row" ) {
			if(DidGoalThisTurnTracker == false) {
				ChangeScore(0);
			}
			DidGoalThisTurnTracker = false;
		}
		else if (MiniDescription == "Don't deal damage X turns in a row" | MiniDescription == "Don't deal damage or move X turns in a row" | 
		         MiniDescription == "Don't play a card X turns in a row" | MiniDescription == "Don't move X turns in a row") {
			if(DidGoalThisTurnTracker == false) {
				AddToScore(1);
			}
			DidGoalThisTurnTracker = false;
		}
	}
	
	public string TheScore() {
		string nextScore = "";
		string tempString = CurrentScore.ToString ();
		//for(int i = 0; i < GoalScore.Length; i++) {
		for(int i = 0; i < GoalScore.Length; i++) {

			if(HigherScoreIsGood) {
				if(CurrentScore >= GoalScore[i]) continue;
				else {
					nextScore += " => " + GoalScore[i].ToString();
					break;
				}
			}
			else {
				if(CurrentScore <= GoalScore[i]) continue;
				else {
					nextScore += " => " + GoalScore[i].ToString();
					break;
				}

			}
		}
		tempString += nextScore;
		return tempString;
	}

	//outside things call this method when they change the score
	void AddToScore(int ScoreChange) {
		CurrentScore += ScoreChange;
		if(HigherScoreIsGood) {
			if(HighScore < CurrentScore) 
					HighScore = CurrentScore;
		SetDisplayScore();
		}
		else {
			if(HighScore > CurrentScore)
					HighScore = CurrentScore;
		SetDisplayScore();
		}
    }
	void ChangeScore(int NewScore) {
		CurrentScore = NewScore;
		if(HigherScoreIsGood) {
			if(HighScore < CurrentScore) 
					HighScore = CurrentScore;
			SetDisplayScore();
		}
		else {
			if(HighScore > CurrentScore)
					HighScore = CurrentScore;
			SetDisplayScore();
		}
	}
    public void ResetTheScore() {
		ChangeScore(0);
		SetDisplayScore();
    }

	public void SetDisplayScore() {
		DisplayScore = TheScore();
	}

	public void SetGodString() {
		if(SaveData.UnlockedGods.Contains(God)) {
			GodString = God.ToString ();
		} else {
			GodString = "A God";
		}
	}
}
