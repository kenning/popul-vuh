using UnityEngine;
using System.Collections;

public class River : Obstacle {

    public enum RiverType { Blood, Pus, Scorpion };
    public RiverType ThisRiverType;
    public Player player;

    public void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    public override void StepIn()
    {
        switch (ThisRiverType)
        {
            case RiverType.Blood:
                S.GameControlInst.SetSick(GameControl.SickTypes.Hunger, 1);
                Debug.Log("hi");
                break;
            case RiverType.Pus:
                S.GameControlInst.SetSick(GameControl.SickTypes.Swollen, 1);
                break;
            case RiverType.Scorpion:
                player.TakeDamage(1, 0);
                break;
        }
    }
}
