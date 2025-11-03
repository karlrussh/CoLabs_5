using System;
using UnityEngine;
using DG.Tweening;
using Unity.Cinemachine;
using System.Collections;
using Unity.Mathematics;

public class PlayerCameraController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private CinemachinePositionComposer _cinemachinePositionComposer;
    [SerializeField] private GameObject _lookAtPoint;

    [Header("CameraSettings")]
    [SerializeField] private float ThirdPersonCamPos;
    [SerializeField] private float FirstPersonCamPos;
    [SerializeField] private float TransitionTime;

    [Header("LookAtPointSettings")]
    [SerializeField] private float lapThirdPersonPos;
    [SerializeField] private float lapFirstPersonPos;
    [SerializeField] private Quaternion lapThirdPersonRotate;
    [SerializeField] private Quaternion lapFirstPersonRotate;

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
                MoveCameraTo(ThirdPersonCamPos, lapThirdPersonPos, lapThirdPersonRotate);
                break;
            case PlayerState.InFirstPerson:
                MoveCameraTo(FirstPersonCamPos, lapFirstPersonPos, lapFirstPersonRotate);
                break;
        }
    }

    private void MoveCameraTo(float _moveTo, float _lapMoveTo, Quaternion _lapRotation)
    {
        StopAllCoroutines();
        StartCoroutine(TweenMovement(_moveTo, _lapMoveTo, _lapRotation));
    }

    private IEnumerator TweenMovement(float _moveToTween, float _lapMoveToTween, Quaternion _lapRotationTween)
    {
        DOTween.To(() => _cinemachinePositionComposer.CameraDistance, x => _cinemachinePositionComposer.CameraDistance = x, _moveToTween, TransitionTime);

        _lookAtPoint.transform.DOMove(new Vector3(_lapMoveToTween, _lookAtPoint.transform.position.y, _lookAtPoint.transform.position.z), TransitionTime);
        _lookAtPoint.transform.DORotateQuaternion(_lapRotationTween, TransitionTime);

        yield return null;
    }
}
