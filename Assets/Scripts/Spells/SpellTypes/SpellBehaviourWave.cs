using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellBehaviourWave : SpellBehaviourBase {

    public CircleCollider2D aoe;

    private float growthRate;
    public float maxRadius;


    public void Start()
    {
        maxRadius = FixAreaValue(stats.area, 1.5f, 6.75f);

        if(stats.type == SpellType.Teleport)
        {
            maxRadius /= 2.0f;
        }

        growthRate = 0.1f;

        if (stats.RollCrit())
        {
            stats.damage *= stats.critMultiplier;
            Debug.Log("*CRIT!*");
        }

	}
	

	void Update () {
        aoe.radius += Time.deltaTime + growthRate;
        if (aoe.radius >= maxRadius)
        {
            Destroy(this.gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D cool)
    {

        if (cool.GetComponent<EnemyController>() != null)
        {
            EnemyController hit;
            hit = cool.GetComponent<EnemyController>();

            //Rever a ordem de operação dos efeitos de armadura
            if (hit.CurrentArmor > 0 && stats.damage>0)
            {
                hit.LoseArmor(1 + stats.extraArmorDamage);
                hit.TakeDamage(stats.damage * stats.armorPierce);
                stats.player.hp += stats.damage * stats.armorPierce * stats.lifeSteal;
            }
            else
            {
                hit.TakeDamage(stats.damage);
                stats.player.hp += stats.damage * stats.lifeSteal;
            }

            //Slow
            if (stats.slowDuration > 0 && stats.slowPower > 0)
            {
                if (hit.currentEffects.slow > 0)
                {
                    hit.CallStopSlow();
                }
                hit.currentEffects.slow = stats.slowPower;
                hit.PullSlow(stats.slowDuration);
            }

            //Poison
            if (stats.poisonDuration > 0 && stats.poisonPower > 0)
            {
                if (hit.currentEffects.poison > 0)
                {
                    hit.StopCoroutine("RoutinePoison");
                }
                hit.currentEffects.poison = stats.poisonPower;
                hit.PullPoison(stats.poisonDuration);
            }

            //Snare
            if (stats.snareDuration > 0)
            {
                if (hit.debuffSnare)
                {
                    hit.StopCoroutine("RoutineSnare");
                }
                hit.PullSnare(stats.snareDuration);
            }
        }
    }


}
