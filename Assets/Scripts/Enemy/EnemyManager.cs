using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using static UnityEngine.EventSystems.EventTrigger;

public class EnemyManager
{
    private readonly EnemyPool _enemyPool;
    private readonly SceneSettings _sceneSettings;
    private readonly UISettings _uiSettings;
    private readonly TargetController _targetController;
    private readonly PlayerWallet _playerWallet;
    private readonly EnemyImprover _enemyImprover;

    public event Action EnemyDied;

    public EnemyManager(EnemyPool enemyPool, SceneSettings sceneSettings, UISettings uISettings, TargetController targetController, PlayerWallet playerWallet, EnemyImprover enemyImprover)
    {
        _enemyPool = enemyPool;
        _sceneSettings = sceneSettings;
        _uiSettings = uISettings;
        _targetController = targetController;
        _playerWallet = playerWallet;
        _enemyImprover = enemyImprover;

        _uiSettings.SlowEnemyButton.EnableBonus.AddListener(ActivateSlowEnemy);
        _uiSettings.SlowEnemyButton.DisableBonus.AddListener(DeActivateSlowEnemy);
    }

    ~EnemyManager()
    {
        _uiSettings.SlowEnemyButton.EnableBonus.RemoveAllListeners();
        _uiSettings.SlowEnemyButton.DisableBonus.RemoveAllListeners();
    }

    public void Create(Vector3 point)
    {
        Enemy enemy = _enemyPool.Spawn() as Enemy;
        enemy.transform.position = point;
        enemy.TargetController = _targetController;
        enemy.Enable();
        enemy.ImproveCharacteristic(_enemyImprover.Health, _enemyImprover.Damage);
        enemy.DiedComplete.AddListener(Destroy);
        enemy.DiedStart.AddListener(CleanTarget);

        Debug.Log(_enemyImprover.Health + " - improve health, " + _enemyImprover.Damage + " - improve damage");
    }

    private void ActivateSlowEnemy(int cost)
    {
        if (_playerWallet.TrySpendGem(cost))
        {
            for (int i = 0; i < _targetController.Enemies.Count; i++)
            {
                var enemy = _targetController.Enemies[i] as Enemy;
                enemy.ChangeSpeedModifyier(0.5f);
                enemy.Animator.SetFloat("Speed", 0.5f);
                enemy.SwitchFreezePartical(true);
            }
        }
    }

    private void DeActivateSlowEnemy(int cost)
    {
        for (int i = 0; i < _targetController.Enemies.Count; i++)
        {
            var enemy = _targetController.Enemies[i] as Enemy;
            enemy.ChangeSpeedModifyier(1f);
            enemy.Animator.SetFloat("Speed", 1f);
            enemy.SwitchFreezePartical(false);
        }
    }

    private void Destroy(GameUnit gameUnit)
    {
        var enemy = gameUnit as Enemy;
        gameUnit.DiedComplete.RemoveAllListeners();
        EnemyDied?.Invoke();
    }

    private void CleanTarget(GameUnit gameUnit)
    {
        var enemy = gameUnit as Enemy;
        _targetController.RemoveTarget(gameUnit);
        _playerWallet.AddGold(enemy.Award);
    }
}
