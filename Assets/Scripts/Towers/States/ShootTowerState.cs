using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootTowerState : BaseState<Tower>
{
    public GameUnit target;
    private Bullet _bullet;

    public override void Enter()
    {
        _bullet = Owner.BulletPool.Spawn();

        _bullet.transform.position = Owner.ShotPoint.position;
        _bullet.transform.forward = Owner.ShotPoint.forward;
        _bullet.Damage = Owner.Damage;

        _bullet.HitTower += OnHit;
        _bullet.Died += BulletComplete;

        Owner.StateMachine.SwitchState<ReloadTowerState, Tower>(Owner);
    }

    private void OnHit(Enemy enemy)
    {
        enemy.TakeDamage(_bullet.Damage);
    }

    private void BulletComplete(Bullet bullet)
    {
        bullet.HitTower -= OnHit;
        bullet.Died -= BulletComplete;
    }


}
