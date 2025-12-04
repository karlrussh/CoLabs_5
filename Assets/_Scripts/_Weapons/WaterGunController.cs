using System;
using UnityEngine;

public class WaterGunController : MonoBehaviour
{
    [SerializeField] ParticleSystem _waterParticles;
    [SerializeField] ParticleSystem _waterSplashParticles;
    [SerializeField] Transform _startPos;

    [SerializeField] Camera _mainCamera;
    [SerializeField] LayerMask _aimMask;

    [SerializeField] private float bulletDamage = 1f;
    [SerializeField] private float bulletsPerShot = 1f;

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
        _waterSplashParticles.transform.position = _startPos.position;

        if (_isShooting)
        {
            AmmoManager.Instance.TakeAmmo(bulletsPerShot);
            AimAtMouse();
        }

        if (AmmoManager.Instance.AmmoCount == 0f)
        { 
            _waterParticles.Stop();
            _waterSplashParticles.Stop();
        }
    }

    private void AimAtMouse()
    {
        Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, 1000f, _aimMask))
        {
            Vector3 dir = hit.point - _startPos.position;
            _waterParticles.transform.rotation = Quaternion.LookRotation(dir);

            hit.transform.gameObject.GetComponent<EnemyController>().DamageEnemy(bulletDamage);
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
        //_waterSplashParticles.Play();

        _isShooting = true;
    }

    private void HandleShootStopped()
    {
        _waterParticles.Stop();
        //_waterSplashParticles.Stop();

        _isShooting = false;
    }
}
