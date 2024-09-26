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
    private bool _isDead;

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
        _isDead = false;
    }

    public void TakeDamage(float damage)
    {
        if(_isDead) return;

        _health -= damage;
        HealthChanged?.Invoke(false);

        if (_health <= 0)
        {
            _isDead = true;
            DiedStart.Invoke(this);
            Debug.Log("DiedStart Invoke");
            Debug.Log(_health + " - HP");
        }
    }

    public void ResetHealth()
    {
        _health = _maxHealth;
        _isDead = false;
        HealthChanged?.Invoke(true);
    }
}
