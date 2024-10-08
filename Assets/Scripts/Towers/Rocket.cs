using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Rocket : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private float _duration;

    private float _elapsedTime = 0;
    private Enemy _enemy;
    private bool _isEnter = false;

    private float timer;
    private float clock;

    public float Speed => _speed;
    public float Damage { get; set; }
    public GameUnit Target { get; set; }

    public event Action<Rocket> Died;
    public event Action<Enemy> HitTower;

    private void OnEnable()
    {
        Invoke("SelfDestraction", 5f);
        timer = 0.3f;
        StartCoroutine(FlyToTarget());
    }

    private void OnDisable()
    {
        StopAllCoroutines();
        ClearEvents();
    }

    //private void Update()
    //{
    //    if (_isEnter)
    //    {
    //        timer -= Time.deltaTime;

    //        if (timer <= 0)
    //        {
    //            SelfDestraction();
    //        }
    //    }
    //}

    private void OnTriggerEnter(Collider other)
    {
        Debug.LogWarning($"Triggering {other.gameObject.name}");
        //Enemy enemy = other.GetComponent<Enemy>();

        //if(enemy != null)
        //{
        //    HitTower?.Invoke(enemy);
        //    Died?.Invoke(this);
        //}

        if (other.TryGetComponent<Enemy>(out var enemy))
        {
            _enemy = enemy;
            HitTower?.Invoke(enemy);
            //gameObject.SetActive(false);
            StopCoroutine(FlyToTarget());
            Died?.Invoke(this);
            //_isEnter = true;
        }

        new WaitForSeconds(0.1f);

        //Debug.LogWarning($"Triggering {other.gameObject.name}");

        //// Игнорируем собственные столкновения
        //if (other.gameObject == gameObject)
        //    return;

        //// Проверяем, что это враг
        //if (other.TryGetComponent<Enemy>(out var enemy))
        //{
        //    HitTower?.Invoke(enemy);
        //    Died?.Invoke(this);
        //    // Дополнительно: чтобы предотвратить дальнейшее взаимодействие
        //    // можно отключить объект ракеты, если это необходимо
        //}
    }

    private void SelfDestraction()
    {
        _isEnter = false;
        //Died?.Invoke(this);
        gameObject.SetActive(false);
    }

    public IEnumerator FlyToTarget()
    {
        float elapsedTime = 0f;

        while (elapsedTime < _duration)
        {
            Debug.LogWarning("Лечу вверх");
            transform.Translate(Vector3.forward * _speed * Time.deltaTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        while (Target != null)
        {
            Debug.LogWarning("Лечу к цели");
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
