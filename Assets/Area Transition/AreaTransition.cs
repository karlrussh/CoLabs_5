using System.Collections;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.UI;

public class AreaTransition : MonoBehaviour
{
    [SerializeField] private Image fadeInOut;
    [SerializeField] private Vector3 teleportLocation;
    [SerializeField] private GameObject player;
    [SerializeField] private Animator animator;
    [SerializeField] private float transTime = 0.5f;

    private void OnCollisionEnter(Collision collision)
    {
        
        StartCoroutine(Transition());
    }

    IEnumerator Transition()
    {
        Debug.Log("lol");
        animator.SetBool("transitionTriggered?", true);
        yield return new WaitForSeconds(transTime);
        player.transform.position = teleportLocation;
        yield return new WaitForSeconds(transTime);
        animator.SetBool("isFinished?", true);
        animator.SetBool("transitionTriggered?", false);
        yield return null;
        animator.SetBool("isFinished?", false);
    }


}
