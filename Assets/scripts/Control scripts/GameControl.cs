using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameControl : MonoBehaviour
{

	#region Variables
	public static string Error = "";

	public static bool MovesArePlays = false;

	public ClickControl clickBoss;
	public ShopControl shopBoss;
	public EventGUI eventGUIBoss;
	public GridControl gridBoss;

	public GameObject handObj;
	public GameObject playBoardObj;
	public GameObject deckObj;
	public GameObject playerObj;
	public CardLibrary library;
	public EnemyLibrary enemyLibrary;
	public Player player;
	public List<GameObject> EnemyObjs;
	
	//Game session data
	public static int Level = 0;
	public int PlaysLeft = 1;
	public int MovesLeft = 1;
	public int Dollars = 0;
	public int BleedingTurns = 0;
	public int SwollenTurns = 0;
	public int HungerTurns = 0;
    public enum SickTypes { Bleeding, Swollen, Hunger };
	
	//Game session data: card lists
	public List<string> Deck;
	public List<GameObject> Hand;
	public List<GameObject> Discard;
	public List<GameObject> PeekedCards;
	
	//Card targeting stuff
	public int CardsToTarget = 0;
	public bool CardsToTargetAreDiscarded;
	public bool CardsToTargetArePeeked;
	public List<GameObject> TargetedCards;
	public Card TargetCardCallback;

	//for complex cards which require you to click on a bunch of stuff
	public Card TargetSquareCallback;
	
	//card display
	public GUISkin GUISKIN;
	SpriteRenderer displayCardRenderer;
	SpriteRenderer dimmer;
	public bool CardDisplay = false;
	GameObject cardObjFromDeck;
	string DisplayName;
	string DisplayRules;
	Texture2D DisplayRangeTexture;
	float DisplayRangeSize;
	Texture2D DisplayAOETexture;
	float DisplayAOESize;
	Sprite DisplayCard;
	Texture2D DisplayCardIcon;
	Texture2D DisplayRarity;
	Texture2D DisplayIcon;
	GUIStyle DisplayTitleFont;
	GUIStyle DisplayTextFont;
	
	//UI data
	bool showingDeck;
	public bool showingDiscard;
	public string Tooltip = "";

	//UI variables
	Vector3 originalDiscardPlacement = new Vector3(3f, 2f, 0);
	Vector3 displayDiscardPlacement = new Vector3(3f, 6.5f, 0);
	TextMesh playsLeftText;
	TextMesh movesLeftText;
	TextMesh dollarsText;
	TextMesh deckText;
	ButtonAnimate playButton;
	ButtonAnimate moveButton;
	ButtonAnimate endTurnButton;
	Sprite endTurn;
	GUISkin gooeyskin;
	#endregion


	void Awake(){

		EventControl.NewLevelReset ();
		QControl.Initialize ();

		playsLeftText = GameObject.Find ("plays left #").GetComponent<TextMesh> ();
		movesLeftText = GameObject.Find ("moves left #").GetComponent<TextMesh> ();
		dollarsText = GameObject.Find ("Dollar count").GetComponent<TextMesh> ();
		deckText = GameObject.Find ("Deck count").GetComponent<TextMesh> ();
		playButton = GameObject.Find ("play end button").GetComponent<ButtonAnimate> ();
		moveButton = GameObject.Find ("move end button").GetComponent<ButtonAnimate> ();
		endTurnButton = GameObject.Find ("end turn button").GetComponent<ButtonAnimate> ();
		displayCardRenderer = GameObject.Find ("Display card").GetComponent<SpriteRenderer> ();
		handObj = GameObject.Find ("Hand");
		playBoardObj = GameObject.Find ("Play board");
		playerObj = GameObject.FindGameObjectWithTag ("Player");
		dimmer = GameObject.Find ("Dimmer").GetComponent<SpriteRenderer>();
		shopBoss = gameObject.GetComponent<ShopControl> ();
		player = GameObject.FindGameObjectWithTag ("Player").GetComponent<Player> ();
		gooeyskin = (GUISkin)Resources.Load ("GUISkins/battleboss guiskin");
		
		library = gameObject.GetComponent<CardLibrary> ();
		enemyLibrary = gameObject.GetComponent<EnemyLibrary>();
		clickBoss = gameObject.GetComponent<ClickControl> ();
		gridBoss = gameObject.GetComponent<GridControl>();

		library.Startup ();
		enemyLibrary.Startup();
		shopBoss.Initialize ();
		shopBoss.goalExpo = false;
		gameObject.GetComponent<Tutorial>().Initialize();

		SaveLoad.Load ();

		if(SaveData.UnlockedCards.Count == 0 | SaveData.UnlockedGods.Count == 0) {
			SaveData.NewSaveFile ();
		}

		MainMenu.UnlockCheck();
	}

	public void BeginGame () {


		Deck = new List<string> ();
		Hand = new List<GameObject> ();
		Discard = new List<GameObject> ();
		TargetedCards = new List<GameObject> ();
		EnemyObjs = new List<GameObject> ();
		GameObject discard = GameObject.FindGameObjectWithTag ("Discard");
		discard.transform.localPosition = originalDiscardPlacement;
        deckObj = GameObject.Find("Deck");


		GameObject[] EnemyObjects = GameObject.FindGameObjectsWithTag ("Enemy");
		if(EnemyObjects.Length > 0) {
			for(int i = EnemyObjects.Length - 1; i >= 0; i--) {
				Destroy(EnemyObjects[i]);
			}
		}
		
		library.Startup ();
		shopBoss.Initialize ();
		player.ResetLife ();

		//DIFFERENT IN TUTORIAL!
		if (Tutorial.TutorialLevel == 0)
		{
			ShopControl.Normaldisplay = true;
			library.SetStartingItems();
		}


		GameObject[] cards = GameObject.FindGameObjectsWithTag ("Card");
		foreach(GameObject GO in cards) {
			Destroy(GO);
		}
		Hand = new List<GameObject> ();

		for(int i = 0; i < library.StartingItems.Count; i++) {
			string tempString = library.StartingItems[i].CardName;
			Deck.Add(tempString);
		}

		StartNewLevel ();
	}
	
	void OnGUI () {

	//	if(Error != "") GUI.Box (new Rect (0, 0, Screen.width, Screen.height*.03f), Error);

//		if(!MainMenu.MainMenuUp && !UnlockMenu.UnlockMenuUp && !GodChoiceMenu.GodChoiceMenuUp && ) {
//			GUI.Box (new Rect (Screen.width*.33f, Screen.height * .71f, Screen.width * .1f, Screen.height * .03f), Deck.Count.ToString(), gooeyskin.customStyles [1]);
//		}
//
		if (showingDeck) {
			string tempString = "Cards left in \nthe deck:\n";
			GUI.Box(new Rect(0, 0, Screen.width*.2f, Screen.height*(Deck.Count+3)*.03f), tempString);
			for(int i = 0; i < Deck.Count; i++) {
				tempString += ((i+1) + ". " + Deck[i]);
				while(GUI.Button(new Rect(Screen.width*.01f, Screen.height*.03f*(i+2), Screen.width*.18f, Screen.height*.028f), Deck[i])){
					string stringCardToDraw = "prefabs/cards/" + Deck[i] + " card";
					cardObjFromDeck = (GameObject)GameObject.Instantiate (Resources.Load (stringCardToDraw));
					Card tempCard = cardObjFromDeck.GetComponent<Card>();
					Display(tempCard);
				}
			}
		}

		if (CardDisplay) {
			GUI.BeginGroup (new Rect (Screen.width*.25f, Screen.height*.2f, Screen.width*.5f, Screen.height*.55f), "");
			GUI.Box (new Rect (0,0, Screen.width*.25f, Screen.height*.2f), DisplayName, DisplayTitleFont);
			GUI.Box (new Rect (Screen.width*.3f, 0, Screen.width*.2f, Screen.height*.2f), DisplayCardIcon, DisplayTitleFont);
			GUI.Box(new Rect(0, Screen.height*.2f, Screen.width*.5f, Screen.height*.4f), DisplayRules, DisplayTextFont); 
			if(DisplayRangeTexture != null) GUI.DrawTexture(new Rect(Screen.width*.05f, Screen.height*.38f, Screen.width*.1f*DisplayRangeSize, Screen.width*.1f*DisplayRangeSize), DisplayRangeTexture);
			if(DisplayAOETexture != null) GUI.DrawTexture(new Rect(Screen.width*.25f, Screen.height*.38f, Screen.width*.1f*DisplayAOESize, Screen.width*.1f*DisplayAOESize), DisplayAOETexture);
			GUI.DrawTexture(new Rect(Screen.width*.0f, Screen.height*.44f, Screen.width*.05f, Screen.width*.05f), DisplayRarity);
			GUI.DrawTexture(new Rect(Screen.width*.4f, Screen.height*.44f, Screen.width*.08f, Screen.width*.08f), DisplayIcon);
			GUI.EndGroup();
		}

		if (Tooltip != "") {
			GUI.Box(new Rect(Screen.width*.02f, Screen.height*.68f, Screen.width*.8f, Screen.height*.08f), Tooltip, gooeyskin.textArea);
		}
	}

	#region Card related methods: Draw(), Peek(), Return()
	#region Draw methods
	public void Draw(string SpecificCard, bool isInvisible) {
		DeselectCards ();

		if(Deck.Count == 0) {
			Debug.Log("oops no cards");
			deckObj.GetComponent<DeckAnimate>().ErrorAnimate();
			return;
		}

		string cardClass;
		if(SpecificCard == "") {
			int randomNumber = Random.Range (0, Deck.Count);
			cardClass = Deck[randomNumber];

			Deck.RemoveAt (randomNumber);
		}
		else {
			cardClass = SpecificCard;
			Deck.Remove(cardClass);
		}

		//stringCardToDraw = "prefabs/cards/" + stringCardToDraw + " card";
		string dummyCard = "prefabs/dummy card";

		GameObject newCardObj = (GameObject)GameObject.Instantiate (Resources.Load (dummyCard));

		string libraryCardName = cardClass;
		cardClass = cardClass.Replace (" ", "");
		cardClass = cardClass.Replace ("'", "");
		cardClass = cardClass.Replace ("-", "");
		newCardObj.AddComponent(System.Type.GetType(cardClass));

		//VV this is here and not in the start() method because it doesn't get called if the card is revealed
		Card newCardScript = newCardObj.GetComponent<Card> ();
		newCardScript.CardName = libraryCardName;
		newCardScript.Initialize ();	
		DrawIntoHand (newCardScript, isInvisible);

		CheckDeckCount ();
		Invoke ("AnimateCardsToCorrectPosition", .3f);
	}
	public void Draw(bool isInvisible) { 
		Draw ("", isInvisible);
	}
	public void Draw() {
		Draw (false);
	}
	public void InvisibleDraw () {
		Draw (true);
	}
	public void DrawIntoHand(Card card, bool invisibleDraw) {
		card.DrawAnimate (Hand.Count-1);	
		
		if(!invisibleDraw) { 
			shopBoss.GoalCheck("Draw X cards in one turn");
			if(card.ThisRarity == Card.Rarity.Paper) {
				shopBoss.GoalCheck("Draw X paper cards in one turn");
			}
		}
	}
	#endregion

	/// <summary>
	/// Peek method. PeekCallback() will be called immediately after peeking, not after card selection
	/// </summary>
	/// <param name="numberOfCards"></param>
	/// <param name="CallbackCard"></param>
	public void Peek (int numberOfCards, Card CallbackCard) {
		DeselectCards ();

		if (Deck.Count < numberOfCards) {
			Debug.Log ("oops not enough cards");
			deckObj = GameObject.Find ("Deck");
			deckObj.GetComponent<DeckAnimate> ().ErrorAnimate ();
			numberOfCards = Deck.Count;
			return;
		}

		for(int i = 0; i < numberOfCards; i++) {
			string cardClass;
			int randomNumber = Random.Range (0, Deck.Count);
			cardClass = Deck [randomNumber];
			string libraryCardName = cardClass;
	
			Deck.RemoveAt (randomNumber);
	
			string stringCardToDraw = "prefabs/dummy card";
			GameObject newCardObj = (GameObject)GameObject.Instantiate (Resources.Load (stringCardToDraw));
			cardClass = cardClass.Replace (" ", "");
			cardClass = cardClass.Replace ("'", "");
			UnityEngineInternal.APIUpdaterRuntimeServices.AddComponent(newCardObj, "Assets/scripts/Control scripts/GameControl.cs (310,4)", cardClass);

			Card newCardScript = newCardObj.GetComponent<Card>();
			newCardScript.CardName = libraryCardName;
			newCardScript.Initialize();
			newCardScript.Peek(i, numberOfCards);
			PeekedCards.Add(newCardObj);
			dimmer.enabled = true;
		}

		CallbackCard.PeekCallback ();
	}

	/// <summary>
	/// Return a card to the deck
	/// </summary>
	/// <param name="ReturnCardObj"></param>
	public void Return (GameObject ReturnCardObj) {
		CardText[] cardTexts = ReturnCardObj.GetComponentsInChildren<CardText> ();
		foreach(CardText cardText in cardTexts) cardText.Initialize (Hand.Count * 2 + 1);
		SpriteRenderer[] renders = ReturnCardObj.GetComponentsInChildren<SpriteRenderer> ();	
		foreach(SpriteRenderer render in renders) {
			render.sortingOrder = Hand.Count * 2;
			render.sortingLayerID = 2;
		}

		Hand.Add (ReturnCardObj);
		Discard.Remove (ReturnCardObj);

		Card card = ReturnCardObj.GetComponent<Card> ();
		card.Discarded = false;

		ReturnCardObj.transform.parent = handObj.transform;
		//ReturnCardObj.transform.position = new Vector3 (2.7f, -4.8f, 0);

		card.MoveAnimate (Hand.Count-1);			
	}
	#endregion

	#region Turn taking methods: StartNewLevel(), StartNewTurn(), EnemyTurn()
	/// <summary>
	/// Load enemies, Invisibledraw 3 cards, start turn.
	/// </summary>
	public void StartNewLevel () {

		handObj.transform.localPosition = new Vector3 (-0.8f, 0, 0);

		playerObj.GetComponent<GridUnit> ().xPosition = 0;
		playerObj.GetComponent<GridUnit> ().yPosition = 0;
		Camera.main.transform.position = new Vector3 (0, -1, -10);
		playerObj.transform.position = new Vector3 (0, 0, 0);

		//DIFFERENT IN TUTORIAL!
		if (Tutorial.TutorialLevel == 0)
		{
			Level++;

			int numberOfGods = 3;
			if (Level < 3)
			{
				numberOfGods = Level;
			}

			shopBoss.NewLevelNewGoals(numberOfGods);

			gridBoss.LoadEnemiesAndObstacles(Level);
			InvisibleDraw();
			InvisibleDraw();
			InvisibleDraw();
		}

		StartNewTurn();
	
		clickBoss.AllowInputUmbrella = false;
	}

	/// <summary>
	/// This method draws and resets goals. If stunned, return; otherwise get your 1 play and 1 move.
	/// Behaves very differently if Tutorial.TutorialLevel != 0.
	/// </summary>
	public void StartNewTurn() {

        if (!player.alive)
            return;

        EventControl.NewLevelReset();

		foreach(Goal g in shopBoss.Goals) {
			g.NewTurnCheck();
		}

		Dim (false);

	//	if (player.StunnedForXTurns != 0)
	//	{
	//		player.StunnedForXTurns--;
	//		Invoke("EnemyTurn", .1f);
	//		return;
	//	}

		//DIFFERENT IN TUTORIAL!
		if (Tutorial.TutorialLevel == 0)
		{
			if (HungerTurns < 2)
				Draw();
			if (HungerTurns > 0)
				HungerTurns--;

			if (BleedingTurns < 2)
				SetMoves(1);
			if (BleedingTurns > 0)
				BleedingTurns--;

			if (SwollenTurns < 2)
				SetPlays(1);
			if (SwollenTurns > 0)
				SwollenTurns--;

			UICheck();

			clickBoss.turnEndedAlready = false;
			clickBoss.AllowEveryInput();
			clickBoss.cardScriptClickedOn = null;

			EventControl.NewTurnReset();

			//sets enemy's moves and actions to their max, preparing them for their turn.
			GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
			foreach (GameObject badguy in enemies)
			{
				Enemy en = badguy.GetComponent<Enemy>();
				if (en != null)
					en.GoToMax();
			}
		}
	}

    public void EnemyTurn() { EnemyTurn(false);  }
	public void EnemyTurn(bool startingEnemyTurn) {

        if (!player.alive) return;

        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        if (enemies.Length == 0)
        {
            LevelIsDone();
            return;
        }

        if (startingEnemyTurn)
        {
	    	clickBoss.DisallowEveryInput ();
        }

        bool turnIsOver = true;
        foreach (GameObject badguy in enemies)
		{
			Enemy en = badguy.GetComponent<Enemy>();
			if (en != null)
			{
                if (startingEnemyTurn)
                {
                    en.TurnAttempts = 0;
                }

                if (en.CurrentPlays > 0 && en.CurrentHealth > 0)
                {
                    en.TakeTurn();
                    turnIsOver = false;
                }
            }
        }

        if (turnIsOver)
            Invoke("StartNewTurn", .3f);
        else
            Invoke("EnemyTurn", .3f);
    }
	#endregion

	#region Level finishing methods: LevelIsDone(), CollectAnimate(), ShuffleInHandAndDiscard(), ReturnToGodChoiceMenu()
	/// makes shopping interface appears. when shopping is done, go to the next level
	public void LevelIsDone(){
		clickBoss.AllowInputUmbrella = false;
		shopBoss.ProduceCards ();
	}

	public void CollectAnimate () {
		clickBoss.AllowInputUmbrella = false;
		DeckAnimate deckAnim = deckObj.GetComponent<DeckAnimate> ();
		deckAnim.ShuffleMoveAnimate();
	}

	/// Move everything in hands and dicard piles into the deck
	public void ShuffleInHandAndDiscard () {
		for(int i = Discard.Count-1; i > -1; i--){
			Card tempCard = Discard[i].GetComponent<Card>();
			Deck.Add(tempCard.CardName.Replace("\n", ""));
			Discard.RemoveAt(i);
			Destroy(tempCard.gameObject);
		}
		for(int i = Hand.Count-1; i > -1; i--){
			Card tempCard = Hand[i].GetComponent<Card>();
			
			Deck.Add(tempCard.CardName.Replace("\n", ""));
			Hand.RemoveAt(i);
			Destroy(tempCard.gameObject);
		}
	}

	public void ReturnToGodChoiceMenu() {
		GodChoiceMenu.GodChoiceMenuUp = true;
		shopBoss.Goals = new Goal[0];
	}
	#endregion

	#region GUI methods: Display and undisplay(), Dim()
	public void Display(Card card) {
		if (Tutorial.TutorialLevel != 0) return;
		dimmer.enabled = true;
		displayCardRenderer.enabled = true;
		DisplayName = card.CardName;
		DisplayName.Replace ("\n", "");
		DisplayCardIcon = Resources.Load (card.IconPath) as Texture2D;
		DisplayRules = card.DisplayText;
		if (card.maxRange != 0) {
			DisplayRangeTexture = (Texture2D)Resources.Load("sprites/targeting icons/range " + card.rangeTargetType.ToString() + " " + card.minRange.ToString() + "-" + card.maxRange.ToString());
			DisplayRangeSize = (card.maxRange*2+1)*.2f;
		}
		else 
			DisplayRangeTexture = null;

		if(card.aoeMaxRange != 0) {
			DisplayAOETexture = (Texture2D)Resources.Load("sprites/targeting icons/aoe " + card.aoeTargetType.ToString() + " " + card.aoeMinRange.ToString() + "-" + card.aoeMaxRange.ToString());
			DisplayAOESize = card.aoeMaxRange;
		}
		else {
			DisplayAOETexture = null;
		}

		DisplayTitleFont = GUISKIN.customStyles[2];
		DisplayTextFont = GUISKIN.box;

		//default text color is black
		DisplayTitleFont.normal.textColor = new Color(0,0,0);
		DisplayTextFont.normal.textColor = new Color(0,0,0);
		
		int godnum = ShopControl.AllGods.IndexOf (card.God);

		DisplayCard = shopBoss.GodDisplayCards [godnum];
		DisplayIcon = shopBoss.GodIcons [godnum];

		if(card.God == ShopControl.Gods.Ekcha | card.God == ShopControl.Gods.Ixchel) {
			DisplayTitleFont.normal.textColor = new Color(1,1,1);
			DisplayTextFont.normal.textColor = new Color(1,1,1);
		}

		switch(card.ThisRarity) {
		case (Card.Rarity.Paper):
			DisplayRarity = shopBoss.PaperTexture;
			break;
		case (Card.Rarity.Copper):
			DisplayRarity = shopBoss.CopperTexture;
			break;
		case (Card.Rarity.Silver):
			DisplayRarity = shopBoss.SilverTexture;
			break;
		case (Card.Rarity.Gold):
			DisplayRarity = shopBoss.GoldTexture;
			break;
		}
		displayCardRenderer.sprite = DisplayCard;

		CardDisplay = true;
	}
	
	public void Undisplay() {
		//this vvv might be a bad condition to base whether or not to turn off the dimmer on. it works for now though.
		if(CardsToTarget == 0) {
			dimmer.enabled = false;
		}
		displayCardRenderer.enabled = false;
		if(cardObjFromDeck != null) Destroy(cardObjFromDeck);
	}

	/// <summary>
	/// Dims the screen. True = dimmer is enabled.
	/// </summary>
	public void Dim()
	{
		Dim(true);
	}
	public void Dim(bool TurnOn)
	{
		if (TurnOn)
		{
			dimmer.enabled = true;
		}
		else
		{
			dimmer.enabled = false;
		}
	}
	#endregion

	#region Show discard
	public void FlipDiscard() {
		GameObject discard = GameObject.FindGameObjectWithTag ("Discard");
		
		
		if(!showingDiscard) {
		    discard.transform.localPosition = displayDiscardPlacement;

            for(int i = 0; i < Discard.Count; i++) {
				Card tempcard = Discard[i].GetComponent<Card>();
				tempcard.MoveAnimateWhileDiscarded(i, true);
			}
		}
		else {
			discard.transform.localPosition = originalDiscardPlacement;
		}
		showingDiscard = !showingDiscard;
	}
	
	public void ShowDeck(bool TurningOn) {
		showingDeck = TurningOn;
	}
	#endregion

	#region Add and Set Plays, Moves, Dollars(). EndTurnCheck(), which gets called after SetPlays and SetMoves()
	/// <summary>
	/// Adds plays, and also does an EndTurnCheck() at the end. 
	/// </summary>
	/// <param name="NumberOfPlays"></param>
	public void AddPlays(int NumberOfPlays) {
		SetPlays(NumberOfPlays + PlaysLeft);
	}
	void SetPlays(int NumberOfPlays)
	{
		PlaysLeft = NumberOfPlays;
		playsLeftText.text = PlaysLeft.ToString();

	    playButton.SetSprite();
	}

	/// <summary>
	/// Adds moves, and also does an EndTurnCheck() at the end. 
	/// </summary>
	/// <param name="NumberOfMoves"></param>
	public void AddMoves(int NumberOfMoves)
	{
		//take out this junk
		if (MovesArePlays)
		{
			SetPlays(PlaysLeft + NumberOfMoves);
		}
		else
		{
			SetMoves(MovesLeft + NumberOfMoves);
		}
	}
	void SetMoves(int NumberOfMoves)
	{
		MovesLeft = NumberOfMoves;
		movesLeftText.text = MovesLeft.ToString();

	    moveButton.SetSprite();

		Invoke("EndTurnCheck", .1f);
	}

	/// <summary>
	/// Adds dollars. 
	/// </summary>
	/// <param name="NumberOfDollars"></param>
	public void AddDollars(int NumberOfDollars) {
		SetDollars(NumberOfDollars + Dollars);
	}
	void SetDollars(int NumberOfDollars)
	{
		Dollars = NumberOfDollars;
		string tempString = Dollars.ToString();
		dollarsText.text = "$" + tempString;
	}

    /// <summary>
    /// Makes player sick. Possible arguments are "bleeding", "
    /// </summary>
    /// <param name="SickType"></param>
    public void SetSick(SickTypes SickType)
    {
        if (SickType == SickTypes.Swollen) 
        {
            SwollenTurns = 1;
            if (PlaysLeft > 0) AddPlays(-1);
            playButton.SetSprite();
        }
        else if (SickType == SickTypes.Hunger)
        {
            HungerTurns = 1;
            deckObj.GetComponent<DeckAnimate>().SetSprite();
        }
        else //bleeding
        {
            BleedingTurns = 1;
            if (MovesLeft > 0) AddMoves(-1);
            moveButton.SetSprite();
        }
    }

	public void EndTurnCheck()
	{
		//Levels don't end in the tutorial
		if (Tutorial.TutorialLevel != 0) return;

		if ((MovesLeft == 0 && PlaysLeft == 0) | (PlaysLeft == 0 && MovesArePlays))
		{
			ButtonSpritesLookClicked();
            EnemyTurn(true);
		}
	}
	#endregion

	#region Utilities
	public void DeselectCards(){
		GameObject[] allCards = GameObject.FindGameObjectsWithTag("Card");
		if (allCards != null) foreach(GameObject cardObject in allCards){
			Card currentCard = cardObject.GetComponent<Card>();
			if(currentCard != null) currentCard.Deselect();
		}
	}
	
	public void ButtonSpritesLookClicked () {
		movesLeftText.text = "";
		playsLeftText.text = "";	
		playButton.SetSprite(true);
		moveButton.SetSprite(true);	
		endTurnButton.SetSprite (true);
	}
	
	public void AnimateCardsToCorrectPosition() {
		handObj.transform.localPosition = (new Vector3(-.712f, 0));
		
		GameObject[] allCards = GameObject.FindGameObjectsWithTag("Card");
		if (allCards != null) {
			foreach(GameObject cardObject in allCards) {
				if(cardObject != null) {
					Card currentCard = cardObject.GetComponent<Card>();
					if(currentCard != null) {
					//	if(currentCard.DrawAnimating) {
					//		currentCard.DrawAnimating = false;
					//		cardObject.transform.localScale = new Vector3(.75f, .8f, .8f);
					//	}
						if(!currentCard.Discarded && !currentCard.BurnAnimating && !currentCard.Peeked && !currentCard.DiscardAnimating && !currentCard.ForcingDiscardOfThis) {
							int i = Hand.IndexOf(cardObject);
							if(currentCard != null) currentCard.MoveAnimate(i); 
						}
					}
				}
			}
		}
	}

	public void CheckDeckCount() {
		deckText.text = Deck.Count.ToString ();
	}

	public void DeleteAllCards()
	{
		for (int i = Discard.Count - 1; i > -1; i--)
		{
			Discard.RemoveAt(i);
		}
		for (int i = Deck.Count - 1; i > -1; i--)
		{
			Deck.RemoveAt(i);
		}
		for (int i = Hand.Count - 1; i > -1; i--)
		{
			Hand.RemoveAt(i);
		}
		GameObject[] cards = GameObject.FindGameObjectsWithTag("Card");
		foreach (GameObject GO in cards)
		{
			Destroy(GO);
		}
	}

	void UICheck()
	{
		playsLeftText.text = PlaysLeft.ToString();
		movesLeftText.text = MovesLeft.ToString();
		moveButton.SetSprite(false);
		playButton.SetSprite(false);
		endTurnButton.SetSprite(false);
		//gotta make bleeding foot, swollen hand and skull deck icons!
	}
	#endregion

    public void TransformPlayer(bool TurnToJaguar)
    {
        Debug.Log("transforming to " + TurnToJaguar.ToString() + "!");
        moveButton.UITransform(TurnToJaguar);
        playButton.UITransform(TurnToJaguar);

        MovesArePlays = TurnToJaguar;
    }
    public void TransformPlayer()
    {
        //called by nagualjaguar.
        TransformPlayer(true);
    }
}
