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
    private PizzaMaker pizzaMaker; // Reference to the PizzaMaker script
    private Oven oven; // Reference to the Oven script
    private GameObject heldPizza; // Reference to the currently held pizza
    private bool isHoldingPizza = false; // To track if the player is holding a pizza

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        pizzaMaker = FindObjectOfType<PizzaMaker>(); // Get the PizzaMaker instance
        oven = FindObjectOfType<Oven>(); // Get the Oven instance
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

        // Check for interaction key (E) and if player is in the PizzaMaker zone and not holding a pizza
        if (Input.GetKeyDown(KeyCode.E) && IsInPizzaMakerZone() && !isHoldingPizza)
        {
            heldPizza = pizzaMaker.SpawnPizza(transform); // Spawn pizza and save reference
            isHoldingPizza = true; // Set the flag to true when spawning pizza
        }

        // Check for interaction key (E) with Oven
        if (Input.GetKeyDown(KeyCode.E) && IsInOvenZone() && isHoldingPizza)
        {
            oven.StartCooking(transform); // Start cooking the pizza
            DestroyHeldPizza(); // Destroy the held pizza after starting the cooking process
        }

        // Check for collecting the cooked pizza
        if (Input.GetKeyDown(KeyCode.E) && IsInOvenZone())
        {
            oven.CollectPizza(transform); // Collect the cooked pizza
        }
    }

    // Placeholder for checking if the player is in the PizzaMaker zone
    private bool IsInPizzaMakerZone()
    {
        // Call the method from PizzaMaker
        return pizzaMaker.IsPlayerInZone(this.transform);
    }

    // Placeholder for checking if the player is in the Oven zone
    private bool IsInOvenZone()
    {
        // Call the method from Oven
        return oven.IsPlayerInZone(this.transform);
    }

    // Method to set the holding pizza status
    public void SetHoldingPizza(bool holding)
    {
        isHoldingPizza = holding; // Update the holding status
    }

    // Method to destroy the held pizza
    private void DestroyHeldPizza()
    {
        if (heldPizza != null)
        {
            Destroy(heldPizza); // Destroy the pizza object
            heldPizza = null; // Reset the reference
        }
        isHoldingPizza = false; // Update the holding status
        Debug.Log("Pizza has been put in the oven!");
    }
}
