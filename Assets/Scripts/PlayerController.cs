using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Data;
using Mirror;

public class PlayerController : NetworkBehaviour
{
    public CharacterController characterController;
    public float speed = 10;
    private float gravity = 9.87f;
    private float verticalSpeed = 0;

    public Camera camera;

    public Animator animator;

    void Start()
    {
        characterController.enabled = hasAuthority;
        camera.gameObject.SetActive(hasAuthority);
    }

    void FixedUpdate()
    {
        if (!hasAuthority || characterController == null)
            return;

        Move();
        Think();
    }

    private void Move()
    {
        float horizontalMove = Input.GetAxis("Horizontal");
        float verticalMove = Input.GetAxis("Vertical");

        if (characterController.isGrounded) verticalSpeed = 0;
        else verticalSpeed -= gravity * Time.deltaTime;

        Vector3 gravityMove = new Vector3(0, verticalSpeed, 0);
        Vector3 move = CameraForward() * verticalMove + CameraRight() * horizontalMove;

        characterController.Move(speed * Time.deltaTime * move + gravityMove * Time.deltaTime);
        animator.SetBool("move", verticalMove != 0 || horizontalMove != 0);
        
        if (move.sqrMagnitude < 0.01f) return;
        transform.rotation = Quaternion.LookRotation(move);
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
}
