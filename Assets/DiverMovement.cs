using System;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
public class UnderwaterMovement : MonoBehaviour
{
    private Animator animator;

    public float speed = 0.001f; // Movement speed

    private CharacterController controller;

    private float animationSpeedFactor = 0.3f;

    private float prevXNonZero = 0;

    void Start()
    {
        animator = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
    }

    public void SetSpeedAndDirection(float prevXNonZero, float x, float y, float speed)
    {
        float horizontalFlip = 0;
        float rotation = 0;
        if (Math.Abs(x) < Math.Abs(y)-0.05)
        { 
            rotation = y < 0 ? -90 : 90;
        }
        if (prevXNonZero < 0)
        {
            horizontalFlip = 180;
        }

        transform.rotation = Quaternion.Euler(0, horizontalFlip, rotation);

        animator.speed = speed == 0? 1:speed * animationSpeedFactor;
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
        bool dead = GameObject.FindWithTag("PlayerHitbox").GetComponent<OxygenManagement>().dead;

        if (!dead)
        {
            // Get input for movement
            float moveX = Input.GetAxis("Horizontal");
            float moveY = Input.GetAxis("Vertical");
            // Create a movement vector
            Vector3 move = new Vector3(moveX, moveY, 0);

            if (move.magnitude > 1)
            {
                move.Normalize();
            }

            controller.Move(move * speed * Time.deltaTime);
            float movementSpeed = controller.velocity.magnitude;
            Vector3 PlayerPOS = GameObject.FindGameObjectWithTag("Player").transform.position;
            SetSpeedAndDirection(prevXNonZero, controller.velocity.x, controller.velocity.y, movementSpeed);
            float direction = (float)Math.Atan(controller.velocity.y / controller.velocity.x) * 180.0f / (float)Math.PI;
            direction = controller.velocity.x < 0 ? direction - 180 : direction;

            if (movementSpeed != 0)
            {
                UpdateLampPose(PlayerPOS.x, PlayerPOS.y, direction);
            }
            prevXNonZero = moveX != 0? moveX : prevXNonZero;
        }
        else
        {
            SetSpeedAndDirection(prevXNonZero, 0, 0, 0);
        }

    }
}
