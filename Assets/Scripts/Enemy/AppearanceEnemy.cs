using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AppearanceEnemy : MonoBehaviour
{
    [SerializeField] private List<GameObject> _bodies;
    [SerializeField] private List<GameObject> _glowes;


    private void OnEnable()
    {
        int randomBodies = Random.Range(0, _bodies.Count);
        int randomGlowes = Random.Range(0, _glowes.Count);

        _bodies[randomBodies].gameObject.SetActive(true);
        _glowes[randomGlowes].gameObject.SetActive(true);
    }

    private void OnDisable()
    {
        foreach (var body in _bodies)
        {
            body.gameObject.SetActive(false);
        }
        
        foreach (var glow in _glowes)
        {
            glow.gameObject.SetActive(false);
        }
    }
}
