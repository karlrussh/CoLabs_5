using UnityEngine;

public class NunArmFollow : MonoBehaviour
{
    public Camera _mainCamera;
    public Transform _Effector;
    public float zDepth = 0f;

    private void Start()
    {
        if (_mainCamera == null)
        {
            _mainCamera = Camera.main;
        } 
    }

    void Update()
    {
        Vector3 mousePos = Input.mousePosition;
        Vector3 worldPos = _mainCamera.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, _mainCamera.transform.position.z * -1));

        worldPos.z = zDepth;

        _Effector.position = worldPos;
    }
}
