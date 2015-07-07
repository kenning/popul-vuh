using UnityEngine;
using System.Collections;

public class Obstacle : MonoBehaviour {

    public string tooltip;
    public GameControl battleBoss;
    public bool Walkable;

    public virtual void Start()
    {
        battleBoss = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameControl>();
    }

    public void ShowTooltip()
    {
        battleBoss.Tooltip = tooltip;
    }

    public virtual void StepIn() { }
}
