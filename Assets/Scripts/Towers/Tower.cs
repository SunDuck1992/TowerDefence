using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Zenject;

public abstract class Tower : GameUnit, IStateMachineOwner
{
    [SerializeField] private Transform _transformTower;
    [SerializeField] private Transform _shotPoint;
    [SerializeField] private float _damage;
    [SerializeField] private float _fireRate;
    [SerializeField] private float _shootDistance;
  
    public float ShootDistance => _shootDistance;
    public Transform TransformTower => _transformTower;
    public Transform ShotPoint => _shotPoint;
    public float Damage => _damage;
    public float FireRate => _fireRate;

    public IStateMachine StateMachine { get; private set; }
    public TargetController TargetController { get; set; }
    public BulletPool BulletPool { get; set; }

    protected override void Awake()
    {
        StateMachine = new StateMachine();
        base.Awake();
    }

    private void Update()
    {
       StateMachine.UpdateState();
    }

    public virtual void Enable()
    {
        //StateMachine.SwitchState<IdleTowerState, MashineGunTower>(this);
    }

    public void Disable()
    {

    }

    public virtual void Die()
    {
        //StateMachine.SwitchState<DieTowerState, Tower>(this);
    }
}
