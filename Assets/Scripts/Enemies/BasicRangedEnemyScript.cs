using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicRangedEnemyScript : EnemyController {

	public float atackRange;
	public GameObject projectile;
	public float atkSpeed;
	public GameObject aimObject;
	public bool isShooting;

	public IEnumerator currentAtackRoutine;

	public override void Start() {
		base.Start();
	}

	public override void Update() {
		base.Update();		

		if (SearchPlayerWide() && !isShooting) {
			aiControler.canMove = false;
			RotateToPlayer();
			isShooting = true;
		} 
		else if (!isShooting) {
			aiControler.canMove = true;
			isShooting = false;
			if(currentAtackRoutine != null) {
				StopCoroutine(currentAtackRoutine);
				currentAtackRoutine = null;
			}

		}

		if (isShooting) {
			if(currentAtackRoutine == null) {
				currentAtackRoutine = RangedAtack();
				StartCoroutine(currentAtackRoutine);
			}
		}

	}

	void RotateToPlayer() {

		Vector3 rotateTarget;

		rotateTarget = new Vector3(player.transform.localPosition.x, player.transform.localPosition.y, -10.0f);
		Quaternion rot = Quaternion.LookRotation(transform.position - rotateTarget, Vector3.forward);
		transform.rotation = rot;
		transform.eulerAngles = new Vector3(0, 0, transform.eulerAngles.z);

	}
	

	bool SearchPlayerWide() {
		int x = 32;
		int a;
		int b;

		x = (x * 4) - 4;

		a = (x / 4) - 1;
		b = 0;

		Vector3 rayOrigin;
		Vector3 rayDirection;

		for (int i = 0; i < x; i++) {

			rayOrigin = aimObject.transform.position;

			if (i < x / 4) {
				rayDirection = aimObject.transform.up * a + aimObject.transform.right * b;
			} 			
			else if (i < 2 * x / 4) {

				if (i == x / 4) {
					a = (x / 4) - 1;
					b = 0;
				}

				rayDirection = Vector3.down * a + Vector3.right * b;
			}
			else if (i < 3 * x / 4) {
				if (i == x * 2 / 4) {
					a = (x / 4) - 1;
					b = 0;
				}
				rayDirection = Vector3.down * a + Vector3.left * b;
			}
			else {
				if (i == x * 3 / 4) {
					a = (x / 4) - 1;
					b = 0;
				}
				rayDirection = aimObject.transform.up * a + aimObject.transform.right * -b;
			}

			a--;
			b++;
			
			RaycastHit2D[] ray;

			if (a>=x/6 && (i<x/4 || i>= 3 * x / 4)) {
				Debug.DrawRay(rayOrigin, rayDirection, Color.blue);
				ray = Physics2D.RaycastAll(rayOrigin, rayDirection, atackRange);
				if (ray.Length > 1) {
					if (ray[0].transform != null) {
						if (ray[0].transform.gameObject.tag == "Player") {
							Debug.Log("Player found");
							return true;
						}
					}
				}
			}			
		}

		return false;

	}

	public IEnumerator RangedAtack() {
		Instantiate(projectile, aimObject.transform.position, aimObject.transform.rotation);
		yield return new WaitForSeconds(1 / atkSpeed);
		StartCoroutine(currentAtackRoutine);
	}

}
