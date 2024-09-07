using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class UpgradeScreen : MonoBehaviour
{
    [SerializeField] private List<UpgradeButton> _upgradeButtons;

    private PlayerUpgradeSystem _playerUpgradeSystem;


    [Inject]
    public void Construct(PlayerUpgradeSystem playerUpgradeSystem)
    {
        _playerUpgradeSystem = playerUpgradeSystem;

        for (int i = 0; i < _upgradeButtons.Count; i++)
        {
            _upgradeButtons[i].Clicked += _playerUpgradeSystem.ApplyUpgrade;
        }
    }

    private void OnDestroy()
    {
        for (int i = 0; i < _upgradeButtons.Count; i++)
        {
            _upgradeButtons[i].Clicked -= _playerUpgradeSystem.ApplyUpgrade;
        }
    }
}
