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
    private UISettings _uiSettings;
    private PlayerWallet _playerWallet;
    private WaveScreen _waveScreen;

    //private float _pingPongScaleDuration = 1f;
    //private float _pingPongHalfScaleDuration = 0.5f;

    //private Vector3 _originalScale;
    //private Vector3 _pingPongDesiredScale;
    //private Vector3 _pingPongScaleAddition = new Vector3(0.2f, 0.2f, 0);

    //private Canvas _canvas;
    //private Transform _canvasTransform;

    //private bool _isEnter;

    public BuildTowersSystem(SceneSettings sceneSettings, TowerSettings towerSettings, TargetController targetController, BulletPool bulletPool, UISettings uiSettings, PlayerWallet playerWallet, WaveScreen waveScreen)
    {
        for (int i = 0; i < sceneSettings.BuildPoints.Count; i++)
        {
            sceneSettings.BuildPoints[i].BuildTowersSystem = this;
        }

        TowerSettings = towerSettings;
        _targetController = targetController;
        _bulletPool = bulletPool;
        _uiSettings = uiSettings;
        _playerWallet = playerWallet;
        _waveScreen = waveScreen;

        _uiSettings.RepairTowersButton.EnableBonus.AddListener(RepairTowers);
        _uiSettings.RepairTowersButton.DisableBonus.AddListener(RepairTowers);

        _waveScreen.OnEndBattle += ResetTowerHealth;
    }

    ~BuildTowersSystem()
    {
        _waveScreen.OnEndBattle -= ResetTowerHealth;
    }

    public TowerSettings TowerSettings { get; }

    public event Action<BuildArea> InteractBuildArea;
    public event Action DeInteractBuildArea;
    public event Action DestroedTower;

    //private void Start()
    //{
    //    _canvas = GetComponentInChildren<Canvas>();
    //    _canvasTransform = _canvas.transform;
    //    _originalScale = _canvasTransform.localScale;
    //    _pingPongDesiredScale = _originalScale + _pingPongScaleAddition;

    //}

    public void OnInteractBuildArea(BuildArea buildArea)
    {
        InteractBuildArea?.Invoke(buildArea);
        _currentBuildArea = buildArea;
        //_canvas = _currentBuildArea.Canvas;
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
        tower.DiedComplete.AddListener(Destroy);
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
        DestroedTower?.Invoke();
        _targetController.RemoveTarget(gameUnit);
        gameUnit.gameObject.SetActive(false);  // временное решение, пока не добавлен пулл
    }

    //private IEnumerator PingPongScale(bool isDelivering)
    //{
    //    while (isDelivering)
    //    {
    //        _canvasTransform.DOScale(_pingPongDesiredScale, _pingPongHalfScaleDuration).SetEase(Ease.InOutSine).OnComplete(() =>
    //        {
    //            _canvasTransform.DOScale(_originalScale, _pingPongHalfScaleDuration).SetEase(Ease.OutBounce);
    //        });

    //        yield return new WaitForSeconds(_pingPongScaleDuration);
    //    }
    //}
}
