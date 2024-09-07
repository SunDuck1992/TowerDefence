using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildArea : MonoBehaviour
{
    [SerializeField] private Transform _buildPoint;

    public Transform BuildPoint => _buildPoint;
    public bool OnBuild { get; set; }

    public BuildTowersSystem BuildTowersSystem { get; set; }

    private void OnTriggerEnter(Collider other)
    {
        BuildTowersSystem.OnInteractBuildArea(this);
        Debug.Log("player here");
    }

    private void OnTriggerExit(Collider other)
    {
        BuildTowersSystem.OnDeInteractBuildArea();
    }
}
