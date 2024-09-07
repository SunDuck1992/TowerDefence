using UnityEngine;
using Zenject;


public class PlayerInstaller : MonoInstaller
{
    [SerializeField] private PlayerShooter _playerShooter;
    [SerializeField] private Enemy _enemies;
    [SerializeField] private PlayerMovement _playerMovement;
    [SerializeField] private GameConfig _gameConfig;

    public override void InstallBindings()
    {
        Container.Bind<GameConfigProxy>().FromInstance(new GameConfigProxy(_gameConfig)).AsSingle();
        Container.Bind<Enemy>().FromInstance(_enemies).AsSingle();
        Container.Bind<EnemyManager>().AsSingle();
        Container.Bind<PlayerShooter>().FromInstance(_playerShooter).AsSingle();
        Container.Bind<PlayerMovement>().FromInstance(_playerMovement).AsSingle();
        Container.Bind<PlayerUpgradeSystem>().AsSingle();
    }
}