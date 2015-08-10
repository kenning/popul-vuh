using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GUIStyleLibrary : MonoBehaviour {

	public GUISkin MAINMENUGUISKIN;
	public GUISkin OPTIONGUISKIN;
	public GUISkin GAMECONTROLGUISKIN;

	public MainMenuStyleLibrary MainStyles;
	public EncyclopediaMenuStyleLibrary EncyclopediaStyles;
	public GodChoiceMenuStyleLibrary GodChoiceStyles;
	public CustomizeStyleLibrary CustomizeStyles;
	public OptionControlStyleLibrary OptionControlStyles;
	public GameControlUIStyleLibrary GameControlUIStyles;
	public TutorialStyleLibrary TutorialStyles;

	// Use this for initialization
	void Start () {
		MainStyles = new MainMenuStyleLibrary (MAINMENUGUISKIN);
		EncyclopediaStyles = new EncyclopediaMenuStyleLibrary (MAINMENUGUISKIN);
		GodChoiceStyles = new GodChoiceMenuStyleLibrary (MAINMENUGUISKIN);
		CustomizeStyles = new CustomizeStyleLibrary (MAINMENUGUISKIN);
		OptionControlStyles = new OptionControlStyleLibrary (OPTIONGUISKIN);
		GameControlUIStyles = new GameControlUIStyleLibrary (GAMECONTROLGUISKIN);
		TutorialStyles = new TutorialStyleLibrary (MAINMENUGUISKIN);
	}

	public class MainMenuStyleLibrary {
		public GUIStyle BlackBackground;
		public GUIStyle Title;
		public GUIStyle Button;
		public GUIStyle NewCardsAvailablePopup;
		public GUIStyle Sidebar;
		public GUIStyle DiedAndGotToLevel;

		public MainMenuStyleLibrary (GUISkin skin) {
			BlackBackground = skin.box;
			Title = skin.customStyles [0];
			Button = skin.button;
			NewCardsAvailablePopup = skin.customStyles [1];
			Sidebar = skin.customStyles [4];
			DiedAndGotToLevel = skin.customStyles[3];
		}
	}

	public class EncyclopediaMenuStyleLibrary {
		public GUIStyle BackButton;
		public GUIStyle TabOn;
		public GUIStyle TabOff;
		public GUIStyle InfoBox;
		public GUIStyle SubTab;
		public GUIStyle CardNameStyle;
		public GUIStyle CardTextStyle;

		public EncyclopediaMenuStyleLibrary (GUISkin skin) {
			BackButton = skin.button;
			TabOff = skin.customStyles[2];
			TabOn = skin.customStyles[1];
			InfoBox = skin.customStyles[5];
			SubTab = skin.customStyles[8];
			CardNameStyle = skin.customStyles[6];
			CardTextStyle = skin.customStyles[7];
		}
	}

	public class GodChoiceMenuStyleLibrary {
		public GUIStyle GodChoiceToggle;
		public GUIStyle GodChoiceToggleText;
		public GUIStyle BackButton;
		public GUIStyle Title;

		public GodChoiceMenuStyleLibrary (GUISkin skin) {
			GodChoiceToggle = skin.toggle;
			GodChoiceToggleText = skin.textField;
			BackButton = skin.button;
			Title = skin.customStyles[0];
		}
	}

	public class CustomizeStyleLibrary {
		public GUIStyle CardToggleAdd;
		public GUIStyle CardToggleRemove;
		public GUIStyle CardToggleOff;
		public GUIStyle RarityToggleOn;
		public GUIStyle RarityToggleOff;
		public GUIStyle CardNameStyle;
		public GUIStyle CardTextStyle;
		public GUIStyle InstructionInfoBox;
		public int CustomizeCardNameFontSize = 16;
		public int CustomizeCardTextFontSize = 13;

		public CustomizeStyleLibrary (GUISkin skin) {
			CardToggleAdd = skin.customStyles[10];
			CardToggleRemove = skin.customStyles[9];
			CardToggleOff = skin.customStyles[1];
			RarityToggleOn = skin.customStyles[1];
			RarityToggleOff = skin.customStyles[2];
			CardNameStyle = skin.customStyles[6];
			CardTextStyle = skin.customStyles[7];
			InstructionInfoBox = skin.customStyles[4];
		}
	}

	public class GameControlUIStyleLibrary {
		public GUIStyle DisplayTitle;
		public GUIStyle DisplayText;

		public GameControlUIStyleLibrary (GUISkin gameControlSkin) {
			DisplayTitle = gameControlSkin.customStyles[2];
			DisplayText = gameControlSkin.box;
		}
	}

	public class OptionControlStyleLibrary {
		public GUIStyle Box;
		public GUIStyle Button;

		public OptionControlStyleLibrary (GUISkin optionSkin) {
			Box = optionSkin.box;
			Button = optionSkin.button;
		}
	}

	public class TutorialStyleLibrary {
		public GUIStyle DialogueBox;
		public GUIStyle StartButton;
		public GUIStyle InfoBox;
		public GUIStyle NextLevelButton;

		public TutorialStyleLibrary (GUISkin skin) {
			DialogueBox = skin.customStyles[3];
			StartButton = skin.button;
			InfoBox = skin.customStyles[4];
			NextLevelButton = skin.customStyles[1];
		}
	}
}


