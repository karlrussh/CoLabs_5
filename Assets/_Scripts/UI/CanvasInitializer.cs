using UnityEngine;

public class CanvasInitializer : MonoBehaviour
{
    private Canvas _canvas;

    private void OnEnable()
    {
        _canvas =  GetComponent<Canvas>();
        _canvas.worldCamera = Camera.main;
    }
}
