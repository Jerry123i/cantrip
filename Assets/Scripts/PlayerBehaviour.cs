using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerBehaviour : MonoBehaviour {

    public List<GameObject> spellPrefab;
    public List<SpellSatistics> spellStats;
    [HideInInspector]
    public Transform targetTransform;

    [HideInInspector]
    public int spellSelector;

    public float movSpeed;
    public float maxHP;

    public int maxMana;
    public int currentMana;

	public float shotSpreadAngle = 15;

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
            HolderObjectScript sh;
            sh = GameObject.Find("SpellHolder").GetComponent<HolderObjectScript>();

            sh.PrintSpellData();

            spellPrefab.Clear();

            spellPrefab.Add(sh.spellData[0].spellShell);
            spellPrefab.Add(sh.spellData[1].spellShell);
            
            Debug.Log("spellPrefab[0].GetComponent<SpellBehaviourBase>().stats:"+spellPrefab[0].GetComponent<SpellBehaviourBase>().stats.damage.ToString());
            Debug.Log("[0]" + spellPrefab[0].GetComponent<SpellBehaviourBase>().stats);
            Debug.Log("[1]" + spellPrefab[1].GetComponent<SpellBehaviourBase>().stats);

            spellPrefab[0].GetComponent<SpellBehaviourBase>().stats.player = this;
            spellPrefab[1].GetComponent<SpellBehaviourBase>().stats.player = this;

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

        GameObject go;
        Vector3 mouse;
        mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        
        spellSelector = a;
        SpellSatistics spell;
        spell = spellPrefab[spellSelector].GetComponent<SpellBehaviourBase>().stats;

        Debug.Log(spell);


        switch (spell.type)
        {           
            case SpellType.Bomb:
                go = Instantiate(spellPrefab[spellSelector], new Vector3(mouse.x, mouse.y, 0.0f), Quaternion.Euler(Vector3.zero));
                go.GetComponent<SpellBehaviourBase>().stats = spell;
                break;

            case SpellType.Wave:
                go = Instantiate(spellPrefab[spellSelector], this.gameObject.GetComponent<Transform>().position, this.gameObject.GetComponent<Transform>().rotation, this.gameObject.GetComponent<Transform>());
                go.GetComponent<SpellBehaviourBase>().stats = spell;
                break;

            case SpellType.Shot:
                Shot(spell);
                break;

            case SpellType.Trap:
                go = Instantiate(spellPrefab[spellSelector], new Vector3(mouse.x, mouse.y, 0.0f), Quaternion.Euler(Vector3.zero));
                go.GetComponent<SpellBehaviourBase>().stats = spell;
                break;

            case SpellType.Laser:
                go = Instantiate(spellPrefab[spellSelector], targetTransform.position, targetTransform.rotation, targetTransform);
                go.GetComponent<SpellBehaviourBase>().stats = spell;
                break;

            case SpellType.Dash:
                Dash(spell);
                break;

            case SpellType.Teleport:
                Teleport(spell);
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

    public void Dash(SpellSatistics spell)
    {
        GameObject go;
        Vector3 aPoint;
        Vector3 bPoint;
        Vector3 spawnPoint;

        aPoint = this.gameObject.transform.position;
        this.gameObject.transform.Translate(Vector3.up * spellPrefab[spellSelector].GetComponent<SpellBehaviourBase>().stats.area);
        bPoint = this.gameObject.transform.position;

        spawnPoint = new Vector3((aPoint.x + bPoint.x) / 2, (aPoint.y + bPoint.y) / 2, 0.0f);

        go = Instantiate(spellPrefab[spellSelector], spawnPoint, Quaternion.Euler(0, 0, angle - 90.0f));
        go.GetComponent<SpellBehaviourBase>().stats = spell;

    }

    public void Teleport(SpellSatistics spell)
    {
        GameObject go;
        float teleportDistance;
        Vector3 flatMouse;

        flatMouse = new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y, 0.0f);

        Debug.Log("ScreenToWorld: " + flatMouse);
        Debug.Log("Distancia: " + Vector3.Distance(flatMouse, this.gameObject.GetComponent<Transform>().localPosition));

        if (Vector3.Distance(flatMouse, this.gameObject.GetComponent<Transform>().localPosition) < spellPrefab[spellSelector].GetComponent<SpellBehaviourBase>().stats.area)
        {
            teleportDistance = Vector3.Distance(flatMouse, this.gameObject.GetComponent<Transform>().localPosition);
        }
        else
        {
            teleportDistance = spellPrefab[spellSelector].GetComponent<SpellBehaviourBase>().stats.area;
        }

        this.gameObject.transform.Translate(Vector3.up * teleportDistance);
        go = Instantiate(spellPrefab[spellSelector], this.gameObject.GetComponent<Transform>().position, this.gameObject.GetComponent<Transform>().rotation, this.gameObject.GetComponent<Transform>());
        go.GetComponent<SpellBehaviourBase>().stats = spell;
    }

    public void Shot(SpellSatistics spell)
    {
        GameObject go;

        Debug.Log(spell.ToString());

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
                go = Instantiate(spellPrefab[spellSelector], targetTransform.position, Quaternion.Euler(0,0, angle + (shotSpreadAngle*i)));
                go.GetComponent<SpellBehaviourBase>().stats = spell;
            }
        }

        else
        {
            go = Instantiate(spellPrefab[spellSelector], targetTransform.position, targetTransform.rotation);
            go.GetComponent<SpellBehaviourBase>().stats = spell;
        }
        
    }

}
