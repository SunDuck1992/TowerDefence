using System;
using UnityEngine;

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
