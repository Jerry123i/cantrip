using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerBehaviour : MonoBehaviour {

    public GameObject[] spellPrefab;
    public Transform targetTransform;
    public Text texto;

    public int spellSelector;

    public float movSpeed;
    public float maxHP;
    public float debugDamage;

    public float hp;
    private bool isDead;

	void Start () {
        hp = maxHP;
        isDead = false;
	}
	
	void Update () {
        Move();
        Rotate();
        if (Input.GetMouseButtonDown(0))
        {
            MainCast();
        }
        else if (Input.GetMouseButtonDown(1))
        {
            AltCast();
        }

        if (Input.GetKeyDown("q"))
        {
            if (spellSelector > 0)
            {
                spellSelector--;
            }
        }
        if (Input.GetKeyDown("e"))
        {
            if (spellSelector < spellPrefab.Length - 1)
            {
                spellSelector++;
            }
        }
        texto.text = spellSelector.ToString();
	}

    void Rotate ()
    {
        // A orientação do player segue a posição do mouse
        Vector2 mousePos = Input.mousePosition;
        Vector2 screenPoint = Camera.main.WorldToScreenPoint(transform.localPosition);
        Vector2 offset = new Vector2(mousePos.x - screenPoint.x, mousePos.y - screenPoint.y);
        float angle = Mathf.Atan2(offset.y, offset.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle - 90.0f);
        
    }

    // Funções só pro prótipo com um inimigo
    void MainCast ()
    {
        if(spellPrefab[spellSelector].GetComponent<SpellSatistics>().type == SpellSatistics.SpellType.Bomb)
        {
            Vector3 mouse;
            mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);            

            Instantiate(spellPrefab[spellSelector], new Vector3(mouse.x, mouse.y, 0.0f), Quaternion.Euler(Vector3.zero));

        }

        else if (spellPrefab[spellSelector].GetComponent<SpellSatistics>().type == SpellSatistics.SpellType.Wave)
        {
            Instantiate(spellPrefab[spellSelector], this.gameObject.GetComponent<Transform>().position, this.gameObject.GetComponent<Transform>().rotation, this.gameObject.GetComponent<Transform>());
        }

        else if (spellPrefab[spellSelector].GetComponent<SpellSatistics>().type == SpellSatistics.SpellType.Shot)
        {
            Instantiate(spellPrefab[spellSelector], targetTransform.position, targetTransform.rotation);
        }
    }

    void AltCast ()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Debug.Log(mousePos);
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
        if (!isDead)
        {
            hp -= damageTaken;
            if (hp <= 0) isDead = true;
        }
        Debug.Log(hp);
    }
}
