﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SpellType { Shot, Laser, Cone, Bomb, Wave, Teleport, Dash, Mine, Trap };



public class SpellSatistics : MonoBehaviour {

    public PlayerBehaviour player;


    public SpellType type;

    public float slowDuration; //Em segundos
    public float slowPower;    //Multiplicador (entre 1 e 0.1)

    public float snareDuration; //Em segundos

    public float poisonDuration; //Em segundos
    public float poisonPower;    //Dano causado em 1s

    public float critMultiplier; //Multiplicador
    public float critChance;     //Entre 0 e 100

    public float lifeSteal;      //Multiplicador

    public float damage;

    public int extraArmorDamage;
    public float armorPierce;

    public int trample;

    public int manaCost;

    public float area; //tornar global e multiplicar no behaviour específico
    public int number;

    public float duration;

    public float distance;

    private void Awake()
    {
        DontDestroyOnLoad(this);
    }

    void Start()
    {
        if (GameObject.FindGameObjectWithTag("Player") != null)
        {
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerBehaviour>();
        }
    }

    public bool RollCrit()
    {
        float r;
        r = Random.Range(0, 100);

        if (r <= critChance)
        {
            return true;
        }
        else
        {
            return false;
        }

    }



}
