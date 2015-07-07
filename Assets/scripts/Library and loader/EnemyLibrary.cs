using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class EnemyLibrary : MonoBehaviour {

	public static Dictionary<string, EnemyLibraryCard> Lib;

	//public method, called by gridcontrol
	public void LoadRandomEnemy()
	{
		string[] allEnemies = Lib.Keys.ToArray();
		int x = Random.Range(0, allEnemies.Length - 1);
		LoadEnemy(allEnemies[x]);
	}

	#region Enemy loading methods that aren't the public method
	
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
	
	public void LoadEnemy(string EnemyName)
	{
		int RandomNumber = Random.Range(0, GridControl.PossibleSpawnPoints.Count - 1);
		Point xycoord = GridControl.PossibleSpawnPoints[RandomNumber];
		GridControl.PossibleSpawnPoints.RemoveAt(RandomNumber);

		LoadEnemy(EnemyName, xycoord.x, xycoord.y);
	}
	#endregion

	//List of enemies
	public void Startup()
	{
		Lib = new Dictionary<string, EnemyLibraryCard>();

		//mud and wooden men. each one is a copy of the previous one, except wooden men have 2 hp and do 1 more damage.
		Lib.Add("Mud Spear Warrior", new EnemyLibraryCard("Mud Spear Warrior",
			"Spear-wielding mud warrior.\nThe mud men were the third attempt by the Gods to make humans.", 
			1, 1, 1, 2, 1, GridControl.TargetTypes.cross, Enemy.MoveTarget.Cross, 1, false));
		Lib.Add("Wooden Spear Warrior", new EnemyLibraryCard("Wooden Spear Warrior",
			"Spear-wielding warrior made of wood.\nThe wooden men were the fourth attempt by the Gods to make humans.",
			2, 1, 1, 2, 2, GridControl.TargetTypes.cross, Enemy.MoveTarget.Cross, 1, false));

		Lib.Add("Mud Ball Player", new EnemyLibraryCard("Mud Ball Player",
			"Ball player made of mud. They even play ball in the Underworld!",
			1, 1, 1, 2, 1, GridControl.TargetTypes.diagonal, Enemy.MoveTarget.Diagonal, 1, false));
		Lib.Add("Wooden Ball Player", new EnemyLibraryCard("Wooden Ball Player",
			"Ball player made of wood. His rubber ball looks heavy. The wooden men were the fourth attempt by the Gods to make humans.",
			2, 1, 1, 2, 2, GridControl.TargetTypes.diagonal, Enemy.MoveTarget.Diagonal, 1, false));

		Lib.Add("Mud Warrior", new EnemyLibraryCard("Mud Warrior",
			"Macana-wielding warrior made of mud. The mud men were the third attempt by the Gods to make humans.",
			1, 1, 1, 1, 2, GridControl.TargetTypes.diamond, Enemy.MoveTarget.Adjacent, 1, false));
		Lib.Add("Wooden Warrior", new EnemyLibraryCard("Wooden Warrior",
			"Macana-wielding warrior made of wood. The wooden men were the fourth attempt by the Gods to make humans.",
			2, 1, 1, 1, 3, GridControl.TargetTypes.diamond, Enemy.MoveTarget.Adjacent, 1, false));

		//Jaguars. Different kinds of jaguars include:
			//basic jaguar
		Lib.Add("Basic Jaguar", new EnemyLibraryCard("Basic Jaguar",
			"Jaguar. These powerful creatures can do three things a turn. They may have other abilities, too...",
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
	public EnemyLibraryCard(string name, string tooltip, int maxhealth, int maxplays, int attackminrange, int attackmaxrange, int attackDamage,
		GridControl.TargetTypes attackTargetType, Enemy.MoveTarget moveTarget, int challengeRating, bool isSubclass)
	{
		Name = name;
		Tooltip = tooltip;
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