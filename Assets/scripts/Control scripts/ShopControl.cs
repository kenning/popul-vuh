using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ShopControl : MonoBehaviour {

	GameControl gameControl;
	ClickControl clickControl;
	ShopControlGUI shopControlGUI;

	CardLibrary library;
	GoalLibrary goalLibrary;

	public Goal[] Goals;

	public List<LibraryCard>[] CardsToBuyFrom;

	string[] possibleGoals;

	//Phases
	public enum Gods {Akan, Buluc, Chac, Ekcha, Ikka, Ixchel, Kinich, Pantheon, none};
	public static string[] GodDescriptions	= {"Alcohols / Discard Pile", "Damage Spells", "Prayers", "Agility", 
		"Protection", "Fire / Card Destruction", "Card draw / Food", "Card draw / Food", "Card draw / Food", "Card draw / Food"};
	public static List<Gods> AllGods = new List<Gods> ();
	

    public void Initialize () {
		gameControl = gameObject.GetComponent<GameControl>();
		clickControl =  gameObject.GetComponent<ClickControl>();
		library =  gameObject.GetComponent<CardLibrary>();
		goalLibrary = gameObject.GetComponent<GoalLibrary> ();
		shopControlGUI = gameObject.GetComponent<ShopControlGUI> ();
		goalLibrary.Startup ();

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
    
    public void SetGoalGUIVariables() {
		for(int i = 0; i < Goals.Length; i++) {
			Goals[i].SetDisplayScore();
		}
		shopControlGUI.ResetTime ();
	}

	public void ProduceCards () {

		SetGoalGUIVariables ();

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

		shopControlGUI.TurnOnShopGUI ();
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
		Goals = new Goal[numberOfGods];
		Goals = goalLibrary.InitializeGoals (numberOfGods);

		foreach(Goal g in Goals) {
			g.SetGodString();
			shopControlGUI.SetGodPicture(g);
			g.ResetTheScore();
		}

		SetGoalGUIVariables ();
		clickControl.DisallowEveryInput ();

		shopControlGUI.NewLevelNewGoals (numberOfGods);
	}
}
