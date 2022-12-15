using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerBattle : MonoBehaviour
{
    public GameObject pokeBall;
    public GameObject[] listPokemon;
    public Animator through;
    public GameObject pokemonBattle;
    public GameObject flash;
    
    public GameObject hpUI;
    public GameObject attackUI;

    public PathType pathSystem = PathType.CatmullRom;
    public Vector3[] pathVal = new Vector3[2];
    private static readonly int IsThroughing = Animator.StringToHash("isThroughing");

    void Start()
    {
        through.SetBool(IsThroughing, false);
        if(CompareTag("Opponent"))
        {
            ChoosePokemon(0);
        }
    }

    void Update()
    {
        
    }

    public void ChoosePokemon(int x)
    {
        pokeBall.SetActive(true);
        through.SetBool(IsThroughing, true);
        pokeBall.transform.DOPath(pathVal, 1f, pathSystem).OnComplete(() =>
        {
            flash.SetActive(true);
            hpUI.SetActive(true);
            pokeBall.SetActive(false);
            through.SetBool(IsThroughing, false);
            GameObject pokemonChoosen = Instantiate(listPokemon[x], pokemonBattle.transform, false);
            pokemonBattle.GetComponent<PokemonBattle>().animAttack = pokemonChoosen.GetComponent<Animator>();
            if(CompareTag("Opponent"))
            {
                return;
            }
            DOVirtual.DelayedCall(1f, () =>
            {
                attackUI.SetActive(true);
            });
        });
    }
}
