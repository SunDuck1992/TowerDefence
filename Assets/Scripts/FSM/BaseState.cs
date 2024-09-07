using System;
using UnityEngine;



//public class TestStateA : BaseState<PlayerMovement>
//{
//    private float timer;
//    public override void Enter()
//    {
//        timer = Time.time + 2;
//        Debug.Log("Test AAAAA");
//    }
//    public override void Update()
//    {
//        if(Time.time >= timer)
//        {
//            Owner.StateMachine.SwitcState<TestStateB, PlayerMovement>(Owner);
//        }
//    }
//}


//public class TestStateB : BaseState<PlayerMovement>
//{
//    private float timer;
//    public override void Enter()
//    {
//        timer = Time.time + 3;
//        Debug.Log("Test BBBBB");
//    }
//    public override void Update()
//    {
//        if(Time.time > timer)
//        {
//            Owner.StateMachine.SwitcState<TestStateA, PlayerMovement>(Owner);
//        }
//    }
//}

public abstract class BaseState<T> : IState
    where T : class, IStateMachineOwner
{
    public T Owner {  get; set; }

    public void Dispose()
    {
        OnDispose();
        GC.SuppressFinalize(this);
    }

    public virtual void Enter() { }
    public virtual void Exit() { }
    public virtual void Update() { }
    protected virtual void OnDispose() { }
}
