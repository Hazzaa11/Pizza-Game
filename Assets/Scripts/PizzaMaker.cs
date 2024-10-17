using UnityEngine;

public class PizzaMaker : MonoBehaviour
{
    [SerializeField] private GameObject pizzaPrefab; // Reference to the pizza prefab
    private Collider2D pizzaMakerZone; // Reference to the pizza maker zone collider

    private void Start()
    {
        pizzaMakerZone = GetComponent<Collider2D>(); // Get the collider of the pizza maker zone
    }

    // Method to spawn pizza and return the GameObject
    public GameObject SpawnPizza(Transform playerTransform)
    {
        // Instantiate the pizza at the player's position
        GameObject pizza = Instantiate(pizzaPrefab, playerTransform.position, Quaternion.identity);
        pizza.transform.SetParent(playerTransform); // Attach pizza to the player
        Debug.Log("Pizza has been spawned and attached to the player!");
        return pizza; // Return the pizza GameObject
    }

    // Check if player is in the PizzaMaker zone
    public bool IsPlayerInZone(Transform playerTransform)
    {
        return pizzaMakerZone != null && pizzaMakerZone.IsTouching(playerTransform.GetComponent<Collider2D>());
    }
}
