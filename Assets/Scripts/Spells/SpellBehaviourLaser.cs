using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellBehaviourLaser : MonoBehaviour {

    public SpellSatistics stats;
    
    public float clock;

	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {

        //Extensão do raio

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

            //Snare (Vai ter que ter alguma forma de coldown)
            /*if (stats.snareDuration>0)
            {
                if (hit.debuffSnare)
                {
                    hit.StopCoroutine("RoutineSnare");
                }
                StartCoroutine(hit.RoutineSnare(stats.snareDuration));
            }*/

        }
    }

    private void OnTriggerStay2D(Collider2D coll)
    {
        if (coll.GetComponent<EnemyController>() != null)
        {
            EnemyController hit;
            hit = coll.GetComponent<EnemyController>();

            //Verificar timing de armadura e efeitos de dano over time
            
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

            //Snare (Vai ter que ter alguma forma de coldown)
            /*if (stats.snareDuration>0)
            {
                if (hit.debuffSnare)
                {
                    hit.StopCoroutine("RoutineSnare");
                }
                StartCoroutine(hit.RoutineSnare(stats.snareDuration));
            }*/

        }

    }

}
