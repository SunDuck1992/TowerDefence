using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AnimationEventListener : MonoBehaviour
{
    public UnityEvent Attack;

   public void EventAttack()
    {
       Attack.Invoke();
    }
}
