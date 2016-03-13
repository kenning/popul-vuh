using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GUIStyleLibrary : MonoBehaviour {
	
	public GUISkin MAINMENUGUISKIN;
	public GUISkin OPTIONGUISKIN;
	public GUISkin GAMECONTROLGUISKIN;
	public GUISkin SHOPSKIN;
	
	public MainMenuStyleLibrary MainStyles;
	public EncyclopediaMenuStyleLibrary EncyclopediaStyles;
	public GodChoiceMenuStyleLibrary GodChoiceStyles;
	public CustomizeStyleLibrary CustomizeStyles;
	public OptionControlStyleLibrary OptionControlStyles;
	public GameControlGUIStyleLibrary GameControlGUIStyles;
	public TutorialStyleLibrary TutorialStyles;
	public CardStyleLibrary CardStyles;
	public ShopStyleLibrary ShopStyles;
	
	// Use this for initialization
	void Start () {
		MainStyles = new MainMenuStyleLibrary (MAINMENUGUISKIN);
		EncyclopediaStyles = new EncyclopediaMenuStyleLibrary (MAINMENUGUISKIN);
		GodChoiceStyles = new GodChoiceMenuStyleLibrary (MAINMENUGUISKIN);
		CustomizeStyles = new CustomizeStyleLibrary (MAINMENUGUISKIN);
		OptionControlStyles = new OptionControlStyleLibrary (OPTIONGUISKIN);
		GameControlGUIStyles = new GameControlGUIStyleLibrary (GAMECONTROLGUISKIN);
		TutorialStyles = new TutorialStyleLibrary (MAINMENUGUISKIN);
		CardStyles = new CardStyleLibrary (GAMECONTROLGUISKIN);
		ShopStyles = new ShopStyleLibrary (SHOPSKIN, MAINMENUGUISKIN);
	}
	
	public class StyleLibrary {
		public GUIStyle SizeDecorator (GUIStyle inputStyle) {
			if (Application.platform == RuntimePlatform.IPhonePlayer) {
				GUIStyle newStyle = new GUIStyle (inputStyle);
				newStyle.fontSize = newStyle.fontSize * 5 / 3;
				return newStyle;
			} else {
				return inputStyle;
			}
		}
		public int SizeIntDecorator(int inputInt) {
			if (Application.platform == RuntimePlatform.IPhonePlayer) {
				return inputInt * 5 / 3;
			} else {
				return inputInt;
			}
		}
	}
	
	public class MainMenuStyleLibrary : StyleLibrary {
		public GUIStyle BlackBackground;
		public GUIStyle Title;
		public GUIStyle Button;
        public GUIStyle HighlitButton;
		public GUIStyle NewCardsAvailablePopup;
		public GUIStyle Sidebar;
		public GUIStyle DiedAndGotToLevel;
		public GUIStyle NeutralButton;
		public GUIStyle Scrollbar;
        public GUIStyle Scrollbarthumb;
		public MainMenuStyleLibrary (GUISkin skin) {
			BlackBackground = SizeDecorator(skin.box);
			Title = SizeDecorator(skin.customStyles [0]);
			Button = SizeDecorator(skin.button);
            HighlitButton = SizeDecorator(skin.customStyles[13]);
			NewCardsAvailablePopup = SizeDecorator(skin.customStyles [9]);
			Sidebar = SizeDecorator(skin.customStyles [4]);
			DiedAndGotToLevel = SizeDecorator(skin.customStyles[12]);
			NeutralButton = SizeDecorator(skin.customStyles[12]);
            Scrollbar = skin.verticalScrollbar;
            Scrollbarthumb = skin.verticalScrollbarThumb;

            Scrollbar.fixedWidth = Screen.width*.1f;
            Scrollbarthumb.fixedWidth = Screen.width*.1f;
		}
	}
	
	public class EncyclopediaMenuStyleLibrary : StyleLibrary {
		public GUIStyle BackButton;
		public GUIStyle TabOn;
		public GUIStyle TabOff;
		public GUIStyle InfoBox;
		public GUIStyle SubTab;
		public GUIStyle CardNameStyle;
		public GUIStyle CardTextStyle;
		public GUIStyle NoneStyleWordWrap;
		public GUIStyle BigText;
		public GUIStyle NeutralButton;
		public int RedirectButtonFontSize;
		public int GoalFontSize;
		public int SlightlyBiggerTextFontSize;
		public int CardButtonFontSize;
		public int GodFontDescriptionSize;
		
		public EncyclopediaMenuStyleLibrary (GUISkin skin) {
			BackButton = SizeDecorator(skin.button);
			TabOff = SizeDecorator(skin.customStyles[2]);
			TabOn = SizeDecorator(skin.customStyles[1]);
			InfoBox = SizeDecorator(skin.customStyles[5]);
			SubTab = SizeDecorator(skin.customStyles[8]);
			CardNameStyle = SizeDecorator(skin.customStyles[6]);
			CardTextStyle = SizeDecorator(skin.customStyles[7]);
			NoneStyleWordWrap = SizeDecorator(new GUIStyle (skin.customStyles[11]));
			NoneStyleWordWrap.wordWrap = true;
			NoneStyleWordWrap.fontSize = SizeIntDecorator(10);
			BigText = SizeDecorator(new GUIStyle(skin.customStyles[11]));
			BigText.fontSize = 50;
			GoalFontSize = SizeIntDecorator(10);
			SlightlyBiggerTextFontSize = SizeIntDecorator(18);
			CardButtonFontSize = SizeIntDecorator(15);
			RedirectButtonFontSize = SizeIntDecorator(20);
			GodFontDescriptionSize = SizeIntDecorator(14);
			NeutralButton = SizeDecorator(skin.customStyles[12]);
		}
	}
	
	public class GodChoiceMenuStyleLibrary : StyleLibrary {
		public GUIStyle GodChoiceToggle;
		public GUIStyle GodChoiceToggleText;
		public GUIStyle BackButton;
		public GUIStyle Title;
		//
		public GodChoiceMenuStyleLibrary (GUISkin skin) {
			//      GodChoiceToggle = SizeDecorator(skin.toggle);
			//      GodChoiceToggleText = SizeDecorator(skin.textField);
			//      BackButton = SizeDecorator(skin.button);
			//      Title = SizeDecorator(skin.customStyles[0]);
		}
	}
	
	public class CustomizeStyleLibrary : StyleLibrary {
		public GUIStyle CardToggleAdd;
		public GUIStyle CardToggleRemove;
		public GUIStyle CardToggleOff;
		public GUIStyle CardNeutral;
		public GUIStyle RarityToggleOn;
		public GUIStyle RarityToggleOff;
		public GUIStyle InstructionInfoBox;
		
		// Card displaying GUI
		public GUIStyle CardNameStyle;
		public GUIStyle CardTextStyle;
		public int CustomizeCardNameFontSize;
		public int CustomizeCardTextFontSize;
		
		public CustomizeStyleLibrary (GUISkin skin) {
			CustomizeCardNameFontSize = SizeIntDecorator(16);
			CustomizeCardTextFontSize = SizeIntDecorator(13);
			CardToggleAdd = SizeDecorator(skin.customStyles[10]);
			CardToggleRemove = SizeDecorator(skin.customStyles[9]);
			CardToggleOff = SizeDecorator(skin.customStyles[2]);
			CardNeutral = SizeDecorator(skin.customStyles[12]);
			RarityToggleOn = SizeDecorator(skin.customStyles[1]);
			RarityToggleOff = SizeDecorator(skin.customStyles[2]);
			CardNameStyle = SizeDecorator(skin.customStyles[6]);
			CardTextStyle = SizeDecorator(skin.customStyles[7]);
			InstructionInfoBox = SizeDecorator(skin.customStyles[4]);
		}
	}
	
	public class GameControlGUIStyleLibrary : StyleLibrary {
		public GUIStyle DisplayTitle;
		public GUIStyle DisplayText;
		public GUIStyle TooltipBox;
		
		public GameControlGUIStyleLibrary (GUISkin gameControlSkin) {
			DisplayTitle = SizeDecorator(gameControlSkin.customStyles[2]);
			DisplayText = SizeDecorator(gameControlSkin.box);
			TooltipBox = SizeDecorator(gameControlSkin.textArea);
		}
	}
	
	public class OptionControlStyleLibrary : StyleLibrary {
		public GUIStyle Box;
		public GUIStyle Button;
		
		public OptionControlStyleLibrary (GUISkin optionSkin) {
			Box = SizeDecorator(optionSkin.box);
			Button = SizeDecorator(optionSkin.button);
		}
	}
	
	public class TutorialStyleLibrary : StyleLibrary {
		public GUIStyle DialogueBox;
		public GUIStyle StartButton;
		public GUIStyle InfoBox;
		public GUIStyle NextLevelButton;
		public int DoneFontSize;
		public int FirstDialogueFontSize;
		
		public TutorialStyleLibrary (GUISkin skin) {
			DoneFontSize = SizeIntDecorator(18);
			FirstDialogueFontSize = SizeIntDecorator(35);
			DialogueBox = SizeDecorator(skin.customStyles[3]);
			StartButton = SizeDecorator(skin.button);
			InfoBox = SizeDecorator(skin.customStyles[4]);
			NextLevelButton = SizeDecorator(skin.customStyles[1]);
            NextLevelButton.fontSize = SizeIntDecorator(14); 
		}
	}
	
	public class CardStyleLibrary : StyleLibrary {
		public GUIStyle Tooltip;
		
		public CardStyleLibrary (GUISkin gameControlSkin) {
			Tooltip = SizeDecorator(gameControlSkin.textArea);
		}
	}
	
	public class ShopStyleLibrary : StyleLibrary {
		// Goal exposition GUI
		public GUIStyle TransparentBackground;
		public GUIStyle GoalExpoBox;
		public GUIStyle GotItButton;
		public GUIStyle SmallerButton;
		
		// Cards to buy GUI
		// Card miniature display
		public GUIStyle DisplayTitle;
		public GUIStyle DisplayText;
		public int DisplayTitleFontSize;
		public int DisplayTextFontSize;
		public GUIStyle ShopBox;
		public GUIStyle ShopGoalGold;
		public GUIStyle ShopGoalSilver;
		public GUIStyle ShopGoalBronze;
		public GUIStyle ShopHoverOverlay;
		public GUIStyle NeutralButton;
		
		// In game goal GUI (shows three goals along top of screen)
		public GUIStyle InGameGoalBox;
		public GUIStyle InGameGoalBoxHoverOverlay;
		
		public ShopStyleLibrary (GUISkin shopSkin, GUISkin mainSkin) {
			DisplayTitleFontSize  = SizeIntDecorator(9);
			DisplayTextFontSize  = SizeIntDecorator(9);

			TransparentBackground = SizeDecorator(shopSkin.customStyles[0]);
			GoalExpoBox = SizeDecorator(shopSkin.box);
			GotItButton = SizeDecorator(shopSkin.button);
			SmallerButton = SizeDecorator(mainSkin.customStyles[1]);
			
			DisplayTitle = SizeDecorator(mainSkin.customStyles[6]);
			DisplayText = SizeDecorator(mainSkin.customStyles[7]);
			ShopBox = SizeDecorator(shopSkin.customStyles[8]);
			ShopGoalGold = SizeDecorator(shopSkin.customStyles[6]);
			ShopGoalSilver = SizeDecorator(shopSkin.customStyles[5]);
			ShopGoalBronze = SizeDecorator(shopSkin.customStyles[4]);
			ShopHoverOverlay = SizeDecorator(shopSkin.customStyles[3]);
			InGameGoalBox = SizeDecorator(shopSkin.textArea);
			InGameGoalBoxHoverOverlay = SizeDecorator(shopSkin.customStyles[1]);
			NeutralButton = SizeDecorator(shopSkin.customStyles[7]);
		}
	}
}


