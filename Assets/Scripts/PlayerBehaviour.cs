using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour {

    public float movSpeed;

	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
        Move();
        Rotate();
        if (Input.GetMouseButtonDown(0))
        {
            MainAttack();
        }
        else if (Input.GetMouseButtonDown(1))
        {
            AltAttack();
        }
	}

    void Rotate ()
    {
        // A orientação do player segue a posição do mouse
        Vector3 mousePos = Input.mousePosition;
        Vector3 screenPoint = Camera.main.WorldToScreenPoint(transform.localPosition);
        Vector2 offset = new Vector2(mousePos.x - screenPoint.x, mousePos.y - screenPoint.y);
        float angle = Mathf.Atan2(offset.y, offset.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle - 90.0f);
        
    }

    void MainAttack ()
    {
        Debug.Log(Input.mousePosition);
    }

    void AltAttack ()
    {
        Debug.Log(Input.mousePosition);
    }

    void Move ()
    {
        // Translação simples
        float dx = Input.GetAxis("Horizontal") * movSpeed * Time.deltaTime;
        float dy = Input.GetAxis("Vertical") * movSpeed * Time.deltaTime;
        transform.Translate(dx, dy, 0, Camera.main.transform);
    }
}
