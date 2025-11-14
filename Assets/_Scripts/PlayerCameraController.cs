using UnityEngine;
using DG.Tweening;
using Unity.Cinemachine;
using System.Collections;
using System;

public class PlayerCameraController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private CinemachinePositionComposer _cinemachinePositionComposer;
    [SerializeField] private GameObject _lookAtPoint;
    [SerializeField] private GameObject _Nun;

    [Header("CameraSettings")]
    [SerializeField] private float ThirdPersonCamPos;
    [SerializeField] private float FirstPersonCamPos;
    [SerializeField] private float TransitionTime;

    [Header("LookAtPointSettings")]
    [SerializeField] private float FirstPersonTargetOffsetX;
    [SerializeField] private float ThirdPersonTargetOffsetX;
    [SerializeField] private Vector3 lapThirdPersonRotate;
    [SerializeField] private Vector3 lapFirstPersonRotate;

    private bool _isSliding = false;

    private void OnEnable()
    {
        PlayerManager.OnPlayerStateChanged += HandlePlayerStateChanged;

        ControlsManager.OnPlayerSlide += HandleOnPlayerSlide;
        PlayerMovement.OnPlayerStopSliding += HandleOnPlayerStopSlide;
    }

    private void OnDisable()
    {
        PlayerManager.OnPlayerStateChanged -= HandlePlayerStateChanged;
        
        ControlsManager.OnPlayerSlide -= HandleOnPlayerSlide;
        PlayerMovement.OnPlayerStopSliding -= HandleOnPlayerStopSlide;
    }

    private void HandlePlayerStateChanged(PlayerState state)
    {
        switch (state)
        {
            case PlayerState.InThirdPerson:
                if (_isSliding) break;
                MoveCameraTo(ThirdPersonCamPos, ThirdPersonTargetOffsetX, lapThirdPersonRotate);
                break;

            case PlayerState.InFirstPerson:
                if (_isSliding) break;
                MoveCameraTo(FirstPersonCamPos, FirstPersonTargetOffsetX, lapFirstPersonRotate);
                break;
        }
    }

    private void HandleOnPlayerSlide()
    {
        _isSliding = true;
        MoveCameraTo(FirstPersonCamPos, FirstPersonTargetOffsetX, lapFirstPersonRotate);
    }

    private void HandleOnPlayerStopSlide()
    {
        _isSliding = false;
        MoveCameraTo(ThirdPersonCamPos, ThirdPersonTargetOffsetX, lapThirdPersonRotate);
    }

    private void MoveCameraTo(float _moveTo, float _targetOffsetX, Vector3 _lapRotation)
    {
        StopAllCoroutines();
        StartCoroutine(TweenMovement(_moveTo, _targetOffsetX, _lapRotation));
    }

    private IEnumerator TweenMovement(float _moveToTween, float _targetOffsetXTween, Vector3 _lapRotationTween)
    {
        if (_Nun.transform.localScale.x < 0)
        {
            _lapRotationTween = _lapRotationTween * -1f;
            _targetOffsetXTween = _targetOffsetXTween * -1f;
        }

        DOTween.To(() => _cinemachinePositionComposer.CameraDistance, x => _cinemachinePositionComposer.CameraDistance = x, _moveToTween, TransitionTime);
        
        _lookAtPoint.transform.DOLocalRotate(_lapRotationTween, TransitionTime);

        DOTween.To(() => _cinemachinePositionComposer.TargetOffset.x, x => _cinemachinePositionComposer.TargetOffset.x = x, _targetOffsetXTween, TransitionTime);

        yield return null;
    }
}
