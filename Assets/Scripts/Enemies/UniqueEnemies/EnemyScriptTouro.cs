using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum TouroBehaviour {WANDERING, HOLDING, CHARGING, STUNNED }

public class EnemyScriptTouro : EnemyController {

	Color normalColor;
	
	float clock;
	public float chargeTime;
	public float stunTime;
	public float sightRange;
	TouroBehaviour behaviour;

	public GameObject aimObject;

	public float chargeDmgMultiplier;
	public float chargeSpeedMultiplier;

	public override void Start() {
		base.Start();				
		behaviour = TouroBehaviour.WANDERING;
		normalColor = this.gameObject.GetComponent<SpriteRenderer>().color;
	}

	public override void Update() {

		EnemyHealthCare();
		
		if(behaviour == TouroBehaviour.STUNNED) {
			clock += Time.deltaTime;
			if (clock >= stunTime) {
				clock = 0.0f;
				behaviour = TouroBehaviour.WANDERING;
			}
		}

		if(behaviour == TouroBehaviour.WANDERING) {
			aiControler.speed = currentSpeed;
		}

		if (behaviour == TouroBehaviour.HOLDING) {
			this.gameObject.GetComponent<SpriteRenderer>().color = Color.red;
		}
		else {
			this.gameObject.GetComponent<SpriteRenderer>().color = normalColor;
		}

		if (behaviour == TouroBehaviour.WANDERING && SearchPlayerFront()) {
			behaviour = TouroBehaviour.HOLDING;
			aiControler.canMove = false;
		}

		if (behaviour == TouroBehaviour.HOLDING) {
			clock += Time.deltaTime;
			if(clock >= chargeTime) {
				clock = 0.0f;
				ToggleCharge(true);
			}
		}

		if (behaviour == TouroBehaviour.CHARGING) {

			this.transform.Translate(Vector3.up * Time.deltaTime * currentSpeed);

		}

	}

	public void OnCollisionEnter2D(Collision2D collision) {
		
		if(behaviour== TouroBehaviour.CHARGING && collision.gameObject.tag == "Wall") {
			ToggleCharge(false);
		}

	}

	void ToggleCharge(bool onOff) {

		if (onOff) {
			behaviour = TouroBehaviour.CHARGING;
			currentSpeed *= chargeSpeedMultiplier;
			initialSpeed *= chargeSpeedMultiplier;
			//ADICIONAR DANO AQUI QUANDO TIVER DANO

			aiControler.canMove = true;
			aiControler.speed = 0.0f;
		}
		else {
			behaviour = TouroBehaviour.STUNNED;
			currentSpeed /= chargeSpeedMultiplier;
			initialSpeed /= chargeSpeedMultiplier;

			aiControler.canMove = true;
		}

	}
	
	public bool SearchPlayerFront()
	{
		Vector3 rayOrigin;
		Vector3 rayDirection;

		bool playerFound = false;

		rayOrigin = aimObject.transform.position;
		rayDirection = this.transform.up;

		RaycastHit2D[] ray;
		ray = Physics2D.RaycastAll(rayOrigin, rayDirection, sightRange);

		for (int j = 0; j < ray.Length; j++) {
			if (ray[j].transform != null) {
				if (ray[j].transform.gameObject.tag == "Player") {
					playerFound = true;
				}
				if (ray[j].transform.gameObject.tag == "Wall") {
					break;
				}
			}
		}

		return playerFound;

	}

}
