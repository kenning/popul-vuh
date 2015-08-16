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

	public Goal(string Name, string minidescription, string description, 
	            ShopControl.Gods god, int[] goalScore, bool higherScoreIsGood) {
		MiniDescription = minidescription;
		Description = description;
		God = god;
		GoalScore = goalScore;
		HigherScoreIsGood = higherScoreIsGood;
	}
	public Goal() { }


	/// <summary>
	/// Universal method for interacting with a goal. Functions like 
	/// an event system; goals take different actions when they hear an event.
	/// </summary>
	public void MakeCheck() {
		// Goal increments by 1 when checked
		if(  MiniDescription == "Kill X enemies in one turn" 		
		   | MiniDescription == "Protect against X attacks" 
		   | MiniDescription == "Play X cards in one turn" 		
		   | MiniDescription == "Play X paper cards in one turn" 
		   | MiniDescription == "Play less than X cards total"	
		   | MiniDescription == "Draw X cards in one turn" 
		   | MiniDescription == "Draw X paper cards in one turn" 	
		   | MiniDescription == "Touch the screen no more than than X times"
		   | MiniDescription == "Move X times in one turn"		
		   | MiniDescription == "Deal X damage in one turn" 
		   | MiniDescription == "Punch X times" ) {
			AddToScore(1);
		}
		// Goal increments by 1 and remembers it was done when checked
		else if(MiniDescription == "Kill enemies X turns in a row" ) {
			if(!DidGoalThisTurnTracker) {
				AddToScore(1);
			}
			DidGoalThisTurnTracker = true;
		}
		// Goal resets to 0 when checked
		else if (MiniDescription == "Don't deal damage X turns in a row" | 
		         MiniDescription == "Don't deal damage or move X turns in a row"  |
		         MiniDescription == "Don't play a card X turns in a row" | 
		         MiniDescription == "Don't move X turns in a row") {
			DidGoalThisTurnTracker = true;
			ChangeScore(0);
		}
		// Special case
		else if (MiniDescription ==  "Discard pile has X cards in it") {
				GameControl gameControl = 
					GameObject.FindGameObjectWithTag("GameController").GetComponent<GameControl>();
				ChangeScore(gameControl.Discard.Count);
		}
		else if	(MiniDescription == "Discard pile has X cards in a row with the same God") {
			GameControl gameControl = 
				GameObject.FindGameObjectWithTag("GameController").GetComponent<GameControl>();
			int inARow = 1;
			int hiScore = 0;
			ShopControl.Gods lastGod = ShopControl.Gods.none;
			for(int i = 0; i < gameControl.Discard.Count; i++) {
				Card c = gameControl.Discard[i].GetComponent<Card>();
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
		}
		SetDisplayScore ();
	}

	/// <summary>
	/// Called on each goal on every new turn.
	/// May or may not modify goal score depending on what the goal is. 
	/// </summary>
	public void NewTurnCheck() {
		// A new turn doesn't affect this goal
		if(  MiniDescription == "Protect against X attacks" 		
		   | MiniDescription == "Touch the screen no more than than X times"	
		   | MiniDescription == "Play less than X cards total"	
		   | MiniDescription == "Discard pile has X cards in a row with the same God" | 
		     MiniDescription == "Punch X times") {
			return;
		}
		// This goal is reset at the beginning of a turn
		else if(MiniDescription == "Kill X enemies in one turn" 
	        | MiniDescription == "Move X times in one turn"			
	        | MiniDescription == "Play X cards in one turn" 		
	        | MiniDescription == "Play X paper cards in one turn" 	
		    | MiniDescription == "Draw X cards in one turn" 		
		 	| MiniDescription == "Draw X paper cards in one turn" 
		   	| MiniDescription == "Deal X damage in one turn"		) {
			ChangeScore(0);
		}
		// This goal wants the user to take an action multiple turns in a row
		else if(MiniDescription == "Kill enemies X turns in a row" ) {
			if(DidGoalThisTurnTracker == false) {
				ChangeScore(0);
			}
			DidGoalThisTurnTracker = false;
		}
		// This goal wants the user to avoid an action multiple turns in a row
		else if (  MiniDescription == "Don't deal damage X turns in a row" 
		         | MiniDescription == "Don't deal damage or move X turns in a row" 
		         | MiniDescription == "Don't play a card X turns in a row" 
		         | MiniDescription == "Don't move X turns in a row") {
			if(DidGoalThisTurnTracker == false) {
				AddToScore(1);
			}
			DidGoalThisTurnTracker = false;
		}
	}

	/// <returns>Current score.</returns>
	public string TheScore() {
		string nextScore = "";
		string tempString = CurrentScore.ToString ();
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

	#region Private methods for changing the score and score display
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
			GodString = "An unknown God";
		}
	}
	#endregion
}
