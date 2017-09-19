using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum State {RANGED, MELEE}

public class EnemyScriptElemental : BasicRangedEnemyScript {

	State behaviour;
	float speedMultiplier = 2.1f;

	public override void Update() {

		EnemyHealthCare();

		if (behaviour == State.RANGED)
		{
			base.Update();
		}		
		else
		{
			aiControler.speed = currentSpeed;
		}

		if ((currentHp <= (maxHP / 2)) && behaviour == State.RANGED)
		{
			behaviour = State.MELEE;
			initialSpeed = initialSpeed * speedMultiplier;
			currentSpeed = currentSpeed * speedMultiplier;
			aiControler.canMove = true;
		}

	}

}
