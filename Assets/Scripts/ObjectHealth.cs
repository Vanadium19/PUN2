using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class ObjectHealth : MonoBehaviour
{
    [SerializeField] private float _maxHealth = 100f;
    
    private float _health;

    public float MaxHealth => _maxHealth;

    public event UnityAction<float> HealthChanged;

    public void TakeDamage(float damage)
    {
        _health -= damage;

        if (_health <= 0)
        {
            _health = 0;
            Die();
        }

        HealthChanged?.Invoke(_health);
    }

    protected void SetMaxHealth()
    {
        _health = _maxHealth;
    }

    protected abstract void Die();
}
