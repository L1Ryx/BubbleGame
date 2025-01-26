using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCSpawner : MonoBehaviour
{
    [SerializeField]
    private float timeBetweenSpawns = 1.0f;

    [SerializeField]
    private List<GameObject> NPCPrefabs;

    [SerializeField]
    public float spawnRadius = 2f;     // The radius to check for collisions

    [SerializeField]
    private float spawnBoundaryX = 10.0f; // Boundary for X axis

    [SerializeField]
    private float spawnBoundaryY = 5.0f; // Boundary for Y axis


    private Coroutine spawnCoroutine; // Reference to the spawn coroutine
    private bool isSpawning = true;   // To track if the spawner is active

    void Start()
    {
        spawnCoroutine = StartCoroutine(Spawn());
    }

    IEnumerator Spawn()
    {
        while(isSpawning)
        {
            SpawnObject(transform.position, 10f);
            yield return new WaitForSeconds(timeBetweenSpawns);
        }

    }

    public void StopSpawner()
    {
        // Set the isSpawning flag to false to stop the loop
        isSpawning = false;

        // Stop the coroutine if it's running
        if (spawnCoroutine != null)
        {
            StopCoroutine(spawnCoroutine);
            spawnCoroutine = null;
        }
    }

    public void SpawnObject(Vector3 spawnCenter, float range)
    {
        Vector3 spawnPosition = GetRandomPosition(spawnCenter, range);

        // Check if the spawn position is occupied by another object using Physics.OverlapSphere
        Collider[] colliders = Physics.OverlapSphere(spawnPosition, spawnRadius);

        if (colliders.Length == 0)
        {
            // No other objects are occupying the spawn position, so we can spawn
            Instantiate(ChooseObjectToSpawn(), spawnPosition, Quaternion.identity);
        }
        else
        {
            // The spawn position is occupied, try again or handle accordingly
            Debug.Log("Spawn position is occupied, trying again...");
            SpawnObject(spawnCenter, range); // You can call this recursively or use a loop
        }
    }

    private GameObject ChooseObjectToSpawn()
    {
        // Calculate the total sum of all spawn rates
        float totalWeight = 0f;
        foreach (var spawnable in NPCPrefabs)
        {
            totalWeight += spawnable.GetComponent<NPCData>().GetSpawnRate();
        }

        // Pick a random value between 0 and totalWeight
        float randomValue = Random.Range(0f, totalWeight);
        print(totalWeight);

        // Select the object based on the random value and spawn rate
        float cumulativeWeight = 0f;
        foreach (var spawnable in NPCPrefabs)
        {
            cumulativeWeight += spawnable.GetComponent<NPCData>().GetSpawnRate();
            if (randomValue <= cumulativeWeight)
            {
                print(spawnable.GetComponent<NPCData>().GetSpawnRate());
                return spawnable;
            }
        }

        return null; // This should never happen if spawnableObjects is not empty
    }

    public Vector3 GetRandomPosition(Vector3 center, float range)
    {
        // Generate a random position within a range
        Vector3 randomPosition = center + new Vector3(
            Random.Range(-spawnBoundaryX, spawnBoundaryX),
            Random.Range(-spawnBoundaryY, spawnBoundaryY),
            0
        );
        return randomPosition;
    }
}
