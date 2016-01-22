
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ES2UserType_Goal : ES2Type
{
	public override void Write(object obj, ES2Writer writer)
	{
		Goal data = (Goal)obj;
		// Add your writer.Write calls here.
		writer.Write(data.God);
		writer.Write(data.GodString);
		writer.Write(data.Description);
		writer.Write(data.MiniDescription);
		writer.Write(data.CurrentScore);
		writer.Write(data.HighScore);
		writer.Write(data.GoalScore);
		writer.Write(data.DisplayScore);
		writer.Write(data.DidGoalThisTurnTracker);
		writer.Write(data.HigherScoreIsGood);

	}
	
	public override object Read(ES2Reader reader)
	{
		Goal data = new Goal();
		Read(reader, data);
		return data;
	}
	
	public override void Read(ES2Reader reader, object c)
	{
		Goal data = (Goal)c;
		// Add your reader.Read calls here to read the data into the object.
		data.God = reader.Read<ShopControl.Gods>();
		data.GodString = reader.Read<System.String>();
		data.Description = reader.Read<System.String>();
		data.MiniDescription = reader.Read<System.String>();
		data.CurrentScore = reader.Read<System.Int32>();
		data.HighScore = reader.Read<System.Int32>();
		data.GoalScore = reader.ReadArray<System.Int32>();
		data.DisplayScore = reader.Read<System.String>();
		data.DidGoalThisTurnTracker = reader.Read<System.Boolean>();
		data.HigherScoreIsGood = reader.Read<System.Boolean>();

	}
	
	/* ! Don't modify anything below this line ! */
	public ES2UserType_Goal():base(typeof(Goal)){}
}
