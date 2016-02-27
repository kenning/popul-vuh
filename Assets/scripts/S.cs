using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// Singletons!
public static class S {
  static bool initialized = false;

  static GameControl gameControlInstance;
  public static GameControl GameControlInst {
      get {
          if (!initialized) initialize();
          return gameControlInstance;
      }
  }
  
  static void initialize() {
  		GameObject tempGO = GameObject.FindGameObjectWithTag ("GameController");
		gameControlInstance = tempGO.GetComponent<GameControl> ();
		// gameControlGUI = tempGO.GetComponent<GameControlGUI> ();
		// shopControl = tempGO.GetComponent<ShopControl> ();
		// shopControlGUI = tempGO.GetComponent<ShopControlGUI> ();
		// clickControl = tempGO.GetComponent<ClickControl> ();
  }
}