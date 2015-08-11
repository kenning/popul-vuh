using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UnlockMenu : MonoBehaviour {

	public static bool UnlockMenuUp = false;
	ShopControl shopControl;

	int selectedGodTab;

	public GUISkin UNLOCKMENUGUISKIN;

	void Awake() {
//		GameObject gameController = GameObject.FindGameObjectWithTag ("GameController");
//		shopControl = gameController.GetComponent<ShopControl>();
	}

	public void ShowMenu() {
//		UnlockMenuUp = true;
//		FindUnlockBaseCost ();
	}

	

	void OnGUI () {
//		if(UnlockMenuUp) {
//
//			GUI.depth = 0;
//
//			if(GUI.Button(new Rect(0,0,Screen.width*.4f, Screen.height*.05f), "unlock all")) {
//				for(int i = 0; i < ShopControl.AllGods.Count; i++) {
//					CardLibrary.UnlockAllCards(ShopControl.AllGods[i]);
//					SaveData.AddGodInOrderToUnlocked(ShopControl.AllGods[i]);
//					SaveData.AddGodInOrderToFullyUnlocked(ShopControl.AllGods[i]);
//					FindUnlockBaseCost();
//				}
//			}
//
//			GUI.Box(new Rect(Screen.width*.1f, Screen.height*.05f, Screen.width*.8f, Screen.height*.1f), "Gods", UNLOCKMENUGUISKIN.customStyles[0]);
//			
//			GUI.BeginGroup(new Rect(Screen.width*.1f, Screen.height*.15f, Screen.width*.8f, Screen.height*.7f), "");
//			for(int i = 0; i < ShopControl.AllGods.Count; i++){
//				if(SaveData.UnlockedGods.Contains(ShopControl.AllGods[i]) && SaveData.FullyUnlockedGods.Contains(ShopControl.AllGods[i])) {
//					//Big box: you unlocked everything!
//					GUI.Box(new Rect(0,Screen.height*.1f*i, Screen.width*.7f, Screen.height*.1f), ShopControl.AllGods[i].ToString() + " fully unlocked!", UNLOCKMENUGUISKIN.customStyles[3]);
//				} else if (SaveData.UnlockedGods.Contains(ShopControl.AllGods[i])) {
//					//You unlocked this god, click button to unlock all cards
//					GUI.Box(new Rect(0, Screen.height*.1f*(i), Screen.width*.35f, Screen.height*.1f), ShopControl.AllGods[i].ToString() + " unlocked!", UNLOCKMENUGUISKIN.customStyles[3]);
//					if(GUI.Button(new Rect(Screen.width*.35f, Screen.height*.1f*(i), Screen.width*.35f, Screen.height*.1f), "Unlock all of " + ShopControl.AllGods[i].ToString() + "'s cards for £" + unlockAllCardCost.ToString(), UNLOCKMENUGUISKIN.customStyles[4])){
//						if(SaveData.UnlockBux >= unlockAllCardCost) {
//							CardLibrary.UnlockAllCards(ShopControl.AllGods[i]);
//							SaveData.AddGodInOrderToFullyUnlocked(ShopControl.AllGods[i]);
//							SaveData.UnlockBux += -unlockAllCardCost;
//						}
//						FindUnlockBaseCost();
//					}
//				} else {
//					//click this button to unlock this god
//					if(GUI.Button(new Rect(0, Screen.height*.1f*(i), Screen.width*.7f, Screen.height*.1f), "Unlock " + ShopControl.AllGods[i].ToString() + " for £" + unlockBaseCost.ToString(), UNLOCKMENUGUISKIN.customStyles[4])){
//						if(SaveData.UnlockBux >= unlockBaseCost) {
//							SaveData.AddGodInOrderToUnlocked(ShopControl.AllGods[i]);
//							SaveData.UnlockBux += -unlockBaseCost;
//						}
//						FindUnlockBaseCost();
//					}
//				}
//				GUI.Box(new Rect(0, Screen.height*.1f*i, Screen.width*.7f, Screen.height*.025f), ShopControl.GodDescriptions[i]);
//				GUI.Box(new Rect(Screen.width*.7f, Screen.height*.1f*(i), Screen.width*.1f, Screen.height*.1f), shopControl.GodIcons[i], GUIStyle.none);
//			}
//			GUI.EndGroup();
//
//			if(GUI.Button(new Rect(Screen.width*.1f, Screen.height*.875f, Screen.width*.4f, Screen.height*.1f), "Go back", UNLOCKMENUGUISKIN.button)) {
//				MainMenu.MainMenuUp = true;
//				UnlockMenuUp = false;
//				SaveLoad.Save();
//			}
//
//			GUI.Box(new Rect(Screen.width*.55f, Screen.height*.875f, Screen.width*.35f, Screen.height*.1f), SaveData.UnlockBux.ToString() + " God£", UNLOCKMENUGUISKIN.customStyles[0]);
//		}
	}
//
//	void FindUnlockBaseCost() {
//		int totalUnlockedThings = 0;
//		totalUnlockedThings += SaveData.UnlockedGods.Count;
//		totalUnlockedThings += SaveData.FullyUnlockedGods.Count;
//		Debug.Log("fully unlocked stuff: " + totalUnlockedThings.ToString());
//
//		unlockBaseCost = Mathf.RoundToInt((totalUnlockedThings*totalUnlockedThings*.25f*.25f) + totalUnlockedThings);
//		unlockAllCardCost = unlockBaseCost + 4;
//	}
}
