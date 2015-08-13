using UnityEngine;
using System.Collections;

public class CardUI : MonoBehaviour {

	Card card;

	// Update is called once per frame
	public void Intialize () {
		card = gameObject.GetComponent<Card> ();
		TextMesh[] textMeshes = gameObject.GetComponentsInChildren<TextMesh>();
		foreach(TextMesh text in textMeshes) {
			if(text.gameObject.name == "name text") {
				text.fontSize = TitleFontSize;
				text.text = DisplayName;
			}
			else {
				text.fontSize = SmallFontSize;
				text.text = MiniDisplayText;
			}
			
			if(God == ShopControl.Gods.Ekcha | God == ShopControl.Gods.Ixchel) {
				text.color = new Color(1, 1, 1);
			}
		}
		
		CardText[] cardTexts = gameObject.GetComponentsInChildren<CardText>();
		SpriteRenderer[] renders = gameObject.GetComponentsInChildren<SpriteRenderer>();	
		foreach(SpriteRenderer render in renders) {
			if (render.gameObject.tag == "cardback") { continue; }
			else if (render.gameObject.name == "picture")
			{
				if (IconPath != "")
				{
					tempSprite = Resources.Load<Sprite>(IconPath);
					render.sprite = Resources.Load<Sprite>(IconPath);
				}
			}
			else if (render.gameObject.name == "rarity")
			{
				switch (tempLibraryCard.ThisRarity)
				{
				case Card.Rarity.Gold:
					render.sprite = shopControlGUI.Gold;
					Cost = 10;
					break;
				case Card.Rarity.Silver:
					render.sprite = shopControlGUI.Silver;
					Cost = 6;
					break;
				case Card.Rarity.Bronze:
					render.sprite = shopControlGUI.Bronze;
					Cost = 3;
					break;
				case Card.Rarity.Paper:
					render.sprite = shopControlGUI.Paper;
					Cost = 0;
					break;
				}
			}
			else if (render.gameObject.name == "god icon")
			{
				int godnum = ShopControl.AllGods.IndexOf(tempLibraryCard.God);
				
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
				godnum = ShopControl.AllGods.IndexOf(tempLibraryCard.God);
				
				render.sprite = shopControlGUI.GodSmallCards[godnum];
			}
		}
		foreach (CardText cardText in cardTexts) cardText.Initialize(101 - HandIndex() * 2);
	}

	//////////////////////////////////////
	/// Card specific tooltip
	//////////////////////////////////////
	
	public virtual void OnGUI() {
		if(Selected && Tooltip != "" && gameControl.Tooltip == ""){
			GUI.Box(new Rect(Screen.width * .02f, Screen.height * .68f, Screen.width * .8f, Screen.height * .08f), 
			        Tooltip, styleLibrary.CardStyles.Tooltip);
		}
		if(gameControl.CardsToTarget != 0 && Tooltip == "") {
			GUI.Box(new Rect(Screen.width*.02f, Screen.height*.72f, Screen.width*.8f, Screen.height*.04f), 
			        "Please select " + gameControl.CardsToTarget.ToString() + " cards", styleLibrary.CardStyles.Tooltip);
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
				if(!BehindPlayBoard &&(DiscardWhenPlayed | ForcingDiscardOfThis)){ 
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
				ForcingDiscardOfThis = false;
				FinishDiscard();
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
				DestroyThisGameObject();
			}
		}
		else if(Animating) {
			
			if(Time.time > DrawStartTime + .48f) {
				//this ends the animation
				Animating = false;
				DrawAnimating = false;	
				ShuffleAnimating = false;
				transform.localPosition = DrawEndPosition;
				if(!Discarded) {
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
