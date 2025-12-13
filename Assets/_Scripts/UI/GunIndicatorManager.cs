using System;
using UnityEngine;
using UnityEngine.UI;

public class GunIndicatorManager : MonoBehaviour
{
    [SerializeField] RawImage _indicator;
    [SerializeField] Texture2D _deagleGun;
    [SerializeField] Texture2D _waterGun;

    private void OnEnable()
    {
        PlayerManager.OnPlayerStateChanged += HandlePlayerStateChanged;
    }

    private void OnDisable()
    {
        PlayerManager.OnPlayerStateChanged -= HandlePlayerStateChanged;
    }

    private void HandlePlayerStateChanged(PlayerState state)
    {
        switch (state)
        {
            case PlayerState.InThirdPerson:
                _indicator.texture = _waterGun;
                break;
            case PlayerState.InFirstPerson:
                _indicator.texture = _deagleGun;
                break;
        }
    }
}
