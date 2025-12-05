using UnityEngine;

public class Billboarding : MonoBehaviour
{
    private Transform _cameraTransform;
    
    void Start()
    {
        _cameraTransform = Camera.main.transform;
    }
    
    void LateUpdate()
    {
        transform.LookAt(2 * transform.position - _cameraTransform.position);
    }
}
