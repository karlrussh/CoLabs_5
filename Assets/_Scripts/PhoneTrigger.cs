using UnityEngine;

public class PhoneTrigger : MonoBehaviour
{
    [SerializeField] PhoneScript phone;
    [SerializeField] PhoneSO call;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(UnityEngine.Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            
        }
    }
}
