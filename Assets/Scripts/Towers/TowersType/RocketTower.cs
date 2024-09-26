using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketTower : Tower
{
    public override void Enable()
    {
        StateMachine.SwitchState<IdleRocketTowerState, RocketTower>(this);
    }
}
