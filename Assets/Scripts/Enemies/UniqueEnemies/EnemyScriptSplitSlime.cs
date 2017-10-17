using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScriptSplitSlime : EnemyController {

	public GameObject spawnedSlimes;
	public int numberOfSpawns;

	public override void EnemyHealthCare() {

		if(currentHp <= 0) {
			for(int i=0; i<numberOfSpawns; i++) {
				GameObject.Instantiate(spawnedSlimes, transform.position, Quaternion.Euler(0,0,0));
			}
		}

		base.EnemyHealthCare();
	}

}
