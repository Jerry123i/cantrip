using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpellMenuScript : MonoBehaviour {

    public SpellSatistics spellStats;
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

        spellStats = gameObject.AddComponent(typeof(SpellSatistics)) as SpellSatistics;

        SetDropdown();

        SetSlider(sliders[0], "Slow Power", 0.0f, 100.0f);
        SetSlider(sliders[1], "Slow Duration", 0.0f, 5.0f);        
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
        ConditionalSliders(SliderByName("Trample"), (typeDropdown.value == (int)SpellType.Shot));

        spellStats.type = (SpellType)typeDropdown.value;

        spellStats.slowPower        = 1.0f - 0.0085f * SliderByName("Slow Power").slider.value;
        spellStats.slowDuration     = SliderByName("Slow Duration").slider.value;
        spellStats.snareDuration    = SliderByName("Snare Duration").slider.value;
        spellStats.poisonPower      = SliderByName("Poison Power").slider.value;
        spellStats.poisonDuration   = SliderByName("Poison Duration").slider.value;
        spellStats.critMultiplier   = SliderByName("Crit Multiplier").slider.value;
        spellStats.critChance       = SliderByName("Crit Chance").slider.value;
        spellStats.lifeSteal        = SliderByName("Life Steal").slider.value;
        spellStats.extraArmorDamage = (int)SliderByName("Extra Armor Damage").slider.value;
        spellStats.armorPierce      = SliderByName("Armor Pierce").slider.value;
        spellStats.trample          = (int)SliderByName("Trample").slider.value;
        spellStats.manaCost         = (int)SliderByName("ManaCost").slider.value;
        spellStats.duration         = SliderByName("Duration").slider.value;
        spellStats.area             = SliderByName("Area").slider.value;
        spellStats.number           = (int)SliderByName("Number").slider.value;
        spellStats.damage           = SliderByName("Damage").slider.value;
        spellStats.distance         = SliderByName("Distance").slider.value;

       

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
        SpellData spellData;
        GameObject spellShell;
        GameObject spellPrefab;
                
        switch (spellStats.type)
        {
            case SpellType.Bomb:
                spellPrefab = basicBomb;
                break;

            case SpellType.Dash:
                spellPrefab = basicDash;
                break;

            case SpellType.Laser:
                spellPrefab = basicLaser;
                break;

            case SpellType.Shot:
                spellPrefab = basicShot;
                break;

            case SpellType.Teleport:
                spellPrefab = basicTeleport;
                break;

            case SpellType.Trap:
                spellPrefab = basicTrap;
                break;

            case SpellType.Wave:
                spellPrefab = basicWave;
                break;

            default:
                spellPrefab = null;
                break;
        }
        
        spellData = SpellData.CreateInstance<SpellData>();
        spellData.spellShell = spellPrefab;
        spellData.spellStats = spellStats;

        spellHolder.GetComponent<HolderObjectScript>().spellData.Add(spellData);
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
