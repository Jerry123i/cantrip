using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {

    public GameObject enemy;
	
	void Start () {
		
	}
	
	
	void Update () {
        // Pressionar ctrl esquerdo pra spawnar inimigo
		if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            Spawn();
        }
	}

    void Spawn ()
    {
        Instantiate(enemy, transform.position, transform.rotation);
    }
}
