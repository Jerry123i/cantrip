using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpellMenuScript : MonoBehaviour {

    public SpellSatistics spell;
    public List<SpellSliderScript> sliders;
    public Dropdown typeDropdown;
    public GridLayoutGroup grid;

    public GameObject basicBomb;
    public GameObject basicDash;
    public GameObject basicLaser;
    public GameObject basicShot;
    public GameObject basicTeleport;
    public GameObject basicTrap;
    public GameObject basicWave;

    public GameObject spellHolder;

    // Use this for initialization
    void Start () {

        spell = gameObject.AddComponent(typeof(SpellSatistics)) as SpellSatistics;

        SetDropdown();

        SetSlider(sliders[0], "Slow Power", 0.0f, 100.0f);
        SetSlider(sliders[1], "Slow Duration", 1.0f, 5.0f);        
        SetSlider(sliders[2], "Snare Duration", 0.0f, 3.0f);
        SetSlider(sliders[3], "Poison Power", 0.0f, 100f);
        SetSlider(sliders[4], "Poison Duration", 1.0f, 10.0f);
        SetSlider(sliders[5], "Crit Multiplier", 1.0f, 3.0f);
        SetSlider(sliders[6], "Crit Chance", 0.0f, 50.0f);
        SetSlider(sliders[7], "Life Steal", 0.0f, 100.0f);
        SetSlider(sliders[8], "Extra Armor Damage", 0.0f, 3.0f, true);
        SetSlider(sliders[9], "Armor Pierce", 0.0f, 50.0f);
        SetSlider(sliders[10], "Trample", 0.0f, 10.0f, true);
        SetSlider(sliders[11], "ManaCost", 10.0f, 100.0f);
        SetSlider(sliders[12], "Duration", 1.5f, 10.0f);
        SetSlider(sliders[13], "Size", 1.0f, 100.0f);
        SetSlider(sliders[14], "Number", 1.0f, 5.0f, true);
        SetSlider(sliders[15], "Damage", 0.0f, 200.0f, true);

	}
	
	// Update is called once per frame
	void Update () {

        ConditionalSliders(sliders[1], (sliders[0].slider.value>sliders[0].slider.minValue));
        ConditionalSliders(sliders[4], (sliders[3].slider.value > sliders[3].slider.minValue));
        ConditionalSliders(sliders[6], (sliders[5].slider.value > sliders[5].slider.minValue));
        ConditionalSliders(sliders[12], (typeDropdown.value == (int)SpellType.Trap));
        ConditionalSliders(sliders[14], (typeDropdown.value == (int)SpellType.Shot));
        ConditionalSliders(sliders[13], (typeDropdown.value == (int)SpellType.Wave) || (typeDropdown.value == (int)SpellType.Bomb) || (typeDropdown.value == (int)SpellType.Laser));

        spell.type = (SpellType)typeDropdown.value;

        spell.slowPower        = sliders[0].slider.value;
        spell.slowDuration     = sliders[1].slider.value;
        spell.snareDuration    = sliders[2].slider.value;
        spell.poisonPower      = sliders[3].slider.value;
        spell.poisonDuration   = sliders[4].slider.value;
        spell.critMultiplier   = sliders[5].slider.value;
        spell.critChance       = sliders[6].slider.value;
        spell.lifeSteal        = sliders[7].slider.value;
        spell.extraArmorDamage = (int)sliders[8].slider.value;
        spell.armorPierce      = sliders[9].slider.value;
        spell.trample          = (int)sliders[10].slider.value;
        spell.manaCost         = (int)sliders[11].slider.value;
        spell.duration         = sliders[12].slider.value;
        spell.area             = sliders[13].slider.value;
        spell.number           = (int)sliders[14].slider.value;
        spell.damage           = sliders[15].slider.value;

       

    }

    void ConditionalSliders(SpellSliderScript spellSliderScript, bool condicao)
    {       
        spellSliderScript.slider.interactable = condicao;   
    }    

    void SetDropdown()
    {

        while (typeDropdown.options.Count < System.Enum.GetNames(typeof(SpellType)).Length)
        {
            typeDropdown.options.Add(new Dropdown.OptionData());

            if(typeDropdown.options.Count >= 20)
            {
                Debug.Break();
            }

        }
        
        for (int i=0; i < System.Enum.GetNames(typeof(SpellType)).Length; i++)
        {            
            typeDropdown.options[i].text = System.Enum.GetNames(typeof(SpellType))[i];
        }
    }

    void SetSlider(SpellSliderScript slider, string name, float min, float max)
    {
        slider.nameText.text = name;
        slider.slider.minValue = min;
        slider.slider.maxValue = max;
    }
    void SetSlider(SpellSliderScript slider, string name, float min, float max, bool isWholeNumbers)
    {
        slider.nameText.text = name;
        slider.slider.minValue = min;
        slider.slider.maxValue = max;
        slider.slider.wholeNumbers = isWholeNumbers;
    }

    public void BuildSpell()
    {
        GameObject completeSpell;
        SpellData newData;
                
        switch (spell.type)
        {
            case SpellType.Bomb:
                completeSpell = basicBomb;
                completeSpell.GetComponent<SpellBehaviourWave>().stats = spell;
                break;

            case SpellType.Dash:
                completeSpell = basicDash;
                completeSpell.GetComponent<SpellBehaviourDash>().stats = spell;
                break;

            case SpellType.Laser:
                completeSpell = basicLaser;
                completeSpell.GetComponent<SpellBehaviourLaser>().stats = spell;
                break;

            case SpellType.Shot:
                completeSpell = basicShot;
                completeSpell.GetComponent<SpellBehaviourShot>().stats = spell;
                break;

            case SpellType.Teleport:
                completeSpell = basicTeleport;
                completeSpell.GetComponent<SpellBehaviourWave>().stats = spell;
                break;

            case SpellType.Trap:
                completeSpell = basicTrap;
                completeSpell.GetComponent<SpellBehaviourTrap>().stats = spell;
                break;

            case SpellType.Wave:
                completeSpell = basicWave;
                completeSpell.GetComponent<SpellBehaviourWave>().stats = spell;
                break;

            default:
                completeSpell = null;
                break;
        }

        newData = new SpellData();
        newData.spellShell = completeSpell;
        newData.spellStatistics = spell;

        spellHolder.GetComponent<HolderObjectScript>().spellData.Add(newData);

    }

}
