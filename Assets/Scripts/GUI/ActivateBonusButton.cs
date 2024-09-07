using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ActivateBonusButton : MonoBehaviour
{
    [SerializeField] private float _duration;

    public UnityEvent EnableBonus;
    public UnityEvent DisableBonus;

    private bool _isButtonPressed = false;

    public void ActivateBonus()
    {
        if (!_isButtonPressed)
        {
            _isButtonPressed = true;
            Debug.Log(_isButtonPressed);

            StartCoroutine(StartTimer());

            EnableBonus.Invoke();
        }
    }

    private IEnumerator StartTimer()
    {
        yield return new WaitForSeconds(_duration);

        _isButtonPressed = false;
        Debug.Log(_isButtonPressed);
        StopCoroutine(StartTimer());

        DisableBonus.Invoke();
    }
}
