using System;
using UnityEngine;

public class WaterGunController : MonoBehaviour
{
    [SerializeField] ParticleSystem _waterParticles;
    [SerializeField] Transform _startPos;

    [SerializeField] Camera _mainCamera;
    [SerializeField] LayerMask _aimMask;

    [SerializeField] float _bulletsPerShot = 1f;

    private bool _isShooting = false;

    void OnEnable()
    {
        ControlsManager.OnShootRequested += HandleShootRequested;
        ControlsManager.OnShootStopped += HandleShootStopped;

        _mainCamera = Camera.main;
    }

    void OnDisable()
    {
        ControlsManager.OnShootRequested -= HandleShootRequested;
        ControlsManager.OnShootStopped -= HandleShootStopped;
    }

    void Update()
    {
        _waterParticles.transform.position = _startPos.position;

        AimAtMouse();

        if (_isShooting)
        {
            AmmoManager.Instance.TakeAmmo(_bulletsPerShot);
        }

        if (AmmoManager.Instance.AmmoCount == 0f)
        { 
            _waterParticles.Stop();
        }
    }

    private void AimAtMouse()
    {
        Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, 1000f, _aimMask))
        {
            Vector3 dir = hit.point - _startPos.position;
            _waterParticles.transform.rotation = Quaternion.LookRotation(dir);
        }
        else
        {
            // if nothing hit, aim far into the distance
            Vector3 fallbackPoint = ray.GetPoint(100f);
            Vector3 dir = fallbackPoint - _startPos.position;
            _waterParticles.transform.rotation = Quaternion.LookRotation(dir);
        }
    }

    private void HandleShootRequested()
    {
        _waterParticles.Play();

        _isShooting = true;
    }

    private void HandleShootStopped()
    {
        _waterParticles.Stop();

        _isShooting = false;
    }
}
