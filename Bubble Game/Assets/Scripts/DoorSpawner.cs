using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorSpawner : MonoBehaviour
{
    [SerializeField]
    private float firstSpawnTime = 30.0f; // Time for the first door spawn

    [SerializeField]
    private float minSpawnInterval = 15.0f; // Minimum interval for subsequent spawns

    [SerializeField]
    private float maxSpawnInterval = 20.0f; // Maximum interval for subsequent spawns

    [SerializeField]
    private float spawnBoundaryX = 10.0f; // Boundary for X axis

    [SerializeField]
    private float spawnBoundaryY = 5.0f; // Boundary for Y axis

    [SerializeField]
    private float minPlayerDistance = 2.0f; // Minimum distance from the player

    [SerializeField]
    private GameObject doorPrefab; // The door prefab to spawn

    private GameObject player;
    private Coroutine spawnCoroutine; // Reference to the spawning coroutine
    private bool isSpawning = false;   // Flag to control spawning

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player"); // Find the player object
        if (player == null)
        {
            Debug.LogError("Player object not found! Make sure the player is tagged as 'Player'.");
            return;
        }

        spawnCoroutine = StartCoroutine(SpawnDoor());
    }

    private IEnumerator SpawnDoor()
    {
        // Wait for the first spawn time
        yield return new WaitForSeconds(firstSpawnTime);

        while (isSpawning)
        {
            Vector3 spawnPosition;
            bool validSpawn = false;

            // Try to find a valid spawn position
            do
            {
                spawnPosition = GetOuterBoundaryPosition();
                validSpawn = IsFarEnoughFromPlayer(spawnPosition);
            }
            while (!validSpawn);

            // Spawn the door at the valid position
            Instantiate(doorPrefab, spawnPosition, Quaternion.identity);

            // Wait for a random interval before spawning the next door
            float nextSpawnTime = Random.Range(minSpawnInterval, maxSpawnInterval);
            yield return new WaitForSeconds(nextSpawnTime);
        }
    }

    public void StopSpawning()
    {
        // Set the spawning flag to false
        isSpawning = false;

        // Stop the coroutine if it's running
        if (spawnCoroutine != null)
        {
            StopCoroutine(spawnCoroutine);
            spawnCoroutine = null;
        }
    }

    public void StartSpawner()
    {
        // Set the isSpawning flag to false to stop the loop
        isSpawning = true;
        spawnCoroutine = StartCoroutine(SpawnDoor());
    }

    private Vector3 GetOuterBoundaryPosition()
    {
        // Randomly decide which outer boundary the door should spawn on
        int side = Random.Range(0, 4); // 0: top, 1: bottom, 2: left, 3: right

        switch (side)
        {
            case 0: // Top boundary
                return new Vector3(Random.Range(-spawnBoundaryX, spawnBoundaryX), spawnBoundaryY, 0);
            case 1: // Bottom boundary
                return new Vector3(Random.Range(-spawnBoundaryX, spawnBoundaryX), -spawnBoundaryY, 0);
            case 2: // Left boundary
                return new Vector3(-spawnBoundaryX, Random.Range(-spawnBoundaryY, spawnBoundaryY), 0);
            case 3: // Right boundary
                return new Vector3(spawnBoundaryX, Random.Range(-spawnBoundaryY, spawnBoundaryY), 0);
            default:
                return Vector3.zero; // Fallback (should never reach here)
        }
    }

    private bool IsFarEnoughFromPlayer(Vector3 position)
    {
        if (player == null) return true; // If the player is missing, assume valid spawn
        float distanceToPlayer = Vector3.Distance(position, player.transform.position);
        return distanceToPlayer >= minPlayerDistance;
    }
}
