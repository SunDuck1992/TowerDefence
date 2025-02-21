using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using static UnityEngine.UI.GridLayoutGroup;

public class TestTower : MonoBehaviour
{
    [SerializeField] private Transform _shotPointPosition;
    [SerializeField] private TestRocket _prefab;

    public GameUnit target;
    public float damage;
    private TestRocket _rocket;

    private void Start()
    {
        StartCoroutine(SpawnRocketWithDelay());
    }

    private bool IsSpawnAreaClear()
    {
        Collider[] colliders = Physics.OverlapSphere(_shotPointPosition.position, 1f);
        return !colliders.Any(c => c.GetComponent<Rocket>() != null);
    }

    private void SpawnRocket()
    {
        _rocket = Instantiate(_prefab);
        _rocket.Target = target;
        _rocket.Damage = damage;
        _rocket.transform.position = _shotPointPosition.position;
        _rocket.transform.forward = _shotPointPosition.forward;
    }

    private IEnumerator SpawnRocketWithDelay()
    {
        while (target != null)
        {
            if (IsSpawnAreaClear())
            {
                SpawnRocket();
            }
            else
            {
                yield return new WaitForSeconds(0.2f);

                if (IsSpawnAreaClear())
                {
                    SpawnRocket();
                }
            }

            yield return new WaitForSeconds(1f);
        }
    }
}
