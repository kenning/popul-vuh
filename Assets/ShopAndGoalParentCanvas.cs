using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/// <summary>
/// Delegates methods to shop rows and generally does stuff that SHOPROWCANVASES and goalcanvases can't.
/// Also does the whole goalexpo step by itself.
/// This object contains no state.
/// Try not to add much code to this file -- think of it as a pure view layer.
/// </summary>
public class ShopAndGoalParentCanvas : MonoBehaviour {

	public ShopGridCanvas SHOPGRIDCANVAS;
	public GoalCanvas[] GOALCANVASES;
	public GameObject[] EXPOGAMEOBJECTS;
	public GameObject FINISHEXPOBUTTON;
	public GameObject FINISHSHOPPINGBUTTON;
	public GameObject SHOPDOLLARCOUNTER;
	public GameObject EXPOBACKGROUND;
	public Sprite[] GRADEBACKGROUNDS;
	public ShopAwardCanvas[] SHOPAWARDS;

	Text dollarsText;

	GameControl gameControl;
	GameControlGUI gameControlGUI;
	ShopControl shopControl;
	ShopControlGUI shopControlGUI;
	ClickControl clickControl;

	void Start () {
		GameObject tempGO = GameObject.FindGameObjectWithTag ("GameController");
		gameControl = tempGO.GetComponent<GameControl> ();
		gameControlGUI = tempGO.GetComponent<GameControlGUI> ();
		shopControl = tempGO.GetComponent<ShopControl> ();
		shopControlGUI = tempGO.GetComponent<ShopControlGUI> ();
		clickControl = tempGO.GetComponent<ClickControl> ();

		dollarsText = SHOPDOLLARCOUNTER.GetComponentInChildren<Text> ();
	}

	#region Initialization methods
	// This just gets used by updategoalinfos and newlevelnewgoals.
	void TurnOnAppropriateGoals (Goal[] goals) {

		for (int i = 0; i < 3; i++) {
			if(i >= goals.Length) {
				GOALCANVASES[i].gameObject.SetActive(false);
			} else if(GOALCANVASES[i] != null) {
				GOALCANVASES[i].gameObject.SetActive(true);
			}
		}
	}

	public void UpdateGoalInfos (Goal[] goals)
	{
		// Happens when clickcontrol.goalcheck() is called
		if(goals == null) return;

		TurnOnAppropriateGoals(goals);

		for(int i = 0; i < goals.Length; i++) {
			goals[i].SetDisplayScore();
			if(GOALCANVASES[i] != null) GOALCANVASES[i].UpdateGoalInfo();
		}
	}

	public void NewLevelNewGoals (Goal[] goals)
	{
		TurnOnAppropriateGoals(goals);

		for (int i = 0; i < 3; i++) {
			if(GOALCANVASES[i] != null && GOALCANVASES[i].gameObject.activeSelf) {
				GOALCANVASES[i].SetInitialGoalInfo(goals[i]);
			}
		}
		
		UpdateGoalInfos (goals);
	}
	public void SetUpShopRows (Goal[] goals, bool[] highScoreNotifications) {
		for(int i = 0; i < goals.Length; i++) {
			SHOPAWARDS[i].SetGradeInfo(goals[i], highScoreNotifications[i]);
		}
		for(int i = 0; i < 3; i++) {
			for (int j = 0; j < shopControl.CardsToBuyFrom[i].Count; j++) {
				SHOPGRIDCANVAS.SetCardInfo(i, j, shopControl.CardsToBuyFrom[i][j]);
			}
		}

		dollarsText.text = "$" + gameControl.Dollars.ToString();
	}
	#endregion

	#region Turning expo, normal and shop guis on and off
	public void TurnOnShopGUI() {
		FINISHSHOPPINGBUTTON.SetActive (true);
		SHOPDOLLARCOUNTER.SetActive (true);
		EXPOBACKGROUND.SetActive(true);
	}
	public void TurnOffShopGUI() {
		if(SHOPGRIDCANVAS != null) SHOPGRIDCANVAS.GetComponent<ShopGridCanvas>().TurnOff();
		FINISHSHOPPINGBUTTON.SetActive (false);
		SHOPDOLLARCOUNTER.SetActive (false);
		EXPOBACKGROUND.SetActive(false);
		for(int i = 0; i < SHOPAWARDS.Length; i++) SHOPAWARDS[i].TurnOff();
	}
	public void TurnOnNormalGUI() {
		for (int i = 0; i < GOALCANVASES.Length; i++) {
			if (i >= shopControl.Goals.Length) {
				GOALCANVASES[i].gameObject.SetActive(false);
			} else {
				GOALCANVASES[i].gameObject.SetActive(true);
			}
		}
	}
	public void TurnOffNormalGUI() {
		foreach(GoalCanvas GC in GOALCANVASES) if(GC != null) GC.gameObject.SetActive(false); 
	}
	public void TurnOnExpoGUI() {
		for(int i = 0; i < EXPOGAMEOBJECTS.Length; i++) {
			if(EXPOGAMEOBJECTS[i] == null) {
				Debug.LogError("expo game object not set");
			} else if (i >= shopControl.Goals.Length) {
				EXPOGAMEOBJECTS[i].SetActive(false);
			} else {
				EXPOGAMEOBJECTS[i].SetActive(true);
				EXPOGAMEOBJECTS[i].GetComponent<GoalExpoCanvas>().SetExpoInfo(shopControl.Goals[i]);
			}
		}
		FINISHEXPOBUTTON.SetActive (true);
		EXPOBACKGROUND.SetActive (true);
	}
	public void TurnOffExpoGUI() {
		foreach(GameObject expoGO in EXPOGAMEOBJECTS) if(expoGO != null) expoGO.SetActive(false);
		FINISHEXPOBUTTON.SetActive (false);
		EXPOBACKGROUND.SetActive (false);
	}

	#endregion

	public void FinishGoalExpo () {
		clickControl.Invoke ("AllowEveryInput", .1f);
		shopControlGUI.TurnOnNormalGUI ();
	}

	public void FinishShopping () {
		EXPOBACKGROUND.SetActive(false);
		gameControl.CollectAnimate();
		gameControlGUI.SetTooltip("Shuffling together your deck and discard...");

		shopControlGUI.TurnOffAllShopGUI ();
	}

	public void SetAddedToCollectionText (string newText) {
		if (newText == "") {
			// turn the box off
		} else {
			newText += "You can add cards in your collection to your starting deck next time you play!";
		
			// do more stuff
		}
	}

	#region Utilities
	public void UnclickGoals () {
		for(int i = 0; i < GOALCANVASES.Length; i++) {
			GOALCANVASES[i].ContractGoalDisplay();
		}
	}
	#endregion
}
