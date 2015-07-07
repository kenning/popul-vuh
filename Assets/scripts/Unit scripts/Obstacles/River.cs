using UnityEngine;
using System.Collections;

public class River : Obstacle {

    public enum RiverType { Blood, Pus, Scorpion };
    public RiverType ThisRiverType;
    public Player player;

    override public void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

        base.Start();
    }

    public override void StepIn()
    {
        switch (ThisRiverType)
        {
            case RiverType.Blood:
                battleBoss.SetSick(GameControl.SickTypes.Hunger);
                Debug.Log("hi");
                break;
            case RiverType.Pus:
                battleBoss.SetSick(GameControl.SickTypes.Swollen);
                break;
            case RiverType.Scorpion:
                player.TakeDamage(1, 0);
                break;
        }
    }
}
