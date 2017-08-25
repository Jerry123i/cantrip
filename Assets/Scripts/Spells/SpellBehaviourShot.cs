﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellBehaviourShot : MonoBehaviour {

    public SpellSatistics stats;

    public float projectileSpeed;
    
    public int trample;

    public float clock;

	// Use this for initialization
	void Start () {
        trample = stats.trample;
	}
	
	// Update is called once per frame
	void Update () {

        gameObject.GetComponent<Transform>().Translate(Vector3.up * projectileSpeed);

        clock += Time.deltaTime;
        if (clock >= 5.0f)
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

            //Rever a ordem de operação dos efeitos de armadura
            if (hit.currentArmor > 0)
            {
                hit.currentArmor -= stats.extraArmorDamage;
                hit.currentHp -= stats.damage * stats.armorPierce;
            }
            hit.TakeDamage(stats.damage);

            //Slow
            if(stats.slowDuration>0 && stats.slowPower > 0)
            {
                if (hit.currentEffects.slow > 0)
                {
                    hit.CallStopSlow();
                }
                hit.currentEffects.slow = stats.slowPower;
                hit.PullSlow(stats.slowDuration);
            }

            //Poison
            if(stats.poisonDuration>0 && stats.poisonPower > 0)
            {
                if(hit.currentEffects.poison > 0)
                {
                    hit.StopCoroutine("RoutinePoison");
                }
                hit.currentEffects.poison = stats.poisonPower;
                StartCoroutine(hit.RoutinePoison(stats.poisonDuration));
            }

            //Snare
            if (stats.snareDuration>0)
            {
                if (hit.debuffSnare)
                {
                    hit.StopCoroutine("RoutineSnare");
                }
                StartCoroutine(hit.RoutineSnare(stats.snareDuration));
            }

            if (trample <= 0)
            {
                Destroy(this.gameObject);
            }
            else
            {
                trample--;
            }

        }
    }

}