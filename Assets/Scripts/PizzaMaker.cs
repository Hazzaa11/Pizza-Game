using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PizzaMaker : MonoBehaviour
{
    [SerializeField] private GameObject pizzaPrefab; // Reference to the pizza prefab
    private Collider2D pizzaMakerZone; // Reference to the trigger collider

    private void Start()
    {
        // Get the PizzaMakerZone collider (child object with Collider2D component)
        pizzaMakerZone = transform.Find("PizzaMakerZone").GetComponent<Collider2D>();
    }

    // Method to spawn a pizza at the player's position
    public void SpawnPizza(Transform playerTransform)
    {
        // Instantiate the pizza prefab at the player's position
        GameObject pizza = Instantiate(pizzaPrefab, playerTransform.position, Quaternion.identity);

        // Set the parent to the player to attach the pizza
        pizza.transform.SetParent(playerTransform);

        // Position the pizza slightly in front of the player or adjust as needed
        pizza.transform.localPosition = new Vector3(0.5f, 0.5f, 0); // Adjust this value to position the pizza correctly

        Debug.Log("Pizza has been spawned and attached to the player!");

        // Notify the player controller that it is now holding a pizza
        PlayerController playerController = playerTransform.GetComponent<PlayerController>();
        if (playerController != null)
        {
            playerController.SetHoldingPizza(true);
        }
    }

    // Method to check if the player is in the PizzaMaker zone
    public bool IsPlayerInZone(Transform playerTransform)
    {
        return pizzaMakerZone.OverlapPoint(playerTransform.position); // Check if the player's position overlaps with the zone
    }
}
