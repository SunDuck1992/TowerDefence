using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class UISettings
{
    [SerializeField] private ActivateBonusButton _slowEnemyButton;
    [SerializeField] private ActivateBonusButton _massDamageButton;
    [SerializeField] private ActivateBonusButton _repairTowersButton;
    [SerializeField] private Joystick _joystickHorizontal;
    [SerializeField] private Joystick _joystickVertical;

    public bool IsMobile { get; set; }

    public ActivateBonusButton SlowEnemyButton => _slowEnemyButton;
    public ActivateBonusButton MassDamageButton => _massDamageButton;
    public ActivateBonusButton RepairTowersButton => _repairTowersButton;
    public Joystick Joystick => IsMobile ? _joystickVertical : _joystickHorizontal;
}
