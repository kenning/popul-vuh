using UnityEngine;
using System.Collections;

public class CardUI : MonoBehaviour {

    Vector3 normalLocalScale = new Vector3(.82f, .85f, .8f);

    public enum AnimatingType 
    {
        none,
        drawanimating,
        discardanimating,
        burnanimating,
        shuffleanimating,
        otheranimating
    };
    
    AnimatingType currentAnimatingType;

	Card card;
	GameControl gameControl;
	GUIStyleLibrary styleLibrary;
	ShopControlGUI shopControlGUI;

	//Animation variables
	GameObject discardPileObj;

	Transform DeckTransform;
	GameObject CardBackObject;
//	Sprite CardBackSprite;
	float AnimationStartTime;
	float DiscardMoveUpStartTime;
	Vector3 StartPosition;
	Vector3 HighestPoint;
	bool BehindPlayBoard = false;
	Vector3 AnimationEndPosition;
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
    float drawAnimationLength = .48f;

	void Start() {
		useGUILayout = false;
        // Not sure about this...
        gameObject.transform.localScale = normalLocalScale;
	}

	//////////////////////////////////////
	/// Animation start methods (+ finishdiscardanimate)
	//////////////////////////////////////

	public void DrawAnimate(int position) {
		gameObject.transform.parent = gameControl.handObj.transform;
		gameObject.transform.localPosition = new Vector3(-2.7f, .5f, 0);
		
		currentAnimatingType = AnimatingType.drawanimating;
        
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
		reorder(card.HandIndex(), false);
   
		transform.localScale = normalLocalScale;
   
		AnimationEndPosition = new Vector3((position)*1.55f - 1f, .1f, 0);
		if (currentAnimatingType != AnimatingType.drawanimating) {
            currentAnimatingType = AnimatingType.otheranimating;
        } 
		AnimationStartTime = Time.time;
	}

	public void MoveAnimateWhileDiscarded(int position, bool expand ) {
		if(expand) {
			AnimationEndPosition = new Vector3(0f,(position) * -.2f + -.5f, 0);
		} else {
			AnimationEndPosition = new Vector3(0f,(position) * -.5f + -.5f, 0);
		}
		currentAnimatingType = AnimatingType.otheranimating;
		AnimationStartTime = Time.time;
	}

	public void ShuffleMoveAnimate(Transform Deck) {
		DeckTransform = Deck;
		transform.parent = gameControl.handObj.transform;
		AnimationStartTime = Time.time;
		currentAnimatingType = AnimatingType.shuffleanimating;
	}

	public void DiscardAnimate() {
		DiscardMoveUpStartTime = Time.time;
		currentAnimatingType = AnimatingType.discardanimating;
		StartPosition = transform.localPosition;
		HighestPoint = StartPosition;
		HighestPoint.y = StartPosition.y + 2f;
	}

	public void FinishDiscardAnimate(bool ActuallyDiscarding) {
		currentAnimatingType = AnimatingType.none;
		if(ActuallyDiscarding) {

			int index = gameControl.Discard.IndexOf(gameObject);
			
			transform.parent = discardPileObj.transform;
			transform.localPosition = new Vector3(0,(index+1) * -.5f, 0);

            reorder(index, true);			
		}
	}

	public void TuckAnimate() {
		AnimationEndPosition = new Vector3(-2.7f, 3f, 0);
		currentAnimatingType = AnimatingType.otheranimating;
		AnimationStartTime = Time.time;
	}

	public void BurnAnimate() {
		AnimationStartTime = Time.time;
	    currentAnimatingType = AnimatingType.burnanimating;
	}

	public void PeekAnimate(int position, int maxPositions) {
		foreach(MeshRenderer meshrenderer in meshrenderers){
			meshrenderer.sortingLayerName = "Card";
			meshrenderer.sortingOrder = 101-position*2;
		}
		
		foreach(SpriteRenderer SRenderer in SRenderers) {
			if(SRenderer.gameObject.name == "rarity" | 
			   SRenderer.gameObject.name == "glow" | 
			   SRenderer.gameObject.name == "god icon" | 
			   SRenderer.gameObject.name == "picture" | 
			   SRenderer.gameObject.name == "aoe icon" | 
			   SRenderer.gameObject.name == "range icon" ) {
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
		if(currentAnimatingType == AnimatingType.none && 
           !card.Discarded && 
		   !card.ForcingDiscardOfThis) {
			
			int i = gameControl.Hand.IndexOf(gameObject);
			if(card != null) MoveAnimate(i); 
		}

	}
		
    //////////////////////////////////////
	/// Update
	//////////////////////////////////////
	public virtual void Update() {
		if(currentAnimatingType == AnimatingType.shuffleanimating) {
			if(Time.time > AnimationStartTime + .48f) {
                currentAnimatingType = AnimatingType.none;
				
				gameObject.GetComponent<SpriteRenderer>().enabled = false;
				return;
			}
			else {
				float time = Time.time - AnimationStartTime;
				transform.position = Vector3.Lerp(transform.position, DeckTransform.position, time);
			}
		}
		else if(currentAnimatingType == AnimatingType.discardanimating) {
			// Card is moving upwards
			if(DiscardMoveUpStartTime + .25f > Time.time) {
				float time =((Time.time-DiscardMoveUpStartTime));
				transform.localPosition = Vector3.Lerp(transform.localPosition, HighestPoint, time*4);
				return;
			}
			// Card switches to moving downwards
			else if(( DiscardMoveUpStartTime + .5f > Time.time ) &&(Time.time > DiscardMoveUpStartTime + .25f)) {  
				if(!BehindPlayBoard &&(card.DiscardWhenPlayed | card.ForcingDiscardOfThis)){ 
					SpriteRenderer[] spriterenderererers = GetComponentsInChildren<SpriteRenderer>();
					foreach(SpriteRenderer sr in spriterenderererers) {
						sr.sortingLayerName = "Field background";
						if (sr.gameObject.name != "rarity" 
						    && sr.gameObject.name != "glow" 
						    && sr.gameObject.name != "god icon" 
						    && sr.gameObject.name != "picture" 
						    && sr.gameObject.name != "aoe icon"  
						    && sr.gameObject.name != "range icon" )
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
					StartPosition = new Vector3(StartPosition.x, StartPosition.y, StartPosition.z);
					return;
				}
				float time =((Time.time-DiscardMoveUpStartTime - .25f));
				transform.localPosition = Vector3.Lerp(transform.localPosition, StartPosition, time*4);
				return;
			}
			else if( Time.time > DiscardMoveUpStartTime + .5f ) {
				FinishDiscardAnimate(card.DiscardWhenPlayed | card.ForcingDiscardOfThis);
                card.FinishDiscard();
			}
		} else if(currentAnimatingType == AnimatingType.drawanimating) {
			if(CardBackObject != null) {
                // Card distorts towards x scale of zero ("turns around" to become less visible to camera)
				gameObject.transform.localScale = Vector3.Lerp(new Vector3(gameObject.transform.localScale.x, .85f, 1f), new Vector3(0, .85f, 1f), Time.time-AnimationStartTime);
				
				if(Time.time >= AnimationStartTime + (drawAnimationLength/2)) {
                    // Halfway through the animation, the back of the card gets destroyed as the card "flips around"
					Destroy(CardBackObject);
				}
			}
			else {
                // Card distorts back to the full x scale ("turns back to face camera")
				gameObject.transform.localScale = Vector3.Lerp(gameObject.transform.localScale, normalLocalScale, Time.time-AnimationStartTime);
			}
			
			if(Time.time > AnimationStartTime + drawAnimationLength) {
				//this ends the animation
				currentAnimatingType = AnimatingType.none;
				//transform.localScale = normalLocalScale;
				
				transform.localPosition = AnimationEndPosition;
				return;
			}
			else {
				float time = Time.time - AnimationStartTime;
				
				transform.localPosition = Vector3.Lerp(transform.localPosition, AnimationEndPosition, time);
			}
		}
		else if(currentAnimatingType == AnimatingType.burnanimating) {
			float fade = Time.time - AnimationStartTime;
			foreach(SpriteRenderer SRenderer in SRenderers) {
				SRenderer.color = new Color(1f, 1f, 1f, 1-fade*2);
			}
			if(AnimationStartTime + .5f < Time.time) {
				card.DestroyThisGameObject();
			}
		}
		else if(currentAnimatingType == AnimatingType.otheranimating) {
			
			if(Time.time > AnimationStartTime + .48f) {
				//this ends the animation
				currentAnimatingType = AnimatingType.none;
				transform.localPosition = AnimationEndPosition;
				if(!card.Discarded) {
					transform.localScale = normalLocalScale;
				}
				
				return;
			}
			else if(Time.time > AnimationStartTime + .25f) {
				//midpoint of animation
				float time = Time.time - AnimationStartTime;
				transform.localPosition = Vector3.Lerp(transform.localPosition, AnimationEndPosition, time);
			}
			else {
				float time = Time.time - AnimationStartTime;
				transform.localPosition = Vector3.Lerp(transform.localPosition, AnimationEndPosition, time);
			}
		}
	}

    ///////////////////////////////
    /// Delegation utilities
    ///////////////////////////////
    
	public void TargetAnimate() {
		transform.Translate(new Vector3(0f, .25f, 0f));
	}

	public void UntargetAnimate() {
		transform.Translate(new Vector3(0f, -.25f, 0f));
	}

	public void ShineAnimate() {
		ShineAnim.Animate();
	}
	
	public void GlowAnimate(bool turnOn)
	{
		if(Glow.enabled != turnOn) Glow.enabled = turnOn;
	}

	public void ErrorAnimate() {
		PlayButton.ErrorAnimation();
	}

    /// A very long and boring initialize method
	public void Initialize (GameObject gameController, bool alreadyDiscarded) {
        //TODO do i need alreadyDiscarded here?
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
			else if (render.gameObject.name == "aoe icon" | render.gameObject.name == "range icon" ) 
			{
				string resourceLoadTarget = "sprites/targeting icons/";
				float baseIconSize = 2.4f;
				if(render.gameObject.name == "range icon") 
				{
					if(card.rangeTargetType == GridControl.TargetTypes.none) 
						continue;
					resourceLoadTarget += ("range " + card.rangeTargetType.ToString() + 
										  " " + card.minRange.ToString() + "-" + card.maxRange.ToString());
					baseIconSize = baseIconSize / (card.maxRange*2+1);
				}
				else if(render.gameObject.name == "aoe icon")
				{
					if(card.aoeTargetType == GridControl.TargetTypes.none) 
						continue;
					resourceLoadTarget += ("aoe " + card.aoeTargetType.ToString() + 
					                       " " + card.aoeMinRange.ToString() + "-" + card.aoeMaxRange.ToString());
					baseIconSize = baseIconSize / (card.aoeMaxRange*2+1);
				}
				render.sprite = Resources.Load<Sprite>(resourceLoadTarget);

				render.gameObject.transform.localScale = new Vector3(baseIconSize, baseIconSize, 0);
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
    
    void reorder (int index, bool discarding) {
        
        if (discarding) index = -index;
        
		foreach(MeshRenderer meshrenderer in meshrenderers){
            if (discarding) meshrenderer.sortingLayerName = "Discard and deck";
            else meshrenderer.sortingLayerName = "Card";            
            
			meshrenderer.sortingOrder = 101-index*2;
		}
		
		foreach(SpriteRenderer SRenderer in SRenderers) {
            if (discarding) SRenderer.sortingLayerName = "Discard and deck";
            else SRenderer.sortingLayerName = "Card";            

			if(SRenderer.gameObject.name == "rarity" | 
			   SRenderer.gameObject.name == "god icon" | 
			   SRenderer.gameObject.name == "picture" | 
			   SRenderer.gameObject.name == "aoe icon" | 
			   SRenderer.gameObject.name == "range icon" ) {
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
    }
}
