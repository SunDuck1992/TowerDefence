using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DieRocketTowerState : BaseState<RocketTower>
{
    public override void Update()
    {
        Owner.StateMachine.SwitchState<IdleRocketTowerState, RocketTower>(Owner);
    }

    public override void Exit()
    {
        Owner.DiedComplete.Invoke(Owner);
    }
}
