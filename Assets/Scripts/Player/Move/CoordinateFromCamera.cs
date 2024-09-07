using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoordinateFromCamera : MonoBehaviour
{
    [SerializeField] private PlayerMovement _playerMovement;

    private Vector3 _offset;

    private void Start()
    {
        _offset = _playerMovement.transform.position - transform.position;
    }

    private void Update()
    {
        transform.position = _playerMovement.transform.position - _offset;
    }
}
