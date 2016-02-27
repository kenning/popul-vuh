using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player : MonoBehaviour {

	int maxHealth = 3;
	public int currentHealth = 3;
	int punchDamage = 1;
	public int StunnedForXTurns = 0;
	ShopControl shopControl;
	GridControl gridControl;
	public Sprite[] HPBars;
	public SpriteRenderer HPBARRENDERER;
	UnitSFX unitSFX;

	public bool alive = true;
	public GridUnit playerGU;

	MenuControl menuControl;
	
	void Start () {
		useGUILayout = false;
		GameObject gameController = GameObject.FindGameObjectWithTag ("GameController");
		shopControl = gameController.GetComponent<ShopControl>();
		gridControl = gameController.GetComponent<GridControl>();
		menuControl = gameController.GetComponent<MenuControl> ();
		playerGU = gameObject.GetComponent<GridUnit>();
		unitSFX = gameObject.GetComponent<UnitSFX>();
		ResetLife ();
	}

	public void Punch(GameObject enemy) {
    //animation aspect. must happen first
        GridUnit enemyGU = enemy.GetComponent<GridUnit>();
        gridControl.MakeSquares(GridControl.TargetTypes.diamond, 0, 0, enemyGU.xPosition, enemyGU.yPosition, false);
        playerGU.PokeTowards(playerGU.AdjacentPosition(enemyGU));

        enemy.GetComponent<Enemy> ().GetPunched (punchDamage);
		S.GameControlInst.AddPlays(-1);

    //triggers
		EventControl.EventCheck ("Punch");
		shopControl.GoalCheck ("Punch X times");
		if (Tutorial.TutorialLevel != 0)
			S.GameControlInst.gameObject.GetComponent<Tutorial>().TutorialTrigger(3);

		QControl.CheckQ ();
	}

	public void TakeDamage(int damage, int distance) {

		List<Card> Armors = new List<Card> ();
		List<Card> SuccessfulArmors = new List<Card> ();

		for(int i = 0; i < S.GameControlInst.Hand.Count; i++) {
			Card tempCard = S.GameControlInst.Hand[i].GetComponent<Card>();
			if(tempCard.CardAction == Card.CardActionTypes.Armor | tempCard.ArmorWithSecondaryAbility) 
				Armors.Add(tempCard);
		}
		if(Armors.Count != 0) {
			for(int i = 0; i < Armors.Count; i++) {
				if(Armors[i].DamageProtection >= damage && Armors[i].DamageDistanceProtectionMax >= distance && 
				   Armors[i].DamageDistanceProtectionMin <= distance && !Armors[i].ArmorHasBeenUsed) {
					SuccessfulArmors.Add(Armors[i]);
				}
			}
		}
		if(SuccessfulArmors.Count != 0) {
			int randomNumber = Random.Range(0, SuccessfulArmors.Count);
			SuccessfulArmors[randomNumber].ArmorPlay
				();
			SuccessfulArmors[randomNumber].ArmorHasBeenUsed = true;
			shopControl.GoalCheck ("Protect against X attacks");
			return;
		}

		currentHealth = currentHealth - damage;
		if (currentHealth < 0) currentHealth = 0;
		HPBARRENDERER.sprite = HPBars [currentHealth];

		if (currentHealth <1) {
			unitSFX.PlayDieSFX();
			menuControl.Die();
		}
	}

	public void ResetLife() {
		currentHealth = maxHealth;
		HPBARRENDERER.sprite = HPBars [currentHealth];
		alive = true;
	}

	internal void MoveClick(string direction)
	{
		bool canMove = S.GameControlInst.MovesLeft > 0;
		if (GameControl.MovesArePlays)
		{
			canMove = S.GameControlInst.PlaysLeft > 0;
		}

		if (canMove)
		{
			GridUnit thisGU = gameObject.GetComponent<GridUnit>();
			int gridSize = GridControl.GridSize;
			if ((thisGU.xPosition > gridSize && direction == "right") | (thisGU.xPosition < -gridSize && direction == "left") |
			   (thisGU.yPosition > gridSize && direction == "up") | (thisGU.yPosition < -gridSize && direction == "down"))
			{
				Debug.Log("blocked!");
				return;
			}

			thisGU.GridMove(direction);
			S.GameControlInst.AddMoves(-1);
			shopControl.GoalCheck("Move X times in one turn");
			shopControl.GoalCheck("Don't move X turns in a row");
			shopControl.GoalCheck("Don't deal damage or move X turns in a row");
			shopControl.GoalCheck("Don't move X turns in a row");
			gridControl.DestroyAllTargetSquares();

			if (Tutorial.TutorialLevel != 0)
			{
				S.GameControlInst.gameObject.GetComponent<Tutorial>().TutorialTrigger(2);
			}

			StateSavingControl.Save();
		}
		else
		{
			ButtonAnimate moveButton = GameObject.Find("move end button").GetComponent<ButtonAnimate>();
			moveButton.ErrorAnimation();
		}
	}
}