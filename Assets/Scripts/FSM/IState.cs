
using System;
public interface IState : IDisposable
{
    void Enter();
    void Update();
    void Exit();

}
