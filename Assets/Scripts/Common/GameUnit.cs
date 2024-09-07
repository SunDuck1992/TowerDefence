using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(AttackSector))]
public abstract class GameUnit : MonoBehaviour
{
    [SerializeField] protected float _maxHealth;

    private float _health;

    public float Health => _health;
    public float MaxHealth => _maxHealth;

    public AttackSector AttackSector { get; private set; }

    public UnityEvent<GameUnit> DiedComplete;
    public UnityEvent<GameUnit> DiedStart;
    public event Action<bool> HealthChanged;

    protected virtual void Awake()
    {
        AttackSector = GetComponent<AttackSector>();
    }

    private void OnEnable()
    {
        _health = _maxHealth;
    }

    public void TakeDamage(float damage)
    {
        _health -= damage;
        HealthChanged?.Invoke(false);

        if (_health <= 0)
        {
            DiedStart.Invoke(this);           
        }
    }

    public void ResetHealth()
    {
        _health = _maxHealth;
        HealthChanged?.Invoke(true);
    }
}
