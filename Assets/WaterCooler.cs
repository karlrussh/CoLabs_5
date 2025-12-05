using UnityEngine;

public class WaterCooler : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Hi");
        if (other.tag == "Player")
        {
            PickUp();
        }
    }

    private void PickUp()
    {
        AmmoManager.Instance.AddAmmo(AmmoManager.Instance.MaxAmmo);
        Destroy(gameObject);
    }


}
