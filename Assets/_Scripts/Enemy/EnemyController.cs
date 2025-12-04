using System;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

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
    [SerializeField] private float healthGainMultiplier = 15f; 

    private bool targetFound;
    private Vector3 spawnPos;
    
    private EnemyState _enemyState;
    private static event Action<EnemyState> OnEnemyStateChanged;

    [SerializeField] private GameObject enemyDemon;
    [SerializeField] private GameObject enemyCleansed;
    [SerializeField] private Slider healthSlider;
    [SerializeField] private TextMeshProUGUI statusText;
    
    private bool _cleansed;
    
    private void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player");
        
        UpdateEnemyState(EnemyState.Demon);
        _currentHp = maxHp;

        if (!healthSlider) return;
        healthSlider.maxValue = maxHp;
        healthSlider.value = maxHp;
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

            if (!healthSlider) return;
            healthSlider.gameObject.SetActive(false);
            
            statusText.SetText("cleansed");
        }
        else
        {
            statusText.SetText("possessed");
        }
    }

    private void Update()
    {
        if(transform.position.x + rangeOfSight > target.transform.position.x && target.transform.position.x > transform.position.x - rangeOfSight)
        {
            if (_cleansed) return;
            targetFound = true;
        }

        AddHealth(healthGainMultiplier);
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
        
        if (_currentHp <= 0f)
        {
            _currentHp = 0f;
            Die();
        }
        if (!healthSlider) return;
        healthSlider.value = _currentHp;
    }

    private void AddHealth(float multiplier)
    {
        _currentHp += Time.deltaTime * multiplier;

        if (_currentHp > maxHp)
        {
            _currentHp = maxHp;
        }
        if (!healthSlider) return;
        healthSlider.value = _currentHp;
    }
    
    private void Die()
    {
        if (_cleansed) return;
        UpdateEnemyState(EnemyState.Cleansed);

        if (numDemons <= 0) return;
        
        if (numDemons > 0)
        {
            for (int i = 0; i < numDemons; i++)
            {
                Instantiate(typeOfDemons, transform.position, quaternion.EulerXYZ(0, 0, 0));
            }
        }
    }
    
    #endregion
}