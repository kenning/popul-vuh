﻿using UnityEngine;
using System.Collections;

public class Obstacle : MonoBehaviour {

    protected string tooltip;
    public bool Walkable;

	public void SetTooltip(string tool) {
		tooltip = tool;
	}

    public void ShowTooltip()
    {
		S.GameControlGUIInst.SetTooltip(tooltip);
    }

    public virtual void StepIn() { }
}
