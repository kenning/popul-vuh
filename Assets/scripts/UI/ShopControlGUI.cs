using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ShopControlGUI : MonoBehaviour {

	ShopControl shopControl;
	ClickControl clickControl;
	GameControlGUI gameControlGUI;
	GUIStyleLibrary styleLibrary;

	public bool IgnoreClicking;

	public GoalCanvas[] GOALCANVASES;
	ShopAndGoalParentCanvas shopAndGoalParentCanvas;	

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

	public Sprite BOUGHTSPRITEADDEDTOCOLLECTION;
	public Sprite BOUGHTSPRITENORMAL;
	
	void Start () {
		useGUILayout = false;
		shopControl = gameObject.GetComponent<ShopControl> ();
		clickControl = gameObject.GetComponent<ClickControl> ();
		styleLibrary = gameObject.GetComponent<GUIStyleLibrary> ();
		gameControlGUI = gameObject.GetComponent<GameControlGUI> ();

		shopAndGoalParentCanvas = GameObject.FindGameObjectWithTag ("canvas").
			GetComponentInChildren<ShopAndGoalParentCanvas> ();
	}

	public void NewLevelNewGoals (int numberOfGods, Goal[] goals) {
		Goals = goals;
		GoalDisplay = new bool[numberOfGods];
		highScoreNotification = new bool[numberOfGods];

		TurnOnExpoGUI ();
		
		shopAndGoalParentCanvas.NewLevelNewGoals (goals);
	}
	
	#region Setting canvas displaying info
	public void UpdateGoalInfos() {
		shopAndGoalParentCanvas.UpdateGoalInfos (Goals); 
	}
	#endregion

	#region Turning off and on GUI
	public void TurnOnExpoGUI () {
		TurnOffAllShopGUI ();

		IgnoreClicking = true;
		
		shopAndGoalParentCanvas.TurnOnExpoGUI ();
	}

	public void TurnOnNormalGUI () {
		TurnOffAllShopGUI ();

		UpdateGoalInfos ();

//		shopAndGoalParentCanvas.TurnOnNormalGUI ();
	}

	public void TurnOnShopGUI() {
		for (int i = 0; i < shopControl.Goals.Length; i++) {
			if(SaveDataControl.CheckForHighScores(shopControl.Goals[i])) {
				highScoreNotification[i] = true;
			}
		}

		IgnoreClicking = true;

		shopAndGoalParentCanvas.SetUpShopRows (Goals, highScoreNotification);

		StateSavingControl.TurnShopModeOn();
		Debug.Log("huh?");
		StateSavingControl.Save();

		shopAndGoalParentCanvas.TurnOnShopGUI ();
	}

	/// <summary>
	/// Completely removes GUI elements. Useful for before you turn one on, and
	/// also used when shuffle animating.
	/// </summary>
	public void TurnOffAllShopGUI () {
		IgnoreClicking = false;
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
