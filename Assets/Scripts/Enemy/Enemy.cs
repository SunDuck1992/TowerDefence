using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.AI;
using Zenject;
using static UnityEngine.UI.GridLayoutGroup;

public class Enemy : GameUnit, IStateMachineOwner
{
    [SerializeField] private NavMeshAgent _agent;
    [SerializeField] private float _speed;
    [SerializeField] private Animator _animator;
    [SerializeField] private AnimationEventListener _listener;
    [SerializeField] private float _damage;
    [SerializeField] private float _duration;
    [SerializeField] private int _award;
    [SerializeField] private Transform _deathParticlePoint;
    [SerializeField] private Transform _hitParticlePoint;
    [SerializeField] private ParticleSystem _deathParticle;
    [SerializeField] private ParticleSystem _hitParticle;
    [SerializeField] private GameObject _freezePartical;


    public TargetController TargetController { get; set; }
    public GameUnit Target { get; set; }
    public Transform TargetAttackPoint { get; set; }
    public Transform DeathParticlePoint => _deathParticlePoint;
    public NavMeshAgent Agent => _agent;
    public Animator Animator => _animator;
    public AnimationEventListener Listener => _listener;
    public float Damage => _damage;
    public float Duration => _duration;
    public int Award => _award;

    public IStateMachine StateMachine { get; private set; }

    protected override void Awake()
    {
        StateMachine = new StateMachine();
        ChangeSpeedModifyier(1);
    }

    public void Enable()
    {
        StateMachine.SwitchState<EnemyIdleState, Enemy>(this);
        ResetHealth();
        TargetController.AddTarget(this);
        Debug.Log($"Создан моб: {this.GetInstanceID()}, Аниматор: {Animator.GetInstanceID()}");
    }

    public void ImproveCharacteristic(float health, float damage)
    {
        _maxHealth += health;
        _damage += damage;
    }

    private void Update()
    {
        StateMachine.UpdateState();
    }

    public void Die()
    {
        StateMachine.SwitchState<EnemyDieState, Enemy>(this);
    }

    public void ChangeSpeedModifyier(float value)
    {
        _agent.speed = value * _speed;
    }

    public void CreateDeathParticle()
    {
        var patricle = Instantiate(_deathParticle, _deathParticlePoint.position, Quaternion.identity);
    }

    public void CreateHitParticle()
    {
        Instantiate(_hitParticle, _hitParticlePoint.position, Quaternion.LookRotation(_hitParticlePoint.forward));
    }

    public void SwitchFreezePartical(bool value)
    {
        _freezePartical.SetActive(value);
    }
}
