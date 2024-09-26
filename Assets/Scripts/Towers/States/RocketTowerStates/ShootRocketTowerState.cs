using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootRocketTowerState : BaseState<RocketTower>
{
    public GameUnit target;

    public override void Enter()
    {
        Debug.Log("In Shoot State");
    }
}
