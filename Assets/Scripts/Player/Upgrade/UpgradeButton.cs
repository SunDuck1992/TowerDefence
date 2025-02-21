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
    [SerializeField] private int _maxLevelUpgrade;

    private int _countClicked;

    public int MaxLevelUpgrade => _maxLevelUpgrade;
    public int CountClicked => _countClicked;

    public event Action<Upgrade> Clicked;

    private void OnEnable()
    {
        _button.onClick.AddListener(() =>
        {
            Clicked?.Invoke(_upgrade);
            _countClicked++;
        });
    }

    private void OnDisable()
    {
        _button.onClick.RemoveAllListeners();
    }
}
