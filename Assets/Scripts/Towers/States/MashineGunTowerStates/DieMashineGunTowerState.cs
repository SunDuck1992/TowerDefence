using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DieMashineGunTowerState : BaseState<MashineGunTower>
{
    public override void Update()
    {
        Owner.StateMachine.SwitchState<IdleMashineGunTowerState, MashineGunTower>(Owner);
    }

    public override void Exit()
    {
        Owner.DiedComplete.Invoke(Owner);
        Debug.Log(Owner.name + " - IN DIE STATE");
    }
}
