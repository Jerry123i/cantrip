using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellBehaviourBase : MonoBehaviour {

    public SpellSatistics stats;

    virtual public void Start()
    {
       // stats = this.GetComponent<SpellSatistics>();
    }

}
