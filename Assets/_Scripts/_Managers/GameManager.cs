using System;
using UnityEngine;

public enum GameState
{
    GameOver,
    GamePaused,
    GameStarted,
    Dialogue,
    Cutscene,
    EndOfLevel,
    LevelTransition
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GameState State;
    public static event Action<GameState> OnGameStateChanged;

    private void Awake()
    {
        Instance = this;
        if (PlayerManager.Instance != null)
        {
            UpdateGameState(GameState.GameStarted); // Change later

        }
        else
        {
            Debug.LogWarning("PlayerManager null");
        }
            
    } 

    public void UpdateGameState(GameState newState)
    {
        Debug.Log($"UpdateGameState: {State} > {newState}");

        State = newState;

        switch (newState)
        {
            case GameState.GameOver:
                HandleGameOver();
                break;
            case GameState.GamePaused:
                HandleGamePaused();
                break;
            case GameState.GameStarted:
                HandleGameStarted();
                break;
            case GameState.Dialogue:
                HandleDialogue();
                break;
            case GameState.Cutscene:
                HandleCutscene();
                break;
            case GameState.EndOfLevel:
                HandleEndOfLevel();
                break;
            case GameState.LevelTransition:
                HandleLevelTransition();
                break;
        }

        OnGameStateChanged?.Invoke(newState);
    }

    private void HandleGameOver()
    {
        PlayerManager.Instance.UpdatePlayerState(PlayerState.InGameOver);
    }

    private void HandleGamePaused()
    {
        throw new NotImplementedException();
    }

    private void HandleGameStarted()
    {
        PlayerManager.Instance.UpdatePlayerState(PlayerState.InThirdPerson);
    }

    private void HandleDialogue()
    {
        PlayerManager.Instance.UpdatePlayerState(PlayerState.InDialogue);
    }

    private void HandleCutscene()
    {
        PlayerManager.Instance.UpdatePlayerState(PlayerState.InCutscene);
    }

    private void HandleEndOfLevel()
    {
        throw new NotImplementedException();
    }

    private void HandleLevelTransition()
    {
        throw new NotImplementedException();
    }
}