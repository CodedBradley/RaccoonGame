using UnityEngine;

public class ItemFloat : MonoBehaviour
{
    [Tooltip("The speed at which the item moves up and down.")]
    public float floatSpeed = 1.0f;

    [Tooltip("The height range for the vertical oscillation.")]
    public float floatHeight = 0.5f;

    [Tooltip("The speed at which the item rotates.")]
    public float rotationSpeed = 30f;

    private Vector3 initialPosition;

    void Start()
    {
        // Store the initial position of the item
        initialPosition = transform.position;
    }

    void Update()
    {
        // Calculate the new Y position using a sine wave for smooth up and down movement
        float newY = initialPosition.y + Mathf.Sin(Time.time * floatSpeed) * floatHeight;

        // Update the position of the item
        transform.position = new Vector3(initialPosition.x, newY, initialPosition.z);

        // Rotate the item smoothly
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);  // Rotate around the Y axis
    }

    // Method to dynamically update the initial position
    public void SetInitialPosition(Vector3 newPosition)
    {
        initialPosition = newPosition;
    }
}
