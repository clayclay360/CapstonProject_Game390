using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RatSpawnSystem : MonoBehaviour
{
    [Header("Variables")]
    public int numberOfRats;
    public int maxNumberOfRats;

    [Header("Spawn")]
    public float maxSpawnTime;
    public float minSpawnTime;
    public GameObject ratPrefab;
    public Transform[] ratSpawnTransform;

    private int spawnIndex;
    private float spawnTime;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnRat());
    }

    IEnumerator SpawnRat()
    {
        while (true)
        {
            spawnTime = Random.Range(minSpawnTime, maxSpawnTime);
            yield return new WaitForSeconds(spawnTime);
            if (numberOfRats < maxNumberOfRats)
            {
                numberOfRats++;
                spawnIndex = Random.Range(0, ratSpawnTransform.Length);
                Instantiate(ratPrefab, ratSpawnTransform[spawnIndex].position, Quaternion.identity);
            }
        }
    }
}
