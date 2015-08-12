using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ShopControl : MonoBehaviour {

	GameControl gameControl;
	ClickControl clickControl;
	ShopControlGUI shopControlGUI;
	GUIStyleLibrary styleLibrary;

	CardLibrary library;
	GoalLibrary goalLibrary;
	ClickBlocker clickBlocker;

	public Goal[] Goals;
	bool[] GoalDisplay;
	public static bool Normaldisplay;

	public List<LibraryCard>[] CardsToBuyFrom;

	string[] possibleGoals;

	//Phases
	public enum Gods {Ikka, Ekcha, Chac, Kinich, Buluc, Ixchel, Akan, Pantheon, none};
	public static string[] GodDescriptions	= {"Alcohols / Discard Pile", "Damage Spells", "Prayers", "Agility", 
		"Protection", "Fire / Card Destruction", "Card draw / Food", "Card draw / Food", "Card draw / Food", "Card draw / Food"};
	public static List<Gods> AllGods = new List<Gods> ();
	

    public void Initialize () {
		gameControl = gameObject.GetComponent<GameControl>();
		clickControl =  gameObject.GetComponent<ClickControl>();
		library =  gameObject.GetComponent<CardLibrary>();
		styleLibrary = gameObject.GetComponent<GUIStyleLibrary> ();
		goalLibrary = gameObject.GetComponent<GoalLibrary> ();
		goalLibrary.Startup ();
		clickBlocker = GameObject.Find ("moving click blocker").GetComponent<ClickBlocker> ();

		Goals = new Goal[0];

		if(AllGods.Count < 5) {
			AllGods.Add (Gods.Akan);
			AllGods.Add (Gods.Buluc);
            AllGods.Add (Gods.Chac);
            AllGods.Add (Gods.Ekcha);
			AllGods.Add (Gods.Ikka);
			AllGods.Add (Gods.Ixchel);
			AllGods.Add (Gods.Kinich);
			AllGods.Add (Gods.Pantheon);
			AllGods.Add (Gods.none);
		}
	}
    
    public void SetGoalGUI() {
		for(int i = 0; i < Goals.Length; i++) {
			Goals[i].SetDisplayScore();
		}
		shopControlGUI.shopGUITime = Time.time;
	}

	public void ProduceCards () {

		SetGoalGUI ();

		CardsToBuyFrom = new List<LibraryCard>[Goals.Length];
		for(int i = 0; i < CardsToBuyFrom.Length; i++) {
			CardsToBuyFrom[i] = new List<LibraryCard>();
		}

		int[] finalScores = FinalScores ();
		for(int i = 0; i < Goals.Length; i++) {

			if(finalScores[i] >= 1) { 
				LibraryCard tempLC = library.PullCardFromPack(Goals[i].God, Card.Rarity.Bronze);
				CardsToBuyFrom[i].Add(tempLC);
			}

			if(finalScores[i] == 1) gameControl.AddDollars(1);
			if(finalScores[i] >= 2) CardsToBuyFrom[i].Add(library.PullCardFromPack(Goals[i].God, Card.Rarity.Silver));
			if(finalScores[i] == 2) gameControl.AddDollars(2);
			if(finalScores[i] >= 3) CardsToBuyFrom[i].Add(library.PullCardFromPack(Goals[i].God, Card.Rarity.Gold));
			if(finalScores[i] == 3) gameControl.AddDollars(3);
			if(finalScores[i] >= 4) CardsToBuyFrom[i].Add(library.PullCardFromPack(Goals[i].God, Card.Rarity.Gold));
			if(finalScores[i] == 4) gameControl.AddDollars(5);
		}

		shopGUI = true;
		clickControl.DisallowEveryInput ();
	}

	int[] FinalScores () {
		int[] finalScores = new int[] {0,0,0};
		for(int i = 0; i < Goals.Length; i++) {
			if(Goals[i].HigherScoreIsGood) {
				for(int j = 0; j < Goals[i].GoalScore.Length; j++) {
					if(Goals[i].HighScore >= Goals[i].GoalScore[j]) finalScores[i]++;
				}
			}
			else {
				for(int j = 0; j < Goals[i].GoalScore.Length; j++) {
					if(Goals[i].HighScore <= Goals[i].GoalScore[j]) finalScores[i]++;
				}
			}
		}
		return finalScores;
	}
	
	public void GoalCheck(string GoalMiniDescription) {
		for(int i = 0; i < Goals.Length; i++){
			if (Goals[i].MiniDescription == GoalMiniDescription) {
				Goals[i].MakeCheck();
			}
		}
	}

	public void NewLevelNewGoals(int numberOfGods) {

		for(int i = 0; i < 3; i++) {
			string clickBlockerName = "click blocker " + (i+1).ToString();
			BoxCollider2D tempClickBlockerCollider = GameObject.Find(clickBlockerName).GetComponent<BoxCollider2D>();
			tempClickBlockerCollider.enabled = (i < numberOfGods);
		}

	//	BoxCollider2D[] clickBlockers = new BoxCollider2D[numberOfGods];
	//	for(int i = 0; i < clickBlockers.Length; i++) {
	//		Debug.Log("this happens, right?");
	//		string clickBlockerName = "click blocker " + (i+1).ToString();
	//		clickBlockers[i] = new BoxCollider2D();
	//		clickBlockers[i] = GameObject.Find(clickBlockerName).GetComponent<BoxCollider2D>();
	//		clickBlockers[i].enabled = true;
	//	}

		GoalDisplay = new bool[numberOfGods];

		Goals = new Goal[numberOfGods];
		Goals = goalLibrary.InitializeGoals (numberOfGods);

		foreach(Goal g in Goals) {
			g.SetGodString();
			SetGodPicture(g);
			g.ResetTheScore();
		}

		SetGoalGUI ();
		goalExpo = true;
		clickControl.DisallowEveryInput ();

	}
}
