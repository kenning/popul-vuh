using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public static class SaveLoad {
	public static SavedGame savedGame = new SavedGame();
//		"UnlockedCards",
//		"StartingDeckCards",
//		"UnlockedGods",
//		"DefeatedEnemies",
//		"GoalHighScores",
//		"FinishedTutorial",
//		"NewCardsAvailable" 

	public static void Save() {

		ES2.Delete("PV");
		ES2.Save<SavedGame>(SaveData.current(), "PV");

////		return;
//		savedGame = (SaveData.current());
//
//		BinaryFormatter bf = new BinaryFormatter ();
//
//
//		// repeat this for everything
//		FileStream file = File.Create (Application.persistentDataPath + "unlockedCards.gd");
//		bf.Serialize (file, SaveLoad.savedGame);
//		file.Close ();
//
//		GameControl.Error = "Saved to " + Application.persistentDataPath + "savedGames.gd";
	}

	public static void Load() {
		if (ES2.Exists ("PV")) {
			SaveData.LoadData(ES2.Load<SavedGame> ("PV"));
		}

////		return;
//
//		if(File.Exists(Application.persistentDataPath + "savedGames.gd")) {
//			BinaryFormatter bf = new BinaryFormatter();
//
//			// make a new saved data file
//
//			//repeat this for everything
//			FileStream file = File.Open(Application.persistentDataPath + "savedGames.gd", FileMode.Open);
//			SaveData.LoadData((SavedGame)bf.Deserialize(file));
//			file.Close();
//
//			GameControl.Error = "Loaded from " + Application.persistentDataPath + "savedGames.gd";
//		}
	}
}
