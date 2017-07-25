using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelModel", menuName = "Generator/Level", order = 2)]
public class LevelModel : ScriptableObject {


    private const int mainRoomsSizeMin = 9, mainRoomsSizeMax = 15, levelSizeMax = 15, extraRoomsSizeMin = 3, extraRoomsSizeMax = 8;
    private Vector3 initialPoint;

    public int mainRoomsSize, pathRoomsSize, extraRoomsSize, fi, fj;
    public int[,] levelMatrix;
	public RoomModel[] mainRooms;
    public RoomModel[] pathRooms;
    public RoomModel[] extraRooms;

    private readonly System.Random rnd = new System.Random();

    private enum LEVEL {
        MAIN = 1,
        PATH = 2,
        EXTRA = 3,
        FINAL = 4
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

    private void PrintMatrix(int[,] matrix, int n, int m) {
        string line;
        for (int i = 0; i < n; i++) 
        {
            line = "";
            for (int j = 0; j < m; j++) {
                line += String.Format("{0} ", matrix[i,j]);
            }    
            Debug.Log(line);
        }
    }

    private int FindEmptyDir(int dir) {
		bool retry = false;
        int newDir = dir;
		while (fi < 0 || fj < 0 || fi > levelSizeMax - 1 || fj > levelSizeMax - 1 || levelMatrix[fi, fj] != 0)
		{
			if (retry)
			{
				if (newDir == 0) fj--;
				if (newDir == 1) fi--;
				if (newDir == 2) fj++;
				if (newDir == 3) fi++;
			}
			newDir = rnd.Next(0, 4);
			if (newDir == 0) fj++;
			if (newDir == 1) fi++;
			if (newDir == 2) fj--;
			if (newDir == 3) fi--;
			retry = true;
		}
        return newDir;
    }

    private RoomModel FindRoomFromIndexes(int indexI, int indexJ) {
        for (int i = 0; i < mainRoomsSize; i++)
        {
            if (mainRooms[i].Pointi == indexI && mainRooms[i].Pointj == indexJ)
                return mainRooms[i];
        }
        return null;
    }

    private void CreateMainRooms()
    {
        Vector3 previous = new Vector3(-1.0f, -1.0f, -1.0f);
        //GameObject room;
        int dir = 0, lastDir = -1;
        initialPoint = new Vector3(0.0f, 0.0f, 0.0f);

        for (int i = 0; i < mainRoomsSize; i++)
        {
            mainRooms[i] = ScriptableObject.CreateInstance("RoomModel") as RoomModel;

            if (i != mainRoomsSize - 1)
                levelMatrix[fi, fj] = (int)LEVEL.MAIN;
            else
                levelMatrix[fi, fj] = (int)LEVEL.FINAL;

            mainRooms[i].Pointi = fi;
            mainRooms[i].Pointj = fj;

            mainRooms[i].Size[0] = rnd.Next(RoomModel.minSize[0], RoomModel.maxSize[0]);
            mainRooms[i].Size[1] = rnd.Next(RoomModel.minSize[1], RoomModel.maxSize[1]);

            dir = FindEmptyDir(dir);

            mainRooms[i].ExitDirs.Add(dir);
            if (i != 0) mainRooms[i].EnterDirs.Add(lastDir);

            if (i != mainRoomsSize - 1)
            {
                mainRooms[i].nextRoomPoints.Add(new Vector3(
                    (dir == (int)DIR.LEFT) ? 0 : (dir == (int)DIR.RIGHT) ? mainRooms[i].Size[0] - 1 : rnd.Next(1, RoomModel.minSize[0] - 1),
                    (dir == (int)DIR.DOWN) ? 0 : (dir == (int)DIR.UP) ? mainRooms[i].Size[1] - 1 : rnd.Next(1, RoomModel.minSize[1] - 1),
                    0.0f
                ));
            }
            else
            {
                mainRooms[i].nextRoomPoints.Add(new Vector3(-1.0f, -1.0f, -1.0f));
            }

            initialPoint = new Vector3(
                RoomModel.maxSize[0] * mainRooms[i].Pointi,
                RoomModel.maxSize[1] * mainRooms[i].Pointj,
                0.0f
            );
            mainRooms[i].initialPoint = initialPoint;

            if (lastDir == (int)DIR.LEFT || lastDir == (int)DIR.DOWN)
            {
                previous = new Vector3(
                    (lastDir == (int)DIR.LEFT) ? mainRooms[i].Size[0] - 1 : previous.x,
                    (lastDir == (int)DIR.DOWN) ? mainRooms[i].Size[1] - 1 : previous.y,
                    0.0f
                );
            }
            mainRooms[i].previousRoomPoints.Add(previous);
            previous = new Vector3(
                (dir == (int)DIR.RIGHT) ? 0 : mainRooms[i].nextRoomPoints[mainRooms[i].nextRoomPoints.Count - 1].x,
                (dir == (int)DIR.UP) ? 0 : mainRooms[i].nextRoomPoints[mainRooms[i].nextRoomPoints.Count - 1].y,
                0.0f
            );
            lastDir = dir;

        }
    }


    private void CreateExtraRooms()
    {
        Vector3 previous = new Vector3(-1.0f, -1.0f, -1.0f);
        List<List<int>> possibles = new List<List<int>>();
        int dir = 0;
        RoomModel mainRoom;

        for (int j = 0; j < levelSizeMax; j++)
        {
            for (int k = 0; k < levelSizeMax; k++)
            {
                if (levelMatrix[j, k] == (int)LEVEL.MAIN)
                {
                    List<int> n = new List<int>();
                    n.Add(j); n.Add(k);
                    possibles.Add(n);
                }
            }
        }

        extraRoomsSize = rnd.Next(extraRoomsSizeMin, possibles.Count);
		extraRooms = new RoomModel[extraRoomsSize];

        for (int i = 0; i < possibles.Count && i < extraRoomsSize; i++)
        {
            extraRooms[i] = ScriptableObject.CreateInstance("RoomModel") as RoomModel;

            fi = possibles[i][0];
            fj = possibles[i][1];
			dir = FindEmptyDir(dir);
			//Debug.Log("index i: " + possibles[i][0] + " - index j: " + possibles[i][1]);
			mainRoom = FindRoomFromIndexes(possibles[i][0], possibles[i][1]);
			if (mainRoom != null)
            {
                mainRoom.ExitDirs.Add(dir);
                extraRooms[i].EnterDirs.Add(dir);

                mainRoom.nextRoomPoints.Add(new Vector3(
                        (dir == (int)DIR.LEFT) ? 0 : (dir == (int)DIR.RIGHT) ? mainRooms[i].Size[0] - 1 : rnd.Next(1, RoomModel.minSize[0] - 1),
                        (dir == (int)DIR.DOWN) ? 0 : (dir == (int)DIR.UP) ? mainRooms[i].Size[1] - 1 : rnd.Next(1, RoomModel.minSize[1] - 1),
                        0.0f
                    ));

                levelMatrix[fi, fj] = (int)LEVEL.EXTRA;
                extraRooms[i].Pointi = fi;
                extraRooms[i].Pointj = fj;
                //Debug.Log("fi: " + fi + " - fj: " + fj);

                extraRooms[i].Size[0] = rnd.Next(RoomModel.minSize[0], RoomModel.maxSize[0]);
                extraRooms[i].Size[1] = rnd.Next(RoomModel.minSize[1], RoomModel.maxSize[1]);

                //extraRooms[i].nextRoomPoint = new Vector3(-1.0f, -1.0f, -1.0f);

                initialPoint = new Vector3(
                    RoomModel.maxSize[0] * extraRooms[i].Pointi,
                    RoomModel.maxSize[1] * extraRooms[i].Pointj,
                    0.0f
                );
                extraRooms[i].initialPoint = initialPoint;

                extraRooms[i].previousRoomPoints.Add(new Vector3(
                    (mainRoom.ExitDirs[mainRoom.ExitDirs.Count - 1] == (int)DIR.LEFT) ? mainRoom.Size[0] - 1 : (mainRoom.ExitDirs[mainRoom.ExitDirs.Count - 1] == (int)DIR.RIGHT) ? 0 : mainRoom.nextRoomPoints[mainRoom.nextRoomPoints.Count - 1].x,
                    (mainRoom.ExitDirs[mainRoom.ExitDirs.Count - 1] == (int)DIR.DOWN) ? mainRoom.Size[1] - 1 : (mainRoom.ExitDirs[mainRoom.ExitDirs.Count - 1] == (int)DIR.UP) ? 0 : mainRoom.nextRoomPoints[mainRoom.nextRoomPoints.Count - 1].y,
                    0.0f
                ));
                extraRooms[i].roomFrom = mainRoom;
                //Debug.Log(extraRooms[i].previousRoomPoints[0].ToString());

            }
            else {
                Debug.Log("Main room not found.");
            }
        }
    }

    public void InstantiateMainRooms(Transform parent) {
        for (int i = 0; i < mainRoomsSize; i++) {
            GameObject room = Instantiate(new GameObject());
            room.name = "mainRoom " + i;
            room.transform.parent = parent;
			mainRooms[i].Init(room.transform);

		}
        for (int i = 1; i < mainRoomsSize; i++) {
            if (mainRooms[i].EnterDirs[0] == (int)DIR.RIGHT || mainRooms[i].EnterDirs[0] == (int)DIR.LEFT)
            {
                mainRooms[i - 1].CreatePathTo(RoomModel.maxSize[0] - ((mainRooms[i].EnterDirs[0] == (int)DIR.LEFT) ? mainRooms[i].Size[0] : mainRooms[i - 1].Size[0]) - 1, mainRooms[i].EnterDirs[0], mainRooms[i - 1].nextRoomPoints[0]);
            }
            else {
                mainRooms[i - 1].CreatePathTo(RoomModel.maxSize[1] - ((mainRooms[i].EnterDirs[0] == (int)DIR.DOWN) ? mainRooms[i].Size[1] : mainRooms[i - 1].Size[1]) - 1, mainRooms[i].EnterDirs[0], mainRooms[i - 1].nextRoomPoints[0]);
            }
        }
	}


    public void InstantiateExtraRooms(Transform parent) {
        for (int i = 0; i < extraRoomsSize; i++) {
            GameObject room = Instantiate(new GameObject());
            room.name = "extraRoom " + i;
            room.transform.parent = parent;
			extraRooms[i].Init(room.transform);
		}
        for (int i = 0; i < extraRoomsSize; i++) {
            if (extraRooms[i].EnterDirs[0] == (int)DIR.RIGHT || extraRooms[i].EnterDirs[0] == (int)DIR.LEFT)
            {
                extraRooms[i].roomFrom.CreatePathTo(RoomModel.maxSize[0] - ((extraRooms[i].EnterDirs[0] == (int)DIR.LEFT) ? extraRooms[i].Size[0] : extraRooms[i].roomFrom.Size[0]) - 1, extraRooms[i].EnterDirs[0], extraRooms[i].previousRoomPoints[0]);
            }
            else
            {
                extraRooms[i].roomFrom.CreatePathTo(RoomModel.maxSize[1] - ((extraRooms[i].EnterDirs[0] == (int)DIR.DOWN) ? extraRooms[i].Size[1] : extraRooms[i].roomFrom.Size[1]) - 1, extraRooms[i].EnterDirs[0], extraRooms[i].previousRoomPoints[0]);
				//extraRooms[i].CreatePathTo(RoomModel.maxSize[1] - ((dir == (int)DIR.DOWN) ? extraRooms[i].Size[1] : mainRooms[i].Size[1]), dir);
			}
        }
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

        CreateMainRooms();
        CreateExtraRooms();
        InstantiateMainRooms(parent);
        InstantiateExtraRooms(parent);

		CreateExtraRooms(parent);
    }
}
