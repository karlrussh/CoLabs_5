using UnityEngine;

public class BaseDestructObject : MonoBehaviour
{
    [SerializeField] private int hitPoints = 1;
    [SerializeField] private GameObject pickup;
    public virtual void DropPickup()
    {
        Debug.Log("Dropping: " +  pickup);
        Instantiate(pickup);
    }

    private void DestroyObject()
    {
        DropPickup();
        Destroy(gameObject);
    }

    public void HandleOnBulletHit()
    {
        if (hitPoints >= 0)
        {
            DestroyObject();
        }
        else hitPoints -= 1;

    }

}
