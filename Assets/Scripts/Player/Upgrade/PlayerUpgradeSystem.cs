using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YG;

[Serializable]
public class UpgradeData
{
    public readonly ReactValue<float> UpgradeDamageLevel = new ReactValue<float>();
    public readonly ReactValue<float> UpgradeShootSpeedLevel = new ReactValue<float>();
    public readonly ReactValue<float> UpgradeHealthLevel = new ReactValue<float>();

}

public class PlayerUpgradeSystem
{
    public PlayerUpgradeSystem()
    {
        if (YandexGame.savesData.upgradeDamageLevel != -1)
        {
            UpgradeData.UpgradeDamageLevel.Value = YandexGame.savesData.upgradeDamageLevel;
        }

        if (YandexGame.savesData.upgradeSpeedWeaponLevel != -1)
        {
            UpgradeData.UpgradeShootSpeedLevel.Value = YandexGame.savesData.upgradeSpeedWeaponLevel;
        }

        if (YandexGame.savesData.upgradeHealthLevel != -1)
        {
            UpgradeData.UpgradeHealthLevel.Value = YandexGame.savesData.upgradeHealthLevel;
        }
    }

    public UpgradeData UpgradeData { get; private set; } = new UpgradeData();

    public void ApplyUpgrade(Upgrade upgrade)
    {
        switch (upgrade)
        {
            case Upgrade.Damage:

                if (YandexGame.savesData.upgradeDamageLevel == -1)
                {
                    YandexGame.savesData.upgradeDamageLevel = 1;
                    UpgradeData.UpgradeDamageLevel.Value = 1;
                }
                else
                {
                    YandexGame.savesData.upgradeDamageLevel++;
                    UpgradeData.UpgradeDamageLevel.Value++;
                }

                break;

            case Upgrade.ShootSpeed:

                if (YandexGame.savesData.upgradeSpeedWeaponLevel == -1)
                {
                    YandexGame.savesData.upgradeSpeedWeaponLevel = 1;
                    UpgradeData.UpgradeShootSpeedLevel.Value = 1;
                }
                else
                {
                    YandexGame.savesData.upgradeSpeedWeaponLevel++;
                    UpgradeData.UpgradeShootSpeedLevel.Value++;
                }

                break;

            case Upgrade.Health:

                if (YandexGame.savesData.upgradeHealthLevel == -1)
                {
                    YandexGame.savesData.upgradeHealthLevel = 1;
                    UpgradeData.UpgradeHealthLevel.Value = 1;

                    YandexGame.SaveProgress();
                }
                else
                {
                    YandexGame.savesData.upgradeHealthLevel++;
                    UpgradeData.UpgradeHealthLevel.Value++;

                    YandexGame.SaveProgress();
                }

                break;
        }
    }


}
