using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Tutorial : MonoBehaviour {

    //progress variables. tutorial level goes from 1-6.
	public static int TutorialLevel = 0;
    
    public string TutorialMessage = "";
    string[] TutorialStartMessages = new string[] {
        "gjlnipokjlehfgmindpkohm",  //level 1
        "agjlpbe gfonhkme", //level 2
        "afhnecjlkbmgdpoi nhofjei", //level 3
        "ingmfhlo ojnmig", //level 4
        "ngljipk jnhipkg ", //level 5
        "hmnlopijk iojkm", //level 6
        "ingmfhlj o ehgmindp", //level 7
        "mp gceldpon jlkb ehgmindp\n\njkolfnmgjl" //Level 8
    };

    //Inserted from within Unity
    public Texture2D BulucTUTORIALFULL;
    public Texture2D BulucTUTORIALMUGSHOT;

    public Texture2D BLACKBOX;

    public static bool PlayedACardLevel6 = false;
    public static bool PlayedACardLevel5 = false;
    GameObject hand;
    List<GameObject> arrowList = new List<GameObject>();
	void Start() {
		useGUILayout = false;
        hand = GameObject.Find("Hand");
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
            string message = (TutorialLevel == 1) ? "What?" : "Thanks?";
            if(GUI.Button(new Rect(Screen.width*.2f, Screen.height*.85f, Screen.width*.6f, Screen.height*.1f), 
			              message, S.GUIStyleLibraryInst.TutorialStyles.StartButton))
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
	        if (GUI.Button(new Rect(Screen.width * .025f, Screen.height * .235f, Screen.width * .55f, Screen.height * .06f), 
			               "Skip this part of the tutorial", S.GUIStyleLibraryInst.TutorialStyles.NextLevelButton))
	        {
	        	continueTutorial();
	        }
	        if (GUI.Button(new Rect(Screen.width * .625f, Screen.height * .235f, Screen.width * .35f, Screen.height * .06f), 
			               "Skip whole tutorial", S.GUIStyleLibraryInst.TutorialStyles.NextLevelButton))
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

        if (TutorialLevel == 2) {
            S.GameControlInst.SetMoves(1);
            S.GameControlInst.SetPlays(1);

            S.ClickControlInst.DisallowEveryInput();
            S.ClickControlInst.AllowMoveInput = true;
            S.ClickControlInst.AllowInputUmbrella = true;

            gameObject.GetComponent<GridControl>().InvokeRepeating("TutorialMovementIllustration", .4f, 1.2f);
            
            MakeArrowInSpot(0, 1);
            MakeArrowInSpot(0, -1);
            MakeArrowInSpot(1, 0);
            MakeArrowInSpot(-1, 0);
        }
        if (TutorialLevel == 3) {
            gameObject.GetComponent<GridControl>().CancelInvoke();
            TurnOffArrows();

            S.ClickControlInst.DisallowEveryInput();
            S.ClickControlInst.AllowNewPlayInput = true;
            S.ClickControlInst.AllowInputUmbrella = true;

            GridUnit p = GameObject.FindGameObjectWithTag("Player").GetComponent<GridUnit>();
            gameObject.GetComponent<EnemyLibrary>().LoadEnemy("pa", p.xPosition+1, p.yPosition);
            MakeArrowInSpot(p.xPosition+1, p.yPosition);
            gameObject.GetComponent<GridControl>().InvokeRepeating("TutorialPunchIllustration", .4f, 1.2f);
        }
        if (TutorialLevel == 4)
        {
            TurnOffArrows();
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
            
            MakeArrow();
            MakeArrow();
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
            
            MakeArrow();
            Invoke("MakeArrow", .1f);            
            Invoke("MakeArrow", .2f);            
            Invoke("MakeArrow", .3f);            
            Invoke("MakeArrow", .4f);            
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
    
    public void TurnOffArrows() {
        foreach(GameObject arrow in arrowList) {
            Destroy(arrow);
        }
        arrowList = new List<GameObject>();
    }
    
    void MakeArrow() {
        arrowList.Add((GameObject)GameObject.Instantiate(Resources.Load("prefabs/pointer arrows")));
        arrowList[arrowList.Count-1].transform.parent = hand.transform;
        arrowList[arrowList.Count-1].transform.position = new Vector3(-2.25f + (arrowList.Count-1)*1.85f, -4.5f, 4);
    }
    
    public void MakeArrowInSpot(float x, float y) {
        arrowList.Add((GameObject)GameObject.Instantiate(Resources.Load("prefabs/pointer arrows")));
        arrowList[arrowList.Count-1].transform.position = new Vector3(x, y+.5f, 4);
    }
}
