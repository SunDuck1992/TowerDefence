using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackState : BaseState<Enemy>
{
    public override void Enter()
    {
        int typeAttack = Random.Range(0, 3);

        Owner.transform.forward = Owner.Target.transform.position - Owner.transform.position;
        Owner.Animator.SetTrigger("Attack");
        Owner.Animator.SetInteger("TypeAttack", typeAttack);

        Owner.Listener.Attack.AddListener(OnAttack);
    }

    public override void Exit()
    {
        Owner.Listener.Attack.RemoveAllListeners();
    }

    private void OnAttack()
    {
        Owner.Target.TakeDamage(Owner.Damage);
        Owner.Target.AttackSector.freePoints.Push(Owner.TargetAttackPoint);
        Owner.TargetAttackPoint = null;
        Owner.StateMachine.SwitchState<EnemyIdleState, Enemy>(Owner);

    }

}
