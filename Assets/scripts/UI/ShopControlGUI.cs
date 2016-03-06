using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ShopControlGUI : MonoBehaviour {

	public bool IgnoreClicking;

	public GoalCanvas[] GOALCANVASES;

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
	}

	public void NewLevelNewGoals (int numberOfGods, Goal[] goals) {
		Goals = goals;
		GoalDisplay = new bool[numberOfGods];

		TurnOnExpoGUI ();
		
		S.ShopAndGoalParentCanvasInst.NewLevelNewGoals (goals);
	}
	
	#region Setting canvas displaying info
	public void UpdateGoalInfos() {
		S.ShopAndGoalParentCanvasInst.UpdateGoalInfos (Goals); 
	}
	#endregion

	#region Turning off and on GUI
	public void TurnOnExpoGUI () {
		TurnOffAllShopGUI ();

		IgnoreClicking = true;
		
		S.ShopAndGoalParentCanvasInst.TurnOnExpoGUI ();
	}

	public void TurnOnNormalGUI () {
		TurnOffAllShopGUI ();

		UpdateGoalInfos ();

//		S.ShopAndGoalParentCanvasInst.TurnOnNormalGUI ();
	}

	public void TurnOnShopGUI() {
		highScoreNotification = new bool[S.ShopControlInst.Goals.Length];

		for (int i = 0; i < S.ShopControlInst.Goals.Length; i++) {
			if(SaveDataControl.CheckForHighScores(S.ShopControlInst.Goals[i])) {
				highScoreNotification[i] = true;
			}
		}

		IgnoreClicking = true;

		S.ShopAndGoalParentCanvasInst.SetUpShopRows ();

		StateSavingControl.TurnShopModeOn();
		StateSavingControl.Save();

		S.ShopAndGoalParentCanvasInst.TurnOnShopGUI ();
	}

	/// <summary>
	/// Completely removes GUI elements. Useful for before you turn one on, and
	/// also used when shuffle animating.
	/// </summary>
	public void TurnOffAllShopGUI () {
		IgnoreClicking = false;
		S.ShopAndGoalParentCanvasInst.TurnOffNormalGUI ();
		S.ShopAndGoalParentCanvasInst.TurnOffShopGUI ();
		S.ShopAndGoalParentCanvasInst.TurnOffExpoGUI ();
	}
	#endregion

	#region Utilities
	public void UnclickGoals () {
		S.ShopAndGoalParentCanvasInst.UnclickGoals ();
	}
	#endregion
}
