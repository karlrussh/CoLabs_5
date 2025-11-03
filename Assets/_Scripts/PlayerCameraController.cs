using System;
using UnityEngine;
using DG.Tweening;
using Unity.Cinemachine;
using System.Collections;

public class PlayerCameraController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private CinemachinePositionComposer _cinemachinePositionComposer;

    [Header("CameraSettings")]
    [SerializeField] private float ThirdPersonCamPos;
    [SerializeField] private float FirstPersonCamPos;
    [SerializeField] private float TransitionTime;

    private void OnEnable()
    {
        PlayerManager.OnPlayerStateChanged += HandlePlayerStateChanged;
    }

    private void OnDisable()
    {
        PlayerManager.OnPlayerStateChanged -= HandlePlayerStateChanged;
    }

    private void HandlePlayerStateChanged(PlayerState state)
    {
        switch (state)
        {
            case PlayerState.InThirdPerson:
                MoveCameraTo(ThirdPersonCamPos);
                break;
            case PlayerState.InFirstPerson:
                MoveCameraTo(FirstPersonCamPos);
                break;
        }
    }

    private void MoveCameraTo(float _moveTo)
    {
        StopAllCoroutines();
        StartCoroutine(TweenMovement(_moveTo));
    }

    private IEnumerator TweenMovement(float _moveToTween)
    {
        DOTween.To(() => _cinemachinePositionComposer.CameraDistance, x => _cinemachinePositionComposer.CameraDistance = x, _moveToTween, TransitionTime);

        yield return null;
    }
}
