using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

[System.Serializable]
public static class SaveData 
{
    //Save data variables will be marked with a $%^ at the places you need to modify them.

    //$%^
	public static List<LibraryCard> UnlockedCards = new List<LibraryCard>();
	public static List<LibraryCard> StartingDeckCards = new List<LibraryCard>();
	public static List<ShopControl.Gods> UnlockedGods = new List<ShopControl.Gods>();
	public static bool FinishedTutorial = false;
    public static bool NewCardsAvailable = false;
	
	public static void LoadData(SavedGame savedGameData) {
		if(File.Exists(Application.persistentDataPath + "savedGames.gd")) {
			if(savedGameData.StartingDeckCards == null) {
				Debug.Log("Oh shit! Can't load this!");
				return;
			}

            //$%^
			SaveData.UnlockedGods = savedGameData.UnlockedGods;
			SaveData.UnlockedCards = savedGameData.UnlockedCards;
			SaveData.StartingDeckCards = savedGameData.StartingDeckCards;
            SaveData.FinishedTutorial = savedGameData.FinishedTutorial;
            SaveData.NewCardsAvailable = savedGameData.NewCardsAvailable;
		}
        MainMenu.UnlockCheck();
		Debug.Log ("Loading! unlocked gods: " + SaveData.UnlockedGods.Count.ToString () + ", " + "New cards available = " + NewCardsAvailable.ToString() + ", Finished tutorial = " + FinishedTutorial.ToString() +
		           "\nunlocked cards: " + SaveData.UnlockedCards.Count.ToString () + ", starting deck cards: " + SaveData.StartingDeckCards.Count.ToString());
	}

	public static SavedGame current() {
		SavedGame sg = new SavedGame ();
		sg.UnlockedCards = SaveData.UnlockedCards;
		sg.StartingDeckCards = SaveData.StartingDeckCards;
		sg.UnlockedGods = SaveData.UnlockedGods;
        sg.FinishedTutorial = SaveData.FinishedTutorial;
        sg.NewCardsAvailable = SaveData.NewCardsAvailable;
        Debug.Log("Saving! unlocked gods: " + SaveData.UnlockedGods.Count.ToString() + ", " + "New cards available = " + NewCardsAvailable.ToString() + ", Finished tutorial = " + FinishedTutorial.ToString() +
		           "\nunlocked cards: " + SaveData.UnlockedCards.Count.ToString () + ", starting deck cards: " + SaveData.StartingDeckCards.Count.ToString());
		return sg;
	}

	public static void NewSaveFile() {
	
        //$%^ (this is where you set all the default values)
		FinishedTutorial = false;
        NewCardsAvailable = false;

		UnlockedCards = new List<LibraryCard> ();
		//Ikka
		UnlockedCards.Add(CardLibrary.Lib["Cloth Shoes"]);
		//Ekcha
		UnlockedCards.Add(CardLibrary.Lib["Quick Prayer"]);
		//Ixchel
		UnlockedCards.Add(CardLibrary.Lib["Cloth Shirt"]);
		UnlockedCards.Add(CardLibrary.Lib["Cloth Armor"]);
		//Buluc
		UnlockedCards.Add(CardLibrary.Lib["Wooden Pike"]);
		UnlockedCards.Add(CardLibrary.Lib["Iron Macana"]);
		//


		UnlockedCards.Add(CardLibrary.Lib["Coffee"]);
		UnlockedCards.Add(CardLibrary.Lib["Apple"]);
		//Kinich
		//Akan

		UnlockedGods = new List<ShopControl.Gods>();
		AddGodInOrderToUnlocked (ShopControl.Gods.Buluc);

		StartingDeckCards = new List<LibraryCard> ();
		StartingDeckCards.Add(CardLibrary.Lib["Cloth Shirt"]);
		StartingDeckCards.Add(CardLibrary.Lib["Cloth Shirt"]);
		StartingDeckCards.Add(CardLibrary.Lib["Cloth Shoes"]);
		StartingDeckCards.Add(CardLibrary.Lib["Cloth Shoes"]);
		StartingDeckCards.Add(CardLibrary.Lib["Coffee"]);
		StartingDeckCards.Add(CardLibrary.Lib["Coffee"]);
		StartingDeckCards.Add(CardLibrary.Lib["Wooden Pike"]);
		StartingDeckCards.Add(CardLibrary.Lib["Wooden Pike"]);
		StartingDeckCards.Add(CardLibrary.Lib["Apple"]);
		StartingDeckCards.Add(CardLibrary.Lib["Apple"]);
        StartingDeckCards.Add(CardLibrary.Lib["Iron Macana"]);
        StartingDeckCards.Add(CardLibrary.Lib["Iron Macana"]);
		StartingDeckCards.Add(CardLibrary.Lib["Quick Prayer"]);
		StartingDeckCards.Add(CardLibrary.Lib["Quick Prayer"]);

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

    public static void AddCardToUnlocked(LibraryCard card)
    {
        UnlockedCards.Add(card);
        NewCardsAvailable = true;
    }
}

[System.Serializable]
public class SavedGame 
{
    //$%^
	public List<LibraryCard> UnlockedCards = new List<LibraryCard>();
	public List<LibraryCard> StartingDeckCards = new List<LibraryCard>();
	public List<ShopControl.Gods> UnlockedGods = new List<ShopControl.Gods>();
	public bool FinishedTutorial = false;
    public bool NewCardsAvailable = false;
}