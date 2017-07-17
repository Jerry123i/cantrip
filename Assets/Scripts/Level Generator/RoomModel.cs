using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RoomModel", menuName = "Generator/Room", order = 1)]
public class RoomModel : ScriptableObject
{
    private static Vector2 maxSize = new Vector2(100.0f, 100.0f), minSize = new Vector2(3.0f, 3.0f);

    public GameObject[] monstersSpawns, obstacles;

    [SerializeField]
    private GameObject[] floorTypes;
    private GameObject[] floor;
    private GameObject exit;
    public Vector2 size;

    public Vector3 initialPoint;
    public Vector3 nextRoomPoint;

    // Magic number that gives the exact distance to put the exit.
    private float distancePath = 0.773f;

    private enum FLOORTYPES
    {
        BASE = 0,
        DOWN = 1,
		DOWNLEFT = 2,
		DOWNRIGHT = 3,
		LEFT = 4,
        PATHHORIZONTALBLOCKED = 7,
        PATHVERTICALBLOCKED = 9,
		RIGHT = 10,
        UP = 11,
		UPLEFT = 12,
		UPRIGHT = 13,
    };

    void Awake()
    {
        floorTypes = Resources.LoadAll<GameObject>("Prefabs/Floor");
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
		if (size.y > maxSize.y || size.y > maxSize.y || size.x < minSize.x || size.y < minSize.y)
		{
			Debug.Log("destroy here because it's bigger than max size or smaller than minsize");
		}
		else
        {
            floor = new GameObject[(int)size.x * (int)size.y]; // START FLOOR

            for (int i = 0; i < size.x; ++i)
            {
                for (int j = 0; j < size.y; ++j)
                {
                    int point = i * (int)size.x + j;
                    Vector3 pos = new Vector3(initialPoint.x + i, initialPoint.y + j, initialPoint.z);
                    if (i > 0 && j > 0 && j < size.y - 1 && i < size.x - 1)
                    {
                        // CREATE WALKABLE PATH
                        floor[point] = Instantiate(floorTypes[(int)FLOORTYPES.BASE], pos, Quaternion.identity);
                    }
                    else
                    {
                        if (i == (int)nextRoomPoint.x && j == (int)nextRoomPoint.y)
                        {
                            if (i > 0 && i < (int)size.x - 1)
                            {
                                floor[point] = Instantiate(floorTypes[(int)FLOORTYPES.BASE], pos, Quaternion.identity);
                                exit = Instantiate(floorTypes[(int)FLOORTYPES.PATHVERTICALBLOCKED], pos + new Vector3(0, 0.773f, 0), Quaternion.identity);
								exit.transform.parent = parent;
							}
                            else if (j > 0 && j < (int)size.y - 1) {
								floor[point] = Instantiate(floorTypes[(int)FLOORTYPES.BASE], pos, Quaternion.identity);
								exit = Instantiate(floorTypes[(int)FLOORTYPES.PATHHORIZONTALBLOCKED], pos + new Vector3(0.773f, 0, 0), Quaternion.identity);
                                exit.transform.parent = parent;
                            }
                            else {
                                Debug.Log("error, invalid position to path");
                            }
                        }
                        else
                        {
                            // CREATING BORDERS
                            if (i == 0)
                            {
                                if (j == 0)
                                {
                                    floor[point] = Instantiate(floorTypes[(int)FLOORTYPES.DOWNLEFT], pos, Quaternion.identity);
                                }
                                else if (j == (int)size.y - 1)
                                {
                                    floor[point] = Instantiate(floorTypes[(int)FLOORTYPES.UPLEFT], pos, Quaternion.identity);
                                }
                                else
                                {
                                    floor[point] = Instantiate(floorTypes[(int)FLOORTYPES.LEFT], pos, Quaternion.identity);
                                }
                            }
                            else if (i == (int)size.x - 1)
                            {
                                if (j == 0)
                                {
                                    floor[point] = Instantiate(floorTypes[(int)FLOORTYPES.DOWNRIGHT], pos, Quaternion.identity);
                                }
                                else if (j == (int)size.y - 1)
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
                            else if (j == (int)size.y - 1)
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
}
