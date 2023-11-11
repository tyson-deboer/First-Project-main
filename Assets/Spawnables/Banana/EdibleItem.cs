using UnityEngine;

public class EdibleItem : MonoBehaviour
{
    public GameObject poopPrefab;
    private Transform playerTransform;
    public float requiredEatTime = 2.0f; // Time to hold before eaten

    private float eatTimer = 0.0f;
    private bool isEating = false;

    void Start()
    {
        // Assign the player's root transform by finding the GorillaPlayer in the hierarchy
        playerTransform = transform.root; // This will get the topmost transform in the current hierarchy
    }

    void Update()
    {
        if (isEating)
        {
            eatTimer += Time.deltaTime;
            if (eatTimer >= requiredEatTime)
            {
                EatItem();
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        // Check if the collided object is the Main Camera
        if (collision.gameObject.name == "Main Camera")
        {
            isEating = true;
            eatTimer = 0f; // Start or reset the eating timer
        }
    }

    void OnCollisionExit(Collision collision)
    {
        // Check if the object exiting the collision is the Main Camera
        if (collision.gameObject.name == "Main Camera")
        {
            isEating = false;
            eatTimer = 0f; // Reset the eating timer
        }
    }

    private void EatItem()
    {
        // Get the Main Camera's transform
        Transform cameraTransform = Camera.main.transform;

        // Calculate a position behind the Main Camera based on its transform
        // Assuming 'behind' is directly opposite to the camera's forward vector
        Vector3 positionBehindCamera = cameraTransform.position - cameraTransform.forward * 0.25f; // Adjust the multiplier to set how far behind you want to spawn

        // Spawn the poop prefab at the calculated position with the camera's rotation
        Instantiate(poopPrefab, positionBehindCamera, cameraTransform.rotation);

        Debug.Log("Spawned Poop at: " + positionBehindCamera); // This will print the spawn position to the console.

        // Destroy the banana
        Destroy(gameObject);
    }


}
