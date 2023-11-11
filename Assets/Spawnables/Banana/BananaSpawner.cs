using System.Collections;
using UnityEngine;

public class BananaSpawner : MonoBehaviour
{
    public GameObject spawnItem;
    [SerializeField, Range(1f, 100f)]
    private float minFrequency = 0.5f;
    [SerializeField, Range(1f, 100f)]
    private float maxFrequency = 5f;
    private float currentFrequency;
    public float initialSpeed;

    [SerializeField]
    private Vector3 minSpawnBounds;
    [SerializeField]
    private Vector3 maxSpawnBounds;

    private float lastSpawnedTime;

    // Start is called before the first frame update
    void Start()
    {
        SetRandomFrequency();
        Spawn(); // This will spawn an item as soon as the game starts
        lastSpawnedTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > lastSpawnedTime + currentFrequency)
        {
            Spawn();
            SetRandomFrequency();
            lastSpawnedTime = Time.time;
        }
    }

    private void SetRandomFrequency()
    {
        currentFrequency = Random.Range(minFrequency, maxFrequency);
    }

    public void Spawn()
    {
        Vector3 spawnPosition = GetRandomPositionWithinBounds();
        GameObject newSpawnedObject = Instantiate(spawnItem, spawnPosition, Quaternion.identity);
        newSpawnedObject.GetComponent<Rigidbody>().velocity = transform.forward * initialSpeed;
        // newSpawnedObject.transform.parent = transform; // Uncomment if you want the spawned objects to be children of the spawner
    }

    private Vector3 GetRandomPositionWithinBounds()
    {
        return new Vector3(
            Random.Range(minSpawnBounds.x, maxSpawnBounds.x),
            Random.Range(minSpawnBounds.y, maxSpawnBounds.y),
            Random.Range(minSpawnBounds.z, maxSpawnBounds.z)
        ) + transform.position; // Added transform.position to consider the spawner's position in the world space
    }
}
