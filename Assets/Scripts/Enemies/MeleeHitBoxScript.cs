using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeHitBoxScript : MonoBehaviour {

    public EnemyController controller;
    public bool isOn;
    public PlayerBehaviour player;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerBehaviour>() != null)
        {
            player = collision.gameObject.GetComponent<PlayerBehaviour>();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerBehaviour>() == player)
        {
            player = null;
        }
    }

}
