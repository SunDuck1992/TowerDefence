using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

public class Enemy : GameUnit, IStateMachineOwner
{
    [SerializeField] private NavMeshAgent _agent;
    [SerializeField] private float _speed;
    [SerializeField] private Animator _animator;
    [SerializeField] private AnimationEventListener _listener;
    [SerializeField] private float _damage;
    [SerializeField] private float _duration;

    //public Player Target { get; set; }
    public TargetController TargetController { get; set; }
    public GameUnit Target {  get; set; }
    public Transform TargetAttackPoint { get; set; }
    public NavMeshAgent Agent => _agent;
    public Animator Animator => _animator;
    public AnimationEventListener Listener => _listener;
    public float Damage => _damage;
    public float Duration => _duration;

    public IStateMachine StateMachine {  get; private set; }  

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
}
