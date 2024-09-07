using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BuildTowersSystem
{
    private BuildArea _currentBuildArea;
    private TargetController _targetController;
    private BulletPool _bulletPool;

    public BuildTowersSystem(SceneSettings sceneSettings, TowerSettings towerSettings, TargetController targetController, BulletPool bulletPool)
    {
        for (int i = 0; i < sceneSettings.BuildPoints.Count; i++)
        {
            sceneSettings.BuildPoints[i].BuildTowersSystem = this;
        }

        TowerSettings = towerSettings;
        _targetController = targetController;
        _bulletPool = bulletPool;
    }

    public TowerSettings TowerSettings { get; }

    public event Action<BuildArea> InteractBuildArea;
    public event Action DeInteractBuildArea;

    public void OnInteractBuildArea(BuildArea buildArea)
    {
        InteractBuildArea?.Invoke(buildArea);
        _currentBuildArea = buildArea;
    }

    public void OnDeInteractBuildArea()
    {
        DeInteractBuildArea?.Invoke();
        _currentBuildArea = null;
    }

    public void BuildTower(Tower prefab)
    {
        if (_currentBuildArea == null) return;

        if (_currentBuildArea.OnBuild) return;

        NavMesh.SamplePosition(_currentBuildArea.BuildPoint.position, out var hit, 1, NavMesh.AllAreas);
        Tower tower = MonoBehaviour.Instantiate(prefab, hit.position, Quaternion.identity);
        tower.TargetController = _targetController;
        tower.BulletPool = _bulletPool;
        tower.Enable();
        _currentBuildArea.OnBuild = true;
        _targetController.AddTarget(tower, true);
    }
}
