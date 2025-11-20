using System;
using UnityEngine;

public class WaterGunController : MonoBehaviour
{
    [SerializeField] ParticleSystem _waterParticles;
    [SerializeField] Transform _startPos;

    void OnEnable()
    {
        ControlsManager.OnShootRequested += HandleShootRequested;
        ControlsManager.OnShootStopped += HandleShootStopped;
    }

    void OnDisable()
    {
        ControlsManager.OnShootRequested -= HandleShootRequested;
        ControlsManager.OnShootStopped -= HandleShootStopped;
    }

    void Update()
    {
        _waterParticles.transform.position = _startPos.position;
    }

    private void HandleShootRequested()
    {
        _waterParticles.Play();
    }

    private void HandleShootStopped()
    {
        _waterParticles.Stop();
    }
}
