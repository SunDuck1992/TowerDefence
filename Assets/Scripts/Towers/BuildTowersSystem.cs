using DG.Tweening;
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
    private RocketPool _rocketPool;
    private UISettings _uiSettings;
    private PlayerWallet _playerWallet;
    private WaveScreen _waveScreen;
    private Tower _towerBuild;

    private Dictionary<BuildArea, Tower> _towerAreaLocations = new();

    public BuildArea CurrentBuildArea => _currentBuildArea;
    public Dictionary<BuildArea, Tower> TowerAreaLocations => _towerAreaLocations;

    public BuildTowersSystem(SceneSettings sceneSettings, TowerSettings towerSettings, TargetController targetController, BulletPool bulletPool, UISettings uiSettings, PlayerWallet playerWallet, WaveScreen waveScreen, RocketPool rocketPool)
    {
        for (int i = 0; i < sceneSettings.BuildPoints.Count; i++)
        {
            sceneSettings.BuildPoints[i].BuildTowersSystem = this;
        }

        
        TowerSettings = towerSettings;
        _targetController = targetController;
        _bulletPool = bulletPool;
        _rocketPool = rocketPool;
        _uiSettings = uiSettings;
        _playerWallet = playerWallet;
        _waveScreen = waveScreen;

        _uiSettings.RepairTowersButton.EnableBonus.AddListener(RepairTowers);
        _uiSettings.RepairTowersButton.DisableBonus.AddListener(RepairTowers);

        _waveScreen.OnEndBattle += ResetTowerHealth;
        _rocketPool = rocketPool;
        _targetController.AddTarget(sceneSettings.Base, true);
    }

    ~BuildTowersSystem()
    {
        _waveScreen.OnEndBattle -= ResetTowerHealth;
    }

    public TowerSettings TowerSettings { get; }

    public event Action<BuildArea> InteractBuildArea;
    public event Action DeInteractBuildArea;
    //public event Action DestroedTower;

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
        //if (_currentBuildArea == null) return;

        if (_currentBuildArea.OnBuild) return;

        NavMesh.SamplePosition(_currentBuildArea.BuildPoint.position, out var hit, 1, NavMesh.AllAreas);
        Tower tower = MonoBehaviour.Instantiate(prefab, hit.position, Quaternion.identity);
        tower.TargetController = _targetController;
        tower.BulletPool = _bulletPool;
        tower.RocketPool = _rocketPool;
        tower.Enable();
        _currentBuildArea.OnBuild = true;
        _targetController.AddTarget(tower, true);
        tower.DiedComplete.AddListener(Destroy);
        _towerBuild = tower;

        _towerAreaLocations.Add(_currentBuildArea, tower);

        Debug.Log(_towerAreaLocations.Count + " - ���������� ��� � �����");
    }

    public Tower GetBuildTower()
    {
        return _towerBuild;
    }

    private void RepairTowers(int cost)
    {
        if (_playerWallet.TrySpendGem(cost))
        {
            for (int i = 0; i < _targetController.Towers.Count; i++)
            {
                var tower = _targetController.Towers[i];

                Debug.Log(tower.Health + " - Towere here");

                tower.ResetHealth();
            }
        }
    }

    private void ResetTowerHealth()
    {
        for (int i = 0; i < _targetController.Towers.Count; i++)
        {
            var tower = _targetController.Towers[i];
            tower.ResetHealth();
        }
    }

    private void Destroy(GameUnit gameUnit)
    {
        gameUnit.DiedComplete.RemoveAllListeners();
        //DestroedTower?.Invoke();
        _targetController.RemoveTarget(gameUnit);
        _currentBuildArea.OnBuild = false;
        gameUnit.gameObject.SetActive(false);  // ��������� �������, ���� �� �������� ����
        _towerAreaLocations.Remove(_currentBuildArea);
    }
}
