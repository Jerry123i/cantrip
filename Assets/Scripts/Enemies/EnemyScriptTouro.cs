using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum TouroBehaviour {WANDERING, HOLDING, CHARGING }

public class EnemyScriptTouro : EnemyController {

	Color normalColor;

	BasicRangedEnemyScript rangedScript;
	public GameObject aimObject;
	Vector3 chargeDirection;
	float clock;
	public float chargeTime;
	TouroBehaviour behaviour;

	public float chargeDmgMultiplier;
	public float chargeSpeedMultiplier;

	public override void Start() {
		base.Start();				
		behaviour = TouroBehaviour.WANDERING;
		normalColor = this.gameObject.GetComponent<SpriteRenderer>().color;

		rangedScript = new BasicRangedEnemyScript();
		rangedScript.atackRange = 5;
		rangedScript.aimObject = aimObject;
		rangedScript.searchRange = 5;
	}

	public override void Update() {


		base.Update();

		if (behaviour == TouroBehaviour.CHARGING) {
			this.gameObject.GetComponent<SpriteRenderer>().color = Color.red;
		}
		else {
			this.gameObject.GetComponent<SpriteRenderer>().color = normalColor;
		}

		if (behaviour == TouroBehaviour.WANDERING && rangedScript.SearchPlayerWide()) {
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
			//charja

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
		}
		else {
			behaviour = TouroBehaviour.WANDERING;
			currentSpeed /= chargeSpeedMultiplier;
			initialSpeed /= chargeSpeedMultiplier;
		}

	}


}
