using UnityEngine;
using System.Collections;

public class ButtonAnimate : MonoBehaviour {

	public Sprite NORMALSPRITE;
	public Sprite EMPTYSPRITE;
//	public Sprite ERRORSPRITE;
    public Sprite SICKSPRITE;
    public Sprite SICKEMPTYSPRITE;
    public Sprite PAWSPRITE;
    public Sprite PAWEMPTYSPRITE;
    public GameControl.SickTypes ThisSickType;
    public bool Transformed = false;

	SpriteRenderer render;

	void Awake() {
		render = GetComponent<SpriteRenderer> ();
	}

	public void ErrorAnimation () {
		Invoke ("ErrorAnimateOn", .05f);
		Invoke ("ErrorAnimateOff", .1f);
		Invoke ("ErrorAnimateOn", .15f);
		Invoke ("ErrorAnimateOff", .2f);
		Invoke ("ErrorAnimateOn", .25f);
		Invoke ("ErrorAnimateOff", .3f);
		Invoke ("ErrorAnimateOn", .35f);
	}

    bool sickCheck()
    {
        if (Transformed) return false;
        
        if (ThisSickType == GameControl.SickTypes.Bleeding && S.GameControlInst.BleedingTurns > 0) return true;
        if (ThisSickType == GameControl.SickTypes.Swollen && S.GameControlInst.SwollenTurns > 0) return true;
        if (ThisSickType == GameControl.SickTypes.Hunger && S.GameControlInst.SwollenTurns > 0) return true;

        return false;
    }

	void ErrorAnimateOn() {
        SetSprite(true, sickCheck());
    }

	void ErrorAnimateOff() {
        SetSprite(false, sickCheck());
	}

	public void SetSprite () {
        bool Empty = S.GameControlInst.PlaysLeft < 1;
        if (gameObject.name == "move end button") Empty = S.GameControlInst.MovesLeft < 1;
        SetSprite(Empty);
	}
    public void SetSprite(bool Empty) { SetSprite(Empty, sickCheck()); }
    void SetSprite(bool Empty, bool Sick)
    {
        if (render == null)
        {
            render = GetComponent<SpriteRenderer>();
        }

        if (Empty)
        {
            if (Transformed)
                render.sprite = PAWEMPTYSPRITE;
            else if (Sick)
                render.sprite = SICKEMPTYSPRITE;
            else
                render.sprite = EMPTYSPRITE;
        }
        else
        {
            if (Transformed)
                render.sprite = PAWSPRITE;
            else if (Sick) 
                render.sprite = SICKSPRITE;
            else
                render.sprite = NORMALSPRITE;
        }
    }

    public void UITransform(bool TurnToJaguar)
    {
//        bool PlayButton = (gameObject.name == "play end button");
//
//        if (PlayButton)
//        {
//            if (TurnToJaguar)
//            {
//                if (Transformed)
//                    return;
//
//                Transformed = true;
//
//                SetSprite();
//
//                TextMesh text = gameObject.GetComponentInChildren<TextMesh>();
//                text.gameObject.transform.localPosition = new Vector3(-.03f, .18f, 0f);
//
//                gameObject.transform.localPosition = new Vector3(3.64f, -.61f, 0f);
//
//            }
//            else
//            {
//                Transformed = false;
//
//                SetSprite();
//
//                TextMesh text = gameObject.GetComponentInChildren<TextMesh>();
//                text.gameObject.transform.localPosition = new Vector3(-.13f, .12f, 0);
//
//                gameObject.transform.localPosition = new Vector3(3.64f, -.32f, 0f);
//            }
//        }
//        else //movebutton
//        {
//            if (TurnToJaguar)
//            {
//                gameObject.GetComponent<SpriteRenderer>().enabled = false;
//                gameObject.GetComponent<BoxCollider2D>().enabled = false;
//                gameObject.GetComponentInChildren<TextMesh>().color = new Color(0, 0, 0, 0);
//            }
//            else
//            {
//                gameObject.GetComponent<SpriteRenderer>().enabled = true;
//                gameObject.GetComponent<BoxCollider2D>().enabled = true;
//                gameObject.GetComponentInChildren<TextMesh>().color = new Color(1, 1, 1, 1);
//            }
//        }
    }
}
