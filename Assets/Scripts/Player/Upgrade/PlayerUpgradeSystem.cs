using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class UpgradeData
{
    public readonly ReactValue<float> UpgradeDamageLevel = new ReactValue<float>();
    public readonly ReactValue<float> UpgradeShootSpeedLevel = new ReactValue<float>();
    public readonly ReactValue<float> UpgradeHealthLevel = new ReactValue<float>();

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
                //Debug.Log(UpgradeData.UpgradeHealthLevel.Value + " - значение Value до изменения");
                UpgradeData.UpgradeHealthLevel.Value++;
                break;
        }
    }

    
}
