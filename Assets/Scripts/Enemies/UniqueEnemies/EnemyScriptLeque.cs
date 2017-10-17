using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScriptLeque : BasicRangedEnemyScript {

	public int nOfProjectiles;
	float shotSpreadAngle=13.0f;

	public override void Shot() {

		float angle;

		angle = this.transform.eulerAngles.z - (((nOfProjectiles - 1) * shotSpreadAngle) / 2.0f);

		for (int i = 0; i < nOfProjectiles; i++) {
			Instantiate(projectile, aimObject.transform.position, Quaternion.Euler(0, 0, angle + (shotSpreadAngle * i)));
		}

		clock = 0.0f;

	}

}
