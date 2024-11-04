using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDieState : BaseState<Enemy>
{
    private float _timer = 1.5f;
    private bool _hasDied;

    public override void Enter()
    {
        _hasDied = false;
        int typeDie = Random.Range(0, 2);

        Owner.Agent.enabled = false;

        Owner.Animator.SetTrigger("Die");
        Owner.Animator.SetInteger("TypeDie", typeDie);
    }

    public override void Update()
    {
        if ((Owner.Animator.GetCurrentAnimatorStateInfo(0).IsName("Die02") || Owner.Animator.GetCurrentAnimatorStateInfo(0).IsName("Die01")) && !_hasDied)
        {
            if (Owner.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
            {
                _hasDied = true;
                Owner.CreateDeathParticle();
            }
        }

        if (_hasDied)
        {
            _timer -= Time.deltaTime;

            if (_timer <= 0)
            {
                Owner.StateMachine.SwitchState<EnemyIdleState, Enemy>(Owner);
            }
        }
    }

    public override void Exit()
    {
        Owner.DiedComplete.Invoke(Owner);

        if (Owner.Target != null && Owner.TargetAttackPoint != null)
        {
            Owner.Target.AttackSector.freePoints.Push(Owner.TargetAttackPoint);
        }
    }
}
