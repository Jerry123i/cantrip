using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RoomModel", menuName = "Generator/Room", order = 1)]
public class RoomModel : ScriptableObject
{
    public static int[] maxSize = { 19, 19 }, minSize = { 10, 10 };

    public GameObject[] monstersSpawns, obstacles;

    [SerializeField]
    private GameObject[] floorTypes;
    private GameObject[] floor;
    private GameObject exit;
    private GameObject[] path;

    private bool isFinal;
    private int[] size;
    private List<int> exitDirs, enterDirs;
    public Vector3 initialPoint;
    public List<Vector3> previousRoomPoints;
    public List<Vector3> nextRoomPoints;

    private int pointi, pointj;

    public RoomModel roomFrom;

    #region Getter and Setters
    public int Pointi {
        get {
            return pointi;
        }
        set {
            pointi = value; 
        }
    }

    public int Pointj {
        get {
            return pointj;
        }
        set {
            pointj = value; 
        }
    }

    public int[] Size {
        get {
            return size;
        }
        set {
            size = value;
        }
    }

    public List<int> ExitDirs {
        get {
            return exitDirs;
        }
        set {
            exitDirs = value;
        }
    }

    public List<int> EnterDirs {
        get {
            return enterDirs;
        }
        set {
            enterDirs = value;
        }
    }

    public bool IsFinal {
        get {
            return isFinal;
        }
        set {
            isFinal = value;
        }
    }
    #endregion

    // Magic number that gives the exact distance to put the exit.
    private float distancePath = 0.773f;

    private enum FLOORTYPES
    {
        BASE = 0,
        DOWN = 1,
		DOWNLEFT = 2,
		DOWNRIGHT = 3,
		LEFT = 4,
		PATHHORIZONTAL = 5,
		PATHHORIZONTALBLOCKED = 6,
        PATHVERTICAL = 8,
        PATHVERTICALBLOCKED = 9,
		RIGHT = 10,
        UP = 11,
		UPLEFT = 12,
		UPRIGHT = 13,
    };

	void Awake()
	{
		floorTypes = Resources.LoadAll<GameObject>("Prefabs/Floor");
        size = new int[2];
        exitDirs = new List<int>();
        enterDirs = new List<int>();
		previousRoomPoints = new List<Vector3>();
        nextRoomPoints = new List<Vector3>();
	}

	void OnDestroy()
	{

	}

	void OnEnable()
	{
	}

	void OnDisable()
	{

	}

	public void Init(Transform parent)
	{
        int point;
        bool notBorder;
		if (size[1] > maxSize[1] || size[1] > maxSize[1] || size[0] < minSize[0] || size[1] < minSize[1])
		{
			Debug.Log("destroy here because it's bigger than max size or smaller than minsize");
		}
		else
        {
            floor = new GameObject[size[0] * size[1]]; // START FLOOR

            //Debug.Log(size[0] + " - " + size[1] + " - " + size[0] * size[1]);
            //Debug.Log(previousRoomPoint + " - " + (int)previousRoomPoint[0] + " - " + (int)previousRoomPoint[1]);

            for (int i = 0; i < size[0]; i++)
            {
                for (int j = 0; j < size[1]; j++)
                {
                    //Debug.Log(parent.name + " " + i + " - " + j);
                    point = i * size[1] + j;
                    Vector3 pos = new Vector3(initialPoint[0] + i, initialPoint[1] + j, initialPoint.z);
                    if (i > 0 && j > 0 && j < size[1] - 1 && i < size[0] - 1)
                    {
                        // CREATE WALKABLE PATH
                        floor[point] = Instantiate(floorTypes[(int)FLOORTYPES.BASE], pos, Quaternion.identity);
                    }
                    else
                    {
                        //Debug.Log(i + " - " + (int)previousRoomPoint[0] + " - " + j + " - " + (int)previousRoomPoint[1]);
                        notBorder = false;
                        for (int k = 0; k < previousRoomPoints.Count; k++)
                        {
                            if (i == (int)previousRoomPoints[k][0] && j == (int)previousRoomPoints[k][1])
                            {
								Debug.Log(previousRoomPoints[k].ToString());
								notBorder = true;
                                floor[point] = Instantiate(floorTypes[(int)FLOORTYPES.BASE], pos, Quaternion.identity);
                            }
                        }
                        for (int k = 0; k < nextRoomPoints.Count; k++) {
							if (i == (int)nextRoomPoints[k][0] && j == (int)nextRoomPoints[k][1])
							{
								notBorder = true;
								if (i > 0 && i < size[0] - 1)
								{
									floor[point] = Instantiate(floorTypes[(int)FLOORTYPES.BASE], pos, Quaternion.identity);
									exit = Instantiate(floorTypes[(int)FLOORTYPES.PATHVERTICALBLOCKED], pos + new Vector3(0, (j == 0) ? -1.229f : 0.773f, 0), Quaternion.identity);
									exit.transform.parent = parent;
								}
								else if (j > 0 && j < size[1] - 1)
								{
									floor[point] = Instantiate(floorTypes[(int)FLOORTYPES.BASE], pos, Quaternion.identity);
									exit = Instantiate(floorTypes[(int)FLOORTYPES.PATHHORIZONTALBLOCKED], pos + new Vector3((i == 0) ? -1.0f : 1.0f, 0, 0), Quaternion.identity);
									exit.transform.parent = parent;
								}
								else
								{
									Debug.Log("error, invalid position to path");
								}
							}
                        }
                        // CREATING BORDERS
                        if (!notBorder)
                        {
                            if (i == 0)
                            {
                                if (j == 0)
                                {
                                    floor[point] = Instantiate(floorTypes[(int)FLOORTYPES.DOWNLEFT], pos, Quaternion.identity);
                                }
                                else if (j == size[1] - 1)
                                {
                                    floor[point] = Instantiate(floorTypes[(int)FLOORTYPES.UPLEFT], pos, Quaternion.identity);
                                }
                                else
                                {
                                    floor[point] = Instantiate(floorTypes[(int)FLOORTYPES.LEFT], pos, Quaternion.identity);
                                }
                            }
                            else if (i == size[0] - 1)
                            {
                                if (j == 0)
                                {
                                    floor[point] = Instantiate(floorTypes[(int)FLOORTYPES.DOWNRIGHT], pos, Quaternion.identity);
                                }
                                else if (j == size[1] - 1)
                                {
                                    floor[point] = Instantiate(floorTypes[(int)FLOORTYPES.UPRIGHT], pos, Quaternion.identity);
                                }
                                else
                                {
                                    floor[point] = Instantiate(floorTypes[(int)FLOORTYPES.RIGHT], pos, Quaternion.identity);
                                }
                            }
                            else if (j == 0)
                            {
                                floor[point] = Instantiate(floorTypes[(int)FLOORTYPES.DOWN], pos, Quaternion.identity);
                            }
                            else if (j == size[1] - 1)
                            {
                                floor[point] = Instantiate(floorTypes[(int)FLOORTYPES.UP], pos, Quaternion.identity);
                            }
                        }
                    }
					floor[point].transform.parent = parent;
                }
			}
		}
    }

    public void CreatePathTo(int count, int dir, Vector3 nextRoomPoint) {
        path = new GameObject[count];
        for (int i = 0; i < count; i++)
        {
            if (dir == 0 || dir == 2)
            {
                path[i] = Instantiate(floorTypes[(int)FLOORTYPES.PATHVERTICAL], initialPoint + new Vector3(nextRoomPoint.x, (dir == 0) ? 1.0f * (i + 1) + size[1] - 0.23f : -1.0f * (i + 2) - 0.23f, 0.0f), Quaternion.identity);
            }
            else {
                path[i] = Instantiate(floorTypes[(int)FLOORTYPES.PATHHORIZONTAL], initialPoint + new Vector3((dir == 1) ? 1.0f * (i + 1) + size[0]: -1.0f * (i + 2), nextRoomPoint.y, 0.0f), Quaternion.identity);
			}
            path[i].transform.parent = floor[0].transform.parent;
        }
    }
}
