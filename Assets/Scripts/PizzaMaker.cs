using UnityEngine;

public class PizzaMaker : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerController>().EnablePizzaMakerInteraction();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerController>().DisablePizzaMakerInteraction();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (other.GetComponent<PlayerController>().IsInZone("PizzaMakerZone"))
            {
                other.GetComponent<PlayerController>().EnablePizzaMakerInteraction();
            }
        }
    }
}
