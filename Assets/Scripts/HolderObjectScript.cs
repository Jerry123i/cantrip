using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HolderObjectScript : MonoBehaviour {

    public List<SpellData> spellData;

    private void Awake()
    {
        DontDestroyOnLoad(this);
    }

    public void PrintSpellData()
    {
        Debug.Log("Print Spell Data");
        Debug.Log("Data0 - " + spellData[0].spellStats.ToString());
        Debug.Log("Data1 - " + spellData[1].spellStats.ToString());
    }

}
