using UnityEngine;
using System.Collections;

public class Obstacle : MonoBehaviour {

    protected string tooltip;
    protected GameControl gameControl;
	protected GameControlGUI gameControlGUI;
    public bool Walkable;

    public virtual void Start()
    {
		GameObject tempGO = GameObject.FindGameObjectWithTag ("GameController");
        gameControl = tempGO.GetComponent<GameControl>();
		gameControlGUI = tempGO.GetComponent<GameControlGUI> ();
    }

	public void SetTooltip(string tool) {
		tooltip = tool;
	}

    public void ShowTooltip()
    {
		gameControlGUI.SetTooltip(tooltip);
    }

    public virtual void StepIn() { }
}
