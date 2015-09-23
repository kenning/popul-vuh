
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ES2UserType_LibraryCard : ES2Type
{
	public override void Write(object obj, ES2Writer writer)
	{
		LibraryCard data = (LibraryCard)obj;
		// Add your writer.Write calls here.
		writer.Write(data.CardName);
		writer.Write(data.DisplayName);
		writer.Write(data.IconPath);
		writer.Write(data.Tooltip);
		writer.Write(data.DisplayText);
		writer.Write(data.MiniDisplayText);
		writer.Write(data.RangeTargetType);
		writer.Write(data.rangeMin);
		writer.Write(data.rangeMax);
		writer.Write(data.AoeTargetType);
		writer.Write(data.aoeMinRange);
		writer.Write(data.aoeMaxRange);
		writer.Write(data.ThisRarity);
		writer.Write(data.CardAction);
		writer.Write(data.God);
		writer.Write(data.Cost);
		writer.Write(data.UnlockCost);

	}
	
	public override object Read(ES2Reader reader)
	{
		LibraryCard data = new LibraryCard();
		Read(reader, data);
		return data;
	}
	
	public override void Read(ES2Reader reader, object c)
	{
		LibraryCard data = (LibraryCard)c;
		// Add your reader.Read calls here to read the data into the object.
		data.CardName = reader.Read<System.String>();
		data.DisplayName = reader.Read<System.String>();
		data.IconPath = reader.Read<System.String>();
		data.Tooltip = reader.Read<System.String>();
		data.DisplayText = reader.Read<System.String>();
		data.MiniDisplayText = reader.Read<System.String>();
		data.RangeTargetType = reader.Read<GridControl.TargetTypes>();
		data.rangeMin = reader.Read<System.Int32>();
		data.rangeMax = reader.Read<System.Int32>();
		data.AoeTargetType = reader.Read<GridControl.TargetTypes>();
		data.aoeMinRange = reader.Read<System.Int32>();
		data.aoeMaxRange = reader.Read<System.Int32>();
		data.ThisRarity = reader.Read<Card.Rarity>();
		data.CardAction = reader.Read<Card.CardActionTypes>();
		data.God = reader.Read<ShopControl.Gods>();
		data.Cost = reader.Read<System.Int32>();
		data.UnlockCost = reader.Read<System.Int32>();

	}
	
	/* ! Don't modify anything below this line ! */
	public ES2UserType_LibraryCard():base(typeof(LibraryCard)){}
}
