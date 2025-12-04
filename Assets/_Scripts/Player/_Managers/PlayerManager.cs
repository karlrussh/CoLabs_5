using System;
using Unity.VisualScripting;
using UnityEngine;

public enum PlayerState
{ 
    InGameOver,
    InDialogue,
    InCutscene,
    InThirdPerson,
    InFirstPerson,
    InUI
}

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance;

    public PlayerState State;
    public static event Action<PlayerState> OnPlayerStateChanged;

    void Awake() => Instance = this;
    
    private void OnEnable()
    {
        ControlsManager.OnAimStart += EnterFirstPerson;
        ControlsManager.OnAimStop += EnterThirdPerson;
    }

    private void OnDisable()
    {
        ControlsManager.OnAimStart -= EnterFirstPerson;
        ControlsManager.OnAimStop -= EnterThirdPerson;
    }

    public void UpdatePlayerState(PlayerState newState)
    {
        Debug.Log($"UpdatePlayerState: {State} > {newState}");

        State = newState;

        switch (newState)
        {
            case PlayerState.InGameOver:
                HandleInGameOver();
                break;
            case PlayerState.InDialogue:
                HandleInDialogue();
                break;
            case PlayerState.InCutscene:
                HandleInCutscene();
                break;
            case PlayerState.InThirdPerson:
                HandleInThirdPerson();
                break;
            case PlayerState.InFirstPerson:
                HandleInFirstPerson();
                break;
            case PlayerState.InUI:
                HandleInUI();
                break;
        }

        OnPlayerStateChanged?.Invoke(newState);
    }

    private void HandleInGameOver()
    {
        throw new NotImplementedException();
    }

    private void HandleInDialogue()
    {
        throw new NotImplementedException();
    }

    private void HandleInCutscene()
    {
        throw new NotImplementedException();
    }

    private void HandleInThirdPerson()
    {

    }

    private void EnterThirdPerson()
    {
        UpdatePlayerState(PlayerState.InThirdPerson);
    }

    private void HandleInFirstPerson()
    {

    }

    private void EnterFirstPerson()
    {
        UpdatePlayerState(PlayerState.InFirstPerson);
    }

    private void HandleInUI()
    {
        throw new NotImplementedException();
    }
}
