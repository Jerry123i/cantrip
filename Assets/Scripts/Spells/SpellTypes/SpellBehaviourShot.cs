﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Refazer isso com flags dps
public enum ValidTargets {ENEMY, PLAYER}

public class SpellBehaviourShot : SpellBehaviourBase {

    public ValidTargets targets;
    
    public float projectileSpeed;
    
    public int trample;

    public float clock;

    // Use this for initialization
    public void Start()
    {
        trample = stats.trample;

        if (stats.RollCrit())
        {
            stats.damage *= stats.critMultiplier;
            Debug.Log("*CRIT!*");
        }

    }
	
	// Update is called once per frame
	void Update () {

        gameObject.GetComponent<Transform>().Translate(Vector3.up * projectileSpeed * Time.deltaTime);

        clock += Time.deltaTime;
        if (clock >= 5.0f)
        {
            Destroy(this.gameObject);
        }

	}

    void OnTriggerEnter2D(Collider2D cool)
    {
		if (cool.tag == "Wall") {
			Destroy(this.gameObject);
		}

        if(cool.GetComponent<EnemyController>() != null && targets==ValidTargets.ENEMY)
        {
            HitEnemy(cool);			
        }
        if (cool.GetComponent<PlayerBehaviour>() != null && targets==ValidTargets.PLAYER)
        {
            HitPlayer(cool);
        }
    }

    void HitEnemy(Collider2D cool)
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
                hit.CallStopPoison();
            }
            hit.currentEffects.poison = stats.poisonPower;
            hit.PullPoison(stats.poisonDuration);
        }

        //Snare
        if (stats.snareDuration > 0)
        {
            if (hit.debuffSnare)
            {
                hit.CallStopSnare();
            }
            hit.PullSnare(stats.snareDuration);
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

    void HitPlayer(Collider2D cool)
    {
        PlayerBehaviour hit;
        hit = cool.GetComponent<PlayerBehaviour>();

        hit.hp -= stats.damage;
        GameObject.Destroy(this.gameObject);
    }
	
}
