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
        if(YandexGame.savesData.playerHealth > 1f)
        {
            _maxHealth = YandexGame.savesData.playerHealth;
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
        if(_maxIncreaseHealthLevel >= _playerUpgradeSystem.UpgradeData.UpgradeHealthLevel.Value)
        {
            _maxHealth += _changeHealthValue;
            YandexGame.savesData.playerHealth = _maxHealth;
            YandexGame.SaveProgress();
            //Debug.Log("maxHealth - " + _maxHealth + ", healthIncreaseLevelValue - " + _playerUpgradeSystem.UpgradeData.UpgradeHealthLevel.Value + ", maxIncreaseHealthLevel - " + _maxIncreaseHealthLevel);
            //Debug.Log(_playerUpgradeSystem.UpgradeData.UpgradeHealthLevel.Value + " - UpgradeLevel");
        }
        else
        {
            //Debug.Log("maxHealth - " + _maxHealth + ", healthIncreaseLevelValue - " + _playerUpgradeSystem.UpgradeData.UpgradeHealthLevel.Value + ", maxIncreaseHealthLevel - " + _maxIncreaseHealthLevel);
            MaxHealthLevelIncreased?.Invoke();  // Дописать логику в UI. Получая это событие вызывать блокировку кнопки и замена Image
        }
    }
}
