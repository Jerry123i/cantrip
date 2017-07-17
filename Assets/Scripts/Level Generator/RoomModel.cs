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
    [SerializeField]
    private GameObject[] floor;
    public Vector2 size;

    public Vector3 initialPoint;

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

            // CREATE WALKABLE PATH
            for (int i = 0; i < size.x; ++i)
            {
                for (int j = 0; j < size.y; ++j)
                {
                    int point = i * (int)size.x + j;
                    Vector3 pos = new Vector3(initialPoint.x + i, initialPoint.y + j, initialPoint.z);
                    if (i > 0 && j > 0 && j < size.y - 1 && i < size.x - 1)
                    {
                        floor[point] = Instantiate(floorTypes[0], pos, Quaternion.identity);
                    }
                    else
                    {
                        if (i == 0)
                        {
                            if (j == 0)
                            {
                                floor[point] = Instantiate(floorTypes[2], pos, Quaternion.identity);
                            }
                            else if (j == (int)size.y - 1)
                            {
                                floor[point] = Instantiate(floorTypes[12], pos, Quaternion.identity);
                            }
                            else {
                                floor[point] = Instantiate(floorTypes[4], pos, Quaternion.identity);
                            }
                        }
                        else if (i == (int)size.x - 1) {
							if (j == 0)
							{
								floor[point] = Instantiate(floorTypes[3], pos, Quaternion.identity);
							}
							else if (j == (int)size.y - 1)
							{
								floor[point] = Instantiate(floorTypes[13], pos, Quaternion.identity);
							}
							else
							{
								floor[point] = Instantiate(floorTypes[10], pos, Quaternion.identity);
							}
                        }
                        else if (j == 0) {
                            floor[point] = Instantiate(floorTypes[1], pos, Quaternion.identity);
                        }
                        else if (j == (int)size.y - 1) {
							floor[point] = Instantiate(floorTypes[11], pos, Quaternion.identity);
						}
					}
					floor[point].transform.parent = parent;
                }
			}

		}
    }
}
