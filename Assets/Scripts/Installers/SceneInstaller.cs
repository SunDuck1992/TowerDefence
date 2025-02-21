using UnityEngine;
using Zenject;

public class SceneInstaller : MonoInstaller
{
    [SerializeField] private SceneSettings _sceneSettings;
    [SerializeField] private UISettings _uiSettings;

    public override void InstallBindings()
    {
        Container.Bind<SceneSettings>().FromInstance(_sceneSettings).AsSingle();
        Container.Bind<UISettings>().FromInstance(_uiSettings).AsSingle();
        Container.Bind<TargetController>().AsSingle();
    }
}