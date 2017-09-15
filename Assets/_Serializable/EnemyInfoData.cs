using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Flags]
public enum EnemyGroup {
        FIRE   = 1,
        EARTH  = 2,
        WATER  = 4,
        AIR    = 8,
        AETHER = 16
}

[System.Flags]
public enum EnemyTag {
    RANGED = 1,
    MELEE = 2,
    SUPPORT = 4,
    IMMOVABLE = 8,
    VANILLA = 16,
    FODDER= 32
}

[CreateAssetMenu(fileName = "EnemyTag", menuName = "Enemy Tag", order = 2)]
public class EnemyInfoData : ScriptableObject {
    
    public EnemyGroup groups;
    public EnemyTag tags;
    

}
