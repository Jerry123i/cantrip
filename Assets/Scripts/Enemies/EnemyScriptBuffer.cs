using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum BufferBehaviour {NORMAL, DYING }

public class EnemyScriptBuffer : EnemyController {

	BufferBehaviour behaviour;

	public float speedMultiplier;
	public float touchRange;

	public override void Update() {
		base.Update();

		if (currentArmor <= 0 && behaviour == BufferBehaviour.NORMAL) {
			ChangeBehaviourToDying();
		}

		if (currentArmor>0 && behaviour == BufferBehaviour.DYING) {
			ChangeBehaviourToNormal();
		}

		if(behaviour == BufferBehaviour.DYING && Vector3.Distance(transform.position, aiControler.target.position)<=touchRange) {
			Buff(aiControler.target.gameObject.GetComponent<EnemyController>());
			Destroy(this.gameObject);
		}

	}

	void ChangeBehaviourToDying() {

		behaviour = BufferBehaviour.DYING;

		if(PickNewTarget() != null) {
			aiControler.target = PickNewTarget();
		} else {
			behaviour = BufferBehaviour.NORMAL;
		}
	}

	void ChangeBehaviourToNormal() {

		behaviour = BufferBehaviour.NORMAL;

		aiControler.target = GameObject.FindGameObjectWithTag("Player").transform;

	}

	void Buff(EnemyController guy) {
		guy.currentHp += currentHp;
		guy.currentSpeed *= speedMultiplier;
		guy.initialSpeed *= speedMultiplier;

		if(guy.gameObject.GetComponent<BasicRangedEnemyScript>() != null) {
			guy.gameObject.GetComponent<BasicRangedEnemyScript>().atkSpeed *= speedMultiplier;
		}
	}
	
	Transform PickNewTarget() {

		GameObject newTarget;
		GameObject[] allEnemies;
		float shortestDistance = float.MaxValue;

		newTarget = null;

		allEnemies = GameObject.FindGameObjectsWithTag("Enemy");

		for (int i = 0; i < allEnemies.Length; i++) {

			if (allEnemies[i].GetComponent<EnemyScriptPixie>() == false) {
				if (Vector3.Distance(transform.position, allEnemies[i].transform.position) < shortestDistance && (allEnemies[i].GetComponent<EnemyController>().maxHP - allEnemies[i].GetComponent<EnemyController>().currentHp > 0)) {
					newTarget = allEnemies[i];
					shortestDistance = Vector3.Distance(transform.position, allEnemies[i].transform.position);
				}
			}
		}

		if (newTarget != null) {
			Debug.Log((newTarget.GetComponent<EnemyController>().maxHP - newTarget.GetComponent<EnemyController>().currentHp).ToString());
			return newTarget.transform;
		} else {
			return null;
		}


	}

}
