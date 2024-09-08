using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class EnemyManager 
{
    private readonly EnemyPool _enemyPool;
    private readonly SceneSettings _sceneSettings;
    private readonly UISettings _uiSettings;
    private readonly TargetController _targetController;

    public event Action EnemyDied;

    public EnemyManager(EnemyPool enemyPool, SceneSettings sceneSettings, UISettings uISettings, TargetController targetController)
    {
        _enemyPool = enemyPool;
        _sceneSettings = sceneSettings;
        _uiSettings = uISettings;
        _targetController = targetController;

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
        enemy.DiedComplete.AddListener(Destroy);

        Debug.Log("ID" + enemy.GetHashCode());
    }

    private void ActivateSlowEnemy()
    {
        for (int i = 0; i < _targetController.Enemies.Count; i++)
        {
            var enemy = _targetController.Enemies[i] as Enemy;
            enemy.ChangeSpeedModifyier(0.5f);
            enemy.Animator.SetFloat("Speed", 0.5f);
        }
    }

    private void DeActivateSlowEnemy()
    {
        for (int i = 0; i < _targetController.Enemies.Count; i++)
        {
            var enemy = _targetController.Enemies[i] as Enemy;
            enemy.ChangeSpeedModifyier(1f);
            enemy.Animator.SetFloat("Speed", 1f);
        }
    }

    private void Destroy(GameUnit gameUnit)
    {
        gameUnit.DiedComplete.RemoveAllListeners();
        EnemyDied?.Invoke();
        _targetController.RemoveTarget(gameUnit);
    }
}
