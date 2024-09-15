using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ActivateBonusButton : MonoBehaviour
{
    [SerializeField] private float _duration;
    [SerializeField] private int _cost;

    public UnityEvent<int> EnableBonus;
    public UnityEvent<int> DisableBonus;

    private bool _isButtonPressed = false;

    public void ActivateBonus()
    {
        if (!_isButtonPressed)
        {
            _isButtonPressed = true;
            Debug.Log(_isButtonPressed);

            StartCoroutine(StartTimer());

            EnableBonus.Invoke(_cost);
        }
    }

    private IEnumerator StartTimer()
    {
        yield return new WaitForSeconds(_duration);

        _isButtonPressed = false;
        Debug.Log(_isButtonPressed);
        StopCoroutine(StartTimer());

        DisableBonus.Invoke(_cost);
    }
}
