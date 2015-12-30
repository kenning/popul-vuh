using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ShopGridCardCanvas : MonoBehaviour {

	bool initialized = false;

	ShopControlGUI shopControlGUI;

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

	Image buyingThisCardBackground;
	Text buyingThisCardText;

	void Start() {
		Initialize();
	}

	void Initialize () {
		if(initialized) return;
		initialized = true;

		shopControlGUI = GameObject.FindGameObjectWithTag("GameController").GetComponent<ShopControlGUI>();

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
			}
		}
	}

	public void SetInfo (LibraryCard thisCard) {

		Initialize();

		gameObject.SetActive(true);
		
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
			rarityIcon.sprite = shopControlGUI.Bronze;
			//set cost box background color
			costRarityIcon.sprite = shopControlGUI.Bronze;
			costBarDescription.text = "$1";
		} else if (thisCard.ThisRarity == Card.Rarity.Silver) {
			//set rarity picture
			rarityIcon.sprite = shopControlGUI.Silver;
			//set cost box background color
			costRarityIcon.sprite = shopControlGUI.Silver;
			costBarDescription.text = "$2";
		} else if (thisCard.ThisRarity == Card.Rarity.Gold) {
			//set rarity picture
			rarityIcon.sprite = shopControlGUI.Gold;
			//set cost box background color
			costRarityIcon.sprite = shopControlGUI.Gold;
			costBarDescription.text = "$3";
		}
		
		// set icon
		Sprite loadIcon = Resources.Load ("sprites/card icons/" + thisCard.IconPath) as Sprite;
		icon.sprite = loadIcon;
		
		// set god icon
		godIcon.sprite = shopControlGUI.SpriteGodIcons[ShopControl.AllGods.IndexOf(thisCard.God)];

		// set card background
		background.sprite = shopControlGUI.GodSmallCards[ShopControl.AllGods.IndexOf(thisCard.God)];

		if (!SaveData.UnlockedCards.Contains (thisCard) && SaveData.UnlockedGods.Contains (thisCard.God)) {
			buyingThisCardBackground.enabled = true;
			buyingThisCardText.enabled = true;
			buyingThisCardText.text = "Buying this card will add it to your collection!";
		} else {
			buyingThisCardBackground.enabled = false;
			buyingThisCardText.enabled = false;
		}
	}
}
