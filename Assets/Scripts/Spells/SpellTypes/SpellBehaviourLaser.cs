using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellBehaviourLaser : MonoBehaviour {

    public SpellSatistics stats;
    //bool isCritMode;

    Transform box;
    
	// Use this for initialization
	void Start () {

        box = this.gameObject.GetComponent<Transform>();

        if (stats.rollCrit())
        {
            stats.damage *= stats.critMultiplier;
        }

	}
	
	// Update is called once per frame
	void Update () {

        //Extensão do raio

        if(box.localScale.y < stats.area)
        {
            box.localScale += new Vector3(0.0f, 0.05f, 0.0f);
            box.localPosition += new Vector3(0.0f, 0.025f, 0.0f);
        }

        if (Input.GetMouseButtonUp(0))
        {
            Destroy(this.gameObject);
        }
    }
        
    void OnTriggerEnter2D(Collider2D cool)
    {
        if(cool.GetComponent<EnemyController>() != null)
        {
            EnemyController hit;
            hit = cool.GetComponent<EnemyController>();

            //Slow
            if(stats.slowDuration>0 && stats.slowPower > 0)
            {
                if (hit.currentEffects.slow > 0)
                {
                    hit.CallStopSlow();
                }
                hit.currentEffects.slow = stats.slowPower;
                hit.PullSlow(float.MaxValue);
            }

            //Poison
            if(stats.poisonDuration>0 && stats.poisonPower > 0)
            {
                if(hit.currentEffects.poison > 0)
                {
                    hit.CallStopPoison();
                }
                hit.currentEffects.poison = stats.poisonPower;
                hit.PullPoison(float.MaxValue);
            }

            //Snare 
            ///Snare em efeitos overtime chama uma vez no enter trigger e o script de inimigo liga e inicia o desligamento do bool
            if (stats.snareDuration>0)
            {
                if (!hit.debuffSnare && !hit.onSnareCD)
                {
                    hit.PullSnare(stats.snareDuration);
                }                
            }
        }
    }

    private void OnTriggerStay2D(Collider2D coll)
    {
        if (coll.GetComponent<EnemyController>() != null)
        {
            EnemyController hit;
            hit = coll.GetComponent<EnemyController>();

            //Verificar timing de armadura e efeitos de dano over time
            if (hit.CurrentArmor > 0)
            {
                if (stats.armorPierce > 0)
                {
                    hit.TakeDamage(stats.damage * Time.deltaTime * stats.armorPierce);
                    stats.player.hp += stats.damage * Time.deltaTime * stats.armorPierce * stats.lifeSteal;
                }

                if (!hit.onArmorCD)
                {
                    hit.LoseArmor(1 + stats.extraArmorDamage);
                    hit.PullArmor();
                }

            }
            else
            {
                hit.TakeDamage(stats.damage*Time.deltaTime);
                stats.player.hp += stats.damage * stats.lifeSteal * Time.deltaTime;
            }
            
        }
    }

    void OnTriggerExit2D(Collider2D coll)
    {
        if (coll.GetComponent<EnemyController>() != null)
        {
            EnemyController hit;
            hit = coll.GetComponent<EnemyController>();

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
            
        }

    }

    /*IEnumerator CritMode()
    {
        isCritMode = true;
        stats.damage *= stats.critMultiplier;
        this.gameObject.GetComponent<SpriteRenderer>().color = Color.red;
        yield return new WaitForSeconds(2.0f);
        isCritMode = false;
        stats.damage /= stats.critMultiplier;
        this.gameObject.GetComponent<SpriteRenderer>().color = Color.white;
    }*/

}
