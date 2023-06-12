using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(PlayerStatistic))]
public class PlayerMovement : MonoBehaviour
{
    private Animator animator;
    private CharacterController controller;
    private PlayerStatistic playerStatistic;
    private Vector2 input;
    private Vector3 rootMotion;
    private Vector3 velocity;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        controller = GetComponentInChildren<CharacterController>();
        playerStatistic = GetComponentInChildren<PlayerStatistic>();
    }

    private void Update()
    {
        input = new(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        animator.SetFloat("InputX", input.x);
        animator.SetFloat("InputY", input.y);
        animator.SetBool("IsJumping", playerStatistic.isJumping);
        animator.SetFloat("MotionSpeed", playerStatistic.playerSpeed);

        if (Input.GetKeyDown(KeyCode.Space)) Jump();
    }

    private void OnAnimatorMove()
    {
        rootMotion += animator.deltaPosition;
    }

    private void LateUpdate()
    {
        if (playerStatistic.isJumping)
        {
            UpdateInAir();
        }

        else
        {
            UpdateOnGround();
        }
    }

    /// <summary>
    /// Move in air
    /// </summary>
    private void UpdateInAir()
    {
        velocity.y -= playerStatistic.gravity * Time.deltaTime;
        Vector3 displacement = velocity * Time.deltaTime;
        displacement += CalculateAirControl();
        controller.Move(displacement);
        playerStatistic.isJumping = !controller.isGrounded;
        rootMotion = Vector3.zero;
    }


    /// <summary>
    /// Move on ground
    /// </summary>
    private void UpdateOnGround()
    {
        Vector3 stepForwardAmount = rootMotion * playerStatistic.playerSpeed;
        Vector3 stepDownAmount = Vector3.down * playerStatistic.stepDown;

        controller.Move(stepForwardAmount + stepDownAmount);
        rootMotion = Vector3.zero;

        if (controller.isGrounded) return;
        // When falling
        SetInAir(0);
    }

    Vector3 CalculateAirControl()
    {
        return (((transform.forward * input.y) + (transform.right * input.x)) * (playerStatistic.airControl / 100)); 
    }

    /// <summary>
    /// On jump press
    /// </summary>
    private void Jump()
    {
        if (playerStatistic.isJumping) return;
        float jumpVelocity = Mathf.Sqrt(2 * playerStatistic.gravity * playerStatistic.jumpHeigth);
        SetInAir(jumpVelocity);
    }


    /// <summary>
    /// Call on jumping
    /// </summary>
    /// <param name="jumpVelocity"></param>
    private void SetInAir(float jumpVelocity)
    {
        playerStatistic.isJumping = true;
        velocity = playerStatistic.jumpDamp * playerStatistic.playerSpeed * animator.velocity;
        velocity.y = jumpVelocity;
    }
}
