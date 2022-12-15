using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;

public class GameController : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera mainCam;
    [SerializeField] private CinemachineVirtualCamera playerCam;
    [SerializeField] private CinemachineVirtualCamera opponentCam;
    [SerializeField] private CinemachineVirtualCamera skillCam;
    private bool isPlayer = false;
    private bool isOpponent = false;
    private bool isSkill = false;

    public PokemonBattle playerPokemon;
    public PokemonBattle opponentPokemon;
    
    public GameObject btn;
    public GameObject attackUi;
    public GameObject waitingUi;
    public GameObject toStoreUi;
    
    public PlayerBattle playerBattle;

    private int toStore = 0;

    private void Update()
    {
        if (toStore == 1)
        {
            toStoreUi.SetActive(true);
        }
    }
    
    private void OnEnable()
    {
        CinemachineVCam.Register(mainCam);
        CinemachineVCam.Register(playerCam);
        CinemachineVCam.Register(opponentCam);
        CinemachineVCam.Register(skillCam);
    }

    private void OnDisable()
    {
        CinemachineVCam.Unregister(mainCam);
        CinemachineVCam.Unregister(playerCam);
        CinemachineVCam.Unregister(opponentCam);
        CinemachineVCam.Unregister(skillCam);
    }

    public void PlayerTurn()
    {
        Attack.instance.arrow1.Kill();
        Attack.instance.arrow2.Kill();
        Attack.instance.GetComponent<RectTransform>().localPosition = new Vector3(-143, -13.4f, 0);
        btn.SetActive(false);
        attackUi.SetActive(false);
        playerPokemon.Attacking();
        switch (playerPokemon.extraDame)
        {
            case 1 or 2:
                CinemachineVCam.SwitchCamera(playerCam);
                break;
            case 3:
                CinemachineVCam.SwitchCamera(skillCam);
                break;
        }
        StartCoroutine(OpponentTurn());
    }

    // ReSharper disable Unity.PerformanceAnalysis
    IEnumerator OpponentTurn()
    {
        yield return new WaitForSeconds(2);
        if (playerPokemon.extraDame == 3)
        {
            yield return new WaitForSeconds(2);
        }
        if (!CinemachineVCam.IsActiveCamera(mainCam))
        {
            CinemachineVCam.SwitchCamera(mainCam);
        }
        waitingUi.SetActive(true);
        yield return new WaitForSeconds(2);
        isOpponent = true;
        waitingUi.SetActive(false);
        opponentPokemon.Attacking();
        switch (opponentPokemon.extraDame)
        {
            case 1 or 2:
                CinemachineVCam.SwitchCamera(opponentCam);
                break;
            case 3:
                CinemachineVCam.SwitchCamera(skillCam);
                break;
        }
        yield return new WaitForSeconds(2);
        if (opponentPokemon.extraDame == 3)
        {
            yield return new WaitForSeconds(2);
        }
        if (!CinemachineVCam.IsActiveCamera(mainCam))
        {
            CinemachineVCam.SwitchCamera(mainCam);
        }
        isOpponent = false;
        isSkill = false;
        attackUi.SetActive(true);
        btn.SetActive(true);
        Attack.instance.Move();
        toStore += 1;
    }
    
    public void GotoStore()
    {
        Luna.Unity.LifeCycle.GameEnded();
        Invoke(nameof(ToCta), 0.1f);
    }

    public void ToCta()
    {
        Luna.Unity.Playable.InstallFullGame();
    }
}
