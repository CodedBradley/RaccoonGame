using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform player;
    public float moveSpeed = 5f;
    public Vector3 offset;
    public float followDistance = 10f;
    public Quaternion rotation;
    public float teleportDistanceThreshold = 100f; // Note the corrected spelling

    private void Update()
    {
        // Check the distance between the camera and the player
        if (Vector3.Distance(transform.position, player.position) > teleportDistanceThreshold)
        {
            // Teleport the camera to the player's position
            transform.position = player.position + offset - transform.forward * followDistance;
        }
        else
        {
            // Smoothly move the camera towards the target position
            Vector3 targetPosition = player.position + offset - transform.forward * followDistance;
            transform.position = Vector3.Lerp(transform.position, targetPosition, moveSpeed * Time.deltaTime);
        }

        // Apply the specified rotation to the camera
        transform.rotation = rotation;
    }
}
