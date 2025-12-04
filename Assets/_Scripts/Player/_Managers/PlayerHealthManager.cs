using System;
using UnityEngine;

public class PlayerHealthManager : MonoBehaviour
{
    public static PlayerHealthManager Instance;

    private void Awake() => Instance = this;
    
    public int PlayerHealth { get; private set; }
    [SerializeField] public int MaxHealth { get; private set; } = 100;
    
    [SerializeField] private float afterDamageTimer = 2f;
    private float _realAfterDamageTimer;
    
    private bool _beenDamaged;
    private bool _startPlayerHeal;
    
    private void Start()
    {
        PlayerHealth = MaxHealth;
        _realAfterDamageTimer = afterDamageTimer;
    }

    private void FixedUpdate()
    {
        if (_beenDamaged)
        {
            _realAfterDamageTimer -= Time.deltaTime;
        }
        if (_realAfterDamageTimer <= 0)
        {
            _beenDamaged = false;
            _startPlayerHeal = true;
        }

        if (_startPlayerHeal)
        {
            AddHealth(1);
        }
        
        if (PlayerHealth == MaxHealth || _beenDamaged)
        {
            _startPlayerHeal = false;
        }
    }

    public void AddHealth(int amount)
    {
        PlayerHealth += amount;
        if (PlayerHealth >  MaxHealth)
        {
            PlayerHealth = MaxHealth;
        }
    }

    public void RemoveHealth(int amount)
    {
        PlayerHealth -= amount;
        
        _beenDamaged = true;
        _realAfterDamageTimer = afterDamageTimer;
        
        if (PlayerHealth <= 0)
        {
            PlayerHealth = 0;
            Die();
        }
    }

    private void Die()
    {
        PlayerManager.Instance.UpdatePlayerState(PlayerState.InGameOver);
    }
}
