using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Tutorial : MonoBehaviour {

    //progress variables. tutorial level goes from 1-6.
	public static int TutorialLevel = 0;
    
    public string TutorialMessage = "";
    string[] TutorialStartMessages = new string[] {
        "piecmklnhdgojf gjlnipokjlehfgmindpkohm pnmolijhk onmgfljpionhkmejklp",  //level 1
        "agjlpbekfchoindm okjlehfgmindp gfljpionhkme", //level 2
        "gfljpionhkme afhnecjlkbmgdpoi glnhofjeikpm", //level 3
        "ingmfhlopkj ojnmigphlk", //level 4
        "nglhojipkm jnhiplkomg ", //level 5
        "blajpeohgifndcmk hmnlopijk iojkmlnp dojbfghcepknlmi", //level 6
        "ingmfhlopkj ojnmigphlk okjlehfgmindp", //level 7
        "mpnokjl omljnpk hlikjmpno mpkjlon mhkgceldpon egmndfoip" + 
			"\n\njkoelpicnmdfbhg hejdcoimnlfgbpk mglidnojkfhecpokphifnmgjl " + 
        	" pnlgkocjefmhid koijplhnm" //Level 8
    };

    //Inserted from within Unity
    public Texture2D BulucTUTORIALFULL;
    public Texture2D BulucTUTORIALMUGSHOT;

    public Texture2D BLACKBOX;

    public static bool PlayedACardLevel6 = false;
    public static bool PlayedACardLevel5 = false;

	void Start() {
		useGUILayout = false;
	}

    public void OnGUI()
    {
        if (TutorialLevel == 1 | TutorialLevel == 8)
        {
            GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), BLACKBOX);
            GUI.DrawTexture(new Rect(Screen.width * .4f, Screen.height * .2f, Screen.width * .55f, Screen.height * .6f), 
			                BulucTUTORIALFULL);
			GUIStyle dialogueStyle = new GUIStyle(S.GUIStyleLibraryInst.TutorialStyles.DialogueBox);
			if(TutorialLevel == 1) dialogueStyle.fontSize = S.GUIStyleLibraryInst.TutorialStyles.FirstDialogueFontSize;
            GUI.Box(new Rect(Screen.width*.05f, Screen.height*.25f, Screen.width*.58f, Screen.height*.5f), 
			        TutorialStartMessages[TutorialLevel-1], dialogueStyle);
            if(GUI.Button(new Rect(Screen.width*.2f, Screen.height*.85f, Screen.width*.6f, Screen.height*.1f), 
			              "OK", S.GUIStyleLibraryInst.TutorialStyles.StartButton))
            {
                if(TutorialLevel == 1) 
                { 
                    continueTutorial();
                }
                else 
                {
                    endTutorial();
                }
            }
        }
        else
        {
         //   GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.width*.3f), BLACKBOX);
            GUI.DrawTexture(new Rect(Screen.width*.7f, 0, Screen.width*.3f, Screen.width*.3f), BulucTUTORIALMUGSHOT);
            GUI.Box(new Rect(Screen.height*.03f, Screen.width*.025f, Screen.width*.75f, Screen.width*.3f), TutorialMessage, S.GUIStyleLibraryInst.TutorialStyles.DialogueBox);

            if (TutorialLevel == 4 )
            {
				GUIStyle OKStyle = new GUIStyle(S.GUIStyleLibraryInst.TutorialStyles.NextLevelButton);
				OKStyle.fontSize = S.GUIStyleLibraryInst.TutorialStyles.DoneFontSize;
                if (GUI.Button(new Rect(Screen.width * .2f, Screen.height * .55f, Screen.width * .6f, Screen.height * .125f), 
				               "I know how to move and attack enemies", OKStyle))
                {
                    continueTutorial();
                }
            }
            if ((TutorialLevel == 6 && PlayedACardLevel5))
            {
				GUIStyle DoneStyle = new GUIStyle(S.GUIStyleLibraryInst.TutorialStyles.NextLevelButton);
				DoneStyle.fontSize = S.GUIStyleLibraryInst.TutorialStyles.DoneFontSize;
                if (GUI.Button(new Rect(Screen.width * .2f, Screen.height * .55f, Screen.width * .6f, Screen.height * .075f), 
				               "I know how to play weapon cards", S.GUIStyleLibraryInst.TutorialStyles.NextLevelButton))
                {
                    continueTutorial();
                }
            }
            if ((TutorialLevel == 7 && PlayedACardLevel6))
            {
				GUIStyle DoneStyle = new GUIStyle(S.GUIStyleLibraryInst.TutorialStyles.NextLevelButton);
				DoneStyle.fontSize = S.GUIStyleLibraryInst.TutorialStyles.DoneFontSize;
                if (GUI.Button(new Rect(Screen.width * .2f, Screen.height * .55f, Screen.width * .6f, Screen.height * .075f), 
				               "I know how to play other cards", S.GUIStyleLibraryInst.TutorialStyles.NextLevelButton))
                {
                    continueTutorial();
                }
            }
        }

        #region always on buttons
        if (TutorialLevel != 1 && TutorialLevel != 8) {
	        GUI.Box(new Rect(Screen.width * .6f, 0, Screen.width * .4f, Screen.height * .035f), 
			        "Tutorial level " + TutorialLevel.ToString(), S.GUIStyleLibraryInst.TutorialStyles.InfoBox);
	        if (GUI.Button(new Rect(Screen.width * .05f, Screen.height * .235f, Screen.width * .4f, Screen.height * .06f), 
			               "Skip this tutorial level", S.GUIStyleLibraryInst.TutorialStyles.NextLevelButton))
	        {
	        	continueTutorial();
	        }
	        if (GUI.Button(new Rect(Screen.width * .55f, Screen.height * .235f, Screen.width * .4f, Screen.height * .06f), 
			               "Skip whole tutorial", S.GUIStyleLibraryInst.TutorialStyles.NextLevelButton))
	        {
	            endTutorial();
	        }
		}
		if(TutorialLevel == 1) {
			if (GUI.Button(new Rect(Screen.width * .3f, Screen.height * .1f, Screen.width * .4f, Screen.height * .08f), 
			               "Skip tutorial", S.GUIStyleLibraryInst.TutorialStyles.NextLevelButton))
			{
				endTutorial();
			}
		}
        #endregion

    }

    #region Functions that happen during the tutorial

    //ends tutorial
    void endTutorial()
    {
        Tutorial.TutorialLevel = 0;

		S.MenuControlInst.TurnOnMenu (MenuControl.MenuType.MainMenu);

        SaveDataControl.FinishedTutorial = true;
        SaveDataControl.Save();
        
        S.ClickControlInst.CardHasntBeenClickedOn();
        
        gameObject.GetComponent<GridControl>().CancelInvoke();
    }

    //continues to next level of tutorial and resets finishedthislevel bool
    void continueTutorial()
    {
        TutorialLevel++;
		Debug.Log ("going to tutorial level" + TutorialLevel.ToString ());
        tutorialLevelSetup();
    }

    //this has all the differences between tutorial stages. 
    //it's very important
    void tutorialLevelSetup()
    {
        TutorialMessage = TutorialStartMessages[TutorialLevel-1];

        if (TutorialLevel == 2)
        {
            S.GameControlInst.SetMoves(1);
            S.GameControlInst.SetPlays(1);

            S.ClickControlInst.DisallowEveryInput();
            S.ClickControlInst.AllowMoveInput = true;
            S.ClickControlInst.AllowInputUmbrella = true;

            gameObject.GetComponent<GridControl>().InvokeRepeating("TutorialMovementIllustration", .4f, 1.2f);
        }
        if (TutorialLevel == 3)
        {
            gameObject.GetComponent<GridControl>().CancelInvoke();

            S.ClickControlInst.DisallowEveryInput();
            S.ClickControlInst.AllowNewPlayInput = true;
            S.ClickControlInst.AllowInputUmbrella = true;

            GridUnit p = GameObject.FindGameObjectWithTag("Player").GetComponent<GridUnit>();
            gameObject.GetComponent<EnemyLibrary>().LoadEnemy("pa", p.xPosition, p.yPosition-1);
            gameObject.GetComponent<GridControl>().InvokeRepeating("TutorialPunchIllustration", .4f, 1.2f);
        }
        if (TutorialLevel == 4)
        {
            gameObject.GetComponent<GridControl>().CancelInvoke();

            GameObject en = GameObject.FindGameObjectWithTag("Enemy");
            if (en != null)
            {
                Destroy(en);
            }
            S.ClickControlInst.DisallowEveryInput();
			S.GameControlGUIInst.Dim();
        }
        if (TutorialLevel == 5)
        {
            gameObject.GetComponent<GridControl>().CancelInvoke();

			S.GameControlGUIInst.Dim(false);
			S.GameControlInst.DeleteAllCards();
			List<string> deck = S.GameControlInst.Deck;
            deck.Add("Wooden Pike");
            deck.Add("Wooden Pike");
            deck.Add("Iron Macana");
            deck.Add("Iron Macana");
			GameObject.Find("Hand").transform.localPosition = new Vector3(-0.7f, 0, 0);
            int count = deck.Count;
            for (int i = 0; i < count; i++) { S.GameControlInst.Draw(); }
            S.GameControlInst.AddPlays(1);

            S.ClickControlInst.DisallowEveryInput();
            S.ClickControlInst.AllowNewPlayInput = true;
            S.ClickControlInst.AllowInfoInput = true;
            S.ClickControlInst.AllowInputUmbrella = true;

            GridUnit p = GameObject.FindGameObjectWithTag("Player").GetComponent<GridUnit>();
            p.xPosition = 0;
            p.yPosition = 0;
            p.gameObject.transform.position = new Vector3(0, 0, 0);

            gameObject.GetComponent<EnemyLibrary>().LoadEnemy("pa", p.xPosition, p.yPosition - 2);
        }
        if (TutorialLevel == 6) {
			S.GameControlGUIInst.Dim();            
            S.ClickControlInst.DisallowEveryInput();
			GameObject en = GameObject.FindGameObjectWithTag("Enemy");
			if (en != null)
			{
				Destroy(en);
			}
			
			S.GameControlInst.DeleteAllCards();
        }
        if (TutorialLevel == 7)
        {
			List<string> deck = S.GameControlInst.Deck;
            deck.Add("Apple");
            deck.Add("Coffee");
            deck.Add("Cloth Shoes");
            deck.Add("Quick Prayer");
            
            int count = deck.Count;
            for (int i = 0; i < count; i++) { S.GameControlInst.Draw(); }
            deck.Add("Quick Prayer");
            deck.Add("Coffee");
            S.GameControlInst.CheckDeckCount();

            S.GameControlInst.AddPlays(4);

            S.ClickControlInst.DisallowEveryInput();
            S.ClickControlInst.AllowNewPlayInput = true;
            S.ClickControlInst.AllowInfoInput = true;
            S.ClickControlInst.AllowInputUmbrella = true;

			S.GameControlGUIInst.Dim(false);
        }
    }

    public void TutorialTrigger(int tutorialLevel)
    {
        if (TutorialLevel == tutorialLevel) 
        {
            Invoke("continueTutorial", .5f);
        }
    }
    #endregion
}
