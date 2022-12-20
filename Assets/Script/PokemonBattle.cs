using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Luna.Unity.FacebookInstantGames;
using Unity.VisualScripting;
using UnityEditor;

public class PokemonBattle : MonoBehaviour
{
    public GameObject target;
    
    public int maxHealth = 50;
    public int currentHealth;
    
    public int baseDame = 5;
    public int bonusDame;

    public Animator animAttack;
    public GameObject skill;
    public GameObject skillExplode;
    public GameObject punchExplode;

    public int extraDame;
    private int trueDame;
    public SliderHealth sliderHealth;

    public static readonly int Punch = Animator.StringToHash("punch");
    public static readonly int Skill = Animator.StringToHash("skill");

    void Start()
    {
        currentHealth = maxHealth;
        sliderHealth.SetMaxHealth(maxHealth);
    }

    void Update()
    {
        if (Attack.instance == null) return;
        if (CompareTag("Player"))
        {
            bonusDame = Attack.instance.dame  switch
            {
                1 => baseDame,
                2 => baseDame * 2,
                3 => baseDame * 3,
                _ => bonusDame
            };
        }
        if (CompareTag("Opponent"))
        {
            bonusDame = baseDame * extraDame;
        }
        trueDame = baseDame + bonusDame;
    }

    void TakeDamage(int damage)
    {
        currentHealth -= damage;
        
        sliderHealth.SetHealth(currentHealth);
    }
    
    // ReSharper disable Unity.PerformanceAnalysis
    public void Attacking()
    {
        if (CompareTag("Player"))
        {
            extraDame = Attack.instance.dame; 
        }
        if (CompareTag("Opponent"))
        {
            extraDame = Random.Range(1, 4);
            bonusDame = baseDame * extraDame;
        }
        
        switch (extraDame)
        {
            case 1:
                StartCoroutine(GetPunch(trueDame));
                break;
            case 2:
                StartCoroutine(GetPunch(trueDame));
                break;
            case 3:
                StartCoroutine(GetSkill(trueDame));
                break;
        }
    }

    // ReSharper disable Unity.PerformanceAnalysis
    public IEnumerator GetPunch(int damedame)
    {
        transform.DOLocalMoveZ(2f, 0.5f);
        animAttack.SetBool(Punch, true);
        yield return new WaitForSeconds(1f);
        target.GetComponent<PokemonBattle>().TakeDamage(damedame);
        Instantiate(punchExplode, transform, false);
        StartCoroutine(CinemachineVCam.Shake());
        yield return new WaitForSeconds(1f);
        animAttack.SetBool(Punch, false);
        transform.DOLocalMoveZ(0.5f, 1f);
    }

    // ReSharper disable Unity.PerformanceAnalysis
    public IEnumerator GetSkill(int damedame)
    {
        GameObject castSkill = Instantiate(skill, transform, false);
        yield return new WaitForSeconds(1f);
        animAttack.SetBool(Skill, true);
        yield return new WaitForSeconds(0.8f);
        castSkill.transform.DOLocalMoveZ(1.7f, 0.6f).OnComplete(() =>
        {
            castSkill.SetActive(false);
            Instantiate(skillExplode, transform, false);
            StartCoroutine(CinemachineVCam.Shake());
            target.GetComponent<PokemonBattle>().TakeDamage(damedame);
        });
        yield return new WaitForSeconds(2f);
        animAttack.SetBool(Skill, false);
        castSkill.SetActive(false);
    }
}

