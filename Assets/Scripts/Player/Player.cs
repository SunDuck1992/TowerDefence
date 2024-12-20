using System;
using UnityEngine;
using Zenject;
using YG;

public class Player : GameUnit
{
    [SerializeField] private float _changeHealthValue;

    private int _maxIncreaseHealthLevel = 5;
    private PlayerUpgradeSystem _playerUpgradeSystem;

    public event Action MaxHealthLevelIncreased;

    private void Start()
    {    
        if(YandexGame.savesData.upgradeHealthLevel != -1)
        {
            _maxHealth += _changeHealthValue * YandexGame.savesData.upgradeHealthLevel;
            Debug.LogWarning("UpgradeHealthLevel - " + _playerUpgradeSystem.UpgradeData.UpgradeHealthLevel.Value + ", yandexUpgradeLevel - " + YandexGame.savesData.upgradeHealthLevel);
        }
    }

    [Inject]
    public void Construct(PlayerUpgradeSystem playerUpgradeSystem, GameConfigProxy gameConfigProxy, TargetController targetController)
    {
        _playerUpgradeSystem = playerUpgradeSystem;
        //_maxHealth = gameConfigProxy.Config.PlayerConfig.Health;
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
            //YandexGame.savesData.playerHealth = _maxHealth;
            //YandexGame.SaveProgress();
            //Debug.Log("maxHealth - " + _maxHealth + ", healthIncreaseLevelValue - " + _playerUpgradeSystem.UpgradeData.UpgradeHealthLevel.Value + ", maxIncreaseHealthLevel - " + _maxIncreaseHealthLevel);
            //Debug.Log(_playerUpgradeSystem.UpgradeData.UpgradeHealthLevel.Value + " - UpgradeLevel");
        }
        else
        {
            //Debug.Log("maxHealth - " + _maxHealth + ", healthIncreaseLevelValue - " + _playerUpgradeSystem.UpgradeData.UpgradeHealthLevel.Value + ", maxIncreaseHealthLevel - " + _maxIncreaseHealthLevel);
            MaxHealthLevelIncreased?.Invoke();  // �������� ������ � UI. ������� ��� ������� �������� ���������� ������ � ������ Image
        }

        Debug.LogWarning("UpgradeHealthLevel - " + _playerUpgradeSystem.UpgradeData.UpgradeHealthLevel.Value + ", yandexUpgradeLevel - " + YandexGame.savesData.upgradeHealthLevel);
    }
}
