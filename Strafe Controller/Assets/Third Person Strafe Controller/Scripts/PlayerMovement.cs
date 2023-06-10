using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    private Animator animator;
    private CharacterController controller;
    private Vector2 input;
    private Vector3 rootMotion;
    private Vector3 velocity;

    public float playerSpeed = 1.0f;
    public float jumpHeigth = 3.0f;
    public float gravity = 20.0f;
    public float stepDown = 0.2f;
    public float airControl = 0.5f;
    public float jumpDamp = 0.5f;
    public bool isJumping;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        controller = GetComponentInChildren<CharacterController>();
    }

    private void Update()
    {
        input = new(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        animator.SetFloat("InputX", input.x);
        animator.SetFloat("InputY", input.y);
        animator.SetBool("IsJumping", isJumping);
        animator.SetFloat("MotionSpeed", playerSpeed);

        if (Input.GetKeyDown(KeyCode.Space)) Jump();
    }

    private void OnAnimatorMove()
    {
        rootMotion += animator.deltaPosition;
    }

    private void LateUpdate()
    {
        if (isJumping)
        {
            velocity.y -= gravity * Time.deltaTime;

            Vector3 displacement = velocity * Time.deltaTime;
            displacement += CalculateAirControl();
            controller.Move(displacement);

            //controller.Move(velocity * Time.deltaTime);
            isJumping = !controller.isGrounded;
            rootMotion = Vector3.zero;
        }

        else
        {
            controller.Move((rootMotion*playerSpeed) + Vector3.down * stepDown);
            rootMotion = Vector3.zero;

            if (controller.isGrounded) return;

            // When falling
            isJumping = true;
            velocity = animator.velocity * jumpDamp * playerSpeed;
            velocity.y = 0;
        }
    }

    Vector3 CalculateAirControl()
    {
        return (((transform.forward * input.y) + (transform.right * input.x)) * (airControl / 100)); 
    }

    private void Jump()
    {
        if (isJumping) return;

        isJumping = true;
        velocity = animator.velocity * jumpDamp * playerSpeed;
        float jumpVelocity = Mathf.Sqrt(2 * gravity * jumpHeigth);
        velocity.y = jumpVelocity;
    }
}
