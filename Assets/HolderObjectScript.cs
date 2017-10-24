using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HolderObjectScript : MonoBehaviour {

    public List<SpellData> spellData;

    private void Awake()
    {
        DontDestroyOnLoad(this);
    }

}
