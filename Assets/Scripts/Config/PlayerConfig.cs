using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = nameof(PlayerConfig), menuName = "Config/" + nameof(PlayerConfig))]
public class PlayerConfig : ScriptableObject
{
    [SerializeField] private float _damage;
    [SerializeField] private float _couldown;
    [SerializeField] private float _health;

    public float Damage => _damage;
    public float Couldown => _couldown;
    public float Health => _health;
}
