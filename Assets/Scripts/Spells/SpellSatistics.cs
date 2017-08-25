using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellSatistics : MonoBehaviour {

    public enum SpellType {Shot, Laser, Cone, Bomb, Wave, Teleport, Dash, Mine, Trap};

    public SpellType type;

    public float slowDuration;
    public float slowPower;

    public float snareDuration;

    public float poisonDuration;
    public float poisonPower;

    public float critMultiplier;

    public float lifeSteal;

    public float damage;

    public int extraArmorDamage;
    public float armorPierce;

    public int trample;

    public int area;
    public int number;

	void Start () {
		
	}
	
	void Update () {
		
	}
}
