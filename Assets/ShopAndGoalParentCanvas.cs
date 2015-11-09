using UnityEngine;
using System.Collections;

/// <summary>
/// Delegates methods to shop rows and generally does stuff that shoprowcanvases and goalcanvases can't.
/// Also does the whole goalexpo step by itself.
/// </summary>
public class ShopAndGoalParentCanvas : MonoBehaviour {

	ShopRowCanvas[] shopRowCanvases;
	GoalCanvas[] goalCanvases;

	ShopControl shopControl;
	ShopControlGUI shopControlGUI;
	GameControl gameControl;
	GameControlGUI gameControlGUI;

	ClickControl clickControl;

	void Start () {

	}

	#region Initialization methods
	public void SetUpShopRows (Goal[] goals, bool[] highScoreNotifications) {
		for(int i = 0; i < goals.Length; i++) {
			shopRowCanvases[i].SetShopRowInfo(goals[i], shopControl.CardsToBuyFrom[i], highScoreNotifications[i]);
		}

	}
	#endregion

	#region Turning expo, normal and shop guis on and off
	public void TurnOnShopGUI() {
		// set a box displaying the # of $ you have
		
		// turn on go to next level button
	}
	public void TurnOffShopGUI() {
	}
	public void TurnOnNormalGUI() {
	}
	public void TurnOffNormalGUI() {
	}
	public void TurnOnExpoGUI() {
		// somehow this stuff got deleted, but i think it's because it's very simple
	}
	public void TurnOffExpoGUI() {
	}

	#endregion

	public void FinishGoalExpo () {
		clickControl.Invoke ("AllowEveryInput", .1f);
		shopControlGUI.TurnOnNormalGUI ();
	}

	public void FinishShopping () {
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
		for(int i = 0; i < goalCanvases.Length; i++) {
			goalCanvases[i].ContractGoalDisplay();
		}
	}
	#endregion
}
