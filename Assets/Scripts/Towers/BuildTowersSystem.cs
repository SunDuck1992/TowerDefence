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
    private UISettings _uiSettings;

    public BuildTowersSystem(SceneSettings sceneSettings, TowerSettings towerSettings, TargetController targetController, BulletPool bulletPool, UISettings uiSettings)
    {
        for (int i = 0; i < sceneSettings.BuildPoints.Count; i++)
        {
            sceneSettings.BuildPoints[i].BuildTowersSystem = this;
        }

        TowerSettings = towerSettings;
        _targetController = targetController;
        _bulletPool = bulletPool;
        _uiSettings = uiSettings;

        _uiSettings.RepairTowersButton.EnableBonus.AddListener(RepairTowers);
        _uiSettings.RepairTowersButton.DisableBonus.AddListener(RepairTowers);
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

    private void RepairTowers()
    {
        for (int i = 0; i < _targetController.Towers.Count; i++)
        {
            var tower = _targetController.Towers[i];

            Debug.Log(tower.Health + " - Towere here");

            tower.ResetHealth();
        }
    }
}
