using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameControl : MonoBehaviour
{

	#region Variables
	public static string Error = "";

	public static bool MovesArePlays = false;

	ClickControl clickControl;
	ShopControl shopControl;
	EventGUI eventGUIgameControl;
	GridControl gridControl;
	GameControlGUI gameControlGUI;

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

	//UI data
	public string Tooltip = "";

	//UI variables
	TextMesh playsLeftText;
	TextMesh movesLeftText;
	TextMesh dollarsText;
	TextMesh deckText;
	ButtonAnimate playButton;
	ButtonAnimate moveButton;
	ButtonAnimate endTurnButton;
	Sprite endTurn;
	public GUISkin gooeyskin;
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
		handObj = GameObject.Find ("Hand");
		playBoardObj = GameObject.Find ("Play board");
		playerObj = GameObject.FindGameObjectWithTag ("Player");
		shopControl = gameObject.GetComponent<ShopControl> ();
		player = GameObject.FindGameObjectWithTag ("Player").GetComponent<Player> ();
		gooeyskin = (GUISkin)Resources.Load ("GUISkins/gameControl guiskin");
		
		library = gameObject.GetComponent<CardLibrary> ();
		enemyLibrary = gameObject.GetComponent<EnemyLibrary>();
		clickControl = gameObject.GetComponent<ClickControl> ();
		gridControl = gameObject.GetComponent<GridControl>();
		gameControlGUI = gameObject.GetComponent<GameControlGUI> ();

		library.Startup ();
		enemyLibrary.Startup();
		shopControl.Initialize ();
		gameObject.GetComponent<Tutorial>().Initialize();

		SaveLoad.Load ();

		if(SaveData.UnlockedCards.Count == 0 | SaveData.UnlockedGods.Count == 0) {
			SaveData.NewSaveFile ();
		}

		MainMenu.UnlockCheck();
	}

	/// <summary>
	/// Initializes game elements. Called from a main menu button.
	/// </summary>
	public void BeginGame () {

		Deck = new List<string> ();
		Hand = new List<GameObject> ();
		Discard = new List<GameObject> ();
		TargetedCards = new List<GameObject> ();
		EnemyObjs = new List<GameObject> ();
		gameControlGUI.SetDiscardPilePosition ();
        deckObj = GameObject.Find("Deck");


		GameObject[] EnemyObjects = GameObject.FindGameObjectsWithTag ("Enemy");
		if(EnemyObjects.Length > 0) {
			for(int i = EnemyObjects.Length - 1; i >= 0; i--) {
				Destroy(EnemyObjects[i]);
			}
		}
		
		library.Startup ();
		shopControl.Initialize ();
		player.ResetLife ();

		if (Tutorial.TutorialLevel == 0)
		{
			//should this really be set to true?
//			normaldisplay = true;
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
			shopControl.GoalCheck("Draw X cards in one turn");
			if(card.ThisRarity == Card.Rarity.Paper) {
				shopControl.GoalCheck("Draw X paper cards in one turn");
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
			newCardObj.AddComponent(System.Type.GetType(cardClass));
			Card newCardScript = newCardObj.GetComponent<Card>();
			newCardScript.CardName = libraryCardName;
			newCardScript.Initialize();
			newCardScript.Peek(i, numberOfCards);
			PeekedCards.Add(newCardObj);
			gameControlGUI.Dim ();
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

			shopControl.NewLevelNewGoals(numberOfGods);

			gridControl.LoadEnemiesAndObstacles(Level);
			InvisibleDraw();
			InvisibleDraw();
			InvisibleDraw();
		}

		StartNewTurn();
	
		clickControl.AllowInputUmbrella = false;
	}

	/// <summary>
	/// This method draws and resets goals. If stunned, return; otherwise get your 1 play and 1 move.
	/// Behaves very differently if Tutorial.TutorialLevel != 0.
	/// </summary>
	public void StartNewTurn() {

        if (!player.alive)
            return;

        EventControl.NewLevelReset();

		foreach(Goal g in shopControl.Goals) {
			g.NewTurnCheck();
		}

		gameControlGUI.Dim (false);

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

			clickControl.turnEndedAlready = false;
			clickControl.AllowEveryInput();
			clickControl.cardScriptClickedOn = null;

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
	    	clickControl.DisallowEveryInput ();
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
		clickControl.AllowInputUmbrella = false;
		shopControl.ProduceCards ();
	}

	public void CollectAnimate () {
		clickControl.AllowInputUmbrella = false;
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
		shopControl.Goals = new Goal[0];
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
						if(!currentCard.Discarded && 
						   !currentCard.BurnAnimating && 
						   !currentCard.Peeked && 
						   !currentCard.DiscardAnimating && 
						   !currentCard.ForcingDiscardOfThis) {
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
