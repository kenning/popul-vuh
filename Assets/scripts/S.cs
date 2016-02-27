using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// Singletons!
public static class S {
  static bool initialized = false;

  static GameControl gameControlInstance;
  static GameControlGUI gameControlGUIInstance;
  static ShopControl shopControlInstance;
  static ShopControlGUI shopControlGUIInstance;
  static ClickControl clickControlInstance;
  static GridControl gridControlInstance;
  static OptionControl optionControlInstance;
  static EventGUI eventGUIInstance;
  static CardLibrary cardLibraryInstance;
  static EnemyLibrary enemyLibraryInstance;
  static GUIStyleLibrary guiStyleLibraryInstance;
  static GoalLibrary goalLibraryInstance;
  static MainMenu mainMenuInstance;
  static EncyclopediaMenu encyclopediaMenuInstance;
  static GodChoiceMenu godChoiceMenuInstance;
  static CustomizeMenu customizeMenuInstance;
  static GridCursorControl gridCursorControlInstance;
  static ShopAndGoalParentCanvas shopAndGoalParentCanvasInstance;
  static ShopGridCanvas shopGridCanvasInstance;
  static MenuControl menuControlInstance;
  static Tutorial tutorialInstance;
  static GameObject handInstance;
  public static GameControl GameControlInst {
      get {
          if (!initialized) initialize();
          return gameControlInstance;
      }
  }
  public static GameControlGUI GameControlGUIInst {
      get {
          if (!initialized) initialize();
          return gameControlGUIInstance;
      }
  }
  public static ShopControl ShopControlInst {
      get {
          if (!initialized) initialize();
          return shopControlInstance;
      }
  }
  public static ShopControlGUI ShopControlGUIInst {
      get {
          if (!initialized) initialize();
          return shopControlGUIInstance;
      }
  }
  public static ClickControl ClickControlInst {
      get {
          if (!initialized) initialize();
          return clickControlInstance;
      }
  }
  public static GridControl GridControlInst {
      get {
          if (!initialized) initialize();
          return gridControlInstance;
      }
  }
  public static OptionControl OptionControlInst {
      get {
          if (!initialized) initialize();
          return optionControlInstance;
      }
  }
  public static EventGUI EventGUIInst {
      get {
          if (!initialized) initialize();
          return eventGUIInstance;
      }
  }
  public static CardLibrary CardLibraryInst {
      get {
          if (!initialized) initialize();
          return cardLibraryInstance;
      }
  }
  public static EnemyLibrary EnemyLibraryInst {
      get {
          if (!initialized) initialize();
          return enemyLibraryInstance;
      }
  }
  public static GUIStyleLibrary GUIStyleLibraryInst {
      get {
          if (!initialized) initialize();
          return guiStyleLibraryInstance;
      }
  }
  public static GoalLibrary GoalLibraryInst {
      get {
          if (!initialized) initialize();
          return goalLibraryInstance;
      }
  }
  public static MenuControl MenuControlInst {
      get {
          if (!initialized) initialize();
          return menuControlInstance;
      }
  }
  public static MainMenu MainMenuInst {
      get {
          if (!initialized) initialize();
          return mainMenuInstance;
      }
  }
  public static EncyclopediaMenu EncyclopediaMenuInst {
      get {
          if (!initialized) initialize();
          return encyclopediaMenuInstance;
      }
  }
  public static GodChoiceMenu GodChoiceMenuInst {
      get {
          if (!initialized) initialize();
          return godChoiceMenuInstance;
      }
  }
  public static CustomizeMenu CustomizeMenuInst {
      get {
          if (!initialized) initialize();
          return customizeMenuInstance;
      }
  }
  public static Tutorial TutorialInst {
      get {
          if (!initialized) initialize();
          return tutorialInstance;
      }
  }
  public static GridCursorControl GridCursorControlInst {
      get {
          if (!initialized) initialize();
          return gridCursorControlInstance;
      }
  }
  public static ShopAndGoalParentCanvas ShopAndGoalParentCanvasInst {
      get {
          if (!initialized) initialize();
          return shopAndGoalParentCanvasInstance;
      }
  }
  public static ShopGridCanvas ShopGridCanvasInst {
      get {
          if (!initialized) initialize();
          return shopGridCanvasInstance;
      }
  }
  public static GameObject HandInst {
      get {
          if (!initialized) initialize();
          return handInstance;
      }
  }
  static void initialize() {
  		GameObject tempGO = GameObject.FindGameObjectWithTag ("GameController");
		gameControlInstance = tempGO.GetComponent<GameControl> ();
		gameControlGUIInstance = tempGO.GetComponent<GameControlGUI> ();
		shopControlInstance = tempGO.GetComponent<ShopControl> ();
		shopControlGUIInstance = tempGO.GetComponent<ShopControlGUI> ();
		clickControlInstance = tempGO.GetComponent<ClickControl> ();
        gridControlInstance = tempGO.GetComponent<GridControl>();
        optionControlInstance = tempGO.GetComponent<OptionControl>();
        eventGUIInstance = tempGO.GetComponent<EventGUI>();
        enemyLibraryInstance = tempGO.GetComponent<EnemyLibrary>();
        cardLibraryInstance = tempGO.GetComponent<CardLibrary>();
        goalLibraryInstance = tempGO.GetComponent<GoalLibrary>();
        guiStyleLibraryInstance = tempGO.GetComponent<GUIStyleLibrary>();
        mainMenuInstance = tempGO.GetComponent<MainMenu>();
        encyclopediaMenuInstance = tempGO.GetComponent<EncyclopediaMenu>();
        godChoiceMenuInstance = tempGO.GetComponent<GodChoiceMenu>();
        customizeMenuInstance = tempGO.GetComponent<CustomizeMenu>();
        gridCursorControlInstance = tempGO.GetComponent<GridCursorControl>();
        menuControlInstance = tempGO.GetComponent<MenuControl>();
        tutorialInstance = tempGO.GetComponent<Tutorial>();

        shopAndGoalParentCanvasInstance = 
            GameObject.FindGameObjectWithTag("goalandshopparent")
            .GetComponent<ShopAndGoalParentCanvas>();
        shopGridCanvasInstance = 
            GameObject.FindGameObjectWithTag("shopgrid")
            .GetComponent<ShopGridCanvas>();
        handInstance = GameObject.Find("Hand");
  }
}