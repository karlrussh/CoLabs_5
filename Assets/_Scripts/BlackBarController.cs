using System;
using System.Collections;
using DG.Tweening;
using NUnit.Framework.Constraints;
using UnityEngine;

public class BlackBarController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private RectTransform TopBar;
    [SerializeField] private RectTransform BottomBar;

    [Header("Position Settings")]
    [SerializeField] private float ThirdPersonBlackBarPos;
    [SerializeField] private float FirstPersonBlackBarPos;

    [SerializeField] private float TopBarOffsetY;

    [SerializeField] private float TransitionTime;

    private void OnEnable()
    {
        PlayerManager.OnPlayerStateChanged += HandlePlayerStateChange;
    }

    private void OnDisable()
    {
        PlayerManager.OnPlayerStateChanged -= HandlePlayerStateChange;
    }

    private void HandlePlayerStateChange(PlayerState state)
    {
        switch (state)
        {
            case PlayerState.InThirdPerson:
                MoveBlackBars(ThirdPersonBlackBarPos);
                break;
            case PlayerState.InFirstPerson:
                MoveBlackBars(FirstPersonBlackBarPos);
                break;
        }
    }

    private void MoveBlackBars(float _barPos)
    {
        StopAllCoroutines();
        StartCoroutine(TweenBlackBars(_barPos));
    }

    private IEnumerator TweenBlackBars(float _barPosTween)
    {
        TopBar.DOMove(new Vector2(960f, TopBarOffsetY + _barPosTween), TransitionTime, true);
        BottomBar.DOMove(new Vector2(960f, _barPosTween = _barPosTween * -1), TransitionTime, true);

        yield return null;
    }
}
