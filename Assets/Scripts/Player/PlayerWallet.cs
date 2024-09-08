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
        _gold = 2000;
        _gem = 3;
    }

    public int Gold => _gold;
    public int Gem => _gem;

    public event Action GoldChanged;

    public void AddGold(int gold)
    {
        if (gold >= 0)
        {
            _gold += gold;
            GoldChanged?.Invoke();
        }
    }

    public bool TrySpendGold(int gold)
    {
        //return true; // ��������� �������

        if (gold > 0 & _gold - gold >= 0)
        {
            _gold -= gold;
            GoldChanged?.Invoke();

            return true;
        }
        return false;
    }
}
