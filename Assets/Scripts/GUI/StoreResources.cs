using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class StoreResources : MonoBehaviour
{
    [SerializeField] private int _costGem;

    private PlayerWallet _playerWallet;

    [Inject]
    public void Construct(PlayerWallet playerWallet)
    {
        _playerWallet = playerWallet;
    }

    public void OnClickButtonSellGem()
    {
        if (_playerWallet.TrySpendGold(_costGem))
        {
            _playerWallet.AddGem();
        }
    }
}
