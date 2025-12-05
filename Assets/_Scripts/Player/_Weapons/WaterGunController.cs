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
        Vector3 mouse = Input.mousePosition;

        Vector3 mouseWorldPos =
            Camera.main.ScreenToWorldPoint(new Vector3(mouse.x, mouse.y,
                Mathf.Abs(Camera.main.transform.position.z)));
        
        mouseWorldPos.z = 0f;
        
        Vector3 direction = (mouseWorldPos - _startPos.position).normalized;
        
        float maxDist = Vector3.Distance(_startPos.position, mouseWorldPos);
        
        if (Physics.Raycast(_startPos.position, direction, out RaycastHit hit, maxDist, _aimMask))
        {
            _waterParticles.transform.rotation = Quaternion.LookRotation(direction);
            
            var enemyController = hit.transform.gameObject.GetComponent<EnemyController>();
            enemyController.DamageEnemy(bulletDamage);
        }

        Debug.DrawRay(_startPos.position, direction * maxDist, Color.red);
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