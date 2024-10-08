using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReloadRocketTowerState : BaseState<RocketTower>
{
    private float _fireRate;

    public override void Enter()
    {
        _fireRate = Owner.FireRate + Time.time;

        Debug.LogWarning("Вошли в состояние перезарядки");

        Debug.LogWarning(Owner.FireRate + " - Owner Firerate, " + _fireRate + " - fireRate, " + Time.time + " - Time.time");

    }

    public override void Update()
    {
        if (Time.time > _fireRate)
        {
            Debug.LogWarning("Перезарядились, идем в состояние поиска цели");
            Owner.StateMachine.SwitchState<IdleRocketTowerState, RocketTower>(Owner);
        }
    }
}
