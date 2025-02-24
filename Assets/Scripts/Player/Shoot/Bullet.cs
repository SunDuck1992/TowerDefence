using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject.SpaceFighter;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private ParticleSystem _hitBulletParticle;

    private Transform _targetPosition;

    public float Damage { get; set; }

    public event Action<Bullet> Died;
    public event Action<Enemy> Hit;
    public event Action<Enemy> HitTower;

    private void OnEnable()
    {
        //Invoke("SelfDestraction", 5f);
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    private void Update()
    {
        Debug.LogWarning(_targetPosition.position + " - targetpositon");

        //Vector3 direction = (_targetPosition.position - transform.position);

        transform.Translate(Vector3.forward * _speed * Time.deltaTime);

        float targetY = _targetPosition.position.y;
        float smoothY = Mathf.Lerp(transform.position.y, targetY, Time.deltaTime * _speed);
        Vector3 newPosition = transform.position;
        newPosition.y = smoothY; 
        transform.position = newPosition; 
    }

    private void OnTriggerEnter(Collider other)
    {
        Enemy enemy = other.GetComponent<Enemy>();

        if (enemy != null)
        {
            Instantiate(_hitBulletParticle, enemy.DeathParticlePoint.position, Quaternion.identity);

            Hit?.Invoke(enemy);
            HitTower?.Invoke(enemy);
            Died?.Invoke(this);
        }
    }

    public void GetTargetPosition(GameUnit target)
    {
        var enemyTarget = target as Enemy;
        _targetPosition = enemyTarget.BulletTarget;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, PlayerShooter.Radius);
    }

    private void SelfDestraction()
    {
        Died?.Invoke(this);
    }
}
