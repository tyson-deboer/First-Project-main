using UnityEngine;

public class BananaCollision : MonoBehaviour
{
    public GameObject poopPrefab; // Assign the prefab of the new object in the inspector
    public GameObject targetObject; // Assign the GameObject with the collider in the inspector

    private BananaSpawner bananaSpawner;
    private Collider targetCollider; // The collider component of the targetObject

    void Start()
    {
        // Find the BananaSpawner in the scene
        bananaSpawner = FindObjectOfType<BananaSpawner>();

        // If the targetObject has been set, try to get the Collider component
        if (targetObject != null)
        {
            targetCollider = targetObject.GetComponent<Collider>();
            if (targetCollider == null)
            {
                // Log an error if no collider is found
                Debug.LogError("The target object does not have a collider component attached.");
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        // Ensure that a targetCollider has been set
        if (targetCollider != null)
        {
            // Check if the collision's collider is the targetCollider
            if (collision.collider == targetCollider)
            {
                // Instantiate the new object at the banana's position and rotation
                Instantiate(poopPrefab, transform.position, transform.rotation);

                // Tell the spawner to spawn a new banana
                bananaSpawner.Spawn();

                // Despawn the current banana
                Destroy(gameObject);
            }
        }
    }
}
