using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelModel", menuName = "Generator/Level", order = 2)]
public class LevelModel : ScriptableObject {


    private const int mainRoomsSizeMin = 5, mainRoomsSizeMax = 10, levelSizeMax = 10;
    private Vector3 initialPoint;

    public int mainRoomsSize, pathRoomsSize, extraRoomsSize, fi, fj;
    public int[,] levelMatrix;
	public RoomModel[] mainRooms;

    private readonly System.Random rnd = new System.Random();

    private enum LEVEL {
        MAIN = 1,
        PATH = 2,
        EXTRA = 3,
    }

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
        Vector3 previous = new Vector3(-1.0f, -1.0f, -1.0f);
        GameObject room;
        int rand, dir = 0, lastDir = -1;
        bool retry = false;
        initialPoint = new Vector3(0.0f, 0.0f, 0.0f);

        for (int i = 0; i < mainRoomsSize; i++)
        {
            levelMatrix[fi, fj] = (int)LEVEL.MAIN;

            mainRooms[i] = ScriptableObject.CreateInstance("RoomModel") as RoomModel;
            room = Instantiate(new GameObject());
            room.name = "room " + i;
            room.transform.parent = parent;

            // Distributing possibilites
            retry = false;
            while (fi < 0 || fj < 0 || fi > levelSizeMax - 1 || fj > levelSizeMax - 1 || levelMatrix[fi, fj] != 0)
            {
                if (retry) {
	                if (dir == 0) fj--;
	                if (dir == 1) fi--;
	                if (dir == 2) fj++;
	                if (dir == 3) fi++;
                }
                rand = 0;
                dir = rnd.Next(0, 4);
                if (dir == 0) fj++;
                if (dir == 1) fi++;
                if (dir == 2) fj--;
                if (dir == 3) fi--;
                retry = true;
            } 

            mainRooms[i].size[0] = rnd.Next(RoomModel.minSize[0], RoomModel.maxSize[0]);
            mainRooms[i].size[1] = rnd.Next(RoomModel.minSize[1], RoomModel.maxSize[1]);

            if (i != mainRoomsSize - 1)
            {
                mainRooms[i].nextRoomPoint = new Vector3(
                    (dir == (int)DIR.LEFT) ? 0 : (dir == (int)DIR.RIGHT) ? mainRooms[i].size[0] - 1 : rnd.Next(1, RoomModel.minSize[0] - 1),
                    (dir == (int)DIR.DOWN) ? 0 : (dir == (int)DIR.UP) ? mainRooms[i].size[1] - 1 : rnd.Next(1, RoomModel.minSize[1] - 1),
                    0.0f
                );
            }
            else 
            {
                mainRooms[i].nextRoomPoint = new Vector3(-1.0f, -1.0f, -1.0f);
            }

            if (i != 0) {
	            initialPoint += new Vector3(
                    (lastDir == (int)DIR.LEFT) ? -1 - RoomModel.maxSize[0] : (lastDir == (int)DIR.RIGHT) ? 1 + RoomModel.maxSize[0] : 0,
                    (lastDir == (int)DIR.DOWN) ? -1 - RoomModel.maxSize[1] : (lastDir == (int)DIR.UP) ? 1 + RoomModel.maxSize[1] : 0,
	                0.0f
	            );
            } 
            else {
                initialPoint = new Vector3(0.0f, 0.0f, 0.0f);
            
            }

			mainRooms[i].initialPoint = initialPoint;

            if (lastDir == (int)DIR.LEFT || lastDir == (int)DIR.DOWN) {
                previous = new Vector3(
                    (lastDir == (int)DIR.LEFT) ? mainRooms[i].size[0] - 1 : previous.x,
                    (lastDir == (int)DIR.DOWN) ? mainRooms[i].size[1] - 1 : previous.y, 
                    0.0f
                );
			}
			mainRooms[i].previousRoomPoint = previous;
            previous = new Vector3(
                (dir == (int)DIR.RIGHT) ? 0 : mainRooms[i].nextRoomPoint.x,
                (dir == (int)DIR.UP) ? 0 : mainRooms[i].nextRoomPoint.y,
                0.0f
            );

			mainRooms[i].Init(room.transform);
            if (i != 0)
            {
				if (lastDir == (int)DIR.RIGHT || lastDir == (int) DIR.LEFT)
                {
                    mainRooms[i - 1].CreatePathBetween(room.transform, RoomModel.maxSize[0] - ((lastDir == (int)DIR.LEFT) ? mainRooms[i].size[0] : mainRooms[i - 1].size[0]), lastDir);
                }
                else
                {
                    mainRooms[i - 1].CreatePathBetween(room.transform, RoomModel.maxSize[1] - ((lastDir == (int)DIR.DOWN) ? mainRooms[i].size[1] : mainRooms[i - 1].size[1]), lastDir);
                }
            }
            lastDir = dir;
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
        levelMatrix = new int[levelSizeMax,levelSizeMax];
        fi = mainRoomsSize / 2;
        fj = mainRoomsSize / 2;
        CreateMainRooms(parent);
        CreatePathRooms(parent);
        CreateExtraRooms(parent);
    }
}
