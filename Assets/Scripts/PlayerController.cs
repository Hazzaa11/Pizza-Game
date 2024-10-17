using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    public GameObject pizzaPrefab; // Reference to the pizza prefab
    public GameObject cookedPizzaPrefab; // Reference to the cooked pizza prefab
    private GameObject currentPizza; // The pizza currently being held
    private bool isCooking = false; // Track if the oven is cooking
    private bool isCooked = false; // Track if the pizza is cooked
    public float cookingTime = 5f; // Serialized field for cooking time
    public float moveSpeed = 5f; // Serialized field for movement speed
    private Rigidbody2D rb; // Reference to the Rigidbody2D component

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // Get the Rigidbody2D component
    }

    private void Update()
    {
        // Get input from the axes
        float horizontalInput = Input.GetAxis("Horizontal"); // A/D or Left/Right arrows
        float verticalInput = Input.GetAxis("Vertical"); // W/S or Up/Down arrows
        Vector2 movement = new Vector2(horizontalInput, verticalInput);

        if (movement.magnitude > 1)
        {
            movement.Normalize();
        }

        rb.velocity = movement * moveSpeed; // Directly set the velocity for instant stopping

        // Check for interaction input
        if (Input.GetKeyDown(KeyCode.E))
        {
            TryInteract();
        }
    }

    private void TryInteract()
    {
        // Check for interactions based on current zone
        if (IsInZone("PizzaMakerZone"))
        {
            if (currentPizza == null)
            {
                PickUpPizza();
            }
            else
            {
                Debug.Log("Hands Full!");
            }
        }
        else if (IsInZone("OvenZone"))
        {
            if (currentPizza != null && !isCooking)
            {
                StartCooking();
            }
            else if (isCooked) // Check if the pizza is cooked
            {
                PickUpCookedPizza(); // Allow player to pick up the cooked pizza
            }
            else
            {
                Debug.Log("Oven Full!");
            }
        }
        else if (IsInZone("ServingZone"))
        {
            ServePizza();
        }
        else if (IsInZone("TrashZone"))
        {
            DisposePizza();
        }
    }

    public bool IsInZone(string zoneTag) // Change to public
    {
        // Check if the player is inside a zone by checking tags
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, 0.5f);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag(zoneTag))
            {
                return true;
            }
        }
        return false;
    }

    private void PickUpPizza()
    {
        currentPizza = Instantiate(pizzaPrefab, transform.position, Quaternion.identity);
        currentPizza.transform.SetParent(transform); // Attach pizza to player
    }

    private void StartCooking()
    {
        Destroy(currentPizza); // Destroy the pizza being held
        isCooking = true;
        isCooked = false; // Reset the cooked state
        StartCoroutine(CookPizza());
    }

    private IEnumerator CookPizza() // Coroutine to cook pizza
    {
        yield return new WaitForSeconds(cookingTime); // Cooking time (can be set as serialized field)
        isCooking = false;
        isCooked = true; // Set pizza as cooked
        Debug.Log("Pizza has been cooked!");
    }

    private void PickUpCookedPizza()
    {
        if (IsInZone("OvenZone") && isCooked) // Ensure in the OvenZone and pizza is cooked
        {
            currentPizza = Instantiate(cookedPizzaPrefab, transform.position, Quaternion.identity);
            currentPizza.transform.SetParent(transform); // Attach cooked pizza to player
            isCooked = false; // Reset the cooked state after picking up
        }
    }

    private void ServePizza()
    {
        if (currentPizza != null && currentPizza.CompareTag("Cooked"))
        {
            Debug.Log("Pizza served!");
            Destroy(currentPizza); // Destroy the served pizza
        }
        else
        {
            Debug.Log("Cannot serve a pizza that is not cooked!");
        }
    }

    private void DisposePizza()
    {
        if (currentPizza != null)
        {
            Destroy(currentPizza); // Destroy the held pizza
            Debug.Log("Pizza disposed!");
        }
    }

    // Methods for enabling and disabling interaction
    public void EnableOvenInteraction()
    {
        Debug.Log("Oven interaction enabled");
    }

    public void DisableOvenInteraction()
    {
        Debug.Log("Oven interaction disabled");
    }

    public void EnableTrashInteraction()
    {
        Debug.Log("Trash interaction enabled");
    }

    public void DisableTrashInteraction()
    {
        Debug.Log("Trash interaction disabled");
    }

    public void EnablePizzaMakerInteraction()
    {
        Debug.Log("Pizza maker interaction enabled");
    }

    public void DisablePizzaMakerInteraction()
    {
        Debug.Log("Pizza maker interaction disabled");
    }

    // Add methods for serving table interaction
    public void EnableServingTableInteraction()
    {
        Debug.Log("Serving table interaction enabled");
    }

    public void DisableServingTableInteraction()
    {
        Debug.Log("Serving table interaction disabled");
    }
}
