using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class IdleTowerState : BaseState<Tower>
{
    public override void Update()
    {
        var target = Owner.TargetController.GetTarget(Owner, 15, true);

        if (target != null)
        {
            Vector3 direction = target.transform.position - Owner.transform.position;
            Vector3 rotation = Quaternion.Lerp(Owner.TransformTower.rotation, Quaternion.LookRotation(direction), 10 * Time.deltaTime).eulerAngles;
            rotation.x = 0;
            rotation.z = 0;

            Owner.TransformTower.eulerAngles = rotation;

            float angle = Vector3.Angle(Owner.TransformTower.forward, direction.normalized);
            Debug.Log(angle);

            if(angle <= 5f)
            {
                Owner.StateMachine.SwitchState<ShootTowerState, Tower>(Owner, state => state.target = target);
            }
        }
    }
}