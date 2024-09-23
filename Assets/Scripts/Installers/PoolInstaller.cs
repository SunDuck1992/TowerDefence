using UnityEngine;
using Zenject;

public class PoolInstaller : MonoInstaller
{
    [SerializeField] private Bullet _bulletPrefab;
    [SerializeField] private Enemy _enemyPrefab;

    public override void InstallBindings()
    {
        Container.Bind<BulletPool>().FromInstance(new BulletPool(_bulletPrefab)).AsSingle();
        Container.Bind<EnemyPool>().FromInstance(new EnemyPool(_enemyPrefab, true)).AsSingle();
    }
}