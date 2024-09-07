using UnityEngine;
using Zenject;

public class PlayerWalletInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<PlayerWallet>().AsSingle();
    }
}