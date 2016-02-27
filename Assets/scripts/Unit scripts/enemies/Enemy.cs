using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Enemy : MonoBehaviour {

	public EnemyLibraryCard ThisEnemyLibraryCard;

	public enum DebugType {Path, AttackSquares, EmptySquares};
	public bool DebugOn = false;
	public DebugType DebugChoice;
	float timee = 0;

	public string Name = "";
	public string SpritePath;
	
	public ShopControl shopControl;
	public GridControl gridControl;
	public GameControlGUI gameControlGUI;
//	public EnemyLibrary enemyLibrary;
	
	public List<GridUnit> EnemyUnits;
	
	public int CurrentHealth;
	public int CurrentPlays;
	public int TurnAttempts = 0;
	public int StunnedForXTurns = 0;

	public GridControl.TargetTypes AttackTargetType;
	public int MaxHealth;
	public int MaxPlays;
	public int AttackMinRange;
	public int AttackMaxRange;
	public int AttackDamage;
	public string Tooltip;
	public MoveTarget ThisMoveTarget;
	public int ChallengeRating;
	
	public int MoveSpeed;

	public GridUnit thisGU;
	public GridUnit playerGU;
	public Player playerScript;
	
	public bool attackAnimating;
	public float attackAnimStartTime;
	public GameObject ProjectileObj;
	
	public TextMesh hpText;
	public TextMesh playsText;
	public GameObject hpBarObject;

	public UnitSFX unitSFX;

	public enum MoveTarget { Adjacent, Diagonal, Cross, Square };

	public virtual void Initialize(EnemyLibraryCard enemyLC) {
		ThisEnemyLibraryCard = enemyLC;
		thisGU = gameObject.GetComponent<GridUnit> ();
		playerGU = GameObject.FindGameObjectWithTag ("Player").GetComponent<GridUnit> ();
		playerScript = GameObject.FindGameObjectWithTag ("Player").GetComponent<Player> ();
		shopControl = S.GameControlInst.gameObject.GetComponent<ShopControl>();
		gridControl = S.GameControlInst.gameObject.GetComponent<GridControl>();
		gameControlGUI = S.GameControlInst.gameObject.GetComponent<GameControlGUI>();
		unitSFX = gameObject.GetComponent<UnitSFX>();

		Name = enemyLC.Name;
		Tooltip = enemyLC.Tooltip;
		MaxHealth = enemyLC.MaxHealth;
		MaxPlays = enemyLC.MaxPlays;
		AttackMinRange = enemyLC.AttackMinRange;
		AttackMaxRange = enemyLC.AttackMaxRange;
		AttackDamage = enemyLC.AttackDamage;
		AttackTargetType = enemyLC.AttackTargetType;
		ThisMoveTarget = enemyLC.ThisMoveTarget;
		SpritePath = enemyLC.SpritePath.Replace(" ", "");
		ChallengeRating = enemyLC.ChallengeRating;

		unitSFX.SetSFX(enemyLC.AttackSoundPath, enemyLC.DieSoundPath, enemyLC.StepSoundPath);

		CurrentHealth = MaxHealth;

		Renderer[] Renderers = gameObject.GetComponentsInChildren<Renderer> ();
		foreach(Renderer renderer in Renderers) {
			renderer.sortingLayerName = "Field foreground";
			if(renderer.gameObject.name == "hp text" ) {
				hpText = renderer.gameObject.GetComponentInChildren<TextMesh> ();
				hpText.text = MaxHealth.ToString();
				renderer.sortingOrder = 2;
			} else if(renderer.gameObject.name == "plays text") {
				if(MaxPlays == 1) {
					renderer.enabled = false;
				} else {
					playsText = renderer.gameObject.GetComponentInChildren<TextMesh> ();
					playsText.text = MaxPlays.ToString();
					renderer.sortingOrder = 2;
				}
			} else if(renderer.gameObject.name == "hp bar" |
					renderer.gameObject.name == "moves icon" ) {
				renderer.sortingOrder = 1;

				if(renderer.gameObject.name == "hp bar") {
					Vector3 parentPosition = thisGU.gameObject.transform.position;
					renderer.transform.localPosition = new Vector3(0, .42f, parentPosition.z);
					renderer.transform.localScale = new Vector3(1, .15f);
					hpBarObject = renderer.gameObject;
				}
			} else if (renderer.gameObject.name == "plays icon") {
				if(MaxPlays == 1) {
					renderer.enabled = false;
				}
			} else {
				//this is the main sprite
			}
		}
		SpriteRenderer mainSprite = gameObject.GetComponent<SpriteRenderer> ();
		mainSprite.sprite = Resources.Load<Sprite> (SpritePath);

		Transform childTransform = transform.Find ("projectile");
		ProjectileObj = childTransform.gameObject;

		gameObject.name = Name;
	}

	public virtual void Update()
	{
		if (attackAnimating)
		{
			if (Time.time > attackAnimStartTime + .25f)
			{
				attackAnimating = false;
				ProjectileObj.GetComponent<SpriteRenderer>().enabled = false;
				ProjectileObj.GetComponent<SpriteRenderer>().gameObject.transform.position = gameObject.transform.position;
				return;
			}

			float time = Time.time - attackAnimStartTime;
			ProjectileObj.transform.position = Vector3.Lerp(transform.position, playerGU.gameObject.transform.position, time * 4);
		}
		if (Time.time > timee + 1 && DebugOn)
		{
			DebugSquaresShow();
			timee = Time.time;
		}
	}

	public virtual void TakeTurn()
	{
		if(StunnedForXTurns != 0) 
        {
            Debug.Log("stunned");
			StunnedForXTurns--;
			return;
		}

        if (!playerGU.GetComponent<Player>().alive)
        {
            Debug.Log("player dead");
            return;
        }

		if (AttackCheck() && CurrentPlays > 0) 
		{
			AttackConnects();
			MakeMove();
		}
		else if (CurrentPlays > 0) 
		{
			//this method has MakeMove() in it already, so it doesn't need to have it in here like the attack if statement above.
			string directionAttempt = gridControl.ReturnNextMove(this, true);
			if(IsOpenToMove(directionAttempt)) 
			{
			    MakeMove();
				thisGU.GridMove(directionAttempt);
			}
			else 
			{
				directionAttempt = gridControl.ReturnNextMove(this, false);
                if (IsOpenToMove(directionAttempt))
                {
                    thisGU.GridMove(directionAttempt);
                    MakeMove();
                }
                else
                {
                    TurnAttempts++;
                }
			}
		} else {
            Debug.Log("current plays is 0");
        }
        
        

		if((CurrentPlays > 0) && TurnAttempts > 2) 
		{
            CurrentPlays = 0;
		}
	}

	//Just lowers CurrentPlays by 1 and changes the text to the new number. It's the enemy so its plays = moves, remember? 
	public void MakeMove() {
		CurrentPlays--;
		if (MaxPlays != 1) {
			playsText.text = CurrentPlays.ToString ();
		}
	}

#region Attacking methods
	public bool AttackCheck() 
	{
		return AttackCheck(thisGU.xPosition, thisGU.yPosition);
	}
	public bool AttackCheck(int EnemyXPosition, int EnemyYPosition)
	{
		//TODO
		//this is being called a lotttt....

		int distance = Mathf.Abs(playerGU.xPosition - EnemyXPosition) + Mathf.Abs(playerGU.yPosition - EnemyYPosition);
		int diff0 = Mathf.Abs(playerGU.xPosition - EnemyXPosition);
		int diff1 = Mathf.Abs(playerGU.yPosition - EnemyYPosition);

		switch (AttackTargetType)
		{
			case GridControl.TargetTypes.diamond:
				return distance >= AttackMinRange && distance <= AttackMaxRange;
			case GridControl.TargetTypes.diagonal:
				return (distance >= AttackMinRange * 2 && distance <= AttackMaxRange * 2 && thisGU.IsDiagonal(playerGU));
			case GridControl.TargetTypes.square:
				int xDifference = Mathf.Abs(thisGU.xPosition - playerGU.xPosition);
				int yDifference = Mathf.Abs(thisGU.yPosition - playerGU.yPosition);
				Debug.Log(xDifference.ToString() + ", " + yDifference.ToString());
				return (xDifference <= AttackMaxRange && xDifference >= AttackMinRange &&
						yDifference <= AttackMaxRange && yDifference >= AttackMinRange);
			case GridControl.TargetTypes.cross:
				return ((diff0 == 0 && diff1 <= AttackMaxRange && diff1 >= AttackMinRange) |
						(diff1 == 0 && diff0 <= AttackMaxRange && diff0 >= AttackMinRange));
			default:
				return false;
		}

	}
	public List<int[]> PossibleAttackSquares()
	{
		List<int[]> list = new List<int[]>();

		int gs = GridControl.GridSize;
		for (int i = -gs; i < gs; i++)
		{
			for (int j = -gs; j < gs; j++)
			{
				if(AttackCheck(i, j)) 
				{
					list.Add(new int[] {i, j});
				}
			}
		}

		return list;
	}
	
	public virtual void AttackConnects() 
    {
		AnimateAttack ();
		unitSFX.PlayAttackSFX();
		Invoke ("DealDefaultDamage", .3f);
	}
	
	public virtual void AnimateAttack () {
		if (thisGU.IsAdjacent(playerGU))
		{
			thisGU.PokeTowards(thisGU.AdjacentPosition(playerGU));
		}
		else 
		{
			ProjectileObj.GetComponent<SpriteRenderer> ().enabled = true;
			attackAnimating = true;
			attackAnimStartTime = Time.time;
		}
	}
#endregion

#region Damage dealing methods
    public virtual void DealDefaultDamage () {
		int distance = Mathf.Abs(thisGU.xPosition - playerGU.xPosition) + Mathf.Abs(thisGU.yPosition - playerGU.yPosition);
		playerScript.TakeDamage (AttackDamage, distance);
	}
	
	public virtual void ForceDiscard () {
		if(S.GameControlInst.Hand.Count == 0) 
			return;

		List<Card> SelectableCards = new List<Card> ();
		for (int i = 0; i < S.GameControlInst.Hand.Count; i++) {
			Card tempCard = S.GameControlInst.Hand[i].GetComponent<Card>();
//			if(!tempCard.DrawAnimating) {
				SelectableCards.Add (tempCard);
//			}
		}

		int randomNumber = Random.Range(0, SelectableCards.Count);
		if(SelectableCards[randomNumber] != null) {
			SelectableCards[randomNumber].ForcingDiscardOfThis = true;
			SelectableCards[randomNumber].Discard();
		}

		gameControlGUI.AnimateCardsToCorrectPositionInSeconds (.3f);
	}

	public virtual void SetSickBleeding () {
		S.GameControlInst.SetSick (GameControl.SickTypes.Bleeding, 2);
	}
	public virtual void SetSickSwollen () {
		S.GameControlInst.SetSick (GameControl.SickTypes.Swollen, 2);
	}
	public virtual void SetSickHunger () {
		S.GameControlInst.SetSick (GameControl.SickTypes.Hunger, 2);
	}
#endregion

#region Damage taking methods
	public virtual void GetPunched (int punchingDamage) {
		TakeDamage (punchingDamage);
	}
	
	public void TakeDamage(int damage, bool Override) {
		
		for(int i = 0; i < damage; i++) {
			shopControl.GoalCheck ("Deal X damage in one turn");
		}
		shopControl.GoalCheck ("Don't deal damage or move X turns in a row");
		shopControl.GoalCheck ("Don't deal damage X turns in a row");
		EventControl.EventCheck ("Enemy Damage");
		
		CurrentHealth = CurrentHealth - damage;
		hpText.text = CurrentHealth.ToString ();
		float hpPercent = (float)CurrentHealth / (float)MaxHealth;
		
		hpBarObject.transform.localPosition = (new Vector3(-.5f*(1-hpPercent), .42f, 0));
		hpBarObject.transform.localScale = new Vector3 (hpPercent, .15f);
		
		if (CurrentHealth < 1) Die();
	}
	public void TakeDamage(int damage) {
		TakeDamage (damage, false);
	}

	void Die () {
		unitSFX.PlayDieSFX();

		shopControl.GoalCheck ("Kill X enemies in one turn");
		shopControl.GoalCheck ("Kill enemies X turns in a row");
		EventControl.EventCheck ("Enemy Death");

		GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
		if (enemies.Length == 1 && Tutorial.TutorialLevel == 0) S.GameControlInst.Invoke("LevelIsDone", .4f);

        // moves this gridunit way out of the way so other enemies can walk around it 
		// (faster than going through all lists of grid units and removing it from them all)
        thisGU.xPosition = 100;
        thisGU.yPosition = 100;

		SaveDataControl.AddEnemyToDefeated (Name);
        S.GameControlInst.EnemyObjs.Remove(gameObject);
		Destroy(gameObject);
	}
#endregion

	public void FindOtherGridUnits()
	{
		EnemyUnits = new List<GridUnit>();

		EnemyUnits.Add(playerGU);
		GameObject[] enemyGOs = GameObject.FindGameObjectsWithTag("Enemy");
		foreach (GameObject eGO in enemyGOs)
		{
			GridUnit GU = eGO.GetComponent<GridUnit>();
			EnemyUnits.Add(GU);
		}

		// Do I need to do this? 
		GameObject[] obstacleGOs = GameObject.FindGameObjectsWithTag("obstacle");
		foreach (GameObject oGO in obstacleGOs) {
			GridUnit GU = oGO.GetComponent<GridUnit>();
			EnemyUnits.Add(GU);
		}
	}

	public virtual void GoToMaxPlays()
	{
		CurrentPlays = MaxPlays;
		if(MaxPlays != 1) {
			playsText.text = CurrentPlays.ToString ();
		}
	}

	public bool IsOpenToMove(string direction)
	{
		int gridSize = GridControl.GridSize;
		for(int i = 0; i < EnemyUnits.Count; i++) {
			GridUnit otherGU = EnemyUnits[i];
			int[] diff = new int[] { 0, 0 };
			diff[0] = otherGU.xPosition - thisGU.xPosition;
			diff[1] = otherGU.yPosition - thisGU.yPosition;
			if ((   diff[0] == -1     && diff[1] == 0     && direction == "left") |
				(   diff[0] == 1    && diff[1] == 0     && direction == "right") |
				(   diff[0] == 0     && diff[1] == -1     && direction == "down") |
				(   diff[0] == 0     && diff[1] == 1    && direction == "up") |
					(thisGU.xPosition > gridSize && direction == "right") | 
			    	(thisGU.xPosition < -gridSize && direction == "left") |
					(thisGU.yPosition > gridSize && direction == "up") | 
			    	(thisGU.yPosition < -gridSize && direction == "down")	)
			{
					return false;
			}
		}
		return true;
	}

	public void DebugSquaresShow()
	{
		switch(DebugChoice) {
		case DebugType.EmptySquares:
			List<Point> emptySquares = gridControl.FindEmptySpots (true);
			for(int i = 0; i < emptySquares.Count; i++) {
				gridControl.MoveTheSquareHere(emptySquares[i].x, emptySquares[i].y, false);
			}
			break;
		case DebugType.Path:
			List<int[]> emptySquareCoordinates = gridControl.FastestPath(this, true);

            try
            {
                for (int i = 0; i < emptySquareCoordinates.Count; i++)
                {
                    gridControl.MoveTheSquareHere(emptySquareCoordinates[i][0], emptySquareCoordinates[i][1], false);
                }
            }
            catch (System.NullReferenceException)
            {
                Debug.Log("welp");
                foreach (int[] x in emptySquareCoordinates)
                {
                    Debug.Log(x);
                }
                throw;
            }
			break;

		case DebugType.AttackSquares:
			List<int[]> attackSquares = PossibleAttackSquares ();
			for(int i = 0; i < attackSquares.Count; i++) {
				gridControl.MoveTheSquareHere(attackSquares[i][0], attackSquares[i][1], false);
			}
			break;
		}
	}
}
