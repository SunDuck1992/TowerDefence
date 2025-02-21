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
    [SerializeField] private ParticleSystem _healParticle;
    [SerializeField] private ParticleSystem _shootParticle;
    [SerializeField] private Transform _healParticalPoint;

    private float _improveDamage = 0.3f;
    private float _currentDamage;
  
    public float ShootDistance => _shootDistance;
    public Transform TransformTower => _transformTower;
    public Transform ShotPoint => _shotPoint;
    public BuildArea BuildArea { get; set; }
    public float Damage => _currentDamage;
    public float FireRate => _fireRate;

    public IStateMachine StateMachine { get; private set; }
    public TargetController TargetController { get; set; }
    public BulletPool BulletPool { get; set; }
    public RocketPool RocketPool { get; set; }

    protected override void Awake()
    {
        StateMachine = new StateMachine();
        base.Awake();
        _currentDamage = _damage;
    }

    private void Update()
    {
       StateMachine.UpdateState();
    }

    public virtual void Enable()
    {
    }

    public void Disable()
    {
    }

    public virtual void Die()
    {
    }

    public void ImproveDamage(int level)
    {
        _currentDamage = _damage + (level * _improveDamage);
    }

    public void EnableHealParticle()
    {
        ParticleSystem particle = Instantiate(_healParticle, _healParticalPoint.position, Quaternion.identity);
        particle.transform.localScale = _healParticalPoint.localScale;
        particle.transform.rotation = _healParticalPoint.rotation;
        StartCoroutine(DestroyParticleAfterDelay(particle, 3f));
    }

    public void CreateShootparticle()
    {
        Instantiate(_shootParticle, ShotPoint.position, Quaternion.LookRotation(ShotPoint.forward));
    }

    public IEnumerator DestroyParticleAfterDelay(ParticleSystem particle, float delay)
    {       
        yield return new WaitForSeconds(delay);

        Destroy(particle.gameObject);
    }
}
