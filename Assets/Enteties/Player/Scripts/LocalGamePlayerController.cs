using System;
using System.Collections.Generic;
using System.Linq;
using Custom.CoroutineExtensions;
using NUnit.Framework;
using Photon.Pun;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class LocalGamePlayerController : MonoBehaviour
{
    [SerializeField] private Transform interactionOrigin;
    [SerializeField] private Animator animator;
    
    private const float PlayerSpeed = 5.0f;
    private const float JumpHeight = 0.75f;
    private const float GravityValue = -25f;
    private const float SprintSpeedMulti = 1.4f;
    private const float CrouchSpeedMulti = 0.5f;
    private const float LeapValue = 8f;
    
    private CharacterController _controller;
    
    
    private Vector3 _playerVelocity;
    private Vector3 _move;
    private bool _grounded;
    private bool _inLeap;
    private bool _inJump;
    private bool _allowMovement;
    private bool _leapStun;
    private bool _caughtCreatureInLeap;

    private InputAction _moveAction;
    private InputAction _jumpAction;
    private InputAction _sprintAction;
    private InputAction _crouchAction;
    private InputAction _interactAction;

    private LayerMask _creatureLayerMask;
    

    private void Awake()
    {
        _creatureLayerMask = LayerMask.GetMask("Creature");
        
        _controller = gameObject.GetComponent<CharacterController>();
        GameData.ChatManager.OnChatMode += ChatMode;
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
    
    private void ChatMode(bool state)
    {
        _allowMovement = !state;
    }

    void Update()
    {
        if ((_interactAction.WasPressedThisFrame() || _leapStun) && !_caughtCreatureInLeap)
        {
            if (CheckForCreature(out var creature))
            {
                GameData.CreatureManager.CreatureCaught(creature);
                if (_leapStun) _caughtCreatureInLeap = true;
            }
        }
        
        _grounded = _controller.isGrounded;
        if (_grounded && _playerVelocity.y < 0)
        {
            _playerVelocity.y = -1f;
            
            if (_inLeap)
            {
                _inLeap = false;
                _leapStun = true;

                this.WaitSecondsToExecute(0.2f, () =>
                {
                    animator.SetBool("Leap", false);
                });
                
                this.WaitSecondsToExecute(0.6f, () =>
                {
                    _leapStun = false;
                    _playerVelocity = Vector3.zero;
                    _move = Vector3.zero;
                    _caughtCreatureInLeap = false;
                });
            }

            if (_inJump)
            {
                _inJump = false;
            }
        }

        var moveActionResult = _moveAction.ReadValue<Vector2>();
        _move = Vector3.Lerp(_move, new Vector3(moveActionResult.x, 0, moveActionResult.y), Time.deltaTime * 16) ;
        if (_move.magnitude > 1) _move = _move.normalized;

        if (_allowMovement && !_leapStun)
        {
            if (!_inLeap)
            {
                var speedMulti = 1 * ((_crouchAction.IsPressed()) ? CrouchSpeedMulti : 1);
                speedMulti *= (!_crouchAction.IsPressed() && _sprintAction.IsPressed()) ? SprintSpeedMulti : 1;
                _controller.Move(_move * Time.deltaTime * PlayerSpeed * speedMulti);
            
                if (_move != Vector3.zero)
                {
                    transform.forward = Vector3.Slerp(transform.forward, _move.normalized, Time.deltaTime * 16);
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
                else if(_inJump && !_inLeap && _move != Vector3.zero)
                {
                    transform.forward = _move;
                    _playerVelocity += new Vector3(moveActionResult.x, 0, moveActionResult.y) * LeapValue + transform.up * LeapValue * 0.3f;
                    _inLeap = true;
                    animator.SetBool("Leap", true);
                }
            }
        }

        if (_leapStun)
        {
            _playerVelocity.x = Mathf.Lerp(_playerVelocity.x, 0, Time.deltaTime * 4);
            _playerVelocity.z = Mathf.Lerp(_playerVelocity.z, 0, Time.deltaTime * 4);
        }

        _playerVelocity.y += GravityValue * Time.deltaTime * ((_playerVelocity.y < 0) ? 1.4f : 1);
        _controller.Move(_playerVelocity * Time.deltaTime);
    }

    private bool CheckForCreature(out Creature closestCreature)
    {
        closestCreature = null;
        var cols = Physics.OverlapSphere(interactionOrigin.position, 1.4f, 
            _creatureLayerMask);
        if (cols == null || cols.Length == 0) return false;

        List<Creature> creatures = new List<Creature>();
        foreach (var col in cols)
        {
            Creature creature = col.GetComponent<Creature>();
            if(creature != null) creatures.Add(creature);
        }

        if (creatures.Count == 0) return false;

        closestCreature = GetClosestCreature(creatures, interactionOrigin.position);
        
        return true;
    }

    private Creature GetClosestCreature(List<Creature> colliders, Vector3 position)
    {
        Creature closest = null;
        float dist = float.MaxValue;

        foreach (var collider in colliders)
        {
            float newDist = Vector3.Distance(position, collider.transform.position);
            if (newDist < dist)
            {
                closest = collider;
                dist = newDist;
            }
        }

        return closest;
    }

    private void OnDrawGizmos()
    {
        if (interactionOrigin != null)
        {
            Gizmos.color = new Color(0, 0, 1, 0.4f);
            Gizmos.DrawSphere(interactionOrigin.position, 1.4f);
        }
    }
}
