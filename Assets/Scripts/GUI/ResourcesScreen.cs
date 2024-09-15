using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class ResourcesScreen : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _goldValueText;
    [SerializeField] private TextMeshProUGUI _gemValueText;

    private PlayerWallet _playerWallet;

    [Inject]
    public void Construct(PlayerWallet playerWallet)
    { 
        _playerWallet = playerWallet;
        _playerWallet.GoldChanged += UpdateResourcesScreen;
        _playerWallet.GemChanged += UpdateResourcesScreen;
    }

    private void Start()
    {
        _goldValueText.text = _playerWallet.Gold.ToString();
        _gemValueText.text= _playerWallet.Gem.ToString();
    }

    private void UpdateResourcesScreen()
    {
        _goldValueText.text = _playerWallet.Gold.ToString();
        _gemValueText.text= _playerWallet.Gem.ToString();
    }

    private void OnDestroy()
    {
        _playerWallet.GoldChanged -= UpdateResourcesScreen;
        _playerWallet.GemChanged -= UpdateResourcesScreen;
    }
}
