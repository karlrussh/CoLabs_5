using UnityEngine;

public class BaseDestructObject : MonoBehaviour, IShootable
{
    [SerializeField] private int hitPoints = 1;
    [SerializeField] private GameObject pickup;

    public void DropPickup()
    {
        Debug.Log("Dropping: " +  pickup);
        
        GameObject pickupInstance = Instantiate(pickup, transform.position, Quaternion.identity);
       // pickupInstance.transform.position = this.transform.position;

    }

    private void DestroyObject()
    {
        Destroy(gameObject);
    }

    public void HandleOnBulletHit()
    {
        hitPoints -= 1;
        Debug.Log("Worked!");
        if (hitPoints <= 0)
        {
            DropPickup();
            DestroyObject();
        }
    }
}
