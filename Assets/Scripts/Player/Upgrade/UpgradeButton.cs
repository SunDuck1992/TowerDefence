using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class UpgradeButton : MonoBehaviour
{
    [SerializeField] private Button _button;
    [SerializeField] private Upgrade _upgrade;

    public event Action<Upgrade> Clicked;

    //public UpgradeButton(PlayerUpgradeSystem playerUpgradeSystem)
    //{
    //    _playerUpgradeSystem = playerUpgradeSystem;
    //}

    

    private void Start()
    {
        _button.onClick.AddListener(() =>
        {
            Clicked?.Invoke(_upgrade);
        });
    }

    private void OnDisable()
    {
        _button.onClick.RemoveAllListeners();
    }
}
