using System;
using Custom.CoroutineExtensions;
using Photon.Pun;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Animator))]
public class LocalGamePlayerController : MonoBehaviour
{
    private const float PlayerSpeed = 4.0f;
    private const float JumpHeight = 0.6f;
    private const float GravityValue = -25f;
    private const float SprintSpeedMulti = 1.4f;
    private const float CrouchSpeedMulti = 0.5f;
    private const float LeapValue = 8f;
    
    private CharacterController _controller;
    private Animator _animator;
    
    [ReadOnly] [SerializeField] private Vector3 _playerVelocity;
    [ReadOnly] [SerializeField] private bool _grounded;
    [ReadOnly] [SerializeField] private bool _inLeap;
    [ReadOnly] [SerializeField] private bool _inJump;
    [ReadOnly] [SerializeField] private bool _allowMovement;

    private InputAction _moveAction;
    private InputAction _jumpAction;
    private InputAction _sprintAction;
    private InputAction _crouchAction;
    private InputAction _interactAction;
    

    private void Awake()
    {
        
        _controller = gameObject.GetComponent<CharacterController>();
        _animator =  gameObject.GetComponent<Animator>();
    }

    private void Start()
    {
        _moveAction = InputSystem.actions.FindAction("Move");
        _jumpAction = InputSystem.actions.FindAction("Jump");
        _sprintAction = InputSystem.actions.FindAction("sprint");
        _crouchAction = InputSystem.actions.FindAction("crouch");
        _interactAction = InputSystem.actions.FindAction("interact");

        _allowMovement = true;
    }

    void Update()
    {
        _grounded = _controller.isGrounded;
        if (_grounded && _playerVelocity.y < 0)
        {
            _playerVelocity.y = -1f;
            
            if (_inLeap)
            {
                _inLeap = false;
                _animator.SetBool("Leap", false);
                _playerVelocity = Vector3.zero;
                _allowMovement = false;
                this.WaitSecondsToExecute(0.6f, () => { _allowMovement = true; });
            }

            if (_inJump)
            {
                _inJump = false;
            }
        }

        var moveActionResult = _moveAction.ReadValue<Vector2>();
        var move = new Vector3(moveActionResult.x, 0, moveActionResult.y);
        if (move.magnitude > 1) move = move.normalized;

        if (_allowMovement)
        {
            if (!_inLeap)
            {
                var speedMulti = 1 * ((_crouchAction.IsPressed()) ? CrouchSpeedMulti : 1);
                speedMulti *= (!_crouchAction.IsPressed() && _sprintAction.IsPressed()) ? SprintSpeedMulti : 1;
                _controller.Move(move * Time.deltaTime * PlayerSpeed * speedMulti);
            
                if (move != Vector3.zero)
                {
                    transform.forward = Vector3.Slerp(transform.forward, move.normalized, Time.deltaTime * 16);
                }
            }

            // Makes the player jump
            if (_jumpAction.WasPressedThisFrame())
            {
                if (_grounded && !_inJump)
                {
                    _playerVelocity.y += Mathf.Sqrt(JumpHeight * -2 * GravityValue);
                    _inJump = true;
                }
                else if(_inJump && !_inLeap && move != Vector3.zero)
                {
                    transform.forward = move;
                    _playerVelocity += new Vector3(moveActionResult.x, 0, moveActionResult.y) * LeapValue + transform.up * LeapValue * 0.3f;
                    _inLeap = true;
                    _animator.SetBool("Leap", true);
                }
            }
        }

        _playerVelocity.y += GravityValue * Time.deltaTime * ((_playerVelocity.y < 0) ? 1.4f : 1);
        _controller.Move(_playerVelocity * Time.deltaTime);
    }
}
