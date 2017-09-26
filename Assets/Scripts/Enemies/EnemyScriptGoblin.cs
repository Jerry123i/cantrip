using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScriptGoblin : BasicRangedEnemyScript {

    public int multiAtack;
    public float aimOffset=1;


    public override void Update()
    {
        base.Update();
        
    }

    public override void Shot()
    {
        base.Shot();
    }

    public override void RotateToPlayer()
    {        
        Vector3 direcao;
        float angle;
        direcao = player.transform.position - transform.position;
        angle = (Mathf.Atan2(direcao.y, direcao.x) * Mathf.Rad2Deg) - 90.0f;
        Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, q, Time.deltaTime * currentSpeed * 5.0f);

    }
}
