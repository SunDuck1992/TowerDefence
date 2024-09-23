using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrayBulletShoot : Weapon
{
    private int _count;

    protected override void CreateBullet(Bullet bullet)
    {
        _count++;

        if(_count > CountBullet)
        {
            _count = 0;
            WeaponPoint.localRotation = Quaternion.identity;
        }

        if(_count > 0)
        {
            WeaponPoint.localRotation = Quaternion.Euler(Vector3.up * (_count == 1 ? -7 : 7));
        }

            base.CreateBullet(bullet);
    }
}
