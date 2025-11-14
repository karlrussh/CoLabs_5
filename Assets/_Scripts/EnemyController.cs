using Mono.Cecil;
using Unity.Mathematics;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private GameObject target;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private int numDemons;
    [SerializeField] private GameObject typeOfDemons;
    [SerializeField] private float rangeOfSight, attackDistance;
    [SerializeField] private float HP, Speed;
    private bool targetFound;
    private Vector3 spawnPos;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.position.x + rangeOfSight > target.transform.position.x && target.transform.position.x > transform.position.x - rangeOfSight)
        {
            targetFound = true;
        }
        if(HP <= 0)
        {
            die();
        }
    }

    public void die()
    {
        if(numDemons <= 0)
        {
        }
        else
        {
            for (int i = 0; i < numDemons; i++)
            {
                RandomPos();
                Instantiate(typeOfDemons, spawnPos, quaternion.EulerXYZ(0, 0, 0));
            }
        }
        Destroy(gameObject);
    }



    private void FixedUpdate()
    {
        if(targetFound)
        {
            if(target.transform.position.x + attackDistance < transform.position.x)
            {
                rb.linearVelocity = new Vector3(-Speed, 0);
            }
            else if(target.transform.position.x - attackDistance > transform.position.x)
            {
                rb.linearVelocity = new Vector3(Speed, 0);
            }
            else
            {
                Attack();
            }
        }
    }

    private void Attack()
    {
        Debug.Log("Attack");
    }

    private void RandomPos()
    {
        spawnPos = new Vector3(transform.position.x + UnityEngine.Random.Range(-2, 2), transform.position.y + 1, transform.position.z + UnityEngine.Random.Range(-2, 2)/2);
    }
    public void testKill()
    {
        HP = 0;
    }
}
