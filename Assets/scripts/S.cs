using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// Singletons!
public class S : MonoBehaviour {
  void Start() {
      useGUILayout = false;
  }
    
  public GameObject gc;
  public GameObject goalandshopparent;
  public GameObject shopgrid;
  public GameObject gridcursorcontrolgui;
  public GameObject deck;
    
  static Player playerInstance;
  static GameControl gameControlInstance;
  static GameControlGUI gameControlGUIInstance;
  static ShopControl shopControlInstance;
  static ShopControlGUI shopControlGUIInstance;
  static ClickControl clickControlInstance;
  static GridControl gridControlInstance;
  static GridCursorControlGUI gridCursorControlGUIInstance;
  static OptionControl optionControlInstance;
  static DragControl dragControlInstance;
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
  static DeckAnimate deckAnimateInstance;
  public static Player PlayerInst {
      get { return playerInstance; }
  }
  public static GameControl GameControlInst {
      get { return gameControlInstance; }
  }
  public static GameControlGUI GameControlGUIInst {
      get { return gameControlGUIInstance; }
  }
  public static ShopControl ShopControlInst {
      get { return shopControlInstance; }
  }
  public static ShopControlGUI ShopControlGUIInst {
      get { return shopControlGUIInstance; }
  }
  public static ClickControl ClickControlInst {
      get { return clickControlInstance; }
  }
  public static GridControl GridControlInst {
      get { return gridControlInstance; }
  }
  public static OptionControl OptionControlInst {
      get { return optionControlInstance; }
  }
  public static DragControl DragControlInst {
      get { return dragControlInstance; }
  }
  public static EventGUI EventGUIInst {
      get { return eventGUIInstance; }
  }
  public static CardLibrary CardLibraryInst {
      get { return cardLibraryInstance; }
  }
  public static EnemyLibrary EnemyLibraryInst {
      get { return enemyLibraryInstance; }
  }
  public static GUIStyleLibrary GUIStyleLibraryInst {
      get { return guiStyleLibraryInstance; }
  }
  public static GoalLibrary GoalLibraryInst {
      get { return goalLibraryInstance; }
  }
  public static MenuControl MenuControlInst {
      get { return menuControlInstance; }
  }
  public static MainMenu MainMenuInst {
      get { return mainMenuInstance; }
  }
  public static EncyclopediaMenu EncyclopediaMenuInst {
      get { return encyclopediaMenuInstance; }
  }
  public static GodChoiceMenu GodChoiceMenuInst {
      get { return godChoiceMenuInstance; }
  }
  public static CustomizeMenu CustomizeMenuInst {
      get { return customizeMenuInstance; }
  }
  public static Tutorial TutorialInst {
      get { return tutorialInstance; }
  }
  public static GridCursorControl GridCursorControlInst {
      get { return gridCursorControlInstance; }
  }
  public static GridCursorControlGUI GridCursorControlGUIInst {
      get { return gridCursorControlGUIInstance; }
  }
  public static ShopAndGoalParentCanvas ShopAndGoalParentCanvasInst {
      get { return shopAndGoalParentCanvasInstance; }
  }
  public static ShopGridCanvas ShopGridCanvasInst {
      get { return shopGridCanvasInstance; }
  }
  public static DeckAnimate DeckAnimateInst {
      get {return deckAnimateInstance;}
  }

  
  void Awake() {
        playerInstance = GameObject.Find("Player").GetComponent<Player>();
        
		gameControlInstance = gc.GetComponent<GameControl> ();
		gameControlGUIInstance = gc.GetComponent<GameControlGUI> ();
		shopControlInstance = gc.GetComponent<ShopControl> ();
		shopControlGUIInstance = gc.GetComponent<ShopControlGUI> ();
		clickControlInstance = gc.GetComponent<ClickControl> ();
        gridControlInstance = gc.GetComponent<GridControl>();
        optionControlInstance = gc.GetComponent<OptionControl>();
        dragControlInstance = gc.GetComponent<DragControl>();
        eventGUIInstance = gc.GetComponent<EventGUI>();
        enemyLibraryInstance = gc.GetComponent<EnemyLibrary>();
        cardLibraryInstance = gc.GetComponent<CardLibrary>();
        goalLibraryInstance = gc.GetComponent<GoalLibrary>();
        guiStyleLibraryInstance = gc.GetComponent<GUIStyleLibrary>();
        mainMenuInstance = gc.GetComponent<MainMenu>();
        encyclopediaMenuInstance = gc.GetComponent<EncyclopediaMenu>();
        godChoiceMenuInstance = gc.GetComponent<GodChoiceMenu>();
        customizeMenuInstance = gc.GetComponent<CustomizeMenu>();
        gridCursorControlInstance = gc.GetComponent<GridCursorControl>();
        menuControlInstance = gc.GetComponent<MenuControl>();
        tutorialInstance = gc.GetComponent<Tutorial>();
        deckAnimateInstance = deck.GetComponent<DeckAnimate>();

        shopAndGoalParentCanvasInstance = goalandshopparent
                                          .GetComponent<ShopAndGoalParentCanvas>();
        shopGridCanvasInstance = shopgrid
                                 .GetComponent<ShopGridCanvas>();
        gridCursorControlGUIInstance = gridcursorcontrolgui
                                       .GetComponent<GridCursorControlGUI>();
  }
}