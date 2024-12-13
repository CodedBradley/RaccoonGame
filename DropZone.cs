using UnityEngine;
using System.Collections.Generic;

public class DropZone : MonoBehaviour
{
    [Tooltip("Tag of the player object.")]
    public string playerTag = "Player";

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"Trigger entered by: {other.name}");

        
        Transform root = other.transform.root;
        if (root.CompareTag(playerTag))
        {
            Debug.Log("Player entered drop zone.");

            
            PlayerCollect playerCollect = root.GetComponent<PlayerCollect>();
            if (playerCollect != null)
            {
                Debug.Log("PlayerCollect component found. Dropping all items.");

                
                playerCollect.DropAllItems();
            }
            else
            {
                Debug.LogWarning("PlayerCollect component not found on player.");
            }
        }
        else
        {
            Debug.Log("Non-player object entered the drop zone.");
        }
    }
}
