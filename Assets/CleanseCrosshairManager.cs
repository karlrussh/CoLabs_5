using System;
using System.Collections;
using UnityEngine;
using DG.Tweening;
using Unity.Collections;
using TMPro;

public class CleanseCrosshairManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] TextMeshProUGUI _cleanserAmmoCount;
    [SerializeField] RectTransform _hitRegister;
    [SerializeField] GameObject Crosshair;
    [Header("Crosshair Vertical Bar")]
    [SerializeField] RectTransform _crosshairVertical;
    [SerializeField] float _cleanseCrosshairVerticalHeight = 5020f;
    [SerializeField] float _normalCrosshairVerticalHeight = 100f;
    [Header("Crosshair Horizontal Bar")]
    [SerializeField] RectTransform _crosshairHorizontal;
    [SerializeField] float _cleanseCrosshairHorizontalHeight = 5020f;
    [SerializeField] float _normalCrosshairHorizontalHeight = 100f;

    private void Start()
    {
        _cleanserAmmoCount.text = $"[{AmmoManager.Instance.CleanseAmmoCount}/{AmmoManager.Instance.CleanseMaxAmmo}]";
    }

    void OnEnable()
    {
        PlayerManager.OnPlayerStateChanged += HandlePlayerStateChanged;
        ControlsManager.OnCleanseShootRequested += HandleCleanseShootRequested;
    }

    void OnDisable()
    {
        PlayerManager.OnPlayerStateChanged -= HandlePlayerStateChanged;
        ControlsManager.OnCleanseShootRequested -= HandleCleanseShootRequested;
    }

    private void HandlePlayerStateChanged(PlayerState state)
    {
        switch (state)
        {
            case PlayerState.InFirstPerson:
                _cleanserAmmoCount.enabled = true;
                CrosshairHeight(_cleanseCrosshairHorizontalHeight, _cleanseCrosshairVerticalHeight);
                break;
            case PlayerState.InThirdPerson:
                _cleanserAmmoCount.enabled = false;
                CrosshairHeight(_normalCrosshairHorizontalHeight, _normalCrosshairVerticalHeight);
                break;
        }
    }
    private void HandleCleanseShootRequested()
    {
        _cleanserAmmoCount.text = $"[{AmmoManager.Instance.CleanseAmmoCount}/{AmmoManager.Instance.CleanseMaxAmmo}]";

        _hitRegister.DOPunchScale(new Vector3(15f, 17f, 0f), 0.1f, 2, 1f);
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