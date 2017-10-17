using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScriptTurret : BasicRangedEnemyScript {

    public float rotateSpeed;

    public override void Update()
    {
		EnemyHealthCare();

        if (SearchPlayerWide())
        {
            isShooting = true;
        }
        else
        {
            isShooting = false;
            clock = 0.0f;
        }

        if (isShooting)
        {
            RotateToPlayer();

            clock += Time.deltaTime;
            if (clock >= 1 / atkSpeed)
            {
                Shot();
            }
        }

    }

    public override void RotateToPlayer()
    {
        Vector3 direcao;
        float angle;
        direcao = player.transform.position - transform.position;
        angle = (Mathf.Atan2(direcao.y, direcao.x) * Mathf.Rad2Deg) - 90.0f;
        Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, q, Time.deltaTime * rotateSpeed);

    }

    public override bool SearchPlayerWide()
    {
        bool playerFound=false;
        int x = 32;
        int a;
        int b;

        x = (x * 4) - 4;

        a = (x / 4) - 1;
        b = 0;

        Vector3 rayOrigin;
        Vector3 rayDirection;

        for (int i = 0; i < x; i++)
        {

            rayOrigin = aimObject.transform.position;
            //Atira raios em todas as direções
            if (i < x / 4)
            {
                rayDirection = aimObject.transform.up * a + aimObject.transform.right * b;
            }
            else if (i < 2 * x / 4)
            {

                if (i == x / 4)
                {
                    a = (x / 4) - 1;
                    b = 0;
                }

                rayDirection = aimObject.transform.up * -a + aimObject.transform.right * b;
            }
            else if (i < 3 * x / 4)
            {
                if (i == x * 2 / 4)
                {
                    a = (x / 4) - 1;
                    b = 0;
                }
                rayDirection = aimObject.transform.up * -a + aimObject.transform.right * -b;
            }
            else {
                if (i == x * 3 / 4)
                {
                    a = (x / 4) - 1;
                    b = 0;
                }
                rayDirection = aimObject.transform.up * a + aimObject.transform.right * -b;
            }

            a--;
            b++;

            RaycastHit2D[] ray;

            Debug.DrawRay(rayOrigin, rayDirection, Color.blue);
            ray = Physics2D.RaycastAll(rayOrigin, rayDirection, atackRange);
            //Pega todos os objetos encontrados no Raycast
            //Procura até ver uma parede ou o jogador

           
            for (int j = 0; j < ray.Length; j++)
            {
                if (ray[j].transform != null)
                {
                    if (ray[j].transform.gameObject.tag == "Player")
                    {
                        playerFound = true;
                    }
                    if (ray[j].transform.gameObject.tag == "Wall")
                    {
                        break;
                    }
                }
            }
        }

        return playerFound;

    }

}

