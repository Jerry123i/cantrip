using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellBehaviourBase : MonoBehaviour {

    public SpellSatistics stats;

    public float FixAreaValue(float x, float minValue, float maxValue)
    {
        return (100 * minValue - maxValue - minValue * x + maxValue * x) / 99;
    }

}
