using System;
using UnityEngine;
using UnityEngine.UI;

public class AmmoBarManager : MonoBehaviour
{
    [SerializeField] Slider _AmmoSlider;
    private bool _isShooting = false;

    private void OnEnable()
    {
        ControlsManager.OnShootRequested += HandleShootRequested;
        ControlsManager.OnShootStopped += HandleShootStopped;
    }

    private void OnDisable()
    {
        ControlsManager.OnShootRequested -= HandleShootRequested;
        ControlsManager.OnShootStopped -= HandleShootStopped;
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

    private void Update()
    {
       // if(_isShooting)
            _AmmoSlider.value = AmmoManager.Instance.AmmoCount;
    }

    private void HandleShootRequested()
    {
        _isShooting = true;
    }

    private void HandleShootStopped()
    {
        _isShooting = false;
    }
}
