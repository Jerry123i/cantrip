using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellSatistics : MonoBehaviour {

    public PlayerBehaviour player;

    public enum SpellType {Shot, Laser, Cone, Bomb, Wave, Teleport, Dash, Mine, Trap};

    public SpellType type;

    public float slowDuration; //Em segundos
    public float slowPower;    //Multiplicador (entre 0.1 e 0.9)

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

    public float area; //tornar global e multiplicar no behaviour específico
    public int number;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerBehaviour>();
    }

    public bool rollCrit()
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
