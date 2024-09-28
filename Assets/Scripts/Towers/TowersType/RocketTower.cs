using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketTower : Tower
{
    public override void Enable()
    {
        StateMachine.SwitchState<IdleRocketTowerState, RocketTower>(this);
    }

    public override void Die()
    {
        StateMachine.SwitchState<DieRocketTowerState, RocketTower>(this);
    }
}
