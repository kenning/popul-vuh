using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DisplayCardCanvas : MonoBehaviour {

	bool initialized = false;
	public bool CardDisplay = false;

	public GameObject CHILDGAMEOBJECT;
	public GameObject EXPOBACKGROUND;

	Text cardname;
	Text description;
	Image background;
	Image icon;
	Image godIcon;
	Image aoeIcon;
	Image rangeIcon;
	Image rarityIcon;

	bool undimAfter;

    void Start() {
        useGUILayout = false;
    }

	void initialize() {
		if(initialized) return;
		initialized = true;

		Text[] texts = CHILDGAMEOBJECT.GetComponentsInChildren<Text>();
		Image[] images = CHILDGAMEOBJECT.GetComponentsInChildren<Image>();

		foreach(Text txt in texts) {
			if(txt.gameObject.name == "Card name") cardname = txt;
			if(txt.gameObject.name == "Card description") description = txt;
		}
		foreach(Image img in images) {
			if(img.gameObject.name == "Card background") background = img;
			if(img.gameObject.name == "Icon") icon = img;
			if(img.gameObject.name == "God icon") godIcon = img;
			if(img.gameObject.name == "AOE icon") aoeIcon = img;
			if(img.gameObject.name == "Range icon") rangeIcon = img;
			if(img.gameObject.name == "Rarity icon") rarityIcon = img;
		}
	}

	// Exactly the same as the other Display() method.
	public void Display (LibraryCard card) {
		if (Tutorial.TutorialLevel != 0) return;

		initialize();

		undimAfter = false;

		CardDisplay = true;

		CHILDGAMEOBJECT.SetActive(true);
		EXPOBACKGROUND.SetActive(true);

		cardname.text = card.CardName.Replace ("\n", "");
		icon.sprite = Resources.Load<Sprite> ("sprites/card icons/" + card.IconPath);
		description.text = card.DisplayText;

		if (card.rangeMax != 0 && card.RangeTargetType != GridControl.TargetTypes.none) {
			rangeIcon.gameObject.SetActive(true);
			rangeIcon.sprite = Resources.Load<Sprite>("sprites/targeting icons/range " + card.RangeTargetType.ToString() + 
				" " + card.rangeMin.ToString() + "-" + card.rangeMax.ToString());
		} else {
			rangeIcon.gameObject.SetActive(false);
			rangeIcon.sprite = null;
		}

		if (card.aoeMaxRange != 0 && card.AoeTargetType != GridControl.TargetTypes.none) {
			aoeIcon.gameObject.SetActive(true);
			aoeIcon.sprite = Resources.Load<Sprite>("sprites/targeting icons/aoe " + card.AoeTargetType.ToString() + 
				" " + card.aoeMinRange.ToString() + "-" + card.aoeMaxRange.ToString());
		} else {
			aoeIcon.gameObject.SetActive(false);
			aoeIcon.sprite = null;
		}

		//default text color is black
		cardname.color = new Color(0,0,0);
		description.color = new Color(0,0,0);

		int godnum = ShopControl.AllGods.IndexOf (card.God);

		background.sprite = S.ShopControlGUIInst.GodDisplayCards[godnum];
		godIcon.sprite = S.ShopControlGUIInst.SpriteGodIcons [godnum];

		if(card.God == ShopControl.Gods.Ekcha | card.God == ShopControl.Gods.Ixchel) {
			cardname.color = new Color(1,1,1);
			description.color = new Color(1,1,1);
		}

		switch(card.ThisRarity) {
		case (Card.Rarity.Paper):
			rarityIcon.sprite = S.ShopControlGUIInst.Paper;
			break;
		case (Card.Rarity.Bronze):
			rarityIcon.sprite = S.ShopControlGUIInst.Bronze;
			break;
		case (Card.Rarity.Silver):
			rarityIcon.sprite = S.ShopControlGUIInst.Silver;
			break;
		case (Card.Rarity.Gold):
			rarityIcon.sprite = S.ShopControlGUIInst.Gold;
			break;
		}
	}

	public void Display (Card card) {
		initialize();

		undimAfter = true;

		CardDisplay = true;

		CHILDGAMEOBJECT.SetActive(true);
		EXPOBACKGROUND.SetActive(true);

		cardname.text = card.CardName.Replace ("\n", "");
		icon.sprite = Resources.Load<Sprite> (card.IconPath);
		description.text = card.DisplayText;

		if (card.maxRange != 0 && card.rangeTargetType != GridControl.TargetTypes.none) {
			rangeIcon.gameObject.SetActive(true);
			rangeIcon.sprite = Resources.Load<Sprite>("sprites/targeting icons/range " + card.rangeTargetType.ToString() + 
				" " + card.minRange.ToString() + "-" + card.maxRange.ToString());
		} else {
			rangeIcon.gameObject.SetActive(false);
			rangeIcon.sprite = null;
		}

		if (card.aoeMaxRange != 0 && card.aoeTargetType != GridControl.TargetTypes.none) {
			aoeIcon.gameObject.SetActive(true);
			aoeIcon.sprite = Resources.Load<Sprite>("sprites/targeting icons/aoe " + card.aoeTargetType.ToString() + 
				" " + card.aoeMinRange.ToString() + "-" + card.aoeMaxRange.ToString());
		} else {
			aoeIcon.gameObject.SetActive(false);
			aoeIcon.sprite = null;
		}

		//default text color is black
		cardname.color = new Color(0,0,0);
		description.color = new Color(0,0,0);

		int godnum = ShopControl.AllGods.IndexOf (card.God);

		background.sprite = S.ShopControlGUIInst.GodDisplayCards[godnum];
		godIcon.sprite = S.ShopControlGUIInst.SpriteGodIcons [godnum];

		if(card.God == ShopControl.Gods.Ekcha | card.God == ShopControl.Gods.Ixchel) {
			cardname.color = new Color(1,1,1);
			description.color = new Color(1,1,1);
		}

		switch(card.ThisRarity) {
		case (Card.Rarity.Paper):
			rarityIcon.sprite = S.ShopControlGUIInst.Paper;
			break;
		case (Card.Rarity.Bronze):
			rarityIcon.sprite = S.ShopControlGUIInst.Bronze;
			break;
		case (Card.Rarity.Silver):
			rarityIcon.sprite = S.ShopControlGUIInst.Silver;
			break;
		case (Card.Rarity.Gold):
			rarityIcon.sprite = S.ShopControlGUIInst.Gold;
			break;
		}
	}

	public void Undisplay() {
		CardDisplay = false;

		CHILDGAMEOBJECT.SetActive(false);
		if(undimAfter) EXPOBACKGROUND.SetActive(false);
	}
}
