using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 100f;

    void Update()
    {
        // Rotate the coin around the Y-axis
        transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
    }

    // This method is called when the coin is collected by the player
    public void Collect()
    {
        // Hide the coin and destroy it after a small delay to make it disappear smoothly
        gameObject.SetActive(false);
    }
}
