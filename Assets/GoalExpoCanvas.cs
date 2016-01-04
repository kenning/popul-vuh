﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GoalExpoCanvas : MonoBehaviour {

	ShopControlGUI shopControlGUI;

	Image god;
	Text description;

	bool initialized;

	void Start() {
		Initialize();
	}

	void Initialize() {
		if(initialized) return;
		initialized = true;

		shopControlGUI = GameObject.FindGameObjectWithTag ("GameController").GetComponent<ShopControlGUI> ();

		Image[] images = gameObject.GetComponentsInChildren<Image> ();
		foreach(Image img in images) if(img.gameObject.name == "Goal expo full icon") god = img;
		description = gameObject.GetComponentInChildren<Text> ();
	}

	public void SetExpoInfo (Goal goal) {
		Initialize();

		int godNumber = ShopControl.AllGods.IndexOf (goal.God);
		if(godNumber == null) godNumber = 7;

		description.text = goal.God.ToString() + goal.Description;

		god.sprite = shopControlGUI.GodFullSprites [godNumber];
	}
}