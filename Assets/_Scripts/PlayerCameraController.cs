using UnityEngine;
using DG.Tweening;
using Unity.Cinemachine;
using System.Collections;

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
    [SerializeField] private float FirstPersonTargetOffsetX;
    [SerializeField] private float ThirdPersonTargetOffsetX;
    [SerializeField] private Vector3 lapThirdPersonRotate;
    [SerializeField] private Vector3 lapFirstPersonRotate;

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
                MoveCameraTo(ThirdPersonCamPos, ThirdPersonTargetOffsetX, lapThirdPersonRotate);
                break;
            case PlayerState.InFirstPerson:
                MoveCameraTo(FirstPersonCamPos, FirstPersonTargetOffsetX, lapFirstPersonRotate);
                break;
        }
    }

    private void MoveCameraTo(float _moveTo, float _targetOffsetX, Vector3 _lapRotation)
    {
        StopAllCoroutines();
        StartCoroutine(TweenMovement(_moveTo, _targetOffsetX, _lapRotation));
    }

    private IEnumerator TweenMovement(float _moveToTween, float _targetOffsetXTween, Vector3 _lapRotationTween)
    {
        DOTween.To(() => _cinemachinePositionComposer.CameraDistance, x => _cinemachinePositionComposer.CameraDistance = x, _moveToTween, TransitionTime);
        
        _lookAtPoint.transform.DOLocalRotate(_lapRotationTween, TransitionTime);

        DOTween.To(() => _cinemachinePositionComposer.TargetOffset.x, x => _cinemachinePositionComposer.TargetOffset.x = x, _targetOffsetXTween, TransitionTime);

        yield return null;
    }
}
