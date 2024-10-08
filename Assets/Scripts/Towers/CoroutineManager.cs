using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoroutineManager : MonoBehaviour
{
    private static CoroutineManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public static Coroutine StartStaticCoroutine(IEnumerator coroutine)
    {
        return instance.StartCoroutine(coroutine);
    }

    public static void StopStaticCoroutine(Coroutine coroutine)
    {
        if (coroutine != null)
            instance.StopCoroutine(coroutine);
    }
}
