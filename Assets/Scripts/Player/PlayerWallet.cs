using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.VisualScripting;

public class PlayerWallet
{
    private int _gold;
    private int _gem;

    public PlayerWallet()
    {
        _gold = 1000;
        _gem = 3;
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
}
