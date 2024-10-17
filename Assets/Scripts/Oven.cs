using System.Collections;
using UnityEngine;

public class Oven : MonoBehaviour
{
    [SerializeField] private GameObject cookedPizzaPrefab; // Reference to the cooked pizza prefab
    [SerializeField] private float cookingTime = 5f; // Time to cook the pizza
    private bool isCooking = false; // To track if the oven is currently cooking
    private Transform playerTransform; // To store the player's transform for spawning the pizza
    private bool hasPizzaInOven = false; // To check if there is a pizza in the oven

    // Method to start cooking
    public void StartCooking(Transform player)
    {
        if (!isCooking && !hasPizzaInOven) // Only start if not already cooking and no pizza in oven
        {
            playerTransform = player; // Store the player's transform
            isCooking = true; // Set cooking state to true
            hasPizzaInOven = true; // Mark that there is a pizza in the oven
            Debug.Log("Cooking started!");
            StartCoroutine(CookingTimer()); // Start the cooking timer
        }
    }

    // Cooking timer coroutine
    private IEnumerator CookingTimer()
    {
        yield return new WaitForSeconds(cookingTime); // Wait for the cooking time
        Debug.Log("Pizza is ready! Press E to collect it.");
        isCooking = false; // Reset cooking state
    }

    // Method to check if the player is within the OvenZone
    public bool IsPlayerInZone(Transform playerTransform)
    {
        Collider2D ovenZoneCollider = GetComponentInChildren<Collider2D>(); // Get the collider of the oven zone
        return ovenZoneCollider != null && ovenZoneCollider.IsTouching(playerTransform.GetComponent<Collider2D>());
    }

    // Method to collect the cooked pizza if the player is in the OvenZone and the oven has cooked a pizza
    public void CollectPizza(Transform playerTransform)
    {
        if (!isCooking && hasPizzaInOven && IsPlayerInZone(playerTransform))
        {
            GameObject cookedPizza = Instantiate(cookedPizzaPrefab, playerTransform.position, Quaternion.identity);
            cookedPizza.transform.SetParent(playerTransform); // Attach the cooked pizza to the player
            Debug.Log("Cooked pizza collected!");
            hasPizzaInOven = false; // Reset pizza in oven status after collection
        }
        else if (isCooking)
        {
            Debug.Log("The pizza is still cooking!");
        }
        else if (!hasPizzaInOven)
        {
            Debug.Log("You need to place a pizza in the oven first!");
        }
        else
        {
            Debug.Log("You must be in the OvenZone to collect the pizza!");
        }
    }
}
