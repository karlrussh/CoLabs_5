using System;
using System.Collections;
using UnityEngine;
using DG.Tweening;
using Unity.Collections;

public class CleanseCrosshairManager : MonoBehaviour
{
    [SerializeField] GameObject Crosshair;
    [SerializeField] RectTransform _crosshairVertical;
    [SerializeField] float _cleanseCrosshairVerticalHeight = 5020f;
    [SerializeField] float _normalCrosshairVerticalHeight = 100f;
    [SerializeField] RectTransform _crosshairHorizontal;
    [SerializeField] float _cleanseCrosshairHorizontalHeight = 5020f;
    [SerializeField] float _normalCrosshairHorizontalHeight = 100f;

    void OnEnable()
    {
        PlayerManager.OnPlayerStateChanged += HandlePlayerStateChanged;
    }

    void OnDisable()
    {
        PlayerManager.OnPlayerStateChanged -= HandlePlayerStateChanged;
    }

    private void HandlePlayerStateChanged(PlayerState state)
    {
        switch (state)
        {
            case PlayerState.InFirstPerson:
                CrosshairHeight(_cleanseCrosshairHorizontalHeight, _cleanseCrosshairVerticalHeight);
                break;
            case PlayerState.InThirdPerson:
                CrosshairHeight(_normalCrosshairHorizontalHeight, _normalCrosshairVerticalHeight);
                break;
        }
    }

    private void CrosshairHeight(float h_Height, float v_Height)
    {
        StopAllCoroutines();
        StartCoroutine(StartCrosshairHeightChange(h_Height, v_Height));
    }

    private IEnumerator StartCrosshairHeightChange(float h_Height, float v_Height)
    {
        DOTween.To(() => _crosshairHorizontal.sizeDelta, x => _crosshairHorizontal.sizeDelta = x, new Vector2(5f, h_Height), 1f);
        DOTween.To(() => _crosshairVertical.sizeDelta, x => _crosshairVertical.sizeDelta = x, new Vector2(5f, v_Height), 1f);
        
        yield return null;
    }

    void Update()
    {
        Crosshair.transform.position = Input.mousePosition;
    }
}