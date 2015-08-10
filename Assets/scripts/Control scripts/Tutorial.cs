using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Tutorial : MonoBehaviour {

    //progress variables. tutorial level goes from 1-6.
    public static int TutorialLevel;

    public string TutorialMessage = "";
    string[] TutorialStartMessages = new string[] {
        "Hi, I'm Buluc Chabtan, the God of sacrifice and war.\n\n" + 
			"I'm here to teach you how to defend yourself in Xibalba, the world underneath yours.",  //level 1
        "You can move by clicking on a spot next to you. You can move one square a turn.", //level 2
        "Not bad. You can punch by clicking on an enemy next to you. Doing it spends your action for that turn.", //level 3
        "You killed it! Nice job, that's all I need you to do to finish a level.", //level 4
        "This time, your enemy is a bit further away from you. First select the pike card.", //level 5
        "Nice job! Now your hand has some boring cards from other Gods. Just tap any card twice.", //level 6
        "If you need to know what a card does, try holding your finger down to preview the full card. " + 
			"\n\nThat should be enough to get you started. I'll be watching over you, " + 
        	"and if you do well enough, other Gods will lend you their power too. Good luck." //Level 7
    };

    //Inserted from within Unity
    public Texture2D BulucTUTORIALFULL;
    public Texture2D BulucTUTORIALMUGSHOT;

    public Texture2D BLACKBOX;

	GUIStyleLibrary styleLibrary;

    ClickControl clickBoss;
    GameControl battleBoss;
	GameControlUI gameControlUI;

    public static bool PlayedACardLevel7 = false;

	void Start() {
		styleLibrary = gameObject.GetComponent<GUIStyleLibrary> ();
		gameControlUI = gameObject.GetComponent<GameControlUI> ();
	}

    public void Initialize()
    {
        clickBoss = gameObject.GetComponent<ClickControl>();
        battleBoss = gameObject.GetComponent<GameControl>();
    }

    public void OnGUI()
    {
        if(TutorialLevel == 0) return;

        if (TutorialLevel == 1 | TutorialLevel == 7)
        {
            GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), BLACKBOX);
            GUI.DrawTexture(new Rect(Screen.width * .55f, Screen.height * .2f, Screen.width * .4f, Screen.height * .6f), 
			                BulucTUTORIALFULL);
            GUI.Box(new Rect(Screen.width*.05f, Screen.height*.25f, Screen.width*.58f, Screen.height*.5f), 
			        TutorialStartMessages[TutorialLevel-1], styleLibrary.TutorialStyles.DialogueBox);
            if(GUI.Button(new Rect(Screen.width*.2f, Screen.height*.85f, Screen.width*.6f, Screen.height*.1f), 
			              "OK", styleLibrary.TutorialStyles.StartButton))
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
            GUI.Box(new Rect(0,0,Screen.width*.78f, Screen.width*.325f), TutorialMessage, styleLibrary.TutorialStyles.DialogueBox);

            if (TutorialLevel == 4 )
            {
                if (GUI.Button(new Rect(Screen.width * .2f, Screen.height * .55f, Screen.width * .6f, Screen.height * .075f), 
				               "OK", styleLibrary.TutorialStyles.StartButton))
                {
                    continueTutorial();
                }
            }
            if ((TutorialLevel == 6 && PlayedACardLevel7))
            {
                if (GUI.Button(new Rect(Screen.width * .2f, Screen.height * .55f, Screen.width * .6f, Screen.height * .075f), 
				               "Done with the tutorial", styleLibrary.TutorialStyles.StartButton))
                {
                    continueTutorial();
                }
            }
        }

        #region always on buttons
        float offset = 0;
        if (TutorialLevel == 1 | TutorialLevel == 7)
            offset = Screen.height * .15f;
        GUI.Box(new Rect(Screen.width * .6f, 0, Screen.width * .4f, Screen.height * .035f), 
		        "Tutorial level " + TutorialLevel.ToString(), styleLibrary.TutorialStyles.InfoBox);
        if (GUI.Button(new Rect(Screen.width * .05f, Screen.height * .235f - offset, Screen.width * .4f, Screen.height * .06f), 
		               "Next topic", styleLibrary.TutorialStyles.NextLevelButton))
        {
            if (TutorialLevel == 7)
                endTutorial();
            else
                continueTutorial();
        }
        if (GUI.Button(new Rect(Screen.width * .55f, Screen.height * .235f - offset, Screen.width * .4f, Screen.height * .06f), 
		               "Skip tutorial", styleLibrary.TutorialStyles.NextLevelButton))
        {
            endTutorial();
        }
        #endregion

    }

    #region Functions that happen during the tutorial

    //ends tutorial
    void endTutorial()
    {
        Tutorial.TutorialLevel = 0;
        MainMenu.MainMenuUp = true;
     
        MainMenu.CleanUpGameboard();

        SaveData.FinishedTutorial = true;
        SaveLoad.Save();
        
        gameObject.GetComponent<GridControl>().CancelInvoke();
    }

    //continues to next level of tutorial and resets finishedthislevel bool
    void continueTutorial()
    {
        TutorialLevel++;
        tutorialLevelSetup();
    }

    //this has all the differences between tutorial stages. 
    //it's very important
    void tutorialLevelSetup()
    {
        TutorialMessage = TutorialStartMessages[TutorialLevel-1];

        if (TutorialLevel == 2)
        {
            clickBoss.DisallowEveryInput();
            clickBoss.AllowMoveInput = true;
            clickBoss.AllowInputUmbrella = true;

            gameObject.GetComponent<GridControl>().InvokeRepeating("TutorialMovementIllustration", .4f, 1.2f);
        }
        if (TutorialLevel == 3)
        {
            gameObject.GetComponent<GridControl>().CancelInvoke();

            clickBoss.DisallowEveryInput();
            clickBoss.AllowNewPlayInput = true;
            clickBoss.AllowInputUmbrella = true;

            GridUnit p = GameObject.FindGameObjectWithTag("Player").GetComponent<GridUnit>();
            gameObject.GetComponent<EnemyLibrary>().LoadEnemy("Mud Warrior", p.xPosition, p.yPosition-1);
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
            clickBoss.DisallowEveryInput();
			gameControlUI.Dim();
        }
        if (TutorialLevel == 5)
        {
            gameObject.GetComponent<GridControl>().CancelInvoke();

			gameControlUI.Dim(false);
            List<string> deck = battleBoss.Deck;
            deck.Add("Wooden Pike");
            deck.Add("Wooden Pike");
            deck.Add("Iron Macana");
            deck.Add("Iron Macana");
            int count = deck.Count;
            for (int i = 0; i < count; i++) { battleBoss.Draw(); }
            battleBoss.AddPlays(1);

            clickBoss.DisallowEveryInput();
            clickBoss.AllowNewPlayInput = true;
            clickBoss.AllowInfoInput = true;
            clickBoss.AllowInputUmbrella = true;

            GridUnit p = GameObject.FindGameObjectWithTag("Player").GetComponent<GridUnit>();
            p.xPosition = 0;
            p.yPosition = 0;
            p.gameObject.transform.position = new Vector3(0, 0, 0);

            gameObject.GetComponent<EnemyLibrary>().LoadEnemy("Mud Warrior", p.xPosition, p.yPosition - 2);
        }
        if (TutorialLevel == 6)
        {
            battleBoss.DeleteAllCards();

            List<string> deck = battleBoss.Deck;
            deck.Add("Apple");
            deck.Add("Coffee");
            deck.Add("Cloth Shoes");
            deck.Add("Quick Prayer");
            int count = deck.Count;
            for (int i = 0; i < count; i++) { battleBoss.Draw(); }
            deck.Add("Quick Prayer");

            battleBoss.AddPlays(4);

            clickBoss.DisallowEveryInput();
            clickBoss.AllowNewPlayInput = true;
            clickBoss.AllowInfoInput = true;
            clickBoss.AllowInputUmbrella = true;
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
