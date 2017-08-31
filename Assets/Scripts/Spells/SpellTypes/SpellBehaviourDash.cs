using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellBehaviourDash : MonoBehaviour {

    public SpellSatistics stats;
    
    float clock = 0.0f;
    public float maxDuration=0.15f;

    void Start()
    {
        this.gameObject.transform.localScale = new Vector3(this.gameObject.transform.localScale.x, stats.area, this.gameObject.transform.localScale.z);

        if (stats.rollCrit())
        {
            stats.damage *= stats.critMultiplier;
            Debug.Log("*CRIT!*");
        }

    }


    void Update()
    {
        clock += Time.deltaTime;
        if (clock >= maxDuration)
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

            if (hit.currentArmor > 0)
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
                    hit.CallStopPoison();
                }
                hit.currentEffects.poison = stats.poisonPower;
                hit.PullPoison(stats.poisonDuration);
            }

            //Snare 
            ///Snare em efeitos overtime chama uma vez no enter trigger e o script de inimigo liga e inicia o desligamento do bool
            if (stats.snareDuration > 0)
            {
                if (!hit.debuffSnare && !hit.onSnareCD)
                {
                    hit.PullSnare(stats.snareDuration);
                }
            }

        }
    }


}
