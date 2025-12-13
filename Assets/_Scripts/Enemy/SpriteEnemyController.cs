using UnityEngine;

public class SpriteEnemyController : MonoBehaviour
{
    [SerializeField] private GameObject target;
    [SerializeField] private Rigidbody rb;
    
    [SerializeField] private int numDemons;
    [SerializeField] private GameObject typeOfDemons;
    
    [SerializeField] private float rangeOfSight, attackDistance;

    [SerializeField] private float Speed;

    [SerializeField] private float maxHp;
    private float _currentHp;

    private bool targetFound;
    private Vector3 spawnPos;
    
    [SerializeField] private ParticleSystem deathparticles;

    private void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player");
        
        _currentHp = maxHp;
    }
    
    private void Update()
    {
        if(transform.position.x + rangeOfSight > target.transform.position.x && target.transform.position.x > transform.position.x - rangeOfSight)
        {
            targetFound = true;
        }
    }
    
    #region follow player
    
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

    #endregion
    
    #region Health

    public void DamageEnemy(float damage)
    {
        _currentHp -= damage;

        if (!(_currentHp < 0f)) return;
        _currentHp = 0f;
        die();
    }

    // ReSharper disable Unity.PerformanceAnalysis
    public void die()
    {
        Instantiate(deathparticles, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
    
    #endregion
}
