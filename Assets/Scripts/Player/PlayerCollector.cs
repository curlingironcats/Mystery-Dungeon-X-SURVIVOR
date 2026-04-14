using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollector : MonoBehaviour
{
    PlayerStats player;
    CircleCollider2D playerCollector;
    public float pullSpeed;

    // Track all pickups currently inside the magnet radius
    private List<Transform> attractedObjects = new List<Transform>();

    void Start()
    {
        player = FindObjectOfType<PlayerStats>();
        playerCollector = GetComponent<CircleCollider2D>();
    }

    void Update()
    {
        playerCollector.radius = player.currentMagnet;

        // Move all attracted objects toward the player
        for (int i = attractedObjects.Count - 1; i >= 0; i--)
        {
            Transform obj = attractedObjects[i];

            if (obj == null)
            {
                attractedObjects.RemoveAt(i);
                continue;
            }

            // Move toward player
            obj.position = Vector2.MoveTowards(
                obj.position,
                transform.position,
                pullSpeed * Time.deltaTime
            );

            // Optional: collect when very close
            if (Vector2.Distance(obj.position, transform.position) < 0.2f)
            {
                if (obj.TryGetComponent(out ICollectible collectible))
                {
                    collectible.Collect();
                }

                Destroy(obj.gameObject);
                attractedObjects.RemoveAt(i);
            }
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.TryGetComponent(out ICollectible collectible))
        {
            // Add to list instead of applying force once
            if (!attractedObjects.Contains(col.transform))
            {
                attractedObjects.Add(col.transform);
            }
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        // Stop pulling if it leaves the radius
        if (attractedObjects.Contains(col.transform))
        {
            attractedObjects.Remove(col.transform);
        }
    }
}