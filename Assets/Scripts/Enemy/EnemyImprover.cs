using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyImprover 
{
    private float _health;
    private float _damage;

    private float _healthValue = 2f;
    private float _damageValue = 0.3f;

    public float Health => _health;
    public float Damage => _damage;

    public void Improve()
    {
        _health += _healthValue;
        _damage += _damageValue;
    }
}
