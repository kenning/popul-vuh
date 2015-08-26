using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public static class SaveLoad {
	public static SavedGame savedGame = new SavedGame();

	public static void Save() {
//		return;
		savedGame = (SaveData.current());
		EasySerializer.SerializeObjectToFile (savedGame, Application.persistentDataPath + "savedGames.gd");
		GameControl.Error = "Saved to " + Application.persistentDataPath + "savedGames.gd";

		savedGame = (SaveData.current());
		BinaryFormatter bf = new BinaryFormatter ();
		FileStream file = File.Create (Application.persistentDataPath + "savedGames.gd");
		bf.Serialize (file, SaveLoad.savedGame);
		file.Close ();
		GameControl.Error = "Saved to " + Application.persistentDataPath + "savedGames.gd";
	}

	public static void Load() {
//		return;
		object loadedData = EasySerializer.DeserializeObjectFromFile (Application.persistentDataPath + "savedGames.gd");
		SaveData.LoadData ((SavedGame)loadedData);
		GameControl.Error = "Loaded from " + Application.persistentDataPath + "savedGames.gd";

		if(File.Exists(Application.persistentDataPath + "savedGames.gd")) {
			BinaryFormatter bf = new BinaryFormatter();
			FileStream file = File.Open(Application.persistentDataPath + "savedGames.gd", FileMode.Open);
			SaveData.LoadData((SavedGame)bf.Deserialize(file));
			file.Close();
			GameControl.Error = "Loaded from " + Application.persistentDataPath + "savedGames.gd";
		}
	}
}
