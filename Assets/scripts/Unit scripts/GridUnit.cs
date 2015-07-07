using UnityEngine;
using System.Collections;

public class GridUnit : MonoBehaviour {

	//Grid control variables
	public int xPosition;
	public int yPosition;

	//Grid movement variables
	private float moveSpeed = 6f;
	private float gridSize = 1f;
	
	public bool isMoving = false;
    private Vector3 startPosition;
	private Vector3 endPosition;
	private float time;

    public bool isPoking = false;
    private Vector3 startPokePosition;
    private Vector3 endPokePosition;
    private float pokeTime;

	public void Start() {
		xPosition = Mathf.RoundToInt (gameObject.transform.position.x);
		yPosition = Mathf.RoundToInt (gameObject.transform.position.y);
	}

	public void GridMove(string direction) {
        if (direction == "none")
            return;
		if(!isMoving){
			switch(direction) {
			case "left":
				endPosition = new Vector3(gameObject.transform.position.x - 1, gameObject.transform.position.y, 0);
				xPosition--;
				break;
			case "right":
				endPosition = new Vector3(gameObject.transform.position.x + 1, gameObject.transform.position.y, 0);
				xPosition++;
				break;
			case "up":
				endPosition = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + 1, 0);
				yPosition++;
				break;
			case "down":
				endPosition = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y - 1, 0);
				yPosition--;
				break;
			default:
				Debug.LogError("your command wasn't left, right, down or up");
                return;
			}
			StartCoroutine(move(gameObject.transform));
		}
	}

    public IEnumerator move(Transform t)
    {
        isMoving = true;
        startPosition = t.position;
        time = 0;

        while (time < 1f)
        {
            time += Time.deltaTime * (moveSpeed / gridSize);
            transform.position = Vector3.Lerp(startPosition, endPosition, time);
            yield return null;
        }

        isMoving = false;
        yield return 0;
    }

    /// <summary>
    /// Gestures unit in a direction. 
    /// </summary>
    /// <param name="direction"></param>
    public void PokeTowards(string direction)
    {
        if(!isPoking){
			switch(direction) {
			case "left":
				endPokePosition = new Vector3(gameObject.transform.position.x - 1, gameObject.transform.position.y, 0);
				break;
			case "right":
                endPokePosition = new Vector3(gameObject.transform.position.x + 1, gameObject.transform.position.y, 0);
				break;
			case "up":
                endPokePosition = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + 1, 0);
				break;
			case "down":
                endPokePosition = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y - 1, 0);
				break;
			default:
				Debug.LogError("your command wasn't left, right, down or up");
				break;
			}
			StartCoroutine(poke(gameObject.transform));
		}
    }
    public IEnumerator poke(Transform t)
    {
        isPoking = true;
        startPokePosition = t.position;
        pokeTime = 0;

        while (pokeTime < 1f)
        {
            pokeTime += Time.deltaTime * (moveSpeed / gridSize);
            if (pokeTime > .5f)
            {
                transform.position = Vector3.Lerp(endPokePosition, startPokePosition, pokeTime);
            }
            else
            {
                transform.position = Vector3.Lerp(startPokePosition, endPokePosition, pokeTime);
            }
            yield return null;
        }

        isPoking = false;
        yield return 0;
    }

    ///
    /// small check utilities
    /// 

    public bool IsAdjacent(GridUnit unit)
    {
        return IsAdjacent(new int[] {unit.xPosition, unit.yPosition}, new int[] {xPosition, yPosition});
    }
    public bool IsAdjacent(int[] unit1Coords, int[] unit2Coords) 
    {
        return ((Mathf.Abs(unit1Coords[0] - unit2Coords[0]) + Mathf.Abs(unit1Coords[1] - unit2Coords[1])) == 1);
    }

    public bool IsDiagonal(GridUnit unit)
    {
        return (Mathf.Abs(xPosition - unit.xPosition) == Mathf.Abs(yPosition - unit.yPosition));
    }

  //  public virtual int DistanceCheck(GridUnit unit)
  //  {
  //      return ((Mathf.Abs(xPosition - unit.xPosition) + Mathf.Abs(yPosition - unit.yPosition)));
  //  }

  //  /// <summary>
    /// Returns the moves this unit would have to make to get to the other unit. 
    /// If this gridunit is at [1, 2] and the other is at [2, 5] it returns [1, 3].
    /// </summary>
    /// <param name="otherGU"></param>
    /// <returns></returns>
  //  public int[] XYDifference(GridUnit otherGU)
  //  {
  //      return new int[] { otherGU.xPosition - xPosition, otherGU.yPosition - yPosition};
  //  }

    /// <summary>
    /// Returns the position the other gridunit is in relation to this. 
    /// If this gridunit is at [0, 0] and the other is at [0, 1] it returns "up"
    /// </summary>
    /// <param name="otherGU"></param>
    /// <returns></returns>
    public string AdjacentPosition(GridUnit otherGU)
    {
        return AdjacentPosition(new int[] { xPosition, yPosition }, new int[] { otherGU.xPosition, otherGU.yPosition });
    }
    public string AdjacentPosition(int[] BaseCoords, int[] RelativeUnitCoords)
    {
        if((Mathf.Abs(RelativeUnitCoords[0] - BaseCoords[0]) + Mathf.Abs(RelativeUnitCoords[1] - BaseCoords[1])) != 1) {
            Debug.Log("BIG FUCKUP");
            return "this broke";
        }
        int diffx = RelativeUnitCoords[0] - BaseCoords[0];
        int diffy = RelativeUnitCoords[1] - BaseCoords[1];
        if (diffx == 0 && diffy == 1)
            return "up";
        if (diffx == 0 && diffy == -1)
            return "down";
        if (diffx == 1 && diffy == 0)
            return "right";
        if (diffx == -1 && diffy == 0)
            return "left";
        return "";
    }
}
