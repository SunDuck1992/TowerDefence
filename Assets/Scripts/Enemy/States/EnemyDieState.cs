using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDieState : BaseState<Enemy>
{
    float timer = 1.5f;

    public override void Enter()
    {
        int typeDie = Random.Range(0, 2);

        Owner.Agent.enabled = false;

        Owner.Animator.SetTrigger("Die");
        Owner.Animator.SetInteger("TypeDie", typeDie);
    }

    public override void Update()
    {
        timer -= Time.deltaTime;

        if(timer <= 0)
        {
            Owner.StateMachine.SwitchState<EnemyIdleState, Enemy>(Owner);
        }
    }

    public override void Exit()
    {
        Owner.DiedComplete.Invoke(Owner);

        if(Owner.Target != null && Owner.TargetAttackPoint != null)
        {
            Owner.Target.AttackSector.freePoints.Push(Owner.TargetAttackPoint);
        }
    }
}
