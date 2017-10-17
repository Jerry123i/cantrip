using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScriptSapper : BasicRangedEnemyScript {

    enum SapperBehaviour {RANGED, MELEE}

    SapperBehaviour behaviour;
    public float turnRange;
    public float speedMultiplier = 1;

    public override void Update()
    {
        EnemyHealthCare();

        if(behaviour == SapperBehaviour.RANGED)
        {
            base.Update();
        }
        else
        {
            aiControler.speed = currentSpeed;
        }

        if (behaviour == SapperBehaviour.RANGED && isShooting && (Vector3.Distance(this.transform.position, player.transform.position) < turnRange))
        {
            behaviour = SapperBehaviour.MELEE;
            initialSpeed = initialSpeed * speedMultiplier;
            currentSpeed = currentSpeed * speedMultiplier;
            aiControler.canMove = true;
        }
    }

}
