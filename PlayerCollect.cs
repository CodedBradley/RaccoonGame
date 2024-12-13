using System.Collections.Generic;
using UnityEngine;

public class PlayerCollect : MonoBehaviour
{
    [Tooltip("The transform where collected items will be held.")]
    public Transform itemHoldPoint;

    [Tooltip("The vertical offset for each collected item.")]
    public float itemStackOffset = 0.5f;

    [Tooltip("Maximum number of items the player can carry.")]
    public int maxItems = 3;

    public List<GameObject> collectedItems = new List<GameObject>(); // List to store collected items

    void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.Q))
        {
            DropItem();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Collectible") && collectedItems.Count < maxItems) 
        {
            CollectItem(other.gameObject);
        }
    }

    void CollectItem(GameObject item)
    {
        
        Rigidbody rb = item.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;
        }

        
        ItemFloat itemFloat = item.GetComponent<ItemFloat>();
        if (itemFloat != null)
        {
            itemFloat.enabled = false;
        }

        
        item.transform.SetParent(itemHoldPoint);

        
        float verticalOffset = collectedItems.Count * itemStackOffset;
        item.transform.localPosition = new Vector3(0, verticalOffset, 0);

        
        collectedItems.Add(item);

        Debug.Log("Collected item: " + item.name + ". Total items: " + collectedItems.Count);
    }

    public void DropItem()
    {
        if (collectedItems.Count > 0) // Check if there are items to drop
        {
            
            GameObject itemToDrop = collectedItems[collectedItems.Count - 1];

            itemToDrop.transform.SetParent(null);

            
            Rigidbody rb = itemToDrop.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = false;
            }

           
            Vector3 dropPosition = transform.position + transform.forward * 3.0f; 
            itemToDrop.transform.position = dropPosition;

            
            ItemFloat itemFloat = itemToDrop.GetComponent<ItemFloat>();
            if (itemFloat != null)
            {
                itemFloat.enabled = true;
                itemFloat.SetInitialPosition(dropPosition); 
            }

            
            collectedItems.RemoveAt(collectedItems.Count - 1);

            Debug.Log("Dropped item: " + itemToDrop.name + ". Total items: " + collectedItems.Count);
        }
        else
        {
            Debug.Log("No items to drop.");
        }
    }

    public void DropAllItems()
    {
        Debug.Log("Dropping all items.");

        
        for (int i = collectedItems.Count - 1; i >= 0; i--)
        {
            GameObject item = collectedItems[i];

            
            item.transform.SetParent(null);

            
            Rigidbody rb = item.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = false;
            }

            
            Collider collider = item.GetComponent<Collider>();
            if (collider != null)
            {
                collider.enabled = false;
            }

            
            ItemFloat itemFloat = item.GetComponent<ItemFloat>();
            if (itemFloat != null)
            {
                itemFloat.enabled = true;
                itemFloat.SetInitialPosition(item.transform.position);
            }
        }

        
        collectedItems.Clear();

        Debug.Log("All items dropped and are now uninteractable.");
    }
}
