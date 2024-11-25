using System;
using Custom.CoroutineExtensions;
using Photon.Pun;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Animator))]
public class LocalGamePlayerController : MonoBehaviour
{
    private const float playerSpeed = 4.0f;
    private const float jumpHeight = 0.6f;
    private const float gravityValue = -25f;
    private const float sprintSpeedMulti = 1.4f;
    private const float crouchSpeedMulti = 0.5f;
    private const float LeapValue = 8f;
    
    private CharacterController controller;
    private Animator animator;

    [ReadOnly] [SerializeField] private Vector3 playerVelocity;
    [ReadOnly] [SerializeField] private bool groundedPlayer;
    [ReadOnly] [SerializeField] private bool inLeap;
    [ReadOnly] [SerializeField] private bool inJump;
    [ReadOnly] [SerializeField] private bool allowMovement;

    private InputAction moveAction;
    private InputAction jumpAction;
    private InputAction sprintAction;
    private InputAction crouchAction;
    private InputAction interactAction;
    

    private void Awake()
    {
        
        controller = gameObject.GetComponent<CharacterController>();
        animator =  gameObject.GetComponent<Animator>();
    }

    private void Start()
    {
        moveAction = InputSystem.actions.FindAction("Move");
        jumpAction = InputSystem.actions.FindAction("Jump");
        sprintAction = InputSystem.actions.FindAction("sprint");
        crouchAction = InputSystem.actions.FindAction("crouch");
        interactAction = InputSystem.actions.FindAction("interact");

        allowMovement = true;
    }

    void Update()
    {
        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = -1f;
            
            if (inLeap)
            {
                inLeap = false;
                animator.SetBool("Leap", false);
                playerVelocity = Vector3.zero;
                allowMovement = false;
                this.WaitSecondsToExecute(0.6f, () => { allowMovement = true; });
            }

            if (inJump)
            {
                inJump = false;
            }
        }

        var moveActionResult = moveAction.ReadValue<Vector2>();
        var move = new Vector3(moveActionResult.x, 0, moveActionResult.y);
        if (move.magnitude > 1) move = move.normalized;

        if (allowMovement)
        {
            if (!inLeap)
            {
                var speedMulti = 1 * ((crouchAction.IsPressed()) ? crouchSpeedMulti : 1);
                speedMulti *= (!crouchAction.IsPressed() && sprintAction.IsPressed()) ? sprintSpeedMulti : 1;
                controller.Move(move * Time.deltaTime * playerSpeed * speedMulti);
            
                if (move != Vector3.zero)
                {
                    transform.forward = Vector3.Slerp(transform.forward, move.normalized, Time.deltaTime * 26);
                }
            }

            // Makes the player jump
            if (jumpAction.WasPressedThisFrame())
            {
                if (groundedPlayer && !inJump)
                {
                    playerVelocity.y += Mathf.Sqrt(jumpHeight * -2 * gravityValue);
                    inJump = true;
                }
                else if(inJump && !inLeap && move != Vector3.zero)
                {
                    transform.forward = move;
                    playerVelocity += new Vector3(moveActionResult.x, 0, moveActionResult.y) * LeapValue + transform.up * LeapValue * 0.3f;
                    inLeap = true;
                    animator.SetBool("Leap", true);
                }
            }
        }

        playerVelocity.y += gravityValue * Time.deltaTime * ((playerVelocity.y < 0) ? 1.4f : 1);
        controller.Move(playerVelocity * Time.deltaTime);
    }
}
