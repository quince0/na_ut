using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;


[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    KeyCode JumpKey = KeyCode.Space;
    KeyCode RunKey = KeyCode.LeftShift;

    public float walkSpeed = 6;
    public float runSpeed = 18;
    public float jumpHeight = 1;
    [Range(0, 1)]
    public float airControlPercent;

    public float turnSmoothTime = 0.2f;
    float turnSmoothVelocity;

    public float speedSmoothTime = 0.1f;
    float speedSmoothVelocity;
    float currentSpeed;




    public Camera playerCamera;
    CharacterController playerController;
    Animator animator;

    void Start()
    {
        playerController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        Vector2 inputMove = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        Vector2 inputMoveDir = inputMove.normalized;
        bool running = Input.GetKey(RunKey);
        
        Move(inputMoveDir, running);
        Action();
    }

    void Move(Vector2 inputMoveDir, bool running)
    {
        if (inputMoveDir != Vector2.zero)
        {
            float targetRotation = Mathf.Atan2(inputMoveDir.x, inputMoveDir.y) * Mathf.Rad2Deg + playerCamera.transform.eulerAngles.y;
            transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref turnSmoothVelocity, GetModifiedSmoothTime(turnSmoothTime));
        }

        float targetSpeed = ((running) ? runSpeed : walkSpeed) * inputMoveDir.magnitude;
        currentSpeed = Mathf.SmoothDamp(currentSpeed, targetSpeed, ref speedSmoothVelocity, GetModifiedSmoothTime(speedSmoothTime));

        Vector3 velocity = transform.forward * currentSpeed;

        playerController.Move(velocity * Time.deltaTime);
        currentSpeed = new Vector2(playerController.velocity.x, playerController.velocity.z).magnitude;
    }

    float GetModifiedSmoothTime(float smoothTime)
    {
        if (playerController.isGrounded)
        {
            return smoothTime;
        }

        if (airControlPercent == 0)
        {
            return float.MaxValue;
        }

        return smoothTime / airControlPercent;
    }

    void Action()
    {
        // Attack
        if (Input.GetMouseButtonDown(0))
        {
            
        }

        // Interact
        if (Input.GetMouseButtonDown(1))
        {
            Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                Interactable interactable = hit.collider.GetComponent<Interactable>();
                if (interactable != null)
                {
                    Player.playerInstance.InteractWithInteractable(interactable, transform);
                }
            }
        }
    }
}