using System.Collections;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TransitionToLevel : MonoBehaviour
{
    [SerializeField] private Image fadeInOut;
    [SerializeField] private GameObject player;
    [SerializeField] private Animator animator;
    [SerializeField] private float transTime = 0.5f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            StartCoroutine(Transition());
        }
    }

    
    

    IEnumerator Transition()
    {
        Debug.Log("lol");
        animator.SetBool("transitionTriggered?", true);
        yield return new WaitForSeconds(transTime);
        SceneManager.LoadScene("Title Screen");
    }


}
