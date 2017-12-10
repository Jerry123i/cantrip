using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MasterSpawnerScript : MonoBehaviour {

    public List<GameObject> nests;


    List<EnemyInfoData> enemyPool;

    List<EnemyInfoData> tagRanged;
    List<EnemyInfoData> tagMelee;
    List<EnemyInfoData> tagSupport;
    List<EnemyInfoData> tagImovable;
    List<EnemyInfoData> tagVanilla;
    List<EnemyInfoData> tagFodder;

    EnemyGroup enemyGroup;

    GameObject newEnemy;

    private void Awake()
    {
        enemyGroup = (EnemyGroup)PlayerPrefs.GetInt("EnemyGroup", 0);

        InitializeLists();

        GrabPool();
        FillTags();
    }

    private void Start()
    {

    }

    private void Update()
    {
        if (Input.GetKeyDown("o"))
        {
            newEnemy = Instantiate(RandomEnemyFromList(enemyPool), nests[Random.Range(0, nests.Count)].transform.position, Quaternion.Euler(0, 0, 0));
        }
    }


    void GrabPool()
    {
        Object[] objects;
        EnemyInfoData holder;

        objects = Resources.LoadAll("EnemyTags", typeof(EnemyInfoData));

        Debug.Log("Enemytags - " + enemyGroup.ToString());
        for (int i = 0; i < objects.Length; i++)
        {
            holder = (EnemyInfoData)objects[i];

            if (((holder.groups & enemyGroup) != 0) && holder.prefab != null)
            {
                enemyPool.Add(holder);
                Debug.Log(holder.prefab.ToString());
            }
        }
    }

    void FillTags()
    {
        for(int i=0; i<enemyPool.Count; i++)
        {
            if((enemyPool[i].tags & EnemyTag.FODDER) != 0)
            {
                tagFodder.Add(enemyPool[i]);
            }
            if ((enemyPool[i].tags & EnemyTag.IMMOVABLE) != 0)
            {
                tagImovable.Add(enemyPool[i]);
            }
            if ((enemyPool[i].tags & EnemyTag.MELEE) != 0)
            {
                tagMelee.Add(enemyPool[i]);
            }
            if ((enemyPool[i].tags & EnemyTag.RANGED) != 0)
            {
                tagRanged.Add(enemyPool[i]);
            }
            if ((enemyPool[i].tags & EnemyTag.SUPPORT) != 0)
            {
                tagSupport.Add(enemyPool[i]);
            }
            if ((enemyPool[i].tags & EnemyTag.VANILLA) != 0)
            {
                tagVanilla.Add(enemyPool[i]);
            }
        }
    }

    void InitializeLists() {

        enemyPool = new List<EnemyInfoData>();
        tagRanged = new List<EnemyInfoData>();
        tagFodder = new List<EnemyInfoData>();
        tagImovable = new List<EnemyInfoData>();
        tagMelee = new List<EnemyInfoData>();
        tagSupport = new List<EnemyInfoData>();
        tagVanilla = new List<EnemyInfoData>();

    }

    GameObject RandomEnemyFromList(List<EnemyInfoData> list)
    {
        if(list.Count == 0 || list == null)
        {
            return tagVanilla[Random.Range(0, tagVanilla.Count)].prefab;
        }
        else
        {
            return list[Random.Range(0, list.Count)].prefab;
        }
    }

}
