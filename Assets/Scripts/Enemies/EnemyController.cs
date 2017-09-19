﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Pathfinding;

public class EnemyController : MonoBehaviour {

	[HideInInspector]
    public CurrentEffects currentEffects;
	[HideInInspector]
	public AIPath aiControler;

    public Image hpBar;
    public Image armorBar;

    public IEnumerator currentSlowRoutine;
    public IEnumerator currentPoisonRoutine;
    public IEnumerator currentSnareRoutine;

	[HideInInspector]
	public bool debuffSlow, debuffPoison, debuffSnare;

	[HideInInspector]
	public bool onSnareCD, onArmorCD;
    
	[HideInInspector]
    public int currentArmor;
	[HideInInspector]
	public float currentSpeed;
	[HideInInspector]
	public float currentHp;

    public float maxHP;
    public int initialArmor;
    public float initialSpeed;
	
    float armorInterval = 0.75f;
    float snareInterval = 5.0f;

	[HideInInspector]
	public GameObject player;
	private Rigidbody2D rb;

     virtual public void Start () {
		aiControler = this.gameObject.GetComponent<AIPath>();
		currentEffects = this.gameObject.GetComponent<CurrentEffects>();
        player = GameObject.FindGameObjectWithTag("Player");
		aiControler.target = player.transform;
        currentHp = maxHP;
        currentArmor = initialArmor;
        currentSpeed = initialSpeed;
		rb = GetComponent<Rigidbody2D>();

        if(initialArmor == 0)
        {
            armorBar.gameObject.SetActive(false);
        }

	}
	
	virtual public void Update () {

		aiControler.speed = currentSpeed;		
		EnemyHealthCare();

	}

    void Move ()
    {
        // Vai na direção do jogador
        Vector2 dir;
        dir = player.transform.position - transform.position;
        if (dir.magnitude > 0.001f)
        {
			rb.MovePosition(rb.position + dir.normalized * currentSpeed * Time.fixedDeltaTime);
            //transform.position = Vector2.MoveTowards(transform.position, player.transform.position, (currentSpeed * Time.deltaTime));
        }
    }

    public void TakeDamage (float damageTaken)
    {
        currentHp -= damageTaken;
    }

    public void LoseArmor(int lostArmor)
    {
        currentArmor -= lostArmor;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
          //  collision.GetComponent<PlayerBehaviour>().TakeDamage(debugDamage);
        }
    }

	virtual public void EnemyHealthCare() {

		//Mantem HP Armor Morte e Poison

		if (initialArmor > 0) {
			armorBar.fillAmount = (float)currentArmor / initialArmor;
		}

		hpBar.fillAmount = currentHp / maxHP;

		if (debuffPoison) {
			currentHp -= currentEffects.poison * Time.deltaTime;
		}

		if (currentHp > maxHP) {
			currentHp = maxHP;
		}

		if (currentHp <= 0) {
			Destroy(this.gameObject);
		}



	}

	public IEnumerator RoutineSlow(float slowDuration)
    {
        debuffSlow = true;
        currentSpeed = initialSpeed * currentEffects.slow;
        Debug.Log("Slowed for: " + slowDuration.ToString() + "s");
        yield return new WaitForSeconds(slowDuration);
        Debug.Log("EndSlow");
        debuffSlow = false;
        currentEffects.slow = 0;
        currentSpeed = initialSpeed;
    }


    public IEnumerator RoutinePoison(float poisonDuration)
    {
        debuffPoison = true;
        yield return new WaitForSeconds(poisonDuration);
        currentEffects.poison = 0;
        debuffPoison = false;
    }

    public IEnumerator RoutineSnare(float snareDuration)
    {
        debuffSnare = true;
        onSnareCD = true;
		aiControler.canMove = false;
		rb.bodyType = RigidbodyType2D.Kinematic;
		Debug.Log("Snare "+snareDuration.ToString()+"s");
        yield return new WaitForSeconds(snareDuration);
        Debug.Log("End Snare");
        debuffSnare = false;
		aiControler.canMove = true;
		rb.bodyType = RigidbodyType2D.Dynamic;
		StartCoroutine(SnareCooldown());
    }

    public IEnumerator ArmorCooldown()
    {
        onArmorCD = true;
        yield return new WaitForSeconds(armorInterval);
        onArmorCD = false;
    }

    public IEnumerator SnareCooldown()
    {
        yield return new WaitForSeconds(snareInterval);
        onSnareCD = false;
    }

    public void PullArmor()
    {
        StartCoroutine(ArmorCooldown());
    }

    public void PullSlow(float duration)
    {
        currentSlowRoutine = RoutineSlow(duration);
        StartCoroutine(currentSlowRoutine);
    }
    public void CallStopSlow()
    {
        StopCoroutine(currentSlowRoutine);
    }

    public void PullPoison(float duration)
    {
        currentPoisonRoutine = RoutinePoison(duration);
        StartCoroutine(currentPoisonRoutine);
    }
    public void CallStopPoison()
    {
        StopCoroutine(currentPoisonRoutine);
    }
    public void PullSnare(float duration)
    {
        currentSnareRoutine = RoutineSnare(duration);
        StartCoroutine(currentSnareRoutine);
    }
    public void CallStopSnare()
    {
        StopCoroutine(currentSnareRoutine);
    }

}
