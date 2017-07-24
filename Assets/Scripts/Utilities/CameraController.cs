using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    // Script simples pra câmera acompanhar o jogador

    public GameObject player;

    private Vector3 offset;
	
	void Start () {
        transform.position = new Vector3(player.transform.position.x, player.transform.position.y, -10);
        offset = transform.position - player.transform.position;
	}
	
	void Update () {
        transform.position = player.transform.position + offset;
	}
}
