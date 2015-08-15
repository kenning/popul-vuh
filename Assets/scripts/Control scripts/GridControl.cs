using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GridControl : MonoBehaviour {

    public List<GameObject> targetSquares = new List<GameObject>();
    public List<GridUnit> gridUnits = new List<GridUnit>();
    public static List<Point> PossibleSpawnPoints;
    int counter = 0;

    public static int GridSize = 4;

    List<GameObject> effectSquares;

    EnemyLibrary enemyL;
    ObstacleLibrary obstacleL;

    GridUnit player;
    //GameControl gameControl;

    public enum TargetTypes {none, diamond, cross, square, diagonal}; 

    //pathing stuff. emptypoints is different from possiblespawnpoints because it's only used in pathing. idk why though
    List<int[]> endList = new List<int[]>();
    List<Point> emptyPoints = new List<Point>();
    Queue<Point> pointsToVisit = new Queue<Point>();

    void Start(){
        FindAllGridUnits ();
        enemyL = gameObject.GetComponent<EnemyLibrary>();
        obstacleL = gameObject.GetComponent<ObstacleLibrary>();

        //gameControl = GameObject.FindGameObjectWithTag ("GameController").GetComponent<GameControl> ();
    }

    //public method, very convenient. called by gamecontrol
    public void LoadEnemiesAndObstacles(int level)
    {
        PossibleSpawnPoints = FindEmptySpots(false);

        #region Load Obstacles
        ObstacleLibrary.LevelTypes obstacleLevelType = ObstacleLibrary.LevelTypes.Empty;

        int rand = Random.Range(0, 10);
        obstacleLevelType = ObstacleLibrary.AllLevelTypes[rand];

        obstacleL.LoadObstacles(obstacleLevelType);
        #endregion

        PossibleSpawnPoints = FindEmptySpots(false);

        #region Load Enemies

		enemyL.LoadEnemiesForLevel(level-1);

        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject badguy in enemies)
        {
            Enemy en = badguy.GetComponent<Enemy>();
            if (en != null)
                en.GoToMax();
        }

        EnemiesFindGridUnits();
        #endregion
    }

    #region Findallgridunits and findemptyspots
    public void FindAllGridUnits() {
        //this method gets called often on enemy turns and could be bad for performance.
        gridUnits = new List<GridUnit> ();
        player = GameObject.FindGameObjectWithTag ("Player").GetComponent<GridUnit> ();
        gridUnits.Add (player);
        GameObject[] enemies = GameObject.FindGameObjectsWithTag ("Enemy");
        foreach(GameObject enemy in enemies) {
            GridUnit tempGU = (enemy.GetComponent<GridUnit>());
            if(tempGU != null) gridUnits.Add(tempGU);
        }
        GameObject[] obstacles = GameObject.FindGameObjectsWithTag("obstacle");
        foreach (GameObject obs in obstacles)
        {
            GridUnit tempGU = obs.GetComponent<GridUnit>();
            if(tempGU != null) gridUnits.Add(tempGU);
        }
    }

    //sets possibleSpawnPoints at the start of LoadEnemiesAndObstacles(). also finds empty spots when doing pathing
    public List<Point> FindEmptySpots(bool IgnoreEnemies)
    {
        List<Point> possiblePoints = new List<Point>();
        //possible enemy spawn points!
        for (int i = -GridSize; i <= GridSize; i++)
        {
            for (int j = -GridSize; j <= GridSize; j++)
            {
                possiblePoints.Add(new Point(i, j));
            }
        }

        FindAllGridUnits();

        foreach (GridUnit gu in gridUnits)
        {
            for (int i = 0; i < possiblePoints.Count; i++)
            {
                if (gu.xPosition == possiblePoints[i].x && gu.yPosition == possiblePoints[i].y)
                {
                    if (gu.gameObject.tag == "Player")
                    {
                        possiblePoints.RemoveAt(i);
                    }
                    else if (gu.gameObject.tag == "obstacle")
                    {
                        if(!(gu.gameObject.transform.parent.gameObject.name == "blood river") && 
                            !(gu.gameObject.transform.parent.gameObject.name == "pus river") &&
                            !(gu.gameObject.transform.parent.gameObject.name == "scorpion-river-bot")) 
                        {
                            if (!gu.gameObject.GetComponent<Obstacle>().Walkable)
                            {
                                possiblePoints.RemoveAt(i);
                            }
                        }
                        break;
                    }
                    else if (!IgnoreEnemies && gu.gameObject.tag == "Enemy")  
                    {
                        Debug.Log("removing the spot at " + gu.xPosition.ToString() + ", " + gu.yPosition.ToString() +
                            " because of " + gu.gameObject.name);
                        possiblePoints.RemoveAt(i);
                    }
                    break;
                }
            }
        }

        return possiblePoints;
    }
    #endregion
    
    #region Pathing stuff
    ///public method, also very convenient. called by enemy to find out where it's going next. if the player is to the left it will return "left"
    public string ReturnNextMove(Enemy en, bool IgnoreEnemies)
                    //IgnoreEnemies ISNT DOING ANYTHING YET
    {
        counter = 0;
        //this returns the next move for an enemy. if the player is to their left it will return "left"
        List<int[]> path = new List<int[]> ();
        path = FastestPath(en, IgnoreEnemies);
        if (path == null) 
            return "none";
        for (int i = 0; i < path.Count; i++)
        {
            if (en.thisGU.IsAdjacent(new int[] { en.thisGU.xPosition, en.thisGU.yPosition }, path[i]))
            {
                return en.thisGU.AdjacentPosition(new int[] { en.thisGU.xPosition, en.thisGU.yPosition }, path[i]);
            }
        }
        Debug.Log("big error!");
        return "error";
    }

    public List<int[]> FastestPath(Enemy en, bool IgnoreEnemies)
    {
        //this returns a set of x and y coordinates that make up the shortest path for an enemy. might be pretty hard to find diagonal movement...

        endList = en.PossibleAttackSquares();
        if (endList.Count == 0 | endList == null)
        {
            endList = new List<int[]>();
            endList.Add(new int[] { player.xPosition, player.yPosition + 1 });
            endList.Add(new int[] { player.xPosition, player.yPosition - 1 });
            endList.Add(new int[] { player.xPosition + 1, player.yPosition });
            endList.Add(new int[] { player.xPosition - 1, player.yPosition });
        }
        emptyPoints = FindEmptySpots(IgnoreEnemies);
        pointsToVisit = new Queue<Point>();

        Point enemyPos = new Point(en.thisGU.xPosition, en.thisGU.yPosition);
        enemyPos.Path.Add(new int[] {enemyPos.x, enemyPos.y});

        counter = 0;

        return Visit(enemyPos);
    }

    List<int[]> Visit(Point start)
    {
        counter++;
        if (counter % 400 == 0) Debug.Log("counter is " + counter.ToString());
        if (counter > 4000000) return null;

        bool GotThere = false;
        
        for (int i = 0; i < emptyPoints.Count; i++)
        {
            Point adjPoint = emptyPoints[i];
//see if it's adjacent
            if (((Mathf.Abs(start.x - adjPoint.x) + Mathf.Abs(start.y - adjPoint.y)) == 1))
            {
                List<int[]> newPath = new List<int[]>();
                for(int l = 0; l < start.Path.Count; l++) 
                {
                    newPath.Add(new int[] { start.Path[l][0], start.Path[l][1] });
                }
                newPath.Add(new int[] { adjPoint.x, adjPoint.y });

                if (adjPoint.Visited) 
                {
                    continue;
                }
                else {
                    GotThere = true;
    //see if it's one of the end points, and if it is, return it
                    for (int j = 0; j < endList.Count; j++)
                    {
                        if (endList[j][0] == adjPoint.x && endList[j][1] == adjPoint.y)
                        {
                            return newPath;
                        }
                    }
    //if it's adjacent, not an end point, and it hasn't been visited yet, add it to the list of neighbors
                    adjPoint.Visit(newPath);
                    pointsToVisit.Enqueue(adjPoint);
                }
            }
        }

        if (GotThere)
        {
            Point visitPoint = pointsToVisit.Dequeue();

            return Visit(visitPoint);
        }
        else return null;
    }
    #endregion

    #region Target squares stuff
    public void EnterTargetingMode(GridControl.TargetTypes targetType, int rangeStart, int rangeEnd) {

        MakeSquares (targetType, rangeStart, rangeEnd, true);
    }

    public void MakeSquares(TargetTypes thisTargetType, int rangeStart, int rangeEnd, int xCenter, int yCenter, bool MakeTargetSquares) {

        for(int i = rangeStart; i <= rangeEnd; i++) {

            switch(thisTargetType) {
            case TargetTypes.diamond:
                for(int j = -i; j <= i; j++) {
                    for(int k = -i; k <= i; k++) {
                        if((Mathf.Abs(j) + Mathf.Abs(k)) == i) {
                            MoveTheSquareHere(j + xCenter, k + yCenter, MakeTargetSquares);
                        }
                    }
                }
                break;
            case TargetTypes.cross:
                for(int j = -i; j <= i; j++) {
                    for(int k = -i; k <= i; k++) {
                        if((Mathf.Abs (j) + Mathf.Abs (k)) == i && (k==0 | j==0)) {
                            MoveTheSquareHere(j + xCenter, k + yCenter, MakeTargetSquares);
                        }
                    }
                }
                break;
            case TargetTypes.diagonal:
                for(int j = -i; j <= i; j++) {
                    for(int k = -i; k <= i; k++) {
                        if((Mathf.Abs (j)==i && Mathf.Abs (k)==i)) {
                            MoveTheSquareHere(j + xCenter, k + yCenter, MakeTargetSquares);
                        }
                    }
                }
                break;
            case TargetTypes.square:
                for(int j = -i; j <= i; j++) {
                    for(int k = -i; k <= i; k++) {
                        if((Mathf.Abs (j)==i | Mathf.Abs (k)==i)) {
                            MoveTheSquareHere(j + xCenter, k + yCenter, MakeTargetSquares);
                        }
                    }
                }
                break;
            }
        }
    }

    public void MakeSquares(TargetTypes thisTargetType, int rangeStart, int rangeEnd, bool MakeTargetSquares) {
        MakeSquares (thisTargetType, rangeStart, rangeEnd, player.xPosition, player.yPosition, MakeTargetSquares);
    }

    public void MoveTheSquareHere(int tempX, int tempY, bool MakeTargetSquares) {

        if (tempX > GridSize | tempX < -GridSize | tempY > GridSize | tempY < -GridSize)
            return;

        GameObject workingPrefab;
        if(MakeTargetSquares) workingPrefab = (GameObject)Instantiate(Resources.Load("prefabs/Target square prefab"));
        else workingPrefab = (GameObject)Instantiate(Resources.Load("prefabs/Effect square prefab"));

        if(MakeTargetSquares) {
            TargetSquare workingSquare = workingPrefab.GetComponent<TargetSquare>();
            workingSquare.MoveToPoint(tempX, tempY);
        }
        else {
            EffectSquare workingEffectSquare = workingPrefab.GetComponent<EffectSquare>();
            workingEffectSquare.MoveToPointAndFadeOut(tempX, tempY);
        }
    }

    public void DestroyAllTargetSquares(){
        GameObject[] allTargetSquares = GameObject.FindGameObjectsWithTag ("Target square");
        foreach(GameObject targetSquare in allTargetSquares){
            Destroy (targetSquare);
        }
    }
    public void TutorialMovementIllustration()
    {
        MakeSquares(GridControl.TargetTypes.diamond, 1, 1, false);
    }
    public void TutorialPunchIllustration()
    {
        MakeSquares(GridControl.TargetTypes.square, 0, 0, player.xPosition, player.yPosition - 1, false);
    }
    #endregion

    #region Obstacle formation methods
    /// <summary>
    /// Returns path spots, maybe flipped x/y negative, maybe switch x and y
    /// </summary>
    /// <returns></returns>
    public List<Point> EmptyPathSpots()
    {
        List<Point> list = new List<Point>()
        {
        #region EmptyPathSpots list
            new Point(-4, 4),
            new Point(-4, 3),
            new Point(-4, 2),
            new Point(-4, 1),
            new Point(-4, 0),
            new Point(-4, -1),
            new Point(-4, -2),
            new Point(-4, -4),

            new Point(-3, 0),
            new Point(-3, -4),

            new Point(-2, 4),
            new Point(-2, 3),
            new Point(-2, 2),
            new Point(-2, -2),

            new Point(-1, 4),
            new Point(-1, 0),
            new Point(-1, -1),
            new Point(-1, -3),

            new Point(0, 2),

            new Point(1, 4),
            new Point(1, 2),
            new Point(1, 1),
            new Point(1, 0),
            new Point(1, -1),
            new Point(1, -3),

            new Point(2, 4),
            new Point(2, 1),
            new Point(2, -3),
            new Point(2, -4),

            new Point(3, 4),
            new Point(3, 3),
            new Point(3, -1),

            new Point(4, 4),
            new Point(4, 1),
            new Point(4, 0),
            new Point(4, -1),
            new Point(4, -3),
            new Point(4, -4)
    #endregion
        };
        int perm = Random.Range(1, 8);
        bool xNeg = false;
        bool yNeg = false;
        bool xYSwitch = false;
        if (perm < 5)
        {
            xNeg = true;
        }
        if (perm%4 < 3)
        {
            yNeg = true;
        }
        if (perm % 2 == 0)
        {
            xYSwitch = true;
        }
        for (int i = 0; i < list.Count; i++)
        {
            if (xNeg) list[i].Flip(0);
            if (yNeg) list[i].Flip(1);
            if (xYSwitch) list[i].Flip(2);
        }
        return list;
    }

    #endregion

    void EnemiesFindGridUnits()
    {
        GameObject[] EnemyObjArray = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject eGU in EnemyObjArray)
        {
            Enemy e = eGU.GetComponent<Enemy>();
            if (e != null)
                e.FindOtherGridUnits();
        }
    }
    

}

public struct Point
{
    public int x;
    public int y;
    public List<int[]> Path;
    public bool Visited;

    public Point(int newX, int newY)
    {
        x = newX;
        y = newY;
        Visited = false;
        Path = new List<int[]>();
    }

    public void SetPath(List<int[]> newPath)
    {
        Path = newPath;
    }
    public void Visit(List<int[]> newPath)
    {
        Visited = true;
        Path = newPath;
    }
    public void Flip(int flipType)
    {
        if (flipType == 0)
        {
            x = -x;
        }
        if (flipType == 1)
        {
            y = -y;
        }
        if (flipType == 2)
        {
            int newx = y;
            y = x;
            x = newx;
        }
    }
}