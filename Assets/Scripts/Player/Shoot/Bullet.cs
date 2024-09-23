using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject.SpaceFighter;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float _speed;

    public float Damage {  get; set; }

    public event Action<Bullet> Died;
    public event Action<Enemy> Hit;
    public event Action<Enemy> HitTower;

    private void OnEnable()
    {
        Invoke("SelfDestraction", 5f);
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    private void Update()
    {
        transform.Translate(Vector3.forward * _speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        Enemy enemy = other.GetComponent<Enemy>();

        if (enemy != null)
        {
            Hit?.Invoke(enemy);
            HitTower?.Invoke(enemy);
            Died?.Invoke(this);
        }
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
