using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerBehaviour : MonoBehaviour {

    public GameObject[] spellPrefab;
    [HideInInspector]
    public Transform targetTransform;

    [HideInInspector]
    public int spellSelector;

    public float movSpeed;
    public float maxHP;

    public int maxMana;
    public int currentMana;

	public float shotSpreadAngle;

    [HideInInspector]
    public float angle;

    public float hp;
    private bool isDead;

    float dmgCooldownClock;
    [HideInInspector]
    public bool onDmgCooldown;
    public float dmgCooldown;

	void Start () {
        hp = maxHP;
        isDead = false;

        if (GameObject.Find("SpellHolder") !=null)
        {
            HolderObjectScript sh = GameObject.Find("SpellHolder").GetComponent<HolderObjectScript>();

            spellPrefab[0] = sh.spellData[0].spellShell;
            spellPrefab[1] = sh.spellData[1].spellShell;

            spellPrefab[0].GetComponent<SpellBehaviourShot>().stats = sh.spellData[0].spellStatistics;
            spellPrefab[1].GetComponent<SpellBehaviourShot>().stats = sh.spellData[1].spellStatistics;
        }

	}
	
	void Update () {
        Move();
        Rotate();

        if (isDead)
        {
            this.gameObject.GetComponent<SpriteRenderer>().color = Color.black;
        }

        if (onDmgCooldown)
        {
            DmgCooldownUpdate();
        }
        
        if (Input.GetMouseButtonDown(0))
        {
            MainCast(0);
        }
        else if (Input.GetMouseButtonDown(1))
        {
            MainCast(1);
        }
	}

    void Rotate ()
    {
        // A orientação do player segue a posição do mouse
        Vector2 mousePos = Input.mousePosition;
        Vector2 screenPoint = Camera.main.WorldToScreenPoint(transform.localPosition);
        Vector2 offset = new Vector2(mousePos.x - screenPoint.x, mousePos.y - screenPoint.y);
        angle = Mathf.Atan2(offset.y, offset.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle - 90.0f);
        
    }

    // Funções só pro prótipo com um inimigo
    void MainCast (int a)
    {
        Vector3 mouse;
        mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        
        spellSelector = a;
        SpellSatistics spell;
        spell = spellPrefab[spellSelector].GetComponent<SpellSatistics>();

        switch (spell.type)
        {           
            case SpellType.Bomb:
                Instantiate(spellPrefab[spellSelector], new Vector3(mouse.x, mouse.y, 0.0f), Quaternion.Euler(Vector3.zero));
                break;

            case SpellType.Wave:
                Instantiate(spellPrefab[spellSelector], this.gameObject.GetComponent<Transform>().position, this.gameObject.GetComponent<Transform>().rotation, this.gameObject.GetComponent<Transform>());
                break;

            case SpellType.Shot:
                Shot();
                break;

            case SpellType.Trap:
                Instantiate(spellPrefab[spellSelector], new Vector3(mouse.x, mouse.y, 0.0f), Quaternion.Euler(Vector3.zero));
                break;

            case SpellType.Laser:
                Instantiate(spellPrefab[spellSelector], targetTransform.position, targetTransform.rotation, targetTransform);
                break;

            case SpellType.Dash:
                Dash();
                break;

            case SpellType.Teleport:
                Teleport();
                break;

            default:
                Debug.Log("Tipo não implementado");
                break;
        }
        
    }

    void Move ()
    {
        // Translação simples
        float dx = Input.GetAxis("Horizontal") * movSpeed * Time.deltaTime;
        float dy = Input.GetAxis("Vertical") * movSpeed * Time.deltaTime;
        transform.Translate(dx, dy, 0, Camera.main.transform);
    }

    public void TakeDamage (float damageTaken)
    {
        if (!onDmgCooldown)
        {
            hp -= damageTaken;
            if (hp <= 0) isDead = true;
            onDmgCooldown = true;
            dmgCooldownClock = 0.0f;
        }
    }

    void DmgCooldownUpdate()
    {
        dmgCooldownClock += Time.deltaTime;
        if (dmgCooldownClock >= dmgCooldown)
        {
            dmgCooldownClock = 0.0f;
            onDmgCooldown = false;
        }
    }

    public void Dash()
    {
        Vector3 aPoint;
        Vector3 bPoint;
        Vector3 spawnPoint;

        aPoint = this.gameObject.transform.position;
        this.gameObject.transform.Translate(Vector3.up * spellPrefab[spellSelector].GetComponent<SpellSatistics>().area);
        bPoint = this.gameObject.transform.position;

        spawnPoint = new Vector3((aPoint.x + bPoint.x) / 2, (aPoint.y + bPoint.y) / 2, 0.0f);

        Instantiate(spellPrefab[spellSelector], spawnPoint, Quaternion.Euler(0, 0, angle - 90.0f));

    }

    public void Teleport()
    {
        float teleportDistance;
        Vector3 flatMouse;

        flatMouse = new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y, 0.0f);

        Debug.Log("ScreenToWorld: " + flatMouse);
        Debug.Log("Distancia: " + Vector3.Distance(flatMouse, this.gameObject.GetComponent<Transform>().localPosition));

        if (Vector3.Distance(flatMouse, this.gameObject.GetComponent<Transform>().localPosition) < spellPrefab[spellSelector].GetComponent<SpellSatistics>().area)
        {
            teleportDistance = Vector3.Distance(flatMouse, this.gameObject.GetComponent<Transform>().localPosition);
        }
        else
        {
            teleportDistance = spellPrefab[spellSelector].GetComponent<SpellSatistics>().area;
        }

        this.gameObject.transform.Translate(Vector3.up * teleportDistance);
        Instantiate(spellPrefab[spellSelector], this.gameObject.GetComponent<Transform>().position, this.gameObject.GetComponent<Transform>().rotation, this.gameObject.GetComponent<Transform>());
    }

    public void Shot()
    {
        SpellSatistics spell;
        spell = spellPrefab[spellSelector].GetComponent<SpellSatistics>();

        if(spell == null)
        {
            Debug.Log("Spell is null");
        }

        float angle;

        if (spell.number > 1)
        {
            angle = this.transform.eulerAngles.z - (((spell.number - 1) * shotSpreadAngle)/2.0f);
            
            for (int i = 0; i < spell.number; i++)
            {
                Instantiate(spellPrefab[spellSelector], targetTransform.position, Quaternion.Euler(0,0, angle + (shotSpreadAngle*i)));
            }
        }

        else
        {
            Instantiate(spellPrefab[spellSelector], targetTransform.position, targetTransform.rotation);
        }
        
    }

}
