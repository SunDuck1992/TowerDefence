using UnityEngine;
using Zenject;

public class PlayerWalletInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        var playerWallet = new PlayerWallet();

        Container.Bind<PlayerWallet>().FromInstance(playerWallet).AsSingle();
    }
}