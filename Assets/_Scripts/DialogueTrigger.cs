using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    [SerializeField] PhoneScript phone;
    [SerializeField] PhoneSO phoneSO;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            phone.PhoneCall(phoneSO);
        }
    }
   
}
