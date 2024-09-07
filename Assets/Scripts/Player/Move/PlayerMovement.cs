using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

[RequireComponent(typeof(Rigidbody))]

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private NavMeshAgent _agent;
    [SerializeField] private Joystick _joystick;
    [SerializeField] private PlayerAnimationController _controller;
    [SerializeField] private float _speed;
    [SerializeField] private PlayerShooter _shooter;
    [SerializeField] private Rotate _rotate;
    [SerializeField] private Animator _weaponAnimator;

    private const string VerticalDirection = "Vertical";
    private const string HorizontalDirection = "Horizontal";

    private Rigidbody _rigidbody;
    private Vector3 _movement;

    [Inject]
    public void Construct(UISettings uISettings)
    {
        _joystick = uISettings.Joystick;
    }

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        Move();

        if (!_shooter.IsShooting && _movement != Vector3.zero)
        {
            _rotate.Direction = _movement;
            _rotate.IsShooting = false;          
        }
        

        if(_movement != Vector3.zero)
        {
            _weaponAnimator.SetBool("isRunning", true);
        }
        else
        {
            _weaponAnimator.SetBool("isRunning", false);
        }
    }

    private void Move()
    {
        float move = 0f;

        float xDirection = _joystick.Horizontal;
        float zDirection = _joystick.Vertical;

        if (Input.GetAxisRaw(HorizontalDirection) != 0 | Input.GetAxisRaw(VerticalDirection) != 0)
        {
            xDirection = Input.GetAxisRaw(HorizontalDirection);
            zDirection = Input.GetAxisRaw(VerticalDirection);

            move = 1;
        }

        if (_joystick.IsHandled)
        {
            move = 1;
        }

        _controller.PlayMoveAnimation(move);

        _movement = new Vector3(xDirection, 0, zDirection);
        _agent.speed = _speed;
        _agent.velocity = _movement.normalized * _agent.speed;
    }
}
