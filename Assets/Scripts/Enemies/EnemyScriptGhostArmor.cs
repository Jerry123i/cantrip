using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScriptGhostArmor : EnemyController {

	public override void EnemyHealthCare() {

		if (initialArmor > 0) {
			armorBar.fillAmount = (float)currentArmor / initialArmor;
		}

		hpBar.fillAmount = currentHp / maxHP;

		if (debuffPoison) {
			currentHp -= currentEffects.poison * Time.deltaTime;
		}

		if (currentHp > maxHP) {
			currentHp = maxHP;
		}

		if (currentArmor <= 0) {
			Destroy(this.gameObject);
		}


	}

}
