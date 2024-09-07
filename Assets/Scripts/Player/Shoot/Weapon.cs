using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    [SerializeField] private int _countBullet;
    [SerializeField] private float _firerate;
    [SerializeField] private float _damage;
    [SerializeField] private Transform _weaponPoint;
    [SerializeField] private IKControl _IKControls;

    public Transform WeaponPoint => _weaponPoint;
    public float FireRate => _firerate;
    public int CountBullet => _countBullet;
   
    public void Shoot(Bullet bullet)
    {
        if(bullet == null)
        {
            return;
        }

        CreateBullet(bullet);       
    }

    public void Activate()

    {
        gameObject.SetActive(true);
        _IKControls.enabled = true;
    }

    public void DeActivate()
    {
        gameObject?.SetActive(false);
        _IKControls.enabled = false;
    }

    protected virtual void CreateBullet(Bullet bullet)
    {
        bullet.transform.position = _weaponPoint.position;
        bullet.transform.forward = _weaponPoint.forward;
        bullet.Damage = _damage;
    }
}
