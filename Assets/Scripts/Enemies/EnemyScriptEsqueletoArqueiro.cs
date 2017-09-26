using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScriptEsqueletoArqueiro : BasicRangedEnemyScript {

	public bool armorDown;
	public float speedBurstMultiplier;
	public float atackBurstMultiplier;

	public override void Update() {
		base.Update();

		if (CurrentArmor == 0 && armorDown == false) {
			armorDown = true;
			currentSpeed *= speedBurstMultiplier;
			initialSpeed *= speedBurstMultiplier;
			atkSpeed *= atackBurstMultiplier;
		}


	}

}
