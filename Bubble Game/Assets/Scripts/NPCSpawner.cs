using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCSpawner : MonoBehaviour
{
    [SerializeField]
    private float timeBetweenSpawns = 1.0f;

    [SerializeField]
    private GameObject NPCPrefab;

    private Coroutine spawnCoroutine; // Reference to the spawn coroutine
    private bool isSpawning = true;   // To track if the spawner is active

    void Start()
    {
        spawnCoroutine = StartCoroutine(Spawn());
    }

    IEnumerator Spawn()
    {
        while (isSpawning)
        {
            // Update the x & z values depending on the specific boundaries of your scene
            Vector3 randomizePosition = new Vector3(Random.Range(-7, 7), Random.Range(-4, 4), 0);

            Quaternion zeroRotation = Quaternion.Euler(0, 0, 0);

            Instantiate(NPCPrefab, randomizePosition, zeroRotation);
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
}
