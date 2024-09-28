using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject.SpaceFighter;

public class ShootRocketTowerState : BaseState<RocketTower>
{
    public GameUnit target;
    private Rocket _rocket;

    public override void Enter()
    {
        _rocket = Owner.RocketPool.Spawn();
        _rocket.Target = target;
        _rocket.Damage = Owner.Damage;
        _rocket.transform.position = Owner.ShotPoint.position;
        _rocket.transform.forward = Owner.ShotPoint.forward;

        Debug.Log(target.name);

        _rocket.HitTower += OnHit;
        _rocket.Died += RocketComplete;

        Owner.StateMachine.SwitchState<ReloadRocketTowerState, RocketTower>(Owner);
    }

    private void OnHit(Enemy enemy)
    {
        enemy.TakeDamage(_rocket.Damage);
    }

    private void RocketComplete(Rocket rocket)
    {
        _rocket.HitTower -= OnHit;
        _rocket.Died -= RocketComplete;
    }
}
