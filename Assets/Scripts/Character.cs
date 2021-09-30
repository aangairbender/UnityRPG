using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Data;
using Mirror;

[RequireComponent(typeof(CharacterController))]
public class Character : NetworkBehaviour
{
    [Header("Speed")]
    public float walkSpeed = 2;
    public float runSpeed = 6;

    [Header("Dependencies")]
    public Camera camera;
    public Animator animator;

    private Vector3 verticalVelocity = Vector3.zero;
    private CharacterController characterController;

    [SyncVar] [SerializeField] private bool isWalking;
    [SyncVar] [SerializeField] private bool isRunning;
    [SyncVar] [SerializeField] private bool isJumping;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        characterController.enabled = hasAuthority;
        camera.gameObject.SetActive(hasAuthority);
    }

    void FixedUpdate()
    {
        if (hasAuthority)
        {
            HandleInput();
        }
    }

    void LateUpdate()
    {
        UpdateAnimator();
    }

    private void HandleInput()
    {
        var movement = Vector3.zero;

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        bool sprint = Input.GetKey(KeyCode.LeftShift);
        if (Mathf.Max(Mathf.Abs(horizontal), Mathf.Abs(vertical)) > 0.001f)
        {
            var direction = CameraForward() * vertical + CameraRight() * horizontal;
            transform.rotation = Quaternion.LookRotation(direction);
            isWalking = !sprint;
            isRunning = sprint;
            
            movement += (isWalking ? walkSpeed : 0f) * Time.deltaTime * direction;
            movement += (isRunning ? runSpeed : 0f) * Time.deltaTime * direction;
        } else
        {
            isWalking = false;
            isRunning = false;
        }

        bool jump = Input.GetKey(KeyCode.Space);
        if (characterController.isGrounded)
        {
            verticalVelocity = Vector3.zero;
            isJumping = jump;
        } else
        {
            verticalVelocity += Physics.gravity * Time.deltaTime;
        }
        movement += verticalVelocity * Time.deltaTime;

        characterController.Move(movement);
    }

    private Vector3 CameraForward()
    {
        var forward = camera.transform.forward;
        forward.y = 0f;
        return forward.normalized;
    }

    private Vector3 CameraRight()
    {
        var right = camera.transform.right;
        right.y = 0f;
        return right.normalized;
    }

    private void Think()
    {
        bool space = Input.GetKey("space");
        animator.SetBool("jump", space);
    }

    private void UpdateAnimator()
    {
        animator.SetBool("walk", isWalking);
        animator.SetBool("run", isRunning);
        animator.SetBool("jump", isJumping);
    }
}
