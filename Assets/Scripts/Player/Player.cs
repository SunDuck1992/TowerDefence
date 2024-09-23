using System;
using UnityEngine;
using Zenject;

public class Player : GameUnit
{
    private PlayerUpgradeSystem _playerUpgradeSystem;

    [Inject]
    public void Construct(PlayerUpgradeSystem playerUpgradeSystem, GameConfigProxy gameConfigProxy, TargetController targetController)
    {
        _playerUpgradeSystem = playerUpgradeSystem;
        _maxHealth = gameConfigProxy.Config.PlayerConfig.Health;
        _playerUpgradeSystem.UpgradeData.UpgradeHealthLevel.ValueChanged += IncreaseHealth;
        targetController.AddTarget(this, true);
    }

    private void OnDestroy()
    {
        _playerUpgradeSystem.UpgradeData.UpgradeHealthLevel.ValueChanged -= IncreaseHealth;
    }

    private void IncreaseHealth()
    {
        _maxHealth += _playerUpgradeSystem.UpgradeData.UpgradeHealthLevel.Value;
        Debug.Log("maxHealth - " + _maxHealth + ", healthIncrease - " + _playerUpgradeSystem.UpgradeData.UpgradeHealthLevel.Value);
        Debug.Log(_playerUpgradeSystem.UpgradeData.UpgradeHealthLevel.Value + " - UpgradeLevel");
    }
}
