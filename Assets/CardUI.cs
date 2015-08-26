﻿using UnityEngine;
using System.Collections;

public class CardUI : MonoBehaviour {

	Card card;
	GameControl gameControl;
	GUIStyleLibrary styleLibrary;
	ShopControlGUI shopControlGUI;

	//Animation variables
	GameObject discardPileObj;

	bool Animating = false;
	bool DrawAnimating = false;
	bool DiscardAnimating = false;
	bool BurnAnimating = false; 
	bool ShuffleAnimating = false;
	Transform DeckTransform;
	GameObject CardBackObject;
//	Sprite CardBackSprite;
	float DrawStartTime;
	float MoveUpStartTime;
	Vector3 StartPosition;
	Vector3 HighestPoint;
	bool BehindPlayBoard = false;
	Vector3 DrawEndPosition;
	Vector3 DiscardEndPosition;
	ButtonAnimate PlayButton;
	SpriteRenderer[] SRenderers;
	MeshRenderer[] meshrenderers;
	ShineAnimation ShineAnim;
	SpriteRenderer Glow;

	//Font size stuff
	int SmallCardTitleFontSize = 40;
	int SmallCardRulesFontSize = 35;
	public int DisplayTitleFontSize = 35;
	public int DisplayRulesFontSize = 25;
	int FontVariableMaxTitleWordLength = 5;
	int FontVariableMaxRulesLength = 25;
	int FontVariableMaxMiniRulesLength = 15;
	int DisplayFontVariableRulesSizeRatio = 3;
	int SmallCardFontVariableRulesSizeRatio = 10;

	void Start() {
		useGUILayout = false;
	}

	//////////////////////////////////////
	/// Universal card methods: animation components
	//////////////////////////////////////

	public void TargetAnimate() {
		transform.Translate(new Vector3(0f, .25f, 0f));
	}

	public void UntargetAnimate() {
		transform.Translate(new Vector3(0f, -.25f, 0f));
	}

	public void ShineAnimate()
	{
		ShineAnim.Animate();
	}
	
	public void GlowAnimate(bool turnOn)
	{
		if(Glow.enabled != turnOn) Glow.enabled = turnOn;
	}

	public void ErrorAnimate() {
		PlayButton.ErrorAnimation();
	}

	public void DrawAnimate(int position) {
		gameObject.transform.parent = gameControl.handObj.transform;
		gameObject.transform.localPosition = new Vector3(-2.7f, .5f, 0);
		
		DrawAnimating = true;
		CardBackObject =(GameObject)GameObject.Instantiate(Resources.Load("prefabs/Drawn card prefab"));
		CardBackObject.transform.parent = gameObject.transform;
		CardBackObject.transform.localPosition = new Vector3(0, 0, 0);
		CardBackObject.transform.localScale = new Vector3(2.9f, 2.8f, 2f);
		SpriteRenderer CardBackSR = CardBackObject.GetComponent<SpriteRenderer>();
		CardBackSR.sortingLayerID = 0;
		CardBackSR.sortingOrder = 4;
		
		MoveAnimate(position);
	}
	
	public void MoveAnimate(int position) {
		
		int index = card.HandIndex();
		
		foreach(MeshRenderer meshrenderer in meshrenderers){
			meshrenderer.sortingLayerName = "Card";
			meshrenderer.sortingOrder = 101-index*2;
		}
		
		foreach(SpriteRenderer SRenderer in SRenderers) {
			if(SRenderer.gameObject.name == "rarity" | SRenderer.gameObject.name == "god icon" | SRenderer.gameObject.name == "picture") {
				SRenderer.sortingOrder = 101-index*2;
			}
			//this catches the card background VVV
			else if (SRenderer.gameObject.name == "glow" | SRenderer.gameObject.name == "shine animation")
			{
				//it's already at sorting order 3000. don't do anything
			}
			else {
				SRenderer.sortingOrder = 100-index*2;
			}
		}
		DrawEndPosition = new Vector3((position)*1.55f - 1f, .1f, 0);
		Animating = true;
		DrawStartTime = Time.time;
	}

	public void MoveAnimateWhileDiscarded(int position, bool expand ) {
		if(expand) {
			DrawEndPosition = new Vector3(0f,(position) * -.2f + -.5f, 0);
		} else {
			DrawEndPosition = new Vector3(0f,(position) * -.5f + -.5f, 0);
		}
		Animating = true;
		DrawStartTime = Time.time;
	}

	public void ShuffleMoveAnimate(Transform Deck) {
		DeckTransform = Deck;
		transform.parent = gameControl.handObj.transform;
		DrawStartTime = Time.time;
		ShuffleAnimating = true;
	}

	public void DiscardAnimate() {
		MoveUpStartTime = Time.time;
		DiscardAnimating = true;
		StartPosition = transform.localPosition;
		HighestPoint = StartPosition;
		HighestPoint.y = StartPosition.y + 2f;
	}

	public void FinishDiscardAnimate(bool ActuallyDiscarding) {
		DiscardAnimating = false;
		if(ActuallyDiscarding) {
			Animating = false;
			BurnAnimating = false;
			DrawAnimating = false;
			ShuffleAnimating = false;
			
			int index = gameControl.Discard.IndexOf(gameObject);
			
			transform.parent = discardPileObj.transform;
			transform.localPosition = new Vector3(0,(index+1) * -.2f, 0);
			
			SpriteRenderer[] SRenderers = gameObject.GetComponentsInChildren<SpriteRenderer>();
			foreach(SpriteRenderer SRenderer in SRenderers) {
				if(SRenderer.gameObject.name == "rarity" | SRenderer.gameObject.name == "glow" | SRenderer.gameObject.name == "god icon" | SRenderer.gameObject.name == "picture") {
					SRenderer.sortingLayerName = "Card";
					SRenderer.sortingOrder = 101+index*2;
				}
				else {
					SRenderer.sortingLayerName = "Card";
					SRenderer.sortingOrder = 100+index*2;
				}
			}
			MeshRenderer[] meshes = gameObject.GetComponentsInChildren<MeshRenderer>();
			foreach(MeshRenderer mesh in meshes) {
				mesh.sortingLayerName = "Card";
				mesh.sortingOrder = 101+index*2;
			}
		}
	}

	public void TuckAnimate() {
		DrawEndPosition = new Vector3(-2.7f, 3f, 0);
		Animating = true;
		DrawStartTime = Time.time;
	}

	public void BurnAnimate() {
		DrawStartTime = Time.time;
		BurnAnimating = true;
	}

	public void PeekAnimate(int position, int maxPositions) {
		foreach(MeshRenderer meshrenderer in meshrenderers){
			meshrenderer.sortingLayerName = "Card";
			meshrenderer.sortingOrder = 101-position*2;
		}
		
		foreach(SpriteRenderer SRenderer in SRenderers) {
			if(SRenderer.gameObject.name == "rarity" | SRenderer.gameObject.name == "glow" | SRenderer.gameObject.name == "god icon" | SRenderer.gameObject.name == "picture") {
				SRenderer.sortingOrder = 101-position*2;
			}
			//this catches the card background VVV
			else {
				SRenderer.sortingOrder = 100-position*2;
			}
		}
		
		transform.localPosition = new Vector3(position + -1f, 1f, 0);
	}

	public void TryToMoveAnimate () {
		if(!card.Discarded && 
		   !BurnAnimating && 
		   !card.Peeked && 
		   !DiscardAnimating && 
		   !card.ForcingDiscardOfThis) {
			
			int i = gameControl.Hand.IndexOf(gameObject);
			if(card != null) MoveAnimate(i); 
		}

	}

	//////////////////////////////////////
	/// OnGUI: Only for tooltip
	//////////////////////////////////////
	
	public virtual void OnGUI() {
		if(card.Selected && card.Tooltip != "" && gameControl.Tooltip == ""){
			GUI.Box(new Rect(Screen.width * .02f, Screen.height * .68f, Screen.width * .8f, Screen.height * .08f), 
			        card.Tooltip, styleLibrary.CardStyles.Tooltip);
		}
		if(gameControl.CardsToTarget != 0 && card.Tooltip == "") {
			GUI.Box(new Rect(Screen.width*.02f, Screen.height*.72f, Screen.width*.8f, Screen.height*.04f), 
			        "Please ` " + gameControl.CardsToTarget.ToString() + " cards", styleLibrary.CardStyles.Tooltip);
		}
	}

	//////////////////////////////////////
	/// delete this header later and move initialize to the top
	//////////////////////////////////////

	public void Initialize (GameObject gameController) {
		card = gameObject.GetComponent<Card> ();
		
		gameControl = gameController.GetComponent<GameControl> ();
		shopControlGUI = gameController.GetComponent<ShopControlGUI> ();
		styleLibrary = gameController.GetComponent<GUIStyleLibrary> ();
		
		PlayButton = GameObject.Find("play end button").GetComponent<ButtonAnimate>();
		meshrenderers = GetComponentsInChildren<MeshRenderer>();
		SRenderers = gameObject.GetComponentsInChildren<SpriteRenderer>();
//		CardBackSprite = Resources.Load("sprites/cards/real pixel card back") as Sprite;
		discardPileObj = GameObject.Find("Discard pile");
		
		TextMesh[] textMeshes = gameObject.GetComponentsInChildren<TextMesh>();
		foreach(TextMesh text in textMeshes) {
			if(text.gameObject.name == "name text") {
				text.fontSize = SmallCardTitleFontSize;
				text.text = card.DisplayName;
			}
			else {
				text.fontSize = SmallCardRulesFontSize;
				text.text = card.MiniDisplayText;
			}
			
			if(card.God == ShopControl.Gods.Ekcha | card.God == ShopControl.Gods.Ixchel) {
				text.color = new Color(1, 1, 1);
			}
		}
		
		CardText[] cardTexts = gameObject.GetComponentsInChildren<CardText>();
		SpriteRenderer[] renders = gameObject.GetComponentsInChildren<SpriteRenderer>();	
		foreach(SpriteRenderer render in renders) {
			if (render.gameObject.tag == "cardback") { continue; }
			else if (render.gameObject.name == "picture")
			{
				if (card.IconPath != "")
				{
					render.sprite = Resources.Load<Sprite>(card.IconPath);
				}
			}
			else if (render.gameObject.name == "rarity")
			{
				switch (card.ThisRarity)
				{
				case Card.Rarity.Gold:
					render.sprite = shopControlGUI.Gold;
					break;
				case Card.Rarity.Silver:
					render.sprite = shopControlGUI.Silver;
					break;
				case Card.Rarity.Bronze:
					render.sprite = shopControlGUI.Bronze;
					break;
				case Card.Rarity.Paper:
					render.sprite = shopControlGUI.Paper;
					break;
				}
			}
			else if (render.gameObject.name == "god icon")
			{
				int godnum = ShopControl.AllGods.IndexOf(card.God);
				
				render.sprite = shopControlGUI.SpriteGodIcons[godnum];
			}
			else if (render.gameObject.name == "shine animation")
			{
				ShineAnim = render.gameObject.GetComponent<ShineAnimation>();
			}
			else if (render.gameObject.name == "shine animation 2") { continue; }
			else if (render.gameObject.name == "glow")
			{
				Glow = render.gameObject.GetComponent<SpriteRenderer>();
			}
			else if (render.gameObject.name != "picture")
			{
				int godnum = 3;
				godnum = ShopControl.AllGods.IndexOf(card.God);
				
				render.sprite = shopControlGUI.GodSmallCards[godnum];
			}
		}
		foreach (CardText cardText in cardTexts) cardText.Initialize(101 - card.HandIndex() * 2);

		//Sets up font sizes
		string[] words = card.CardName.Split (' ');
		int longestWordLength = words [0].Length;
		for (int i = 0; i < words.Length; i++) {
			if(words[i].Length > longestWordLength) longestWordLength = words[i].Length;
		}
		if (longestWordLength > FontVariableMaxTitleWordLength) {
			SmallCardTitleFontSize = SmallCardTitleFontSize*FontVariableMaxTitleWordLength/longestWordLength;
			DisplayTitleFontSize = DisplayTitleFontSize*FontVariableMaxTitleWordLength/longestWordLength;
		}
		
		if (card.DisplayText.Length > FontVariableMaxRulesLength) {
			// ultimate font size = maxlength - (difference * ratio)
			DisplayRulesFontSize = DisplayRulesFontSize -  ((card.DisplayText.Length - FontVariableMaxRulesLength) / SmallCardFontVariableRulesSizeRatio);
		}
		if (card.MiniDisplayText.Length > FontVariableMaxMiniRulesLength) {
			SmallCardRulesFontSize = SmallCardRulesFontSize - ((card.DisplayText.Length - FontVariableMaxRulesLength) / DisplayFontVariableRulesSizeRatio);
		}

	}

	
	//////////////////////////////////////
	/// Update: just for animations
	//////////////////////////////////////
	
	public virtual void Update() {
		if(ShuffleAnimating) {
			if(Time.time > DrawStartTime + .48f) {
				Animating = false;
				DrawAnimating = false;	
				ShuffleAnimating = false;
				
				gameObject.GetComponent<SpriteRenderer>().enabled = false;
				return;
			}
			else {
				float time = Time.time - DrawStartTime;
				transform.position = Vector3.Lerp(transform.position, DeckTransform.position, time);
			}
		}
		else if(DiscardAnimating) {
			if(MoveUpStartTime + .25f > Time.time) {
				float time =((Time.time-MoveUpStartTime));
				transform.localPosition = Vector3.Lerp(transform.localPosition, HighestPoint, time*4);
				
				return;
			}
			else if(( MoveUpStartTime + .5f > Time.time ) &&(Time.time > MoveUpStartTime + .25f)) {  
				if(!BehindPlayBoard &&(card.DiscardWhenPlayed | card.ForcingDiscardOfThis)){ 
					SpriteRenderer[] spriterenderererers = GetComponentsInChildren<SpriteRenderer>();
					foreach(SpriteRenderer sr in spriterenderererers) {
						sr.sortingLayerName = "Field background";
						if (sr.gameObject.name != "rarity" && sr.gameObject.name != "glow" && sr.gameObject.name != "god icon" && sr.gameObject.name != "picture")
						{
							sr.sortingOrder = 0;
						}
						else
						{
							sr.sortingOrder = 1;
						}
					}
					MeshRenderer[] meshrenderers = GetComponentsInChildren<MeshRenderer>();
					foreach(MeshRenderer meshrenderer in meshrenderers){
						meshrenderer.sortingLayerName = "Field background";
						meshrenderer.sortingOrder = 1;
					}
					
					BehindPlayBoard = true;
					StartPosition = new Vector3(StartPosition.x, StartPosition.y-1f, StartPosition.z);
					return;
				}
				float time =((Time.time-MoveUpStartTime - .25f));
				transform.localPosition = Vector3.Lerp(transform.localPosition, StartPosition, time*4);
				return;
			}
			else if( Time.time > MoveUpStartTime + .5f ) {
				card.FinishDiscard();
				return;
			}
		}
		else if(DrawAnimating) {
			if(CardBackObject != null) {
				gameObject.transform.localScale = Vector3.Lerp(new Vector3(gameObject.transform.localScale.x, .85f, 1f), new Vector3(0, .85f, 1f), Time.time-DrawStartTime);
				
				if(Time.time >= DrawStartTime + .25f) {
					Destroy(CardBackObject);
				}
			}
			else {
				gameObject.transform.localScale = Vector3.Lerp(gameObject.transform.localScale, new Vector3(.82f, .85f, .8f), Time.time-DrawStartTime);
			}
			
			if(Time.time > DrawStartTime + .48f) {
				//this ends the animation
				Animating = false;
				DrawAnimating = false;	
				transform.localScale = new Vector3(.82f, .85f, .8f);
				
				transform.localPosition = DrawEndPosition;
				return;
			}
			else {
				float time = Time.time - DrawStartTime;
				
				transform.localPosition = Vector3.Lerp(transform.localPosition, DrawEndPosition, time);
			}
		}
		else if(BurnAnimating) {
			float fade = Time.time - DrawStartTime;
			foreach(SpriteRenderer SRenderer in SRenderers) {
				SRenderer.color = new Color(1f, 1f, 1f, 1-fade*2);
			}
			if(DrawStartTime + .5f < Time.time) {
				card.DestroyThisGameObject();
			}
		}
		else if(Animating) {
			
			if(Time.time > DrawStartTime + .48f) {
				//this ends the animation
				Animating = false;
				DrawAnimating = false;	
				ShuffleAnimating = false;
				transform.localPosition = DrawEndPosition;
				if(!card.Discarded) {
					transform.localScale = new Vector3(.82f, .85f, 0);
				}
				
				return;
			}
			else if(Time.time > DrawStartTime + .25f) {
				//midpoint of animation
				float time = Time.time - DrawStartTime;
				transform.localPosition = Vector3.Lerp(transform.localPosition, DrawEndPosition, time);
			}
			else {
				float time = Time.time - DrawStartTime;
				transform.localPosition = Vector3.Lerp(transform.localPosition, DrawEndPosition, time);
			}
		}
	}

}
