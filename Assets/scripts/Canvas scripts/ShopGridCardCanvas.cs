using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ShopGridCardCanvas : MonoBehaviour {

	public int GODNUMBER;

	bool initialized = false;

	LibraryCard thisCard;

	Image background;
	Image icon;
	Text title;
	Text description;
	Image rarityIcon;
	Image rangeIcon;
	Image aoeIcon;
	Image godIcon;

//	Image costBarImage;
	Text costBarDescription;
	Image costRarityIcon;

	Image showThisCardButton;
	Image buyingThisCardBackground;
	Text buyingThisCardText;

	Image boughtThisCardDialog;
	Text boughtThisCardText;

	public DisplayCardCanvas DISPLAYCARDCANVAS;

    void Start() {
        useGUILayout = false;
		Initialize();
	}

	void Initialize () {
		if(initialized) return;
		initialized = true;

		Image[] Images = gameObject.GetComponentsInChildren<Image>();
		foreach (Image img in Images) {
			if (img.gameObject.name == "Card icon") {
				icon = img;
			} else if (img.gameObject.name == "Card rarity icon") {
				rarityIcon = img;
			} else if (img.gameObject.name == "Card range icon") {
				rangeIcon = img;
			} else if (img.gameObject.name == "Card aoe icon") {
				aoeIcon = img;
			} else if (img.gameObject.name == "Card god icon") {
				godIcon = img;
			} else if (img.gameObject.name == "Card") {
				background = img;
			} else if (img.gameObject.name == "Cost rarity icon") {
				costRarityIcon = img;
			} else if (img.gameObject.name == "Buying this card background") {
				buyingThisCardBackground = img;
			} else if (img.gameObject.name == "Show this card button") {
				showThisCardButton = img;
			} else if (img.gameObject.name == "Bought this card box") {
				boughtThisCardDialog = img;
			}
		}
		Text[] Texts = gameObject.GetComponentsInChildren<Text>();
		foreach (Text txt in Texts) {
			if (txt.gameObject.name == "Card description") {
				description = txt;
			} else if (txt.gameObject.name == "Card title") {
				title = txt;
			} else if (txt.gameObject.name == "Cost description") {
				costBarDescription = txt;
			} else if (txt.gameObject.name == "Buying this card text") {
				buyingThisCardText = txt;
			} else if (txt.gameObject.name == "Bought this card description") {
				boughtThisCardText = txt;
			}
		}
	}

	public void SetInfo (LibraryCard card) {

		Initialize();

		thisCard = card;

		gameObject.SetActive(true);

		background.gameObject.SetActive(true);
		showThisCardButton.gameObject.SetActive(true);
		buyingThisCardBackground.gameObject.transform.parent.gameObject.SetActive(true);
		costBarDescription.gameObject.transform.parent.gameObject.SetActive(true);
		showThisCardButton.gameObject.transform.parent.gameObject.SetActive(true);

		boughtThisCardDialog.gameObject.SetActive(false);

		description.text = card.DisplayText;
		title.text = card.CardName;
		
		if (thisCard.God == ShopControl.Gods.Akan | thisCard.God == ShopControl.Gods.Buluc |
		    thisCard.God == ShopControl.Gods.Ikka | thisCard.God == ShopControl.Gods.Kinich | 
		    thisCard.God == ShopControl.Gods.Chac) {
			// set text color to black
			title.color = new Color(0, 0, 0);
			description.color = new Color(0, 0, 0);
		} else {
			// set text color to white
			title.color = new Color(1, 1, 1);
			description.color = new Color(1, 1, 1);
		}
		
		if (thisCard.ThisRarity == Card.Rarity.Bronze) {
			//set rarity picture			
			rarityIcon.sprite = S.ShopControlGUIInst.Bronze;
			//set cost box background color
			costRarityIcon.sprite = S.ShopControlGUIInst.Bronze;
			costBarDescription.text = "$2\n←";
		} else if (thisCard.ThisRarity == Card.Rarity.Silver) {
			//set rarity picture
			rarityIcon.sprite = S.ShopControlGUIInst.Silver;
			//set cost box background color
			costRarityIcon.sprite = S.ShopControlGUIInst.Silver;
			costBarDescription.text = "$4\n←";
		} else if (thisCard.ThisRarity == Card.Rarity.Gold) {
			//set rarity picture
			rarityIcon.sprite = S.ShopControlGUIInst.Gold;
			//set cost box background color
			costRarityIcon.sprite = S.ShopControlGUIInst.Gold;
			costBarDescription.text = "$6\n←";
		}
		
		// set icon
		Sprite loadIcon = Resources.Load<Sprite> ("sprites/card icons/" + thisCard.IconPath);
		icon.sprite = loadIcon;

		if (thisCard.rangeMax != 0) {
			rangeIcon.gameObject.SetActive(true);
			rangeIcon.sprite = Resources.Load<Sprite>("sprites/targeting icons/range " + thisCard.RangeTargetType.ToString() + 
				" " + thisCard.rangeMin.ToString() + "-" + thisCard.rangeMax.ToString());
		}
		else {
			rangeIcon.gameObject.SetActive(false);
		}

		if(thisCard.aoeMaxRange != 0) {
			aoeIcon.gameObject.SetActive(true);
			aoeIcon.sprite = Resources.Load<Sprite>("sprites/targeting icons/aoe " + thisCard.AoeTargetType.ToString() + 
				" " + thisCard.aoeMinRange.ToString() + "-" + thisCard.aoeMaxRange.ToString());
		}
		else {
			aoeIcon.gameObject.SetActive(false);
		}

		
		// set god icon
		godIcon.sprite = S.ShopControlGUIInst.SpriteGodIcons[ShopControl.AllGods.IndexOf(thisCard.God)];

		// set card background
		background.sprite = S.ShopControlGUIInst.GodSmallCards[ShopControl.AllGods.IndexOf(thisCard.God)];

		GameObject go = buyingThisCardText.transform.parent.gameObject;
		if (!SaveDataControl.UnlockedCards.Contains (thisCard) && SaveDataControl.UnlockedGods.Contains (thisCard.God)) {
			go.SetActive(true);
			Invoke("HideCollectionAddDialog", 2.0f);
		} else {
			go.SetActive(false);
		}
	}

	public void HideCollectionAddDialog () {
		// only will appear when the card is new. 
		buyingThisCardText.transform.parent.gameObject.SetActive(false);
	}

	// Used in the invoke function at the end of Buy() directly below. Doesn't show up in stupid IDE
	void HideThisCard () {
		gameObject.SetActive(false);
	}

	public void Buy () {
		Debug.Log("buy");
		S.ShopControlInst.BoughtCardFromGod(GODNUMBER);
		S.ShopControlInst.RemoveBoughtCard(title.text);
		background.gameObject.SetActive(false);
		showThisCardButton.gameObject.SetActive(false);
		buyingThisCardBackground.gameObject.transform.parent.gameObject.SetActive(false);
		costBarDescription.gameObject.transform.parent.gameObject.SetActive(false);
		boughtThisCardDialog.gameObject.SetActive(true);

		if (S.GameControlInst.Dollars >= thisCard.Cost) {
			string tempString = thisCard.CardName.ToString ();
			S.GameControlInst.Deck.Add (tempString);
			S.GameControlInst.AddDollars (-thisCard.Cost);

			if (SaveDataControl.TryToUnlockCard (thisCard)) {
				boughtThisCardText.text = ("Added " + thisCard.God.ToString () + 
					"'s card " + tempString + " to your deck as well as your collection!");
				boughtThisCardDialog.sprite = S.ShopGridCanvasInst.BOUGHTSPRITEADDEDTOCOLLECTION;
			} else {
				boughtThisCardText.text = ("This card has been added to your deck!");
				boughtThisCardDialog.sprite = S.ShopGridCanvasInst.BOUGHTSPRITENORMAL;
			}

			Invoke("HideThisCard", 2.0f);

			StateSavingControl.Save();

		} else {
			boughtThisCardText.text = ("You don't have enough money! This card costs $" + thisCard.Cost.ToString() + ".");
			boughtThisCardDialog.sprite = S.ShopGridCanvasInst.BOUGHTSPRITENORMAL;
		}

		StateSavingControl.Save();
	}

	public void DisplayThisCard() {
		DISPLAYCARDCANVAS.Display(thisCard);
	}
}
