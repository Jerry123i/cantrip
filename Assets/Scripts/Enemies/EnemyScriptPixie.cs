using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum States {SEEKING, HEALING, WANDERING }

public class EnemyScriptPixie : EnemyController {

	States behaviour;
	float healingDistance = 0.42f;
	public float healingPower;
	float healingCooldown=1.1f;
	float clock = 0.0f;
	GameObject wanderPoint;

	// Use this for initialization
	public override void Start () {
		base.Start();
		aiControler.target = PickNewTarget();		
	}

	public override void Update() {

		EnemyHealthCare();

		if (aiControler.target == null || behaviour == States.WANDERING) {

			if (!debuffSnare) {
				Wander();
			}

			if(PickNewTarget() == null) 
			{
				behaviour = States.WANDERING;
			}			
			else 
			{
				aiControler.target = PickNewTarget();
				behaviour = States.SEEKING;
			}

		}

		else if (behaviour == States.SEEKING) {

			base.Update();

			if ((behaviour == States.SEEKING && (Vector3.Distance(this.transform.position, aiControler.target.position) <= healingDistance))) {
				clock = 0.0f;
				behaviour = States.HEALING;
				aiControler.speed = 0.0f;
			}
					

		}

		if (behaviour == States.HEALING) {

			if (debuffSnare == false) {
				this.transform.RotateAround(aiControler.target.position, Vector3.forward, currentSpeed * 70 * Time.deltaTime);
				RotationMove();
				Healing();
			}

			if(Vector3.Distance(transform.position, aiControler.target.position)>healingDistance+0.5f) {
				behaviour = States.SEEKING;
			}
		}
	}

	Transform PickNewTarget() {

		GameObject newTarget;
		GameObject[] allEnemies;
		float shortestDistance = float.MaxValue;

		newTarget = null;

		allEnemies = GameObject.FindGameObjectsWithTag("Enemy");

		for(int i =0; i<allEnemies.Length; i++) {

			if (allEnemies[i].GetComponent<EnemyScriptPixie>() == false) {
				if (Vector3.Distance(transform.position, allEnemies[i].transform.position) < shortestDistance && (allEnemies[i].GetComponent<EnemyController>().maxHP - allEnemies[i].GetComponent<EnemyController>().currentHp > 0)) {
					newTarget = allEnemies[i];
					shortestDistance = Vector3.Distance(transform.position, allEnemies[i].transform.position);
				}
			}
		}

		if(newTarget != null) {
			Debug.Log((newTarget.GetComponent<EnemyController>().maxHP - newTarget.GetComponent<EnemyController>().currentHp).ToString());
			return newTarget.transform;
		}

		else {
			return null;
		}

	}

	void Healing() {

		clock += Time.deltaTime;

		if(clock>= healingCooldown) {
			aiControler.target.GetComponent<EnemyController>().currentHp += healingPower;
			clock = 0.0f;

			if(aiControler.target.GetComponent<EnemyController>().maxHP - aiControler.target.GetComponent<EnemyController>().currentHp <= 0) {
				behaviour = States.WANDERING;
			}

		}

	}

	void RotationMove() {

		Vector3 direcao;
		float angle;

		direcao = aiControler.target.transform.position - transform.position;

		angle = (Mathf.Atan2(direcao.y, direcao.x) * Mathf.Rad2Deg) - 90.0f;

		Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);

		transform.rotation = Quaternion.Slerp(transform.rotation, q, Time.deltaTime * currentSpeed * 10.0f);

		if (direcao.magnitude - healingDistance > 0.3f) {
			transform.position = Vector3.MoveTowards(transform.position, aiControler.target.GetComponent<Transform>().position, Time.deltaTime*currentSpeed);
		} else if (direcao.magnitude - healingDistance < -0.3f) {
			transform.Translate(Vector3.down * Time.deltaTime * (3.0f));
		}

		transform.Translate(Vector3.right * Time.deltaTime * currentSpeed);
	}

	void Wander() {

		clock += Time.deltaTime;

		base.Update();

		if(clock >= 1.5f) {
			clock = 0.0f;

			if (wanderPoint != null) {
				Destroy(wanderPoint);
			}

			wanderPoint = new GameObject("WanderPoint");
			wanderPoint.transform.position = RandomNearbyVector();
			aiControler.target = wanderPoint.transform;
		}
	}

	Vector3 RandomNearbyVector() {
		Vector3 vector;
		float x;
		float y;

		x = transform.position.x + Random.Range(-3.0f, 3.0f);
		y = transform.position.y + Random.Range(-3.0f, 3.0f);

		vector = new Vector3(x, y, 0);

		return vector;
	}

}
