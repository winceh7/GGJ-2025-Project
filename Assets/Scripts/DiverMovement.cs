using System;
using UnityEngine;

public class UnderwaterMovement : MonoBehaviour
{
    private Animator animator;
    private Rigidbody2D rb2d;

    public float speed = 2f; // Movement speed
    private float animationSpeedFactor = 0.3f;

    private float prevXNonZero = 0;

    void Start()
    {
        animator = GetComponent<Animator>();
        rb2d = GetComponent<Rigidbody2D>();
    }

    public void SetSpeedAndDirection(float prevXNonZero, float x, float y, float speed)
    {
        float horizontalFlip = 0;
        float rotation = 0;

        // Determine rotation based on movement direction
        if (Math.Abs(x) < Math.Abs(y) - 0.05f)
        {
            rotation = y < 0 ? -90 : 90;
        }
        else if (prevXNonZero < 0)
        {
            horizontalFlip = 180; // Flip horizontally if moving left
        }

        transform.rotation = Quaternion.Euler(0, horizontalFlip, rotation);

        // Update animator parameters
        animator.speed = speed == 0 ? 1 : speed * animationSpeedFactor;
        animator.SetFloat("speed", speed);
        animator.SetFloat("xInput", x);
        animator.SetFloat("yInput", y);
    }

    public void UpdateLampPose(float x, float y, float theta)
    {
        GameObject lamp = GameObject.FindGameObjectWithTag("DiverLamp");
        float xOffset = (float)Math.Cos(theta * Mathf.Deg2Rad);
        float yOffset = (float)Math.Sin(theta * Mathf.Deg2Rad);

        lamp.transform.position = new Vector3(x + xOffset, y + yOffset);
        lamp.transform.rotation = Quaternion.Euler(0, 0, theta);
    }

    void Update()
    {
        bool dead = GameObject.FindWithTag("Player").GetComponent<OxygenManagement>().dead;

        if (!dead)
        {
            // Get input for movement
            float moveX = Input.GetAxis("Horizontal");
            float moveY = Input.GetAxis("Vertical");

            // Create a movement vector
            Vector2 move = new Vector2(moveX, moveY);

            // Normalize movement to prevent diagonal speed boost
            if (move.magnitude > 1)
            {
                move.Normalize();
            }

            // Apply movement using Rigidbody2D
            rb2d.velocity = move * speed;

            // Calculate movement speed
            float movementSpeed = rb2d.velocity.magnitude;

            // Update direction and animation
            SetSpeedAndDirection(prevXNonZero, rb2d.velocity.x, rb2d.velocity.y, movementSpeed);

            // Update lamp position and rotation
            Vector2 playerPos = rb2d.position;
            float direction = Mathf.Atan2(rb2d.velocity.y, rb2d.velocity.x) * Mathf.Rad2Deg;

            if (movementSpeed > 0)
            {
                UpdateLampPose(playerPos.x, playerPos.y, direction);
            }

            // Keep track of the last non-zero horizontal movement
            prevXNonZero = moveX != 0 ? moveX : prevXNonZero;
        }
        else
        {
            // Stop all movement if the player is dead
            rb2d.velocity = Vector2.zero;
            SetSpeedAndDirection(prevXNonZero, 0, 0, 0);
        }
    }
}