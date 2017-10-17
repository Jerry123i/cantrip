using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScriptEsqueleto : EnemyController {

	public bool armorDown;
	public float speedBurstMultiplier;

	public override void Update() {
		base.Update();

		if(CurrentArmor == 0 && armorDown == false) {
			armorDown = true;
			currentSpeed *= speedBurstMultiplier;
			initialSpeed *= speedBurstMultiplier;
		}

	}

}
