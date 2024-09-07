using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class UpgradeData
{
    public ReactValue<int> UpgradeDamageLevel = new ReactValue<int>();
    public ReactValue<float> UpgradeShootSpeedLevel = new ReactValue<float>();
    public ReactValue<int> UpgradeHealthLevel = new ReactValue<int>();

}

public class PlayerUpgradeSystem
{
    public UpgradeData UpgradeData { get; private set; } = new UpgradeData();

    public void ApplyUpgrade(Upgrade upgrade)
    {
        switch (upgrade)
        {
            case Upgrade.Damage:
                UpgradeData.UpgradeDamageLevel.Value++;
                break;

            case Upgrade.ShootSpeed:
                UpgradeData.UpgradeShootSpeedLevel.Value++;
                break;

            case Upgrade.Health:
                UpgradeData.UpgradeHealthLevel.Value++;
                break;
        }
    }
}
