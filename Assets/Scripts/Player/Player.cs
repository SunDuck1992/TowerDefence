using System;
using UnityEngine;
using Zenject;
using YG;

public class Player : GameUnit
{
    [SerializeField] private float _changeHealthValue;

    private int _maxIncreaseHealthLevel = 100;
    private PlayerUpgradeSystem _playerUpgradeSystem;

    public event Action MaxHealthLevelIncreased;

    private void Start()
    {    
        if(YandexGame.savesData.upgradeHealthLevel != -1)
        {
            _maxHealth += _changeHealthValue * YandexGame.savesData.upgradeHealthLevel;
        }
    }

    [Inject]
    public void Construct(PlayerUpgradeSystem playerUpgradeSystem, GameConfigProxy gameConfigProxy, TargetController targetController)
    {
        _playerUpgradeSystem = playerUpgradeSystem;
        _playerUpgradeSystem.UpgradeData.UpgradeHealthLevel.ValueChanged += IncreaseHealth;
        targetController.AddTarget(this, true);
    }

    private void OnDestroy()
    {
        _playerUpgradeSystem.UpgradeData.UpgradeHealthLevel.ValueChanged -= IncreaseHealth;
    }

    private void IncreaseHealth()
    {
        if(_maxIncreaseHealthLevel >= YandexGame.savesData.upgradeHealthLevel)
        {
            _maxHealth += _changeHealthValue;
        }
        else
        {
            MaxHealthLevelIncreased?.Invoke();
        }
    }
}
