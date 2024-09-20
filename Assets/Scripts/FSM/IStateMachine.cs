using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IStateMachine 
{
    IState CurrentState {  get; }
    void SwitchState<T, Owner>(Owner owner, Action<T> callback = null)
         where T : BaseState<Owner>, new()
        where Owner : class, IStateMachineOwner;
    void UpdateState();
}
