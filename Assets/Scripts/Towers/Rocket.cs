using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private float _duration;

    private float _elapsedTime = 0;
    public float Speed => _speed;
    public float Damage {  get; set; }
    public GameUnit Target {  get; set; }

    public event Action<Rocket> Died;
    public event Action<Enemy> HitTower;

    private void OnEnable()
    {
        //Invoke("SelfDestraction", 5f);
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    private void Start()
    {
        StartCoroutine(FlyToTarget());
    }

    //private void Update()
    //{
    //    Test();
    //}

    private void OnTriggerEnter(Collider other)
    {
        Enemy enemy = other.GetComponent<Enemy>();

        if (enemy != null)
        {
            HitTower?.Invoke(enemy);
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

        while(elapsedTime < _duration)
        {
            Debug.LogWarning("1");
            transform.Translate(Vector3.forward * _speed * Time.deltaTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        while (Target != null)
        {
            Debug.LogWarning("2");
            Vector3 direction = (Target.transform.position - transform.position).normalized;
            transform.position += direction * _speed * Time.deltaTime;

            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, _speed * Time.deltaTime);
            yield return null;
        }
    }

    //public void Test()
    //{
    //    //float elapsedTime = 0f;

    //    if (_elapsedTime < _duration)
    //    {
    //        transform.Translate(Vector3.forward * _speed * Time.deltaTime);
    //        _elapsedTime += Time.deltaTime;
    //    }
    //}
}
