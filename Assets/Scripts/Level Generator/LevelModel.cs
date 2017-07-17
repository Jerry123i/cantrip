using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelModel", menuName = "Generator/Level", order = 2)]
public class LevelModel : ScriptableObject {


    private const int mainRoomsSizeMin = 3, mainRoomsSizeMax = 7;
    private Vector3 initialPoint;

	public int mainRoomsSize, pathRoomsSize, extraRoomsSize;
	public RoomModel[] mainRooms;

    private System.Random rnd = new System.Random();

    private enum DIR
    {
        UP = 0,
        RIGHT = 1,
        DOWN = 2,
        LEFT = 3
    }

    private int[] dirProb = { 25, 25, 25, 25 };

    void Awake()
    {
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

    private void CreateMainRooms(Transform parent) {
        Vector3 previous = new Vector3(0.0f, 0.0f, 0.0f);
        GameObject room;
        int rand = 101, dir = 0, lastDir = 0;
		initialPoint = new Vector3(0.0f, 0.0f, 0.0f);
        for (int i = 0; i < mainRoomsSize; i++)
        {
            mainRooms[i] = ScriptableObject.CreateInstance("RoomModel") as RoomModel;
            room = Instantiate(new GameObject());
            room.name = "room " + i;
            room.transform.parent = parent;

            rand = 101;
            while (rand > dirProb[dir])
            {
                rand = rnd.Next(1, 100);
                dir = rnd.Next(0, 3);
                //Debug.Log(dir);
            }
            dirProb[dir] = 33;
            dirProb[(dir + 1) % 4] = 33;
            dirProb[(dir + 2) % 4] = 0;
            dirProb[(dir + 3) % 4] = 33;

            mainRooms[i].size[0] = rnd.Next(RoomModel.minSize[0], RoomModel.maxSize[0]);
            mainRooms[i].size[1] = rnd.Next(RoomModel.minSize[1], RoomModel.maxSize[1]);

			if (i == 0)
			{
				mainRooms[i].previousRoomPoint = new Vector3(-1.0f, -1.0f, -1.0f);
				mainRooms[i].nextRoomPoint = new Vector3(
					(dir == (int)DIR.LEFT) ? 0 : (dir == (int)DIR.RIGHT) ? mainRooms[i].size[0] - 1 : rnd.Next(1, mainRooms[i].size[0] - 2),
					(dir == (int)DIR.DOWN) ? 0 : (dir == (int)DIR.UP) ? mainRooms[i].size[1] - 1 : rnd.Next(1, mainRooms[i].size[1] - 2),
					0.0f);
			}
			else if (i == mainRoomsSize - 1)
			{
				mainRooms[i].previousRoomPoint = new Vector3(
					(lastDir == (int)DIR.LEFT) ? mainRooms[i].size[0] - 1 : (lastDir == (int)DIR.RIGHT) ? 0 : 1,
					(lastDir == (int)DIR.DOWN) ? mainRooms[i].size[1] - 1 : (lastDir == (int)DIR.UP) ? 0 : 1,
					0.0f
				);
				mainRooms[i].nextRoomPoint = new Vector3(-1.0f, -1.0f, -1.0f);
			}
			else
			{
				mainRooms[i].previousRoomPoint = new Vector3(
                    (lastDir == (int)DIR.LEFT) ? mainRooms[i].size[0] - 1 : (lastDir == (int)DIR.RIGHT) ? 0 : 1,
                    (lastDir == (int)DIR.DOWN) ? mainRooms[i].size[1] - 1 : (lastDir == (int)DIR.UP) ? 0 : 1,
                    0.0f
                );
				mainRooms[i].nextRoomPoint = new Vector3(
					(dir == (int)DIR.LEFT) ? 0 : (dir == (int)DIR.RIGHT) ? mainRooms[i].size[0] - 1 : rnd.Next(1, mainRooms[i].size[0] - 2),
					(dir == (int)DIR.DOWN) ? 0 : (dir == (int)DIR.UP) ? mainRooms[i].size[1] - 1 : rnd.Next(1, mainRooms[i].size[1] - 2),
					0.0f);
			}

			if (i != 0)
			{
				initialPoint += new Vector3(
                    (lastDir == (int)DIR.LEFT || lastDir == (int)DIR.RIGHT) ? 0 : previous.x - 1,
					(lastDir == (int)DIR.DOWN || lastDir == (int)DIR.UP) ? 0 : previous.y - 1,
					0.0f
				);
				Debug.Log(i + "- " + lastDir + " - " + initialPoint);
                initialPoint += new Vector3(
                    (lastDir == (int)DIR.LEFT) ? -1.0f - mainRooms[i].size[0] : 0,
                    (lastDir == (int)DIR.DOWN) ? -1.0f - mainRooms[i].size[1] : 0,
                    0.0f
                );
            }
            Debug.Log(i + "- " + lastDir + " - " + initialPoint);


			mainRooms[i].initialPoint = initialPoint;

			initialPoint += new Vector3(
                (dir == (int)DIR.RIGHT) ? 1.0f + mainRooms[i].size[0] : 0.0f,
                (dir == (int)DIR.UP) ? 1.0f + mainRooms[i].size[1] : 0.0f,
                0.0f
			);
			Debug.Log(i + "- " + dir + " - " + initialPoint);

			mainRooms[i].Init(room.transform);
            lastDir = dir;

            previous = new Vector3(
                (dir == (int)DIR.LEFT) ? mainRooms[i].size[0] - 1 : (dir == (int)DIR.RIGHT) ? 0 : mainRooms[i].nextRoomPoint.x,
                (dir == (int)DIR.DOWN) ? mainRooms[i].size[1] - 1 : (dir == (int)DIR.UP) ? 0 : mainRooms[i].nextRoomPoint.y,
                0.0f
            );
        }
    }

	private void CreatePathRooms(Transform parent)
	{

	}

	private void CreateExtraRooms(Transform parent)
	{

	}

	public void GenerateLevel(Transform parent)
	{
		mainRoomsSize = rnd.Next(mainRoomsSizeMin, mainRoomsSizeMax);
		Debug.Log(mainRoomsSizeMin + " - " + mainRoomsSizeMax + " - " + mainRoomsSize);
		mainRooms = new RoomModel[mainRoomsSize];
        CreateMainRooms(parent);
        CreatePathRooms(parent);
        CreateExtraRooms(parent);
    }
}
