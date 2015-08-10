using UnityEngine;
using System.Collections;

public class Obstacle : MonoBehaviour {

    public string tooltip;
    public GameControl gameControl;
    public bool Walkable;

    public virtual void Start()
    {
        gameControl = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameControl>();
    }

    public void ShowTooltip()
    {
        gameControl.Tooltip = tooltip;
    }

    public virtual void StepIn() { }
}
