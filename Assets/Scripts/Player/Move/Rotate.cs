using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    [SerializeField] private float _rotationSpeed;
    [SerializeField] private GameObject _playerView;

    private bool _isShooting;

    public bool IsShooting 
    {
        get => _isShooting; 
        set
        {
            _isShooting = value;

            if (!_isShooting)
            {
                _playerView.transform.localRotation = Quaternion.identity;
            }
        }
    }

    public Vector3 Direction { get; set; }

    private void Update()
    {
        Quaternion rotation = Quaternion.LookRotation(Direction);

        if(_isShooting)
        {
            _playerView.transform.rotation = Quaternion.RotateTowards(_playerView.transform.rotation, rotation, _rotationSpeed);
        }
        else
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, _rotationSpeed);
        }
    }
}
