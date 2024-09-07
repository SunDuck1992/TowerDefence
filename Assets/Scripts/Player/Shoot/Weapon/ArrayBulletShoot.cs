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

    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.blue;
    //    Gizmos.DrawWireSphere(WeaponPoint.position, 1f);

    //    for (int i = 0; i < 1; i++)
    //    {
    //        Gizmos.DrawSphere(GetPointOnCircule(90), 0.1f);
    //    }
    //}

    //private Vector3 GetPointOnCircule(float angle)
    //{
    //    Vector3 point = WeaponPoint.position;
    //    point.x += 1 * Mathf.Cos(angle * Mathf.Deg2Rad);
    //    point.z += 1 * Mathf.Sin(angle * Mathf.Deg2Rad);

    //    return point;
    //}
}
