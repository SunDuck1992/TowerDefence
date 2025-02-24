using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilityManager : MonoBehaviour
{
    [SerializeField] private WaveScreen _waveScreen;
    [SerializeField] private List<Button> _buttons;

    private void OnEnable()
    {
        _waveScreen.OnEndBattle += TurnOffTheButton;
    }

    private void OnDisable()
    {
        _waveScreen.OnEndBattle -= TurnOffTheButton;
    }

    private void TurnOffTheButton()
    {
        for (int i = 0; i < _buttons.Count; i++)
        {
            _buttons[i].interactable = false;
        }
    }

    public void TurnOnTheButton()
    {
        for (int i = 0; i < _buttons.Count; i++)
        {
            _buttons[i].interactable = true;
        }
    }
}
