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
    private float spawnRadius = 2f;     // The radius to check for collisions

    [SerializeField]
    private float spawnBoundaryX = 10.0f; // Boundary for X axis

    [SerializeField]
    private float spawnBoundaryY = 5.0f; // Boundary for Y axis

    [SerializeField]
    private LevelData levelData; // Reference to LevelData SO

    private Coroutine spawnCoroutine; // Reference to the spawn coroutine
    private bool isSpawning = false;   // To track if the spawner is active
    private LevelManager levelManager;

    private void Awake() 
    {
        levelManager = FindObjectsByType<LevelManager>(FindObjectsSortMode.None)[0];
    }

    void Start()
    {
        spawnCoroutine = StartCoroutine(Spawn());
    }

    IEnumerator Spawn()
    {
        while (isSpawning)
        {
            SpawnObject(transform.position, 10f);
            yield return new WaitForSeconds(timeBetweenSpawns);
        }
    }

    public void StopSpawner()
    {
        isSpawning = false;
        if (spawnCoroutine != null)
        {
            StopCoroutine(spawnCoroutine);
            spawnCoroutine = null;
        }
    }

    public void StartSpawner()
    {
        isSpawning = true;
        spawnCoroutine = StartCoroutine(Spawn());
    }

    public void SpawnObject(Vector3 spawnCenter, float range)
    {
        Vector3 spawnPosition = GetRandomPosition(spawnCenter, range);

        // Check if the spawn position is occupied
        Collider[] colliders = Physics.OverlapSphere(spawnPosition, spawnRadius);
        if (colliders.Length == 0)
        {
            GameObject npcToSpawn = ChooseObjectToSpawn();

            if (npcToSpawn != null)
            {
                GameObject newNPC = Instantiate(npcToSpawn, spawnPosition, Quaternion.identity);
                AssignCorrectAnimations(newNPC); // Assign child or adult animations
            }
        }
        else
        {
            Debug.Log("Spawn position is occupied, trying again...");
            SpawnObject(spawnCenter, range);
        }
    }

    private GameObject ChooseObjectToSpawn()
    {
        float totalWeight = 0f;
        foreach (var spawnable in NPCPrefabs)
        {
            totalWeight += spawnable.GetComponent<NPCData>().GetSpawnRate(levelManager.GetLevel());
        }

        float randomValue = Random.Range(0f, totalWeight);
        float cumulativeWeight = 0f;

        foreach (var spawnable in NPCPrefabs)
        {
            cumulativeWeight += spawnable.GetComponent<NPCData>().GetSpawnRate(levelManager.GetLevel());
            if (randomValue <= cumulativeWeight)
            {
                return spawnable;
            }
        }

        return null;
    }

    public Vector3 GetRandomPosition(Vector3 center, float range)
    {
        return center + new Vector3(
            Random.Range(-spawnBoundaryX, spawnBoundaryX),
            Random.Range(-spawnBoundaryY, spawnBoundaryY),
            0
        );
    }

    private void AssignCorrectAnimations(GameObject spawnedNPC)
    {
        if (spawnedNPC == null) return;

        // Check if this is the first NPC prefab in the list (default NPC type)
        if (NPCPrefabs.Count > 0 && spawnedNPC.name.Contains(NPCPrefabs[0].name))
        {
            int level = levelData.GetLevelCount(); // Get the current level from LevelData
            NPCTrigger npcTrigger = spawnedNPC.GetComponent<NPCTrigger>();

            if (npcTrigger != null)
            {
                if (level <= 0)
                {
                    npcTrigger.SetChildAnimations();

                }
                else
                {
                    npcTrigger.SwitchToAdultAnimations();
                }
            }
        }
    }
}
