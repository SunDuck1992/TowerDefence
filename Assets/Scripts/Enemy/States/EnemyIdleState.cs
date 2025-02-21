using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyIdleState : BaseState<Enemy>
{
    public override void Enter()
    {
        Owner.Target = Owner.TargetController.GetTarget(Owner, 1000);
    }

    public override void Update()
    {
        if (Owner.Target != null)
        {
            Owner.Agent.enabled = true;

            Owner.StateMachine.SwitchState<EnemyMovementState, Enemy>(Owner);
        }
    }
}