using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class EventGUI : MonoBehaviour {

    string[] GUIStrings = new string[] {"", "", ""};
    float[] GUIStartTimes = new float[] {0f, 0f, 0f};

    public GameObject[] EventMessages;

    //this is the public method. it should be the only thing that gets called, ever.
    public void AddGUIString(string guistring)
    {
        //If all 3 are taken up, remove the last one and add a new one at 0
        if (GUIStrings[0] != "" && GUIStrings[1] != "" && GUIStrings[2] != "")
        {
            Debug.Log("had to move them around yo");
            GUIStrings[2] = GUIStrings[1];
            GUIStrings[1] = GUIStrings[0];
            GUIStartTimes[2] = GUIStartTimes[1];
            GUIStartTimes[1] = GUIStartTimes[0];
            SetGUIString(0, guistring);
        }
        else
        {
            for (int i = 0; i < GUIStrings.Length; i++)
            {
                if (GUIStrings[i] == "")
                {
                    SetGUIString(i, guistring);
                    return;
                }
            }
        }
    }

    //this is the method to set a new guistring at a particular place. the public method is terminal here so it also connects to 
    //the method setguieventmessages, which actually makes them appear.
    void SetGUIString(int place, string guistring)
    {
        Debug.Log("setguistring");
        GUIStrings[place] = guistring;
        GUIStartTimes[place] = Time.time;
        SetGUIEventMessages();
        Invoke("UnsetGUIStrings", 1.7f);
    }

    //iterates through eventmessages and if they are empty, sets them to inactive; otherwise, it makes the object active, plays the animation and sets the text.
    void SetGUIEventMessages()
    {
        for (int i = 0; i < EventMessages.Length; i++)
        {
            if (GUIStrings[i] == "")
            {
                EventMessages[i].SetActive(false);
            }
            else
            {
                EventMessages[i].SetActive(true);
                EventMessages[i].GetComponent<Animator>().Play("fade out");
                EventMessages[i].GetComponentInChildren<Text>().text = GUIStrings[i];
            }
        }
    }

    void UnsetGUIStrings()
    {
        for (int i = 0; i < GUIStrings.Length; i++)
        {
            if (GUIStartTimes[i] < (Time.time - 1.7f))
            {
                GUIStrings[i] = "";
                SetGUIEventMessages();
            }
        }
    }
}
