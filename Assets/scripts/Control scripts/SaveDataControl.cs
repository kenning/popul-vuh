using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

[System.Serializable]
public class SaveDataControl {
    //Save data variables will be marked with a $%^ at the places you need to modify them
	//(they have to be modified in each place, or code will break)

    //$%^
	public static List<LibraryCard> UnlockedCards = new List<LibraryCard>();
	public static List<LibraryCard> StartingDeckCards = new List<LibraryCard>();
	public static List<ShopControl.Gods> UnlockedGods = new List<ShopControl.Gods>();
	public static List<string> DefeatedEnemies = new List<string> ();
	public static Dictionary<string, int> GoalHighScores = new Dictionary<string, int> ();
	public static bool FinishedTutorial = false;
    public static bool NewCardsAvailable = false;

	public static SavedGame current() {
		SavedGame sg = new SavedGame ();
		sg.UnlockedCards = SaveDataControl.UnlockedCards;
		sg.StartingDeckCards = SaveDataControl.StartingDeckCards;
		sg.UnlockedGods = SaveDataControl.UnlockedGods.Select(i=>(int)i).ToList();
		sg.DefeatedEnemies = SaveDataControl.DefeatedEnemies;
        sg.FinishedTutorial = SaveDataControl.FinishedTutorial;
        sg.NewCardsAvailable = SaveDataControl.NewCardsAvailable;
        Debug.Log("Saving! unlocked gods: " + SaveDataControl.UnlockedGods.Count.ToString() + 
		          ", " + "New cards available = " + NewCardsAvailable.ToString() + 
		          ", Finished tutorial = " + FinishedTutorial.ToString() +
		          "\nunlocked cards: " + SaveDataControl.UnlockedCards.Count.ToString () + 
		          ", starting deck cards: " + SaveDataControl.StartingDeckCards.Count.ToString() + 
		          ", defeated enemies: " + SaveDataControl.DefeatedEnemies.Count.ToString());
		return sg;
	}

	public static void NewSaveFile() {
	
        //$%^ (this is where you set all the default values)
		FinishedTutorial = false;
        NewCardsAvailable = false;

		UnlockedCards = new List<LibraryCard> ();
		//Ikka
		//Ekcha
		UnlockedCards.Add(CardLibrary.Lib["Quick Prayer"]);
		//Ixchel
		UnlockedCards.Add(CardLibrary.Lib["Cloth Shirt"]);
		UnlockedCards.Add(CardLibrary.Lib["Cloth Armor"]);
		//Buluc
		UnlockedCards.Add(CardLibrary.Lib["Wooden Pike"]);
		UnlockedCards.Add(CardLibrary.Lib["Iron Macana"]);
		UnlockedCards.Add(CardLibrary.Lib["Wooden Bow"]);
		//Chac
		UnlockedCards.Add(CardLibrary.Lib["Coffee"]);
		UnlockedCards.Add(CardLibrary.Lib["Apple"]);
		//Kinich
		//Akan

		UnlockedGods = new List<ShopControl.Gods>();
		AddGodInOrderToUnlocked (ShopControl.Gods.Buluc);

		DefeatedEnemies = new List<string> ();

		StartingDeckCards = new List<LibraryCard> ();
		//Ikka
		//Ekcha
		StartingDeckCards.Add(CardLibrary.Lib["Quick Prayer"]);
		//Ixchel
		StartingDeckCards.Add(CardLibrary.Lib["Cloth Shirt"]);
		StartingDeckCards.Add(CardLibrary.Lib["Cloth Shoes"]);
		//Buluc
		StartingDeckCards.Add(CardLibrary.Lib["Wooden Bow"]);
		StartingDeckCards.Add(CardLibrary.Lib["Wooden Bow"]);
		StartingDeckCards.Add(CardLibrary.Lib["Wooden Pike"]);
		StartingDeckCards.Add(CardLibrary.Lib["Wooden Pike"]);
		StartingDeckCards.Add(CardLibrary.Lib["Iron Macana"]);
        StartingDeckCards.Add(CardLibrary.Lib["Iron Macana"]);
		//Chac
		StartingDeckCards.Add(CardLibrary.Lib["Coffee"]);
		StartingDeckCards.Add(CardLibrary.Lib["Apple"]);
		//Kinich
		//Akan

        MainMenu.UnlockCheck();
	}

	public static void AddGodInOrderToUnlocked (ShopControl.Gods newGod) {
		if(!UnlockedGods.Contains(newGod)) {
			UnlockedGods.Add (newGod);
		}
		List<ShopControl.Gods> newList = new List<ShopControl.Gods>();
		for(int i = 0; i < ShopControl.AllGods.Count; i++) {
			if(UnlockedGods.Contains(ShopControl.AllGods[i])) {
				newList.Add(ShopControl.AllGods[i]);
			}
		}
		UnlockedGods = newList;
	}

    public static bool TryToUnlockCard(LibraryCard card)
    {
		if (!UnlockedCards.Contains(card) && UnlockedGods.Contains(card.God))
		{
			UnlockedCards.Add(card);
			NewCardsAvailable = true;
			SaveDataControl.Save();
			return true;
		}

		return false;
    }

	public static void AddEnemyToDefeated(string enemyName) {
		if (!DefeatedEnemies.Contains (enemyName)) {
			DefeatedEnemies.Add(enemyName);
		}
	}

	public static bool CheckForHighScores(Goal goal) {
		if (!SaveDataControl.GoalHighScores.ContainsKey(goal.MiniDescription)) {
			if((goal.HighScore != 0) | (goal.HighScore == 0 && !goal.HigherScoreIsGood))
			SaveDataControl.GoalHighScores[goal.MiniDescription] = goal.HighScore;
			
			SaveDataControl.Save();
			return true;
		}
		if (goal.HigherScoreIsGood) {
			if(SaveDataControl.GoalHighScores[goal.MiniDescription] < goal.HighScore) {
				Debug.Log("new high score is " + goal.HighScore);
				SaveDataControl.GoalHighScores[goal.MiniDescription] = goal.HighScore;
				
				SaveDataControl.Save();
				return true;
			}
		} else {
			if(SaveDataControl.GoalHighScores[goal.MiniDescription] > goal.HighScore) {
				Debug.Log("new high score is " + goal.HighScore);
				SaveDataControl.GoalHighScores[goal.MiniDescription] = goal.HighScore;

				SaveDataControl.Save();
				return true;
			}
		}
		return false;
	}

	public static void Save() {
		ES2.Delete("PV");
		ES2.Save<SavedGame>(current(), "PV");
	}

	public static void Load() {
		if (ES2.Exists ("PV")) {
			SavedGame savedGameData = ES2.Load<SavedGame> ("PV");

			UnlockedGods = new List<ShopControl.Gods> ();
			for (int i = 0; i < savedGameData.UnlockedGods.Count; i++) {
				UnlockedGods.Add ((ShopControl.Gods)savedGameData.UnlockedGods [i]);
			}
			UnlockedCards = savedGameData.UnlockedCards;
			DefeatedEnemies = savedGameData.DefeatedEnemies;
			StartingDeckCards = savedGameData.StartingDeckCards;
			FinishedTutorial = savedGameData.FinishedTutorial;
			NewCardsAvailable = savedGameData.NewCardsAvailable;
			GoalHighScores = savedGameData.GoalHighScores;

			MainMenu.UnlockCheck();
			Debug.Log ("Loaded! Unlocked gods: " + SaveDataControl.UnlockedGods.Count.ToString () + 
				", New cards available = " + NewCardsAvailable.ToString() + 
				", Finished tutorial = " + FinishedTutorial.ToString() +
				"\nunlocked cards: " + SaveDataControl.UnlockedCards.Count.ToString () + 
				", starting deck cards: " + SaveDataControl.StartingDeckCards.Count.ToString() + 
				", defeated enemies: " + SaveDataControl.DefeatedEnemies.Count.ToString());
			
		}
	}
}

[System.Serializable]
public class SavedGame 
{
    //$%^
	public List<LibraryCard> UnlockedCards = new List<LibraryCard>();
	public List<LibraryCard> StartingDeckCards = new List<LibraryCard>();
	public List<string> DefeatedEnemies = new List<string>();
	public List<int> UnlockedGods = new List<int>();
	public Dictionary<string, int> GoalHighScores = new Dictionary<string, int> ();
	public bool FinishedTutorial = false;
    public bool NewCardsAvailable = false;
}
