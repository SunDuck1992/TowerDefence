using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerWallet
{
    private int _gold;
    private int _gem;

    public int Gold => _gold;
    public int Gem => _gem;

    public event Action GoldChanged;

    public void AddGold(int gold)
    {
        if (gold < 0)
        {
            _gold += gold;
            GoldChanged?.Invoke();
        }
    }

    public bool TrySpendGold(int gold)
    {
        return true; // временное решение

        if (gold > 0 & _gold - gold >= 0)
        {
            _gold -= gold;
            GoldChanged?.Invoke();

            return true;
        }
        return false;
    }
}
