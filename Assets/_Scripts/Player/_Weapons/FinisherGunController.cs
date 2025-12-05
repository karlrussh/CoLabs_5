using System;
using UnityEngine;

public class FinisherGunController : MonoBehaviour
{
    [SerializeField] private GameObject _handStartPos;
    [SerializeField] LayerMask _layerMask;
    private float _maxDistance = 100f;

    private void OnEnable()
    {
        ControlsManager.OnCleanseShootRequested += HandleCleanseShootRequested;
    }

    private void OnDisable()
    {
        ControlsManager.OnCleanseShootRequested -= HandleCleanseShootRequested;
    }

    private void HandleCleanseShootRequested()
    {
        Ray cameraRay = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(cameraRay, out RaycastHit hit, _maxDistance, _layerMask))
        {
            Vector3 aimDirection = (hit.point - _handStartPos.transform.position).normalized;

            Debug.Log($"Hit: {hit.transform.gameObject.name}");
            Debug.DrawRay(_handStartPos.transform.position, aimDirection * _maxDistance, Color.red, 0.02f);

            if (hit.transform.gameObject.GetComponent<EnemyController>() && hit.transform.gameObject.name == "Demon(Clone)") 
            {
                Destroy(hit.transform.gameObject); // We need to setup a proper enemy health manager - this will do for now
            }
            else if (hit.transform.gameObject.GetComponent<BaseDestructObject>())
            {
                BaseDestructObject obj = hit.transform.gameObject.GetComponent<BaseDestructObject>();
                IShootable shootable = obj;
                shootable.HandleOnBulletHit();
            }
            hit.transform.gameObject.GetComponent<SpriteEnemyController>().DamageEnemy(100f);
        }
        else
        {
            Vector3 aimDirection = cameraRay.direction;
            Debug.DrawRay(_handStartPos.transform.position, aimDirection * _maxDistance, Color.red, 0.02f);
        }
    }
}
