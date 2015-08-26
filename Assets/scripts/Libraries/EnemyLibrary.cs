using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class EnemyLibrary : MonoBehaviour {

	public static Dictionary<string, EnemyLibraryCard> Lib;
	public List<List<int>> challengeRatingsForEachLevel = new List<List<int>>();

	void Start() {
		challengeRatingsForEachLevel.Add(new List<int> { 1, 1 });
		challengeRatingsForEachLevel.Add(new List<int> { 1, 1, 1 });
		challengeRatingsForEachLevel.Add(new List<int> { 1, 1, 2 });
		challengeRatingsForEachLevel.Add(new List<int> { 1, 1, 2, 2 });
		challengeRatingsForEachLevel.Add(new List<int> { 1, 1, 1, 2, 3 });
		challengeRatingsForEachLevel.Add(new List<int> { 1, 1, 1, 2, 2, 3 });
		challengeRatingsForEachLevel.Add(new List<int> { 1, 1, 1, 2, 2, 3, 3 });
	}

	//base method. only really used to load an enemy into a specific place in the tutorial. 
	public void LoadEnemy(string EnemyName, int xPosition, int yPosition)
	{
		string EnemyToLoad = "prefabs/enemies/dummy enemy";

		GameObject tempGO = (GameObject)Instantiate((GameObject)Resources.Load(EnemyToLoad));

		tempGO.GetComponent<GridUnit>().xPosition = xPosition;
		tempGO.GetComponent<GridUnit>().yPosition = yPosition;
		tempGO.transform.position = new Vector3(xPosition, yPosition, 0);

		EnemyLibraryCard EnemyLC = Lib[EnemyName];

		if (EnemyLC.IsSubclass)
		{
			string enemyScriptName = EnemyName;
            enemyScriptName = enemyScriptName.Replace(" ", "");
			tempGO.AddComponent(System.Type.GetType(enemyScriptName));
		}
		else
		{
			tempGO.AddComponent<Enemy>();
		}

		Enemy tempEScript = tempGO.GetComponent<Enemy>();
		tempEScript.Initialize(EnemyLC);
	}

	public void LoadEnemiesForLevel (int level) {
		if(level > 6) level = 6;
		for(int i = 0; i < challengeRatingsForEachLevel[level].Count; i++) {
			LoadRandomEnemyOfChallengeRating(challengeRatingsForEachLevel[level][i]);
		}
	}

	void LoadRandomEnemyOfChallengeRating(int rating) {
		string[] allEnemies = Lib.Keys.ToArray();
		List<EnemyLibraryCard> appropriateChallengeRatingList = new List<EnemyLibraryCard> ();
		for(int i = 0; i < allEnemies.Length; i++) {
			if(Lib[allEnemies[i]].ChallengeRating == rating) {
				appropriateChallengeRatingList.Add(Lib[allEnemies[i]]);
			}
		}
		int x = Random.Range(0, appropriateChallengeRatingList.Count - 1);
		LoadEnemy(appropriateChallengeRatingList[x].Name);
	}
	
	void LoadEnemy(string EnemyName)
	{
		int RandomNumber = Random.Range(0, GridControl.PossibleSpawnPoints.Count - 1);
		Point xycoord = GridControl.PossibleSpawnPoints[RandomNumber];
		GridControl.PossibleSpawnPoints.RemoveAt(RandomNumber);

		if (xycoord.x > 3) xycoord.x = 3;
		if (xycoord.x < -3) xycoord.x = -3;
		if (xycoord.y > 3) xycoord.y = 3;
		if (xycoord.y < -3) xycoord.y = -3;

		LoadEnemy(EnemyName, xycoord.x, xycoord.y);
	}

	//List of enemies
	public void Startup()
	{
		Lib = new Dictionary<string, EnemyLibraryCard>();

		//mud and wooden men. each one is a copy of the previous one, except wooden men have 2 hp and do 1 more damage.
		Lib.Add("Mud Spear Warrior", new EnemyLibraryCard("Mud Spear Warrior",
			"Spear-wielding mud warrior. Attacks from two squares away.", 
		    "Spear-wielding mud warrior. Attacks from two squares away. The mud men were the third attempt by the Gods to make humans.", 
			1, 1, 1, 2, 1, GridControl.TargetTypes.cross, Enemy.MoveTarget.Cross, 1, false));
		Lib.Add("Wooden Spear Warrior", new EnemyLibraryCard("Wooden Spear Warrior",
			"Spear-wielding warrior made of wood. Attacks for two damage from two squares away.",
			"Spear-wielding warrior made of wood. Attacks for two damage from two squares away. The wooden men were the fourth attempt by the Gods to make humans.",
			2, 1, 1, 2, 2, GridControl.TargetTypes.cross, Enemy.MoveTarget.Cross, 2, false));

		Lib.Add("Mud Ball Player", new EnemyLibraryCard("Mud Ball Player",
			"Ball player made of mud. They even play ball in the Underworld! Attacks diagonally.",
			"Ball player made of mud. They even play ball in the Underworld! Attacks diagonally. The mud men were the third attempt by the Gods to make humans.",
			1, 1, 1, 2, 1, GridControl.TargetTypes.diagonal, Enemy.MoveTarget.Diagonal, 1, false));
		Lib.Add("Wooden Ball Player", new EnemyLibraryCard("Wooden Ball Player",
			"Ball player made of wood. Its rubber ball looks heavy. Attacks diagonally.",
			"Ball player made of wood. Its rubber ball looks heavy. Attacks diagonally.  The wooden men were the fourth attempt by the Gods to make humans.",
			2, 1, 1, 2, 2, GridControl.TargetTypes.diagonal, Enemy.MoveTarget.Diagonal, 2, false));

		Lib.Add("Mud Warrior", new EnemyLibraryCard("Mud Warrior",
			"Macana-wielding warrior made of mud. Deals two damage.",
			"Macana-wielding warrior made of mud. Deals two damage. The mud men were the third attempt by the Gods to make humans.",
			1, 1, 1, 1, 2, GridControl.TargetTypes.diamond, Enemy.MoveTarget.Adjacent, 1, false));
		Lib.Add("Wooden Warrior", new EnemyLibraryCard("Wooden Warrior",
			"Macana-wielding warrior made of wood. Deals three damage, watch out!",
			"Macana-wielding warrior made of wood. Deals three damage, watch out! The wooden men were the fourth attempt by the Gods to make humans.",
			2, 1, 1, 1, 3, GridControl.TargetTypes.diamond, Enemy.MoveTarget.Adjacent, 2, false));

		Lib.Add("Mud Archer", new EnemyLibraryCard("Mud Archer", 
		        "Bow-wielding warrior made of mud. Attacks across rows or columns.",
		        "Bow-wielding warrior made of mud. Attacks across rows or columns. The mud men were the third attempt by the Gods to make humans.",
		        1, 1, 1, 5, 1, GridControl.TargetTypes.cross, Enemy.MoveTarget.Cross, 2, false));
		Lib.Add("Wooden Archer", new EnemyLibraryCard("Mud Archer", 
		        "Bow-wielding warrior made of wood. Attacks for two damage across rows or columns.",
		        "Bow-wielding warrior made of wood. Attacks for two damage across rows or columns. The wooden men were the fourth attempt by the Gods to make humans.",
		        2, 1, 1, 5, 2, GridControl.TargetTypes.cross, Enemy.MoveTarget.Cross, 3, false));


		//Jaguars. Different kinds of jaguars include:
			//basic jaguar
		Lib.Add("Basic Jaguar", new EnemyLibraryCard("Basic Jaguar",
			"Jaguar. These powerful creatures can do three things a turn. They may have other abilities, too...",
			"Jaguar. These powerful creatures can do three things a turn.",
			1, 3, 1, 1, 1, GridControl.TargetTypes.diamond, Enemy.MoveTarget.Adjacent, 1, false));
		//naguals: can turn you into a jaguar
        //THIS SHIT BROKE
        //Lib.Add("Nagual Jaguar", new EnemyLibraryCard("Nagual Jaguar", 
        //    "Jaguar. These powerful creatures can do three things a turn. They may have other abilities, too...",
        //    1, 3, 1, 1, 0, GridControl.TargetTypes.diamond, Enemy.MoveTarget.Adjacent, 1, true));
	}
}

public class EnemyLibraryCard 
{
	public string Name;
	public string Tooltip;
	public string EncyclopediaEntry;
	public string SpritePath;
		
	public int MaxHealth;
	public int MaxPlays;
	public int AttackMinRange;
	public int AttackMaxRange;
	public int AttackDamage;
	public GridControl.TargetTypes AttackTargetType;
	public Enemy.MoveTarget ThisMoveTarget;
	public int ChallengeRating;
	public bool IsSubclass = false;

	public EnemyLibraryCard() { }
	public EnemyLibraryCard(string name, string tooltip, string encyclopediaEntry, int maxhealth, int maxplays, int attackminrange, int attackmaxrange, int attackDamage,
		GridControl.TargetTypes attackTargetType, Enemy.MoveTarget moveTarget, int challengeRating, bool isSubclass)
	{
		Name = name;
		Tooltip = tooltip;
		EncyclopediaEntry = encyclopediaEntry;
		MaxHealth = maxhealth;
		MaxPlays = maxplays;
		AttackMinRange = attackminrange;
		AttackMaxRange = attackmaxrange;
		AttackDamage = attackDamage;
		AttackTargetType = attackTargetType;
		ThisMoveTarget = moveTarget;
		ChallengeRating = challengeRating;
		IsSubclass = isSubclass;

		SpritePath = "sprites/enemies/" + name;
	}
}