using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SliderHealth : MonoBehaviour
{
    public TextMeshProUGUI hp;
    public Slider slider;
    public PokemonBattle pokemonBattle;

    private void Update()
    {
        hp.text = pokemonBattle.currentHealth.ToString() + "/50";
    }

    public void SetMaxHealth(int health)
    {
        slider.maxValue = health;
        slider.value = health;
    }
    
    public void SetHealth(int health)
    {
        slider.value = health;
    }
}
