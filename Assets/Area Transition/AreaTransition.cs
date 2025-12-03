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
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        
        StartCoroutine(Transition());
    }

    IEnumerator Transition()
    {
        Debug.Log("lol");
        animator.SetBool("transitionTriggered?", true);
        yield return new WaitForSeconds(2f);
        player.transform.position = teleportLocation;
        yield return new WaitForSeconds(2f);
        animator.SetBool("isFinished?", true);
        animator.SetBool("transitionTriggered?", false);
        yield return null;
        animator.SetBool("isFinished?", false);
    }


}
