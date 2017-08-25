using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour {

    public CurrentEffects currentEffects;

    public Image hpBar;
    public Image armorBar;

    public IEnumerator currentSlowRoutine;
    public IEnumerator currentPoisonRoutine;
    public IEnumerator currentSnareRoutine;

    public bool debuffSlow;
    public bool debuffPoison;
    public bool debuffSnare;
    
    public int currentArmor;
    public float currentSpeed;
    public float currentHp;

    public float maxHP;
    public int initialArmor;
    public float initialSpeed;

    public float debugDamage;

    
    private GameObject player;

    void Start () {
        player = GameObject.FindGameObjectWithTag("Player");
        currentHp = maxHP;
        currentArmor = initialArmor;
        currentSpeed = initialSpeed;

        if(initialArmor == 0)
        {
            armorBar.gameObject.SetActive(false);
        }

	}
	
	void Update () {

        if (initialArmor > 0)
        {
            armorBar.fillAmount = (float)currentArmor/initialArmor;
        }
        hpBar.fillAmount = currentHp / maxHP;

        if (!debuffSnare)
        {
            Move();
        }


        if (debuffPoison)
        {
            currentHp -= currentEffects.poison * Time.deltaTime;
        }

        if (currentHp <= 0)
        {
            Destroy(this.gameObject);
        }

	}

    void Move ()
    {
        // Vai na direção do jogador
        Vector2 dir;
        dir = player.transform.position - transform.position;
        if (dir.magnitude > 0.001f)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, (currentSpeed * Time.deltaTime));
        }
    }

    public void TakeDamage (float damageTaken)
    {
        if (currentArmor <= 0)
        {
            currentHp -= damageTaken;            
        }
        else
        {
            currentArmor--;
        }
        
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            collision.GetComponent<PlayerBehaviour>().TakeDamage(debugDamage);
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
        Debug.Log("Snare: " + snareDuration.ToString() + "s");
        yield return new WaitForSeconds(snareDuration);
        Debug.Log("End Snare");
        debuffSnare = false;
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
