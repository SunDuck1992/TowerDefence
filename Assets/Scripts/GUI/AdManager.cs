using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YG;
using Zenject;

public class AdManager : MonoBehaviour
{
    [SerializeField] private ShopScreen _shopScreen;
    [SerializeField] private GameOver _gameOver;

    private PlayerWallet _playerWallet;

    private int _gemCountRevard = 3;

    [Inject]
    public void Construct(PlayerWallet playerWallet)
    {
        _playerWallet = playerWallet;
    }

    private void OnEnable()
    {
        YandexGame.RewardVideoEvent += Revarded;
    }

    private void OnDisable()
    {
        YandexGame.RewardVideoEvent -= Revarded;
    }

    public void ShowFullScreenAd()
    {
        YandexGame.FullscreenShow();
    }

    public void CheckOrShowRevardAd(int id)
    {
        if (_shopScreen.CheckPurchaseItem(id))
        {
            _shopScreen.ChangeWeaponButtonClick(id);
        }
        else
        {
            YandexGame.RewVideoShow(id);
        }
    }

    private void Revarded(int id)
    {
        if (id == 1)
        {
            _playerWallet.AddGem(_gemCountRevard);
        }
        if (id == 2)
        {
            _shopScreen.ChangeWeaponButtonClick(id);
        }
        if (id == 3)
        {
            _gameOver.TryAgain();
        }
    }
}
