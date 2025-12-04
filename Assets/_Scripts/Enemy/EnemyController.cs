using System;
using Unity.Mathematics;
using UnityEngine;

internal enum EnemyState
{
    Demon,
    Cleansed
}
public class EnemyController : MonoBehaviour
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
    
    private EnemyState _enemyState;
    private static event Action<EnemyState> OnEnemyStateChanged;

    [SerializeField] private GameObject enemyDemon;
    [SerializeField] private GameObject enemyCleansed;

    private bool _cleansed;
    
    private void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player");
        
        UpdateEnemyState(EnemyState.Demon);
        _currentHp = maxHp;
    }
    
        
    private void UpdateEnemyState(EnemyState newState )
    {
        Debug.Log($"UpdateEnemyState: {_enemyState} > {newState}");

        _enemyState = newState;

        switch (newState)
        {
            case EnemyState.Demon:
                StateInitialized(true);
                break;
            case EnemyState.Cleansed:
                StateInitialized(false);
                break;
        }

        OnEnemyStateChanged?.Invoke(newState);
    }

    private void StateInitialized(bool state)
    {   
        enemyDemon.SetActive(state);
        enemyCleansed.SetActive(!state);
        _cleansed = !state;

        if (!state)
        {   
            transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + 1f);
        }
    }

    private void Update()
    {
        if(transform.position.x + rangeOfSight > target.transform.position.x && target.transform.position.x > transform.position.x - rangeOfSight)
        {
            if (_cleansed) return;
            targetFound = true;
        }
    }
    
    #region follow player
    
    private void FixedUpdate()
    {
        if(targetFound && !_cleansed)
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
        if (_cleansed) return;
        _currentHp -= damage;

        if (!(_currentHp < 0f)) return;
        _currentHp = 0f;
        Die();
    }

    // ReSharper disable Unity.PerformanceAnalysis
    public void Die()
    {
        if (_cleansed) return;
        UpdateEnemyState(EnemyState.Cleansed);

        if (numDemons <= 0) return;
        
        if (numDemons > 0)
        {
            for (int i = 0; i < numDemons; i++)
            {
                Instantiate(typeOfDemons, new Vector3(transform.position.x, transform.position.y, transform.position.z), quaternion.EulerXYZ(0, 0, 0));
            }
        }
    }
    
    #endregion
}