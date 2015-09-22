using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObstacleLibrary : MonoBehaviour {

   // GameControl gameControl;
    public enum LevelTypes { Empty, Rocks, FewRocks, ScorpionRiver, BloodRiver, PusRiver, CrushBallcourt, Crossroad, CalabashTree, BatHouse };
    public static LevelTypes[] AllLevelTypes = new LevelTypes[] { LevelTypes.Empty, LevelTypes.Rocks, LevelTypes.FewRocks, LevelTypes.ScorpionRiver, 
                      LevelTypes.BloodRiver, LevelTypes.PusRiver, LevelTypes.CrushBallcourt, LevelTypes.Crossroad, LevelTypes.CalabashTree, 
                      LevelTypes.BatHouse };
    GameObject obstacleParent;

    //public method, called by gridcontrol
    public void LoadObstacles(LevelTypes levelType)
    {
        string tooltip;

        GridControl gridControl = gameObject.GetComponent<GridControl>();
        if (levelType == LevelTypes.Rocks | levelType == LevelTypes.FewRocks)
        {
            tooltip = "A sturdy rock. Won't break, even if you punch it.";

            int numberOfRocks = Random.Range(0, 30);
            if (levelType == LevelTypes.FewRocks && numberOfRocks > 9)
            {
                numberOfRocks = 9;
            }
			else if(numberOfRocks > 20) 
			{
				numberOfRocks = 20;
			}
            GridControl.PossibleSpawnPoints = gridControl.EmptyPathSpots();
            for (int i = 0; i < numberOfRocks; i++)
            {
                LoadObstacle("Rock", tooltip);
            }
        }
        else if (levelType == LevelTypes.ScorpionRiver) LoadObstacleSet("Scorpion river");
        else if (levelType == LevelTypes.BloodRiver) LoadObstacleSet("Blood river");
        else if (levelType == LevelTypes.PusRiver) LoadObstacleSet("Pus river"); 
    }

    #region Obstacle loading methods that aren't the public method
    //base method
    void LoadObstacle(string ObstacleName, int xPosition, int yPosition, string Tooltip)
    {
        if(obstacleParent == null) 
        {
            obstacleParent = GameObject.Find("Obstacles");
        }

        string ObstacleToLoad = "prefabs/obstacles/" + ObstacleName;

        GameObject tempGO = (GameObject)Instantiate((GameObject)Resources.Load(ObstacleToLoad));

        tempGO.GetComponent<GridUnit>().xPosition = xPosition;
        tempGO.GetComponent<GridUnit>().yPosition = yPosition;
        tempGO.transform.position = new Vector3(xPosition, yPosition, 0);
        tempGO.transform.parent = obstacleParent.transform;

        tempGO.GetComponent<Obstacle>().SetTooltip(Tooltip);
    }
    void LoadObstacle(string ObstacleName, string Tooltip)
    {
        int randomNumber = Random.Range(0, GridControl.PossibleSpawnPoints.Count - 1);
        Point xycoord = GridControl.PossibleSpawnPoints[randomNumber];
        GridControl.PossibleSpawnPoints.RemoveAt(randomNumber);

        LoadObstacle(ObstacleName, xycoord.x, xycoord.y, Tooltip);
    }
    void LoadObstacleSet(string ObstacleSetName)
    {
		// TODO
		// Should be a huge prefab instead of individual tiles.
    }
    #endregion
}
