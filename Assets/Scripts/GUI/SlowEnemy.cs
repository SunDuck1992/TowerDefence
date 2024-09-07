using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SlowEnemy : MonoBehaviour
{
    [SerializeField] private UnityEvent EnableSlowEnemy;
    [SerializeField] private UnityEvent DisableSlowEnemy;
    [SerializeField] private float _duration;

    public void ActivateSlow()
    {

    }
}
