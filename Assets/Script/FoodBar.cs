using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FoodBar : MonoBehaviour
{
    public Slider slider;
    public Pokemon pokemon;

    public void SetMaxFood(int food)
    {
        slider.maxValue = (float)food;
        slider.value = 0;
    }
    public void IncreaseFood()
    {
        slider.value += 1;
        if (slider.value == slider.maxValue)
        {
            pokemon.Evolve();
        }
    }

}
