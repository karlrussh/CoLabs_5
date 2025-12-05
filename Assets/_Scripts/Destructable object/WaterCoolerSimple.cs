using UnityEngine;

public class WaterCoolerSimple : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Untagged") return;
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
