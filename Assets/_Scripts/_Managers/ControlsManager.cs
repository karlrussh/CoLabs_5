using UnityEngine;
using System;
using UnityEngine.InputSystem;

public class ControlsManager : MonoBehaviour
{
    public static ControlsManager Instance;

    public bool IsReloading { get; private set; }
    public bool IsAiming { get; private set; }

    public static event Action OnShootRequested;
    public static event Action OnCleanseShootRequested;

    public static event Action OnAimStart;
    public static event Action OnAimStop;

    public static event Action OnPlayerJump;

    void Awake() => Instance = this;

    private void OnEnable()
    {
        GameManager.OnGameStateChanged += HandleGameStateChanged;
    }

    private void OnDisable()
    {
        GameManager.OnGameStateChanged -= HandleGameStateChanged;
    }

    private void HandleGameStateChanged(GameState state)
    {
        // Disables player input when needed
        switch (state)
        {
            case GameState.Cutscene:
                enabled = false;
                break;
            case GameState.Dialogue:
                enabled = false;
                break;
            case GameState.GameStarted:
                enabled = true;
                break;   
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !IsAiming && !IsReloading)
        {
            OnShootRequested?.Invoke();

            Debug.Log("Shoot normal");
        }

        if (Input.GetMouseButtonDown(0) && IsAiming && !IsReloading)
        {
            OnCleanseShootRequested?.Invoke();

            Debug.Log("Shoot cleanse");
        }

        if (Input.GetMouseButtonDown(1) && !IsReloading)
        {
            SetAim(true);
            OnAimStart?.Invoke();

            //Debug.Log("Aiming");
        }
        if (Input.GetMouseButtonUp(1))
        {
            SetAim(false);
            OnAimStop?.Invoke();

            //Debug.Log("Stopped Aiming");
        }

        if (Input.GetButtonDown("Jump"))
        {
            OnPlayerJump?.Invoke();
        }
    }

    public void SetReloading(bool reloading)
    {
        IsReloading = reloading;
    }

    public void SetAim(bool aiming)
    {
        IsAiming = aiming;
    }
}
