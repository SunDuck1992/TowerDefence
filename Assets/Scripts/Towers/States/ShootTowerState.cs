using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootTowerState : BaseState<Tower>
{
    public GameUnit target;

    public override void Enter()
    {
        Bullet bullet = Owner.BulletPool.Spawn();

        bullet.transform.position = Owner.ShotPoint.position;
        bullet.transform.forward = Owner.ShotPoint.forward;
        bullet.Damage = Owner.Damage;

        Owner.StateMachine.SwitchState<ReloadTowerState, Tower>(Owner);
    }


}
