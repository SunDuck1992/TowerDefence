using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovementState : BaseState<Enemy>
{
    private float _radius = 6f;
    private Vector3 _randomPoint;
    private float _timer;

    public override void Update()
    {
        if (Owner.Target.AttackSector.freePoints.Count == 0)
        {
            float distanceToTarget = Vector3.Distance(Owner.transform.position, Owner.Target.transform.position);

            if (distanceToTarget <= _radius)
            {
                //Owner.Agent.SetDestination(Owner.transform.position);
                //Owner.StateMachine.SwitchState<EnemyIdleState, Enemy>(Owner);

                //}
                //else
                //{
                if (_timer <= Time.time)
                {
                    _randomPoint = Owner.transform.position + Random.insideUnitSphere * 2.5f;
                    _randomPoint.y = Owner.transform.position.y;
                    Owner.Agent.SetDestination(_randomPoint);
                    _timer = Time.time + 0.3f;
                }
            }

            return;
        }

        Owner.TargetAttackPoint = Owner.Target.AttackSector.freePoints.Peek();
        NavMesh.SamplePosition(Owner.TargetAttackPoint.position, out var hit, 1, NavMesh.AllAreas);

        Owner.Agent.SetDestination(hit.position);

        float distance = Vector3.Distance(Owner.transform.position, Owner.TargetAttackPoint.position);

        if (Owner.Agent.path.corners.Length > 1 & distance <= 1.5f)
        {
            Owner.TargetAttackPoint = Owner.Target.AttackSector.freePoints.Pop();
            Owner.StateMachine.SwitchState<EnemyAttackState, Enemy>(Owner);
        }
        else
        {
            Owner.Animator.SetBool("isGo", true);
        }
    }

    public override void Exit()
    {
        Owner.Animator.SetBool("isGo", false);

        Owner.Agent.SetDestination(Owner.transform.position);
    }
}
