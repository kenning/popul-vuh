using UnityEngine;
using System.Collections;

public class GameControlGUI : MonoBehaviour {
    
	bool showingDeck;
	
	bool displayDim = false;
	SpriteRenderer displayCardRenderer;
	DimAnimate dimmer;
	public bool CardDisplay = false;
	GameObject cardObjFromDeck;

	string tooltip = "";

	bool showingDiscard;
	Vector3 originalDiscardPlacement = new Vector3(3f, 2f, 0);
	Vector3 displayDiscardPlacement = new Vector3(3f, 6.5f, 0);

	DisplayCardCanvas displayCardCanvas;

	void Start() {
		useGUILayout = false;
		dimmer = GameObject.Find ("Dimmer").GetComponent<DimAnimate>();
		displayCardCanvas = GameObject.FindGameObjectWithTag("displaycard").GetComponent<DisplayCardCanvas>();
		MeshRenderer deckText = GameObject.Find("Deck count").GetComponent<MeshRenderer>();
		deckText.sortingLayerName = "PlayBoard bg";
		deckText.sortingOrder = 1;
	}

	void Update() {
		if (displayDim && !Input.GetMouseButton (0)) {
			displayDim = false;
			Dim (false);
		}
	}

	void OnGUI () {
		
		if (showingDeck) {
			//TODO displaying cards from the deck
		}
		
		if (tooltip != "") {
			GUI.Box(new Rect(Screen.width*.02f, Screen.height*.68f, Screen.width*.8f, Screen.height*.08f), 
			        tooltip, S.GUIStyleLibraryInst.GameControlGUIStyles.TooltipBox);
		}
	}

	public void Display(Card card) {
		SetTooltip("");
		displayCardCanvas.Display(card);
	}
	
	public void Undisplay() {
		displayCardCanvas.Undisplay();
		CardDisplay = false;
		//this vvv might be a bad condition to base whether or not to turn off the dimmer on. it works for now though.
		if(S.GameControlInst.CardsToTarget == 0) {
			Dim(false);
		}
		if(cardObjFromDeck != null) {
			// TODO display cards from the deck. i dont think this ever worked?
			Destroy(cardObjFromDeck);
		}
	}

	public void DisplayDim() {
		displayDim = true;
		Dim ();
	}

	public void Dim()
	{
		Dim(true);
	}

	public void Dim(bool TurnOn)
	{
		if (TurnOn)
		{
			dimmer.Dim();
		}
		else
		{
			dimmer.Undim();
		}
	}

	public void ForceDim() {
		dimmer.ForceDim ();
	}

	public void UnlockDim() {
		dimmer.UnlockDim ();
	}

	public void SetTooltip(string newToolTip) {
		tooltip = newToolTip;
	}

	public void ShowDeck(bool TurningOn) {
		return;
		showingDeck = TurningOn;
	}

	public void MoveHandPositionWhenOutOfPlace() {
		if (S.GameControlInst.Hand.Count < 5) {
			S.GameControlInst.handObj.transform.localPosition = new Vector3(-.7f, 0, 0);

			return;
		}
		if ((S.GameControlInst.handObj.transform.localPosition.x <= ((S.GameControlInst.Hand.Count) * -1.55f) + 5.35f)) {
			//this is for after the exact position has gotten nailed down, purpose is to lock it to the edge. 
			//the key numbers are: 3.95 one line above and .75 six lines above.
			S.GameControlInst.handObj.transform.localPosition = new Vector3 (((S.GameControlInst.Hand.Count) * -1.55f) + 5.3f, 0, 0);
			return;
		}
		return;
	}

	public void SetDiscardPilePosition ()
	{
		GameObject discard = GameObject.FindGameObjectWithTag ("Discard");
		discard.transform.localPosition = originalDiscardPlacement;
	}

	public void FlipDiscard() {
		GameObject discard = GameObject.FindGameObjectWithTag ("Discard");
		
		if(!showingDiscard) {
			discard.transform.localPosition = displayDiscardPlacement;
			
			for(int i = 0; i < S.GameControlInst.Discard.Count; i++) {
				CardUI tempcardui = S.GameControlInst.Discard[i].GetComponent<CardUI>();
				tempcardui.MoveAnimateWhileDiscarded(i, false);
			}
		}
		else {
			discard.transform.localPosition = originalDiscardPlacement;			
			
            for(int i = 0; i < S.GameControlInst.Discard.Count; i++) {
				CardUI tempcardui = S.GameControlInst.Discard[i].GetComponent<CardUI>();
				tempcardui.MoveAnimateWhileDiscarded(i, true);
			}
		}
		showingDiscard = !showingDiscard;
	}

	public void AnimateCardsToCorrectPosition() {
        Debug.Log("Rearranging cards!");
		MoveHandPositionWhenOutOfPlace ();
		
		GameObject[] allCards = GameObject.FindGameObjectsWithTag("Card");
		if (allCards != null) {
			foreach(GameObject cardObject in allCards) {
				if(cardObject != null) {
					Card currentCard = cardObject.GetComponent<Card>();
					if(currentCard != null && !currentCard.Discarded) {
						currentCard.cardUI.TryToMoveAnimate();
					}
				}
			}
		}
	}
	public void AnimateCardsToCorrectPositionInSeconds(float seconds) {
		Invoke ("AnimateCardsToCorrectPosition", seconds);
	}
}
