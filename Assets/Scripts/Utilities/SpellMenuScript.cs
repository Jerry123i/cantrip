﻿using System.Collections;
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
        SetSlider(sliders[7], "Life Steal", 0.0f, 75.0f);
        SetSlider(sliders[8], "Extra Armor Damage", 0.0f, 3.0f, true);
        SetSlider(sliders[9], "Armor Pierce", 0.0f, 50.0f);
        SetSlider(sliders[10], "Trample", 0.0f, 10.0f, true);
        SetSlider(sliders[11], "ManaCost", 10.0f, 100.0f);
        SetSlider(sliders[12], "Duration", 1.5f, 10.0f);
        SetSlider(sliders[13], "Area", 1.0f, 100.0f);
        SetSlider(sliders[14], "Number", 1.0f, 5.0f, true);
        SetSlider(sliders[15], "Damage", 0.0f, 200.0f, true);
        SetSlider(sliders[16], "Distance", 1.75f, 9.0f);

	}
	
	// Update is called once per frame
	void Update () {

        ConditionalSliders(sliders[1], (sliders[0].slider.value>sliders[0].slider.minValue));
        ConditionalSliders(sliders[4], (sliders[3].slider.value > sliders[3].slider.minValue));
        ConditionalSliders(sliders[6], (sliders[5].slider.value > sliders[5].slider.minValue));
        ConditionalSliders(sliders[12], (typeDropdown.value == (int)SpellType.Trap));
        ConditionalSliders(sliders[14], (typeDropdown.value == (int)SpellType.Shot));
        ConditionalSliders(sliders[13], (typeDropdown.value == (int)SpellType.Wave) || (typeDropdown.value == (int)SpellType.Bomb) || (typeDropdown.value == (int)SpellType.Laser));
        ConditionalSliders(SliderByName("Distance"), (typeDropdown.value == (int)SpellType.Teleport || typeDropdown.value == (int)SpellType.Dash));

        spell.type = (SpellType)typeDropdown.value;

        spell.slowPower        = 1.0f - 0.0085f * SliderByName("Slow Power").slider.value;
        spell.slowDuration     = SliderByName("Slow Duration").slider.value;
        spell.snareDuration    = SliderByName("Snare Duration").slider.value;
        spell.poisonPower      = SliderByName("Poison Power").slider.value;
        spell.poisonDuration   = SliderByName("Poison Duration").slider.value;
        spell.critMultiplier   = SliderByName("Crit Multiplier").slider.value;
        spell.critChance       = SliderByName("Crit Chance").slider.value;
        spell.lifeSteal        = SliderByName("Life Steal").slider.value;
        spell.extraArmorDamage = (int)SliderByName("Extra Armor Damage").slider.value;
        spell.armorPierce      = SliderByName("Armor Pierce").slider.value;
        spell.trample          = (int)SliderByName("Trample").slider.value;
        spell.manaCost         = (int)SliderByName("ManaCost").slider.value;
        spell.duration         = SliderByName("Duration").slider.value;
        spell.area             = SliderByName("Area").slider.value;
        spell.number           = (int)SliderByName("Number").slider.value;
        spell.damage           = SliderByName("Damage").slider.value;
        spell.distance         = SliderByName("Distance").slider.value;

       

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

        typeDropdown.captionText.text = typeDropdown.options[typeDropdown.value].text;
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
        SpellData theData;
        GameObject completeSpell;
                
        switch (spell.type)
        {
            case SpellType.Bomb:
                completeSpell = basicBomb;
                break;

            case SpellType.Dash:
                completeSpell = basicDash;
                break;

            case SpellType.Laser:
                completeSpell = basicLaser;
                break;

            case SpellType.Shot:
                completeSpell = basicShot;
                break;

            case SpellType.Teleport:
                completeSpell = basicTeleport;
                break;

            case SpellType.Trap:
                completeSpell = basicTrap;
                break;

            case SpellType.Wave:
                completeSpell = basicWave;
                break;

            default:
                completeSpell = null;
                break;
        }

        completeSpell.GetComponent<SpellBehaviourBase>().stats = spell;

        theData = SpellData.CreateInstance<SpellData>();
        theData.spellShell = completeSpell;
        theData.spellStats = spell;

        spellHolder.GetComponent<HolderObjectScript>().spellData.Add(theData);

        Debug.Log("spell eh null?: " + spell);
        Debug.Log("completeSpell eh null?: " + completeSpell.GetComponent<SpellBehaviourBase>().stats);
    }

    public SpellSliderScript SliderByName(string name)
    {
        for(int i =0; i<sliders.Count; i++)
        {
            if(sliders[i].nameText.text == name)
            {
                return sliders[i];
            }
        }

        Debug.Log("Slider finder error: " + name + " not found");
        return null;
    }

}
