using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class ShopGridCanvas : MonoBehaviour {

	bool initialized = false;

	ShopAndGoalParentCanvas shopAndGoalParentCanvas;
	ShopControlGUI shopControlGUI;
	GameControl gameControl;
	List<LibraryCard> libraryCards;
	

	//OK basically this will have

	//This is a list of the card scripts in the grid
	public List<ShopGridCardCanvas> COLUMN1;
	public List<ShopGridCardCanvas> COLUMN2;
	public List<ShopGridCardCanvas> COLUMN3;
	List<List<ShopGridCardCanvas>> cardGrid;

	void Initialize() {
		if(initialized) return;
		initialized = true;

		cardGrid = new List<List<ShopGridCardCanvas>> {COLUMN1, COLUMN2, COLUMN3};
	}

	//Here's a stupid method for setting one grade/whatever notification
	public void SetGradeInfo (int position, Goal goal, bool highScoreNotification) {
		string[] awards = {"Nothing! +$0", "Bronze! +$1", "Silver! +$2", "Gold! +$3"};
		string grade = "Nothing! +$0";

		if(goal.HigherScoreIsGood) {
			if(goal.HighScore >= goal.GoalScore[2]) {
				grade = awards[3];
			} else if (goal.HighScore >= goal.GoalScore[1]) {
				grade = awards[2];
			} else if(goal.HighScore >= goal.GoalScore[0]) {
				grade = awards[1];
			}
			
		} else {
			if(goal.HighScore <= goal.GoalScore[2]) { 
				//apply 'gold' style to award box
				grade = awards[3];
			} else if(goal.HighScore <= goal.GoalScore[1]) {
				//apply 'silver' style to award box
				grade = awards[2];
			} else if(goal.HighScore <= goal.GoalScore[0]) { 
				//apply 'bronze' style to award box
				grade = awards[1];
			} else {
				//apply 'none' style to award box (?)
				
			}
		}

		if (grade == awards [3]) {
			//apply 'gold' style to award box
		} else if (grade == awards [2]) {
			//apply 'silver' style to award box
		} else if (grade == awards [1]) {
			//apply 'bronze' style to award box
		} else {
			//apply 'none' style to award box (?)
		}

		if(highScoreNotification) {
			string scoreText = "New highest score!\n";
			if(!goal.HigherScoreIsGood) scoreText = "New lowest score!\n";
			scoreText += goal.CurrentScore.ToString();
			//do something here
		}
	}

	//Here's the stupid method for setting the info of a specific card in the grid
	public void SetCardInfo (int ColumnNumber, int RowNumber, LibraryCard thisCard) {

		Initialize();
			
		if (thisCard == null) {
			Debug.LogError("what the fuck");
		}

		Debug.Log(ColumnNumber);
		Debug.Log(RowNumber);
		Debug.Log(thisCard);
		Debug.Log(cardGrid);


		cardGrid[ColumnNumber][RowNumber].SetInfo(thisCard);
	}

	// i dont even know if i should put this method in this script or in the card script. probably in the 
	// card script...
	public void ClickCard (int position) {
		if (gameControl.Dollars >= libraryCards[position].Cost) {
			string tempString = libraryCards[position].CardName.ToString ();
			gameControl.Deck.Add (tempString);
			gameControl.AddDollars (-libraryCards[position].Cost);
			RemoveCardFromList(position);
			
			if (SaveData.TryToUnlockCard (libraryCards[position])) {
				shopAndGoalParentCanvas.SetAddedToCollectionText("Added " + libraryCards[position].God.ToString () + 
				    "'s card " + tempString + " to your collection!");
			}
		} else {
			Debug.Log ("Not enough money");
		}
	}

	void RemoveCardFromList (int position) {

	}

	public void TurnOff () {
		for (int i = 0; i < cardGrid.Count; i++) {
			for (int j = 0; j < cardGrid[i].Count; j++) {
				cardGrid[i][j].gameObject.SetActive(false);
			}
		}
	}
}
