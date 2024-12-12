using UnityEngine;
using System.Collections.Generic;

public class PlayerCollect : MonoBehaviour
{
    [Tooltip("The transform where collected items will be held.")]
    public Transform itemHoldPoint;

    [Tooltip("The vertical offset for each collected item.")]
    public float itemStackOffset = 0.5f;

    [Tooltip("Maximum number of items the player can carry.")]
    public int maxItems = 3;

    private List<GameObject> collectedItems = new List<GameObject>(); // List to store collected items

    void Update()
    {
        // Check for the drop key press
        if (Input.GetKeyDown(KeyCode.Q))
        {
            DropItem();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Collectible") && collectedItems.Count < maxItems) // Check if it's a collectible and under the limit
        {
            CollectItem(other.gameObject);
        }
    }

    void CollectItem(GameObject item)
    {
        // Disable item's physics to make it follow the player
        Rigidbody rb = item.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;
        }

        // Disable the ItemFloat script to stop floating/rotating
        ItemFloat itemFloat = item.GetComponent<ItemFloat>();
        if (itemFloat != null)
        {
            itemFloat.enabled = false;
        }

        // Parent the item to the player's hold point
        item.transform.SetParent(itemHoldPoint);

        // Position the item relative to the stack
        float verticalOffset = collectedItems.Count * itemStackOffset;
        item.transform.localPosition = new Vector3(0, verticalOffset, 0);

        // Add the item to the collected items list
        collectedItems.Add(item);

        Debug.Log("Collected item: " + item.name + ". Total items: " + collectedItems.Count);
    }

    void DropItem()
    {
        if (collectedItems.Count > 0) // Check if there are items to drop
        {
            // Get the topmost item in the stack
            GameObject itemToDrop = collectedItems[collectedItems.Count - 1];

            // Unparent the item
            itemToDrop.transform.SetParent(null);

            // Enable physics for the item
            Rigidbody rb = itemToDrop.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = false;
            }

            // Reset item's position slightly away from the player to avoid instant re-collection
            Vector3 dropPosition = transform.position + transform.forward * 1.0f; // Drop 1 unit in front of the player
            itemToDrop.transform.position = dropPosition;

            // Re-enable the ItemFloat script and update its initial position
            ItemFloat itemFloat = itemToDrop.GetComponent<ItemFloat>();
            if (itemFloat != null)
            {
                itemFloat.enabled = true;
                itemFloat.SetInitialPosition(dropPosition); // Update the initial position
            }

            // Remove the item from the list
            collectedItems.RemoveAt(collectedItems.Count - 1);

            Debug.Log("Dropped item: " + itemToDrop.name + ". Total items: " + collectedItems.Count);
        }
        else
        {
            Debug.Log("No items to drop.");
        }
    }

}
