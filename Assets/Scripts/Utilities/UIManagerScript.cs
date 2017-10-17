using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManagerScript : MonoBehaviour {

    Image hpBar;
    Image manaBar;
    PlayerBehaviour player;

    float manaFill;

	// Use this for initialization
	void Start () {

        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerBehaviour>();
        hpBar = GameObject.Find("HealthBar").GetComponent<Image>();
        manaBar = GameObject.Find("ManaBar").GetComponent<Image>();
    }
	
	// Update is called once per frame
	void Update () {

        manaFill = (float)player.currentMana / (float)player.maxMana;

        hpBar.fillAmount = player.hp / player.maxHP;
        manaBar.fillAmount = manaFill;
		
	}
}
