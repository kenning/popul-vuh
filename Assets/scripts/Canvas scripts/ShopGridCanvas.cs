using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class ShopGridCanvas : MonoBehaviour {

	bool initialized = false;

	List<LibraryCard> libraryCards;
	
	//This is a list of the card scripts in the grid
	public List<ShopGridCardCanvas> COLUMN1;
	public List<ShopGridCardCanvas> COLUMN2;
	public List<ShopGridCardCanvas> COLUMN3;
	List<List<ShopGridCardCanvas>> cardGrid;

	public List<ShopAwardCanvas> SHOPAWARDS;

	public Sprite BOUGHTSPRITEADDEDTOCOLLECTION;
	public Sprite BOUGHTSPRITENORMAL;

	void Initialize() {
		if (initialized) return;
		initialized = true;

		cardGrid = new List<List<ShopGridCardCanvas>> {COLUMN1, COLUMN2, COLUMN3};
	}

	//Here's the stupid method for setting the info of a specific card in the grid
	public void SetCardInfo (int ColumnNumber, int RowNumber, LibraryCard thisCard) {

		Initialize();
			
		if (thisCard == null) {
			Debug.LogError("what the fuck");
		}

		cardGrid[ColumnNumber][RowNumber].SetInfo(thisCard);
	}

	public void TurnOff () {
		Initialize();

		for (int i = 0; i < cardGrid.Count; i++) {
			for (int j = 0; j < cardGrid[i].Count; j++) {
				cardGrid[i][j].gameObject.SetActive(false);
			}
		}
	}
}
