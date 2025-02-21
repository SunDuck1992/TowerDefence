using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReloadRocketTowerState : BaseState<RocketTower>
{
    private float _fireRate;

    public override void Enter()
    {
        _fireRate = Owner.FireRate + Time.time;
    }

    public override void Update()
    {
        if (Time.time > _fireRate)
        {
            Owner.StateMachine.SwitchState<IdleRocketTowerState, RocketTower>(Owner);
        }
    }
}
