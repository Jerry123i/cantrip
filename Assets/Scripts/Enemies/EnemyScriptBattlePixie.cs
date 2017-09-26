using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScriptBattlePixie : EnemyScriptPixie {
        
    int armorUsed=0;
    int pixieArmorLimit=4;
    public bool berserk;

    public override void Update()
    {
        base.Update();

        if(berserk == true && behaviour != States.ATACKING)
        {
            behaviour = States.ATACKING;
            aiControler.target = player.transform;
        }

    }

    public override void Healing()
    {
        clock += Time.deltaTime;

        if (clock >= healingCooldown)
        {
            aiControler.target.GetComponent<EnemyController>().CurrentArmor ++;
            clock = 0.0f;
            armorUsed++;

            if(PickNewTarget() == null || armorUsed>= pixieArmorLimit)
            {
                behaviour = States.ATACKING;
                aiControler.target = player.transform;
                berserk = true;
            }
            else
            {
                aiControler.target = PickNewTarget();
            }

        }

    }

    public override Transform PickNewTarget()
    {
        GameObject newTarget;
        GameObject[] allEnemies;
        float shortestDistance = float.MaxValue;

        newTarget = null;

        allEnemies = GameObject.FindGameObjectsWithTag("Enemy");

        for (int i = 0; i < allEnemies.Length; i++)
        {

            if (allEnemies[i].GetComponent<EnemyScriptPixie>() == false)
            {
                if (Vector3.Distance(transform.position, allEnemies[i].transform.position) < shortestDistance && (allEnemies[i].GetComponent<EnemyController>().CurrentArmor==0))
                {
                    newTarget = allEnemies[i];
                    shortestDistance = Vector3.Distance(transform.position, allEnemies[i].transform.position);
                }
            }
        }

        if (newTarget != null)
        {
            Debug.Log((newTarget.GetComponent<EnemyController>().maxHP - newTarget.GetComponent<EnemyController>().currentHp).ToString());
            return newTarget.transform;
        }

        else
        {
            return null;
        }

    }

}
