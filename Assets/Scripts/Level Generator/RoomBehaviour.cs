using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomBehaviour : MonoBehaviour {

    public RoomModel room;

	// Use this for initialization
    void Start () {
        room.Init(this.transform);
		/*
        if (room.size.y > room.maxSize.y || room.size.y > maxSize.y || room.size.x < minSize.x || room..y < room.minSize.y)
        {
            Debug.Log("destroy here because it's bigger than max size or smaller than minsize");
        }
        else
        {
            room.floor = new GameObject[(int) room.size.x * (int) room.size.y];
            for (int i = 1; i < room.size.x - 1; ++i)
            {
                for (int j = 1; j < room.size.y - 1; ++j)
                {
                    room.floor[i * (int) room.size.x + j] = Instantiate(room.floorTypes[0], new Vector3(room.initialPoint.x + i, room.initialPoint.y + j, room.initialPoint.z), Quaternion.identity);
                }
            }
        }
        */

	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
