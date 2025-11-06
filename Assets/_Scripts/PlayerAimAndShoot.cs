using UnityEngine;

public class PlayerAimAndShoot : MonoBehaviour
{

    [SerializeField] GameObject arm;
    [SerializeField] Transform bulletSpawnPoint;
    [SerializeField] GameObject bullet;
    private GameObject bulletInst;

    // Update is called once per frame
    void Update()
    {
        
    }



    public void ShootNormal()
    {
        Debug.Log(arm.transform.rotation);
        bulletInst = Instantiate(bullet, bulletSpawnPoint.position, arm.transform.rotation);
    }

}
