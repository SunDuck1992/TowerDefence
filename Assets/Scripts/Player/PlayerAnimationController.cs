using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    [SerializeField] private Animator _animator;

    public bool IsShooting { get; set; }

    public void PlayMoveAnimation(float value)
    {
        if (IsShooting)
        {
            _animator.SetFloat("Move", value);
        }
        else
        {
            _animator.SetFloat("Move", value < 0? 1 : value);
        }
    } 
}
