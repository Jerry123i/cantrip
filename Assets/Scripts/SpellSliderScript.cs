using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpellSliderScript : MonoBehaviour {

    public Slider slider;
    public Text nameText;
    public Text valueText;

	// Update is called once per frame
	void Update () {        

        if(slider.IsInteractable() == false)
        {
            valueText.text = "";
            slider.value = slider.minValue;
        }
        else
        {
            valueText.text = System.String.Format("{0:F2}", slider.value);
        }

	}
}
