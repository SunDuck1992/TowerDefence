using UnityEngine;
using Zenject;

public class SceneInstaller : MonoInstaller
{
    [SerializeField] private SceneSettings _sceneSettings;
    [SerializeField] private UISettings _uiSettings;

    public override void InstallBindings()
    {
        Container.Bind<SceneSettings>().FromInstance(_sceneSettings).AsSingle();
        //_uiSettings.IsMobile = true;SDK
        Container.Bind<UISettings>().FromInstance(_uiSettings).AsSingle();
        Container.Bind<TargetController>().AsSingle();
        //Container.Bind<MashineGunTower>().AsSingle();
    }
}