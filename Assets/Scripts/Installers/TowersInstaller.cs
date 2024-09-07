using UnityEngine;
using Zenject;

public class TowersInstaller : MonoInstaller
{
    [SerializeField] private TowerSettings _towerSettings;


    public override void InstallBindings()
    {
        Container.Bind<TowerSettings>().FromInstance(_towerSettings).AsSingle();
        Container.Bind<BuildTowersSystem>().AsSingle();
    }
}