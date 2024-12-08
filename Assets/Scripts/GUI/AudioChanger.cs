using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioChanger : MonoBehaviour
{
    [SerializeField] private float _duration;

    public void ChangeVolume(float targetVolume)
    {
       StartCoroutine(FadeOut(targetVolume));
    }

    private IEnumerator FadeOut(float targetVolume)
    {
        float startVolume = AudioListener.volume;

        for (float i = 0; i < _duration; i += Time.deltaTime)
        {
            AudioListener.volume = Mathf.Lerp(startVolume, targetVolume, i / _duration);
            yield return null;
        }

        AudioListener.volume = targetVolume;
    }
}
