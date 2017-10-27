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
        Debug.Log("Dano spellData[0] : " + spellData[0].spellShell.GetComponent<SpellBehaviourBase>().stats.damage.ToString());
        Debug.Log("Dano spellData[1] : " + spellData[1].spellShell.GetComponent<SpellBehaviourBase>().stats.damage.ToString());
        Debug.Log("");
        Debug.Log("[0] pela shell eh null? : " + spellData[0].spellShell.GetComponent<SpellBehaviourBase>().stats);
        Debug.Log("[1] pels shell eh null? : " + spellData[1].spellShell.GetComponent<SpellBehaviourBase>().stats);
        Debug.Log("[0] pelo SO eh null? : " + spellData[0].spellStats);
        Debug.Log("[1] pelo SO eh null? : " + spellData[1].spellStats);

    }

}
