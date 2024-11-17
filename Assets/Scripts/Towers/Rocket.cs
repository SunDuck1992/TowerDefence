using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Rocket : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private float _duration;
    [SerializeField] private Transform _fire;
    [SerializeField] private ParticleSystem _flyParticle;
    [SerializeField] private ParticleSystem _hitParticle;

    private ParticleSystem _particle;

    public float Speed => _speed;
    public float Damage { get; set; }
    public GameUnit Target { get; set; }

    public event Action<Rocket> Died;
    public event Action<Enemy> HitTower;

    private void OnEnable()
    {
        Invoke("SelfDestraction", 7f);
        StartCoroutine(FlyToTarget());
    }

    private void OnDisable()
    {
        StopAllCoroutines();
        ClearEvents();
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.LogWarning($"Triggering {other.gameObject.name}");

        if (other.TryGetComponent<Enemy>(out var enemy))
        {
            HitTower?.Invoke(enemy);

            Instantiate(_hitParticle, enemy.DeathParticlePoint.position, Quaternion.identity);

            if (_particle != null)
            {
                Destroy(_particle.gameObject);
                _particle = null;
            }

            StopCoroutine(FlyToTarget());
            Died?.Invoke(this);
        }
    }

    private void SelfDestraction()
    {
        Died?.Invoke(this);
    }

    public IEnumerator FlyToTarget()
    {
        float elapsedTime = 0f;

        while (elapsedTime < _duration)
        {
            Debug.Log("Лечу вверх");
            transform.Translate(Vector3.forward * _speed * Time.deltaTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        _particle = Instantiate(_flyParticle, _fire.position, Quaternion.LookRotation(-_fire.forward));
        _particle.transform.parent = _fire;

        while (Target != null)
        {
            Debug.Log("Лечу к цели");
            Vector3 direction = (Target.transform.position - transform.position).normalized;
            transform.position += direction * _speed * Time.deltaTime;

            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, _speed * Time.deltaTime);
            yield return null;
        }
    }

    private void ClearEvents()
    {
        Died = null;
        HitTower = null;
    }
}
