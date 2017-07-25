using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerBehaviour : MonoBehaviour {

    public float movSpeed;
    public float maxHP;
    public float debugDamage;

    private float hp;
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
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);
        if (hit && hit.collider.tag == "Enemy")
        {
            hit.collider.GetComponent<SandbagController>().TakeDamage(debugDamage);
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
        else
        {
            // SceneManager.LoadScene("example");
        }
        Debug.Log(hp);
    }
}
