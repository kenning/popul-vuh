
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ES2UserType_SavedState : ES2Type
{
	public override void Write(object obj, ES2Writer writer)
	{
		SavedState data = (SavedState)obj;
		// Add your writer.Write calls here.
		writer.Write(data.Level);
		writer.Write(data.ObstacleXPositions);
		writer.Write(data.ObstacleYPositions);
		writer.Write(data.ObstacleLevelType);
		writer.Write(data.CardsInHand);
		writer.Write(data.CardsInDeck);
		writer.Write(data.CardsInDiscard);
		writer.Write(data.Enemies);
		writer.Write(data.EnemyHealths);
		writer.Write(data.EnemyXPositions);
		writer.Write(data.EnemyYPositions);
		writer.Write(data.EnemyPlays);
		writer.Write(data.PlayerPosition);
		writer.Write(data.PlayerHealth);
		writer.Write(data.PlayerMoves);
		writer.Write(data.PlayerPlays);
		writer.Write(data.Dollars);
		writer.Write(data.BleedingTurns);
		writer.Write(data.SwollenTurns);
		writer.Write(data.HungerTurns);
		writer.Write(data.Goals);
		writer.Write(data.TriggerList);
		writer.Write(data.ShopCardList1);
		writer.Write(data.ShopCardList2);
		writer.Write(data.ShopCardList3);
		writer.Write(data.ShopMode);

	}
	
	public override object Read(ES2Reader reader)
	{
		SavedState data = new SavedState();
		Read(reader, data);
		return data;
	}
	
	public override void Read(ES2Reader reader, object c)
	{
		SavedState data = (SavedState)c;
		// Add your reader.Read calls here to read the data into the object.
		data.Level = reader.Read<System.Int32>();
		data.ObstacleXPositions = reader.ReadList<System.Int32>();
		data.ObstacleYPositions = reader.ReadList<System.Int32>();
		data.ObstacleLevelType = reader.Read<ObstacleLibrary.LevelTypes>();
		data.CardsInHand = reader.ReadList<LibraryCard>();
		data.CardsInDeck = reader.ReadList<System.String>();
		data.CardsInDiscard = reader.ReadList<LibraryCard>();
		data.Enemies = reader.ReadList<System.String>();
		data.EnemyHealths = reader.ReadList<System.Int32>();
		data.EnemyXPositions = reader.ReadList<System.Int32>();
		data.EnemyYPositions = reader.ReadList<System.Int32>();
		data.EnemyPlays = reader.ReadList<System.Int32>();
		data.PlayerPosition = reader.ReadArray<System.Int32>();
		data.PlayerHealth = reader.Read<System.Int32>();
		data.PlayerMoves = reader.Read<System.Int32>();
		data.PlayerPlays = reader.Read<System.Int32>();
		data.Dollars = reader.Read<System.Int32>();
		data.BleedingTurns = reader.Read<System.Int32>();
		data.SwollenTurns = reader.Read<System.Int32>();
		data.HungerTurns = reader.Read<System.Int32>();
		data.Goals = reader.ReadArray<Goal>();
		data.TriggerList = reader.ReadList<System.String>();
		data.ShopCardList1 = reader.ReadList<LibraryCard>();
		data.ShopCardList2 = reader.ReadList<LibraryCard>();
		data.ShopCardList3 = reader.ReadList<LibraryCard>();
		data.ShopMode = reader.Read<System.Boolean>();

	}
	
	/* ! Don't modify anything below this line ! */
	public ES2UserType_SavedState():base(typeof(SavedState)){}
}
