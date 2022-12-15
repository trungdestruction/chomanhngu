using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pokemon : MonoBehaviour
{
    public GameObject pokemon;
    public GameObject pokemonEvo;
    public Animator feed;

    public void Evolve()
    {
        pokemon.SetActive(false);
        pokemonEvo.SetActive(true);
    }
}
