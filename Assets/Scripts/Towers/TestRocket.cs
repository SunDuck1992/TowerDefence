using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestRocket : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private float _duration;

    public float Speed => _speed;
    public float Damage { get; set; }
    public GameUnit Target { get; set; }

    public event Action<Rocket> Died;
    public event Action<Enemy> HitTower;

    private void Start()
    {
        StartCoroutine(FlyToTarget());
    }

    public IEnumerator FlyToTarget()
    {
        float elapsedTime = 0f;

        while (elapsedTime < _duration)
        {
            transform.Translate(Vector3.forward * _speed * Time.deltaTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        while (Target != null)
        {
            Vector3 direction = (Target.transform.position - transform.position).normalized;
            transform.position += direction * _speed * Time.deltaTime;

            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, _speed * Time.deltaTime);
            yield return null;
        }
    }
}
