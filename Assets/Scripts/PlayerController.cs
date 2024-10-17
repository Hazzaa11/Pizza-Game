using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    public GameObject pizzaPrefab; // Reference to the pizza prefab
    public GameObject cookedPizzaPrefab; // Reference to the cooked pizza prefab
    private GameObject currentPizza; // The pizza currently being held
    private bool isCooking = false; // Track if the oven is cooking
    public float cookingTime = 5f; // Serialized field for cooking time
    public float moveSpeed = 5f; // Serialized field for movement speed
    private Rigidbody2D rb; // Reference to the Rigidbody2D component
    private bool cookedPizzaAvailable = false; // Track if a cooked pizza is ready to be picked up
    public Vector2 MovementSpeed = new Vector2(100.0f, 100.0f); // 2D Movement speed to have independant axis speed
    private new Rigidbody2D rigidbody2D; // Local rigidbody variable to hold a reference to the attached Rigidbody2D component
    private Vector2 inputVector = new Vector2(0.0f, 0.0f);

    void Awake()
    {
        // Setup Rigidbody for frictionless top down movement and dynamic collision
        // If RequireComponent is used uncomment the GetComponent and comment the AddComponent out
        rigidbody2D = gameObject.GetComponent<Rigidbody2D>();

        rigidbody2D.angularDrag = 0.0f;
        rigidbody2D.gravityScale = 0.0f;
    }

    void FixedUpdate()
    {
        // Rigidbody2D affects physics so any ops on it should happen in FixedUpdate
        // See why here: https://learn.unity.com/tutorial/update-and-fixedupdate#
        rigidbody2D.MovePosition(rigidbody2D.position + (inputVector * MovementSpeed * Time.fixedDeltaTime));
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // Get the Rigidbody2D component
    }

    private void Update()
    {
        inputVector = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;


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
            // If the player is in the oven zone
            if (currentPizza != null && !isCooking)
            {
                StartCooking();
            }
            else if (isCooking)
            {
                Debug.Log("Oven Full!"); // Cannot insert a pizza while one is cooking
            }
            else if (!isCooking && cookedPizzaAvailable)
            {
                PickUpCookedPizza(); // Only allow pickup if there is a cooked pizza available
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
        if (!cookedPizzaAvailable) // Only start cooking if there is no cooked pizza available
        {
            Destroy(currentPizza); // Destroy the pizza being held 
            isCooking = true;
            StartCoroutine(CookPizza());
        }
    }

    private IEnumerator CookPizza() // Coroutine to cook pizza
    {
        yield return new WaitForSeconds(cookingTime); // Cooking time
        isCooking = false;
        cookedPizzaAvailable = true; // Set the flag that a cooked pizza is ready
        Debug.Log("Pizza has been cooked!"); // Notify that the pizza is cooked
    }

    private void PickUpCookedPizza()
    {
        // Instantiate the cooked pizza prefab and attach it to the player
        currentPizza = Instantiate(cookedPizzaPrefab, transform.position, Quaternion.identity);
        currentPizza.transform.SetParent(transform); // Attach pizza to player
        cookedPizzaAvailable = false; // Reset the flag after picking it up
        Debug.Log("Picked up cooked pizza!");
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
