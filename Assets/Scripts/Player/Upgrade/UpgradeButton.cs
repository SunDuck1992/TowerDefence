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

    private void OnEnable()
    {
        _button.onClick.AddListener(() =>
        {
            Clicked?.Invoke(_upgrade);
            Proverka();
        });
    }

    private void OnDisable()
    {
        _button.onClick.RemoveAllListeners();
    }

    private void Proverka()
    {
        Debug.Log(" нопка " + _upgrade + " была нажата");
    }
}
