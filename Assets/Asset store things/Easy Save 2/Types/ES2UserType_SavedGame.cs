
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ES2UserType_SavedGame : ES2Type
{
	public override void Write(object obj, ES2Writer writer)
	{
		SavedGame data = (SavedGame)obj;
		// Add your writer.Write calls here.
		writer.Write(data.UnlockedCards);
		writer.Write(data.StartingDeckCards);
		writer.Write(data.DefeatedEnemies);
		writer.Write(data.UnlockedGods);
		writer.Write(data.GoalHighScores);
		writer.Write(data.FinishedTutorial);
		writer.Write(data.NewCardsAvailable);

	}
	
	public override object Read(ES2Reader reader)
	{
		SavedGame data = new SavedGame();
		Read(reader, data);
		return data;
	}
	
	public override void Read(ES2Reader reader, object c)
	{
		SavedGame data = (SavedGame)c;
		// Add your reader.Read calls here to read the data into the object.
		data.UnlockedCards = reader.ReadList<LibraryCard>();
		data.StartingDeckCards = reader.ReadList<LibraryCard>();
		data.DefeatedEnemies = reader.ReadList<System.String>();
		data.UnlockedGods = reader.ReadList<System.Int32>();
		data.GoalHighScores = reader.ReadDictionary<System.String,System.Int32>();
		data.FinishedTutorial = reader.Read<System.Boolean>();
		data.NewCardsAvailable = reader.Read<System.Boolean>();

	}
	
	/* ! Don't modify anything below this line ! */
	public ES2UserType_SavedGame():base(typeof(SavedGame)){}
}
