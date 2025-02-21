using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IncreaseManager : MonoBehaviour
{
    [SerializeField] private List<UpgradeButton> _upgradeButtons;

    private void OnEnable()
    {
        foreach (UpgradeButton upgradeButton in _upgradeButtons)
        {
            if(upgradeButton != null)
            {
                if (upgradeButton.CountClicked == upgradeButton.MaxLevelUpgrade)
                {
                    var button = upgradeButton.GetComponent<Button>();

                    if (button != null)
                    {
                        button.interactable = false;
                    }
                }
            }
            
        }
    }
}
