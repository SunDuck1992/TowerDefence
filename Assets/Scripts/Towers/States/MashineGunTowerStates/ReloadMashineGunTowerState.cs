using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ReloadMashineGunTowerState : BaseState<MashineGunTower>
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
            Owner.StateMachine.SwitchState<IdleMashineGunTowerState, MashineGunTower>(Owner);
        }
    }
}
