using System;
using UnityEngine;
using Zenject;

public class Player : GameUnit
{
    private PlayerUpgradeSystem _playerUpgradeSystem;

    public event Action<int> GoldChanged;

    public int Gold { get; private set; }

    [Inject]
    public void Construct(PlayerUpgradeSystem playerUpgradeSystem, GameConfigProxy gameConfigProxy, TargetController targetController)
    {
        _playerUpgradeSystem = playerUpgradeSystem;
        _maxHealth = gameConfigProxy.Config.PlayerConfig.Health;
        _playerUpgradeSystem.UpgradeData.UpgradeHealthLevel.ValueChanged += IncreaseHealth;
        targetController.AddTarget(this, true);
    }

    private void Start()
    {
        Gold = 2000;
    }

    private void OnDestroy()
    {
        _playerUpgradeSystem.UpgradeData.UpgradeHealthLevel.ValueChanged -= IncreaseHealth;
    }

    public void AddGold(int gold)
    {
        if (gold > 0)
        {
            Gold += gold;
            GoldChanged?.Invoke(Gold);
        }
    }

    public bool TrySpendGold(int gold)
    {
        if (gold > 0 && Gold - gold >= 0)
        {
            Gold -= gold;
            GoldChanged?.Invoke(Gold);

            return true;
        }

        return false;
    }

    private void IncreaseHealth(int health)
    {
        _maxHealth += health * _playerUpgradeSystem.UpgradeData.UpgradeHealthLevel.Value;
        Debug.Log("maxHealth - " + _maxHealth + ", healthIncrease - " + health);
    }


}
