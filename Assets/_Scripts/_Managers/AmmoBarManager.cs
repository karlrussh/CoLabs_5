using System;
using UnityEngine;
using UnityEngine.UI;

public class AmmoBarManager : MonoBehaviour
{
    [SerializeField] Slider _AmmoSlider;

    private void OnEnable()
    {
        ControlsManager.OnShootRequested += HandleShootRequested;
    }

    private void OnDisable()
    {
        ControlsManager.OnShootRequested -= HandleShootRequested;
    }

    private void Start()
    {
        if (_AmmoSlider == null)
        { 
            _AmmoSlider = GetComponent<Slider>();
        }

        _AmmoSlider.maxValue = AmmoManager.Instance.MaxAmmo;
        _AmmoSlider.value = AmmoManager.Instance.AmmoCount;
    }

    private void HandleShootRequested()
    {
        _AmmoSlider.value = AmmoManager.Instance.AmmoCount;
    }
}
