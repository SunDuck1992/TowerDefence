using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DieTowerState : BaseState<Tower>
{
    public override void Update()
    {
        Owner.StateMachine.SwitchState<IdleTowerState, Tower>(Owner);
    }

    public override void Exit()
    {
        Owner.DiedComplete.Invoke(Owner);
    }
}
