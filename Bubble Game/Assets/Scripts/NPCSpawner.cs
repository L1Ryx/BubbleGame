using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCSpawner : MonoBehaviour
{
    [SerializeField]
    private float timeBetweenSpawns = 1.0f;

    [SerializeField]
    private GameObject NPCPrefab;

    GameObject NPC;

    void Start()
    {
        StartCoroutine(Spawn());
    }

    IEnumerator Spawn()
    {
        while (true)
        {
            //update the x & z values depending on the specific boundaries of your scene
            Vector3 randomizePosition = new Vector3(Random.Range(-5, 5), Random.Range(-5, 5),0);

            Quaternion zeroRotation = Quaternion.Euler(0, 0, 0);

            NPC = Instantiate(NPCPrefab, randomizePosition, zeroRotation);
            yield return new WaitForSeconds(timeBetweenSpawns);
        }
    }
}
