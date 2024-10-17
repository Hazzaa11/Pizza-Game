using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;  // Maximum movement speed
    [SerializeField] private float acceleration = 10f;  // How quickly the player reaches full speed
    [SerializeField] private float deceleration = 15f;  // How quickly the player stops when no input

    private Vector2 currentVelocity;  // Keeps track of player's velocity
    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        // Get input for movement (WASD or arrow keys)
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        // Normalize input to avoid diagonal speed boost
        Vector2 inputVector = new Vector2(moveX, moveY).normalized;

        if (inputVector.magnitude > 0) // If there is input, accelerate towards the direction
        {
            currentVelocity = Vector2.MoveTowards(currentVelocity, inputVector * moveSpeed, acceleration * Time.deltaTime);
        }
        else // If no input, decelerate to a stop
        {
            currentVelocity = Vector2.MoveTowards(currentVelocity, Vector2.zero, deceleration * Time.deltaTime);
        }

        // Apply the movement to the Rigidbody2D
        rb.velocity = currentVelocity;
    }
}
