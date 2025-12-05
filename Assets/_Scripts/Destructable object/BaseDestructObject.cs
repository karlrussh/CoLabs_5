using UnityEngine;

public class BaseDestructObject : MonoBehaviour, IShootable
{
    [SerializeField] private int hitPoints = 1;
    [SerializeField] private GameObject pickup;

    public virtual void DropPickup()
    {
        if (pickup = null) return;
        Debug.Log("Dropping: " +  pickup);
        GameObject pickupInstance = Instantiate(pickup/*, this.transform,true*/);
        pickupInstance.transform.position = this.transform.position;

    }

    private void DestroyObject()
    {
        DropPickup();
        Destroy(gameObject);
    }

    public void HandleOnBulletHit()
    {
        Debug.Log("Worked!");
        if (hitPoints >= 0)
        {
            DestroyObject();
        }
        else hitPoints -= 1;

    }
}
