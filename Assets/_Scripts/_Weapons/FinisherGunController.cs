using System;
using UnityEngine;

public class FinisherGunController : MonoBehaviour
{
    [SerializeField] private GameObject _handStartPos;
    private bool _isAiming = false;
    private float _maxDistance = 100f;

    private void OnEnable()
    {
        ControlsManager.OnAimStart += HandleAimStart;
        ControlsManager.OnAimStop += HandleAimStop;

        ControlsManager.OnCleanseShootRequested += HandleCleanseShootRequested;
    }

    private void OnDisable()
    {
        ControlsManager.OnAimStart -= HandleAimStart;
        ControlsManager.OnAimStop -= HandleAimStop;

        ControlsManager.OnCleanseShootRequested -= HandleCleanseShootRequested;
    }

    private void HandleAimStart() => _isAiming = true;

    private void HandleAimStop() => _isAiming = false;

    private void HandleCleanseShootRequested()
    {
        Ray cameraRay = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(cameraRay, out RaycastHit hit, _maxDistance))
        {
            Vector3 aimDirection = (hit.point - _handStartPos.transform.position).normalized;

            Debug.Log($"Hit: {hit.transform.gameObject.name}");
            Debug.DrawRay(_handStartPos.transform.position, aimDirection * _maxDistance, Color.red, 0.02f);

            if (hit.transform.gameObject.GetComponent<EnemyController>() && hit.transform.gameObject.name == "Demon(Clone)") 
            {
                Destroy(hit.transform.gameObject); // We need to setup a proper enemy health manager - this will do for now
            }
        }
        else
        {
            Vector3 aimDirection = cameraRay.direction;
            Debug.DrawRay(_handStartPos.transform.position, aimDirection * _maxDistance, Color.red, 0.02f);
        }
    }
}
