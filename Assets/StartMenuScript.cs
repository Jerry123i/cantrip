using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartMenuScript : MonoBehaviour {

    public Text title;
    public Text description;

    public void SelectType()
    {
        int i;
        EnemyGroup enemyGroup;

        i = Random.Range(0, System.Enum.GetNames(typeof(EnemyGroup)).Length);
        i = 1 << i;
        enemyGroup = (EnemyGroup)i;
        title.text = enemyGroup.ToString();

        switch (enemyGroup)
        {
            case EnemyGroup.AETHER:
                description.text = "Atacam rápido e a distância";
                break;

            case EnemyGroup.AIR:
                description.text = "Se movimentam rápido e atacam em grandes números";
                break;

            case EnemyGroup.EARTH:
                description.text = "Armadura e HP altos";
                break;

            case EnemyGroup.FIRE:
                description.text = "Atacam a distância";
                break;

            case EnemyGroup.WATER:
                description.text = "Mudam de coportamento quando tomam dano";
                break;

            default:
                Debug.Log("Default");
                break;
        }

        PlayerPrefs.SetInt("EnemyGroup",i);
        
    }

    public void GoToMenu()
    {
        SceneManager.LoadScene("spellMenu");
    }
    	
}
