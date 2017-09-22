using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum ElementalBehaviour {RANGED, MELEE}

public class EnemyScriptElemental : BasicRangedEnemyScript {

	ElementalBehaviour behaviour;
	float speedMultiplier = 2.1f;

	public override void Update() {

		EnemyHealthCare();

		if (behaviour == ElementalBehaviour.RANGED)
		{
			base.Update();
		}		
		else
		{
			aiControler.speed = currentSpeed;
		}

		if ((currentHp <= (maxHP / 2)) && behaviour == ElementalBehaviour.RANGED)
		{
			behaviour = ElementalBehaviour.MELEE;
			initialSpeed = initialSpeed * speedMultiplier;
			currentSpeed = currentSpeed * speedMultiplier;
			aiControler.canMove = true;
		}

	}

}
