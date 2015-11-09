using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ShopRowCanvas : MonoBehaviour {

	ShopAndGoalParentCanvas shopAndGoalParentCanvas;
	ShopControlGUI shopControlGUI;
	GameControl gameControl;
	List<LibraryCard> libraryCards;

	public void SetShopRowInfo (Goal goal, List<LibraryCard> cardsToBuyFrom, bool highScoreNotification) {
		libraryCards = cardsToBuyFrom;

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
				grade = awards[3];
			} else if(goal.HighScore <= goal.GoalScore[1]) {
				grade = awards[2];
			} else if(goal.HighScore <= goal.GoalScore[0]) { 
				grade = awards[1];
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



		for (int i = 0; i < cardsToBuyFrom.Count; i++) {

			LibraryCard thisCard = cardsToBuyFrom [i];
			
			if (thisCard == null) 
				break;
		
			// set background to this:
			// = shopControlGUI.CardTextures [ShopControl.AllGods.IndexOf (thisCard.God)];
			
			if (thisCard.God == ShopControl.Gods.Akan | thisCard.God == ShopControl.Gods.Buluc |
				thisCard.God == ShopControl.Gods.Ikka | thisCard.God == ShopControl.Gods.Kinich | 
				thisCard.God == ShopControl.Gods.Chac) {
				// set text color to black
			} else {
				// set text color to white
			}
			
			Texture2D icon = Resources.Load ("sprites/card icons/" + thisCard.IconPath) as Texture2D;
			// set icon

			Card.Rarity rarity = thisCard.ThisRarity;
			if (rarity == Card.Rarity.Bronze) {
				//set rarity picture				
				//set cost box background color
			} else if (rarity == Card.Rarity.Silver) {
				//set rarity picture
				//set cost box background color
			} else if (rarity == Card.Rarity.Gold) {
				//set rarity picture
				//set cost box background color
			}

			//set god icon

			if (!SaveData.UnlockedCards.Contains (thisCard) && SaveData.UnlockedGods.Contains (thisCard.God)) {
				//turn on the box that says "Buying this card will add it to your collection!";
			}
		}
	}

	public void ClickCard (int position) {
		if (gameControl.Dollars >= libraryCards[position].Cost) {
			string tempString = libraryCards[position].CardName.ToString ();
			gameControl.Deck.Add (tempString);
			gameControl.AddDollars (-libraryCards[position].Cost);
			RemoveCardFromList(position);
			
			if (SaveData.TryToUnlockCard (libraryCards[position])) {
				shopAndGoalParentCanvas.SetAddedToCollectionText("Added " + libraryCards[position].God.ToString () + "'s card " + tempString + 
					" to your collection!");
			}
		} else {
			Debug.Log ("Not enough money");
		}
	}

	void RemoveCardFromList (int position) {

	}

}
