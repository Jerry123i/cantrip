using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelBehaviour : MonoBehaviour {

    public LevelModel level;

	// Use this for initialization
	void Start () {
        level.GenerateLevel(this.transform);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
