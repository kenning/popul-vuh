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

	int[] shopCardRarityLevelsByGod = {0, 0, 0, 0, 0, 0, 0, 0};

	//Phases
	public enum Gods {Akan, Buluc, Chac, Ekcha, Ikka, Ixchel, Kinich, Pantheon, none};
	public static string[] GodDescriptions	= {
		"Acan is the God of Alcohol and one of the Gods of Death; his name means 'Belch,' " + 
		"and his favorite drink is Balche. His fermented drinks can interact with your discarded cards.", 
		"Buluc Chabtan is the God of war, death and human sacrifice. If you don't appease him enough, he " + 
		"may bring you misfortune; however, if you impress him, he will provide you with destructive weapons and spells.", 
		"Card draw / Food", 
		"Prayers", 
		"Agility", 
		"Protection", 
		"Fire / Card Destruction", 
		"Card draw / Food", 
		"Card draw / Food", 
		"Card draw / Food"};
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

	public void ProduceCards () {

		shopControlGUI.UpdateGoalInfos ();

		CardsToBuyFrom = new List<LibraryCard>[3];
		for(int i = 0; i < CardsToBuyFrom.Length; i++) {
			CardsToBuyFrom[i] = new List<LibraryCard>();
		}
			
		// Count - 2 because the last ones are 'pantheon' and 'none'
		for (int i = 0, j = 0; i < AllGods.Count - 2; i++) {
			Debug.Log(AllGods[i]);
			Debug.Log(SaveDataControl.UnlockedGods.IndexOf(AllGods[i]));
			if(SaveDataControl.UnlockedGods.IndexOf(AllGods[i]) != -1) {
				Card.Rarity rare = Card.Rarity.Bronze;
				if (shopCardRarityLevelsByGod[i] == 1) {
					rare = Card.Rarity.Silver;
				} else if (shopCardRarityLevelsByGod[i] == 2) {
					rare = Card.Rarity.Gold;
				}
				LibraryCard tempLC = library.PullCardFromPack(AllGods[i], rare);
				CardsToBuyFrom[j].Add(tempLC);
			}

			if(i == 1 | i == 4) j++;
		}



		int[] finalScores = FinalScores ();
		for(int i = 0; i < Goals.Length; i++) {

			if(finalScores[i] == 1) gameControl.AddDollars(1);
			if(finalScores[i] >= 2) CardsToBuyFrom[i].Add(library.PullCardFromPack(Goals[i].God, Card.Rarity.Silver));
			if(finalScores[i] == 2) gameControl.AddDollars(2);
			if(finalScores[i] >= 3) CardsToBuyFrom[i].Add(library.PullCardFromPack(Goals[i].God, Card.Rarity.Gold));
			if(finalScores[i] == 3) gameControl.AddDollars(3);
			if(finalScores[i] >= 4) CardsToBuyFrom[i].Add(library.PullCardFromPack(Goals[i].God, Card.Rarity.Gold));
			if(finalScores[i] == 4) gameControl.AddDollars(5);
		}

		TurnOnShopGUI();
	}

	public void TurnOnShopGUI() {
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
		shopControlGUI.UpdateGoalInfos();

	}

	public void NewLevelNewGoals(int numberOfGods) {
		Goals = new Goal[numberOfGods];
		Goals = goalLibrary.InitializeGoals (numberOfGods);

		shopControlGUI.NewLevelNewGoals (numberOfGods, Goals);

		shopControlGUI.UpdateGoalInfos ();
		clickControl.DisallowEveryInput ();

	}

	public void BoughtCardFromGod(int godNumber) {
		if(godNumber > 6) Debug.LogError("whaaaa??????");
		if(shopCardRarityLevelsByGod[godNumber] < 2) {
			shopCardRarityLevelsByGod[godNumber]++; 
		}
	}

	public void RemoveBoughtCard(string cardname) {
		foreach (List<LibraryCard> column in CardsToBuyFrom) {
			foreach (LibraryCard lc in column) {
				if (cardname == lc.CardName) {
					column.Remove(lc);
					Debug.Log("removed card from shopcontrol cardstobuyfrom list");
					return;
				}
			}
		}
	}
}
