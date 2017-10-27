using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SpellChoiceSliderScript : MonoBehaviour {

    public GameObject canvas1;
    public GameObject canvas2;
    
    public Button goButton;

    public Slider slider;

    private void Start()
    {
        slider = this.GetComponent<Slider>();
        slider.onValueChanged.AddListener(delegate { ValueChange(); });
    }
    
    public void ValueChange()
    {
        if(slider.value == 1)
        {
            canvas1.SetActive(true);
            canvas2.SetActive(false);
        }
        else
        {
            canvas1.SetActive(false);
            canvas2.SetActive(true);
        }
    }

    public void GoToGame()
    {
        SceneManager.LoadScene("pathfindingTest");
    }


}
