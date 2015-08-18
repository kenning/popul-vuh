using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GoalLibrary : MonoBehaviour {

	public static List<Goal> allGoals;
	public static List<Goal> unusedGoals;

	//VVV this is a utility used by GodChoiceMenu to see if you have selected enough gods to start the game.
	public static int NumberOfGoalsPossible(bool[] bools) {
		int x = 0;
		for(int i = 0; i < allGoals.Count; i++) {
			for(int j = 0; j < bools.Length; j++) {
				if(bools[j] && allGoals[i].God == ShopControl.AllGods[j]){
					x++;
				}
			}
		}
		return x;
	}

	public Goal[] InitializeGoals(int numberOfGods) {

		unusedGoals = new List<Goal> ();

		GodChoiceMenu menu = gameObject.GetComponent<GodChoiceMenu> ();

		//If godchoicemenu is unlocked, pick from the selected gods' goals; otherwise, pick from all goals
		if(SaveData.UnlockedGods.Count == 7) {
			for(int i = 0; i < allGoals.Count; i++) {
				for(int j = 0; j < menu.GodChoiceSelection.Length; j++) {
					if(menu.GodChoiceSelection[j] && allGoals[i].God == ShopControl.AllGods[j]){
						unusedGoals.Add(allGoals[i]);
					}
				}
			}
		} else {
			for(int i = 0; i < allGoals.Count; i++) {
				unusedGoals.Add(allGoals[i]);
			}
		}

		Goal[] Goals = new Goal[numberOfGods];
		for(int i = 0; i < numberOfGods; i++){
			Goals[i] = new Goal();
			int randomNumber = Random.Range (0, unusedGoals.Count);
			Goals[i] = unusedGoals[randomNumber];
			unusedGoals.RemoveAt(randomNumber);
		}

		return Goals;
	}


	public void Startup () {

		allGoals = new List<Goal> ();
		unusedGoals = new List<Goal> ();

	//Ikka: rogue esque things
		allGoals.Add (new Goal("Move X times in one turn", "Move X times in one turn", " wants you to move several squares in one turn.", 
		                   ShopControl.Gods.Ikka, new int[]{2, 4, 6}, true));
		allGoals.Add(new Goal("Punch X times", "Punch X times", " wants you to punch an enemy X times.", 
		                   ShopControl.Gods.Ikka, new int[]{3, 5, 7}, true));
//		allGoals.Add (new Goal("Don't play a card X turns in a row", "Don't play a card X turns in a row", " wants you not to play a card X turns in a row.",
//							ShopControl.Gods.Ikka, new int[]{3, 5, 7, 10}, true));
		allGoals.Add (new Goal("Don't move X turns in a row", "Don't move X turns in a row", " wants you not to move X turns in a row.", 
		                   ShopControl.Gods.Ikka, new int[]{2, 4, 6}, true));

	//Ekcha: god favoring things? god pissing off things? fourth wall breaking things?
		allGoals.Add (new Goal("Touch the screen no more than than X times", "Touch the screen no more than than X times", " wants you to touch the screen no more than X times.", 
		                   ShopControl.Gods.Ekcha, new int[]{30, 15, 10}, false));
		///////////////////////////////////
		/// "Change the deck count no more/less than X times"
		///////////////////////////////////
		
	//Ixchel: shielding, blocking things, being protected >>> Ixchel
		allGoals.Add (new Goal("Protect against X attacks", "Protect against X attacks", " wants you to protect yourself against X attacks.", 
		                   ShopControl.Gods.Ixchel, new int[]{1, 2, 4}, true));
//		allGoals.Add (new Goal("Don't deal damage X turns in a row", "Don't deal damage X turns in a row", " wants you not to deal damage X turns in a row.",
//							ShopControl.Gods.Ixchel, new int[]{4, 7, 10, 15}, true));


	//Buluc: weapons, damage
		allGoals.Add (new Goal("Kill X enemies in one turn", "Kill X enemies in one turn", " wants you to kill several enemies in one turn.", 
		                   ShopControl.Gods.Buluc, new int[]{2, 3, 4}, true));
		allGoals.Add (new Goal("Kill enemies X turns in a row", "Kill enemies X turns in a row", " wants you to kill at least one enemy several turns in a row.", 
		                   ShopControl.Gods.Buluc, new int[]{2, 3, 5}, true));
		allGoals.Add (new Goal("Deal X damage in one turn", "Deal X damage in one turn", " wants you to deal X damage in one turn.", 
		                   ShopControl.Gods.Buluc, new int[]{2, 3, 5}, true));

	//Chac: food, using lots of your deck (maybe spread those out?) >>> Chac
		allGoals.Add (new Goal("Play X cards in one turn", "Play X cards in one turn", " wants you to play X cards in one turn.", 
		                   ShopControl.Gods.Chac, new int[]{2, 4, 7}, true));
		allGoals.Add (new Goal("Draw X cards in one turn", "Draw X cards in one turn", " wants you to draw X cards in one turn.", 
		                   ShopControl.Gods.Chac, new int[]{3, 5, 8}, true));
		allGoals.Add (new Goal("Play X paper cards in one turn", "Play X paper cards in one turn", " wants you to play X cards of paper rarity in one turn.", 
		                   ShopControl.Gods.Chac, new int[]{2, 3, 5}, true));
		allGoals.Add (new Goal("Draw X paper cards in one turn", "Draw X paper cards in one turn", " wants you to draw X cards of paper rarity in one turn.", 
		                   ShopControl.Gods.Chac, new int[]{2, 3, 5}, true));

	//Kinich: trashing, efficient deck
		allGoals.Add (new Goal("Play less than X cards total", "Play less than X cards total", " wants you to play less than X cards before finishing the level.", 
		                   ShopControl.Gods.Kinich, new int[]{4, 3, 2}, false));
		allGoals.Add (new Goal("Don't deal damage or move X turns in a row", "Don't deal damage or move X turns in a row", " wants you not to deal damage or move X turns in a row.", 
		                   ShopControl.Gods.Kinich, new int[]{1, 2, 4}, true));

	//Akan: graveyard interaction 
		allGoals.Add (new Goal("Discard pile has X cards in it", "Discard pile has X cards in it", " wants you to have X cards in your discard pile.", 
		                   ShopControl.Gods.Akan, new int[]{5, 8, 12}, true));
//		allGoals.Add (new Goal("Discard pile has X cards in a row of the same God", "Discard pile has X cards in a row of the same God", " wants you to have X cards in a row in your discard pile of the same God.", 
//		                   ShopControl.Gods.Akan, new int[]{3, 5, 7}, true));
		
		/////That's all folks!
		
	}
}
