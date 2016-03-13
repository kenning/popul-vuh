using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// Man i'm gonna write this lazily. Hopefully black magic will make it not horribly slow

public class StateSavingControl : MonoBehaviour {

	static GameControl gameControl;
	static ShopControl shopControl;
    static ClickControl clickControl;
    static GridControl gridControl;
    static ShopControlGUI shopControlGUI;
	static ObstacleLibrary obstacleLibrary;
	static EnemyLibrary enemyLibrary;
    static ShopAndGoalParentCanvas shopAndGoalParentCanvas;
	static Player player;
	static List<int> ObstacleXPositions = new List<int>();
	static List<int> ObstacleYPositions = new List<int>();
	static List<string> TriggerList = new List<string>();
	static bool ShopMode = false;

	public static void Initialize (GameControl gc, Player pl) {
		gameControl = gc;
		shopControl = gc.gameObject.GetComponent<ShopControl>();
        clickControl = gc.gameObject.GetComponent<ClickControl>();
        gridControl = gc.gameObject.GetComponent<GridControl>();
        shopControlGUI = gc.gameObject.GetComponent<ShopControlGUI>();
		obstacleLibrary = gc.gameObject.GetComponent<ObstacleLibrary>();
		enemyLibrary = gc.gameObject.GetComponent<EnemyLibrary>();
        shopAndGoalParentCanvas = GameObject.FindGameObjectWithTag("goalandshopparent")
                                  .GetComponent<ShopAndGoalParentCanvas>();
		player = pl;
	}
		
	/// <summary>
	/// This could be optimized by splitting it into multiple functions that only grab and save 
	/// data for particular things. It could even be saved as separate files -- "PVEnemyState", etc.
	/// But let's see how slow it is first. 
	/// </summary>
	public static void Save() {
		ES2.Delete("PVState");

		if (!MainMenu.InGame | Tutorial.TutorialLevel != 0) return;

		SavedState ss = new SavedState ();
		ss.Level = GameControl.Level;
		ss.ObstacleLevelType 	= ObstacleLibrary.CurrentLevelType;
		ss.ObstacleXPositions 	= ObstacleXPositions;
		ss.ObstacleYPositions 	= ObstacleYPositions;
		foreach (GameObject cardGO in gameControl.Hand) {
			ss.CardsInHand.Add(cardGO.GetComponent<Card>().ThisLibraryCard);
		}
		ss.CardsInDeck 	    	= gameControl.Deck;	
		foreach (GameObject cardGO in gameControl.Discard) {
			ss.CardsInDiscard.Add(cardGO.GetComponent<Card>().ThisLibraryCard);
		}
		foreach (GameObject enemyGO in gameControl.EnemyObjs) {
			Enemy thisEnemy = enemyGO.GetComponent<Enemy>();
			ss.Enemies.Add(thisEnemy.ThisEnemyLibraryCard.Name);
			ss.EnemyHealths.Add(thisEnemy.CurrentHealth);
			ss.EnemyPlays.Add(thisEnemy.CurrentPlays);
			GridUnit enemyGU = enemyGO.GetComponent<GridUnit>();
			ss.EnemyXPositions.Add(enemyGU.xPosition);
			ss.EnemyYPositions.Add(enemyGU.yPosition);
		}
		ss.PlayerPosition 		= new int[] {player.playerGU.xPosition, player.playerGU.yPosition}; 	
		ss.PlayerHealth 		= player.currentHealth;
		ss.PlayerMoves			= gameControl.MovesLeft;
		ss.PlayerPlays 			= gameControl.PlaysLeft;
		ss.Dollars 				= gameControl.Dollars;
		ss.BleedingTurns 		= gameControl.BleedingTurns;
		ss.SwollenTurns 		= gameControl.SwollenTurns;
		ss.HungerTurns 			= gameControl.HungerTurns;
		ss.TriggerList 			= TriggerList;
		ss.Goals				= shopControl.Goals;
		ss.ShopMode 			= ShopMode;

		if (ShopMode) {
			ss.ShopCardList1 = shopControl.CardsToBuyFrom[0];
			ss.ShopCardList2 = shopControl.CardsToBuyFrom[1];
			ss.ShopCardList3 = shopControl.CardsToBuyFrom[2];
		} 

		ES2.Save<SavedState>(ss, "PVState");
	}

	public static bool StateWasSaved () {
		return ES2.Exists("PVState");
	}

	public static void DeleteState() {
		ES2.Delete("PVState");
	}

	public static void Load() {
		if (ES2.Exists ("PVState")) {
			SavedState loaded = ES2.Load<SavedState> ("PVState");

            GameControl.Level = loaded.Level;

			gameControl.BeginGame(true);
            
			int[][] positions = new int[loaded.ObstacleXPositions.Count][];
			for (int i = 0; i < loaded.ObstacleXPositions.Count; i++) {
				positions[i] = new int[] {loaded.ObstacleXPositions[i], loaded.ObstacleYPositions[i]};
                AddObstacle(loaded.ObstacleXPositions[i], loaded.ObstacleYPositions[i]);
			}
			obstacleLibrary.LoadObstaclesFromState(loaded.ObstacleLevelType, positions);

			foreach (LibraryCard lc in loaded.CardsInHand) {
				Card card = gameControl.Create(lc.CardName);
				gameControl.DrawIntoHand(card, true);
			}
            
			foreach (LibraryCard lc in loaded.CardsInDiscard) {
				Card card = gameControl.Create(lc.CardName, true);
				card.InvisibleDiscard();
			}

			gameControl.Deck = loaded.CardsInDeck;

            S.GameControlInst.CheckDeckCount();

			shopControl.Goals = loaded.Goals;
				
			if (loaded.ShopMode) {
				Debug.Log("shop mode!");
				shopControl.CardsToBuyFrom = new List<LibraryCard>[3];
				shopControl.CardsToBuyFrom[0] = loaded.ShopCardList1;
				shopControl.CardsToBuyFrom[1] = loaded.ShopCardList2;
				shopControl.CardsToBuyFrom[2] = loaded.ShopCardList3;
                S.GameControlGUIInst.ForceDim ();
                S.ShopControlInst.ProduceCards ();
			} else {
				for (int i = 0; i < loaded.Enemies.Count; i++) {
					Enemy newEnemy = enemyLibrary.LoadEnemy(loaded.Enemies[i], 
						loaded.EnemyXPositions[i], 
						loaded.EnemyYPositions[i], 
						loaded.EnemyHealths[i]);
                    newEnemy.CurrentPlays = loaded.EnemyPlays[i];
				}

                gridControl.EnemiesFindGridUnits();                
                shopControlGUI.NewLevelNewGoals(loaded.Goals.Length, loaded.Goals);                
                
                gameControl.BleedingTurns = loaded.BleedingTurns;
                gameControl.SwollenTurns = loaded.SwollenTurns;
                gameControl.HungerTurns = loaded.HungerTurns;
                EventControl.LoadTriggerListState(loaded.TriggerList);
			}

			player.transform.position = new Vector3(loaded.PlayerPosition[0], loaded.PlayerPosition[1], 1);
			player.playerGU.xPosition = loaded.PlayerPosition[0];
			player.playerGU.yPosition = loaded.PlayerPosition[1];
			player.currentHealth = loaded.PlayerHealth;
			gameControl.SetMoves(loaded.PlayerPlays);
			gameControl.SetMoves(loaded.PlayerMoves);
			gameControl.SetDollars(loaded.Dollars);
            clickControl.AllowForfeitButtonInput = true;
		}		
	}

	#region Utilities
	public static void ResetObstacleList () {
		ObstacleXPositions = new List<int>();
		ObstacleYPositions = new List<int>();
	}

	public static void AddObstacle(int x, int y) {
		ObstacleXPositions.Add(x);
		ObstacleYPositions.Add(y);
	}

	public static void AddToTriggerList(string name) {
		TriggerList.Add(name);
	}

	public static void RemoveFromTriggerList(string name) {
		TriggerList.Remove(name);
	}

	public static void ResetTriggerList() {
		TriggerList = new List<string>();
	}

	public static void TurnShopModeOn() {
		ShopMode = true;
	}

	public static void TurnShopModeOff() {
		ShopMode = false;
	}
	#endregion
}

[System.Serializable]
public class SavedState {
	public int 							Level				= 0;
	public List<int> 					ObstacleXPositions 	= new List<int>();
	public List<int> 					ObstacleYPositions 	= new List<int>();
	public ObstacleLibrary.LevelTypes 	ObstacleLevelType 	= 0;
	public List<LibraryCard> 			CardsInHand 		= new List<LibraryCard>();
	public List<string> 				CardsInDeck 		= new List<string>();
	public List<LibraryCard> 			CardsInDiscard 		= new List<LibraryCard>();
	public List<string> 				Enemies 			= new List<string>();
	public List<int> 					EnemyHealths 		= new List<int>();
	public List<int> 					EnemyXPositions 	= new List<int>();
	public List<int> 					EnemyYPositions 	= new List<int>();
	public List<int> 					EnemyPlays			= new List<int>();
	// Might need a list of stunned enemies, etc. later on
	public int[] 						PlayerPosition 		= new int[] {0, 0};
	public int 							PlayerHealth 		= 0;
	public int							PlayerMoves			= 0;
	public int 							PlayerPlays 		= 0;
	public int 							Dollars 			= 0;
	public int 							BleedingTurns 		= 0;
	public int 							SwollenTurns 		= 0;
	public int 							HungerTurns 		= 0;
	public Goal[]						Goals				= new Goal[] {new Goal(), new Goal(), new Goal()};
	public List<string>					TriggerList 		= new List<string>();
	public List<LibraryCard>			ShopCardList1		= new List<LibraryCard>();
	public List<LibraryCard>			ShopCardList2		= new List<LibraryCard>();
	public List<LibraryCard>			ShopCardList3		= new List<LibraryCard>();
	public bool 						ShopMode			= false;
}
