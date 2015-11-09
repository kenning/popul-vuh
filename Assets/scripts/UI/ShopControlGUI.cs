using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ShopControlGUI : MonoBehaviour {

	ShopControl shopControl;
	GameControl gameControl;
	ClickControl clickControl;
	GameControlGUI gameControlGUI;
	GUIStyleLibrary styleLibrary;

	GoalCanvas[] goalCanvases;
	ShopAndGoalParentCanvas shopAndGoalParentCanvas;	

	public bool goalExpo = false;
	public bool normaldisplay = false;
	public bool shopGUI = false;
	
	public Sprite[] GodFullSprites;
	public Texture2D[] GodFullTextures;
	public Sprite[] SpriteGodIcons;
	public Texture2D[] GodIcons;
	public Sprite[] GodDisplayCards;
	public Texture2D[] CardTextures;
	public Sprite[] GodSmallCards;
	
	public Sprite Paper;
	public Sprite Bronze;
	public Sprite Silver;
	public Sprite Gold;
	public Texture2D PaperTexture;
	public Texture2D BronzeTexture;
	public Texture2D SilverTexture;
	public Texture2D GoldTexture;
	public Texture2D STOPLIGHTTEXTURE;

	Goal[] Goals;
	bool[] GoalDisplay;
	public bool[] highScoreNotification;
	
	public float shopGUITime = 0f;
	float cardWidth = Screen.width*.3f;
	float cardHeight = Screen.height*.16f;

	string AddedToCollText = "";
	
	void Start () {
		useGUILayout = false;
		gameControl = gameObject.GetComponent<GameControl> ();
		shopControl = gameObject.GetComponent<ShopControl> ();
		clickControl = gameObject.GetComponent<ClickControl> ();
		styleLibrary = gameObject.GetComponent<GUIStyleLibrary> ();
		gameControlGUI = gameObject.GetComponent<GameControlGUI> ();

		goalCanvases [0] = GameObject.Find ("Goal canvas 0").GetComponent<GoalCanvas>();
		goalCanvases [1] = GameObject.Find ("Goal canvas 1").GetComponent<GoalCanvas>();
		goalCanvases [2] = GameObject.Find ("Goal canvas 2").GetComponent<GoalCanvas>();

		goalExpo = false;
	}

	public void NewLevelNewGoals (int numberOfGods) {
		Goals = shopControl.Goals;
		GoalDisplay = new bool[numberOfGods];
		highScoreNotification = new bool[numberOfGods];
		
		goalExpo = true;
	}
	
	#region Setting canvas displaying info
	public void SetInitialGoalInfo (Goal[] goals) {
		for (int i = 0; i < goalCanvases.Length; i++) {
			goalCanvases[i].SetInitialGoalInfo(goals[i]);
		}
	}

	public void SetGoalGUIVariables() {
		for(int i = 0; i < Goals.Length; i++) {
			Goals[i].SetDisplayScore();
			goalCanvases[i].UpdateGoalInfo();
		}
	}
	#endregion

	#region Turning off and on GUI
	public void TurnOnExpoGUI () {
		TurnOffAllShopGUI ();
		
		shopAndGoalParentCanvas.TurnOnExpoGUI ();
	}

	public void TurnOnNormalGUI () {
		TurnOffAllShopGUI ();

		SetGoalGUIVariables ();

		shopAndGoalParentCanvas.TurnOnNormalGUI ();
	}

	public void TurnOnShopGUI() {
		for (int i = 0; i < shopControl.Goals.Length; i++) {
			if(SaveData.CheckForHighScores(shopControl.Goals[i])) {
				highScoreNotification[i] = true;
			}
		}

		shopAndGoalParentCanvas.SetUpShopRows (Goals, highScoreNotification);

		TurnOnShopGUI ();
	}

	/// <summary>
	/// Completely removes GUI elements. Useful for before you turn one on, and
	/// also used when shuffle animating.
	/// </summary>
	public void TurnOffAllShopGUI () {
		shopAndGoalParentCanvas.TurnOffNormalGUI ();
		shopAndGoalParentCanvas.TurnOffShopGUI ();
		shopAndGoalParentCanvas.TurnOffExpoGUI ();
	}
	#endregion

	#region Utilities
	public void UnclickGoals () {
		shopAndGoalParentCanvas.UnclickGoals ();
	}
	#endregion
}
