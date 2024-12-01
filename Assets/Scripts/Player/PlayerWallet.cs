using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.VisualScripting;
using YG;

public class PlayerWallet
{
    private int _gold;
    private int _gem;

    public PlayerWallet()
    {
        YandexGame.GetDataEvent += SetValue;
    }

    ~PlayerWallet()
    {
        YandexGame.GetDataEvent -= SetValue;
    }

    public int Gold => _gold;
    public int Gem => _gem;

    public event Action<int> GoldChanged;
    public event Action GemChanged;

    public void AddGold(int gold)
    {
        if (gold >= 0)
        {
            _gold += gold;
            GoldChanged?.Invoke(_gold);
        }
    }

    public void AddGem()
    {
        _gem++;
        GemChanged?.Invoke();
    }

    public bool TrySpendGold(int gold)
    {
        if (gold > 0 & _gold - gold >= 0)
        {
            _gold -= gold;
            GoldChanged?.Invoke(_gold);

            return true;
        }
        return false;
    }

    public bool TrySpendGem(int gem)
    {
        if (gem > 0 & _gem - gem >= 0)
        {
            _gem -= gem;
            GemChanged?.Invoke();

            return true;
        }
        return false;
    }

    public void SaveWallet()
    {
        YandexGame.savesData.gold = _gold;
        YandexGame.savesData.gem = _gem;
    }

    private void SetValue()
    {
        if (YandexGame.savesData.gold == -1)
        {
            _gold = 800;
        }
        else
        {
            _gold = YandexGame.savesData.gold;
        }

        if (YandexGame.savesData.gem == -1)
        {
            _gem = 3;
        }
        else
        {
            _gem = YandexGame.savesData.gem;
        }
    }
}
