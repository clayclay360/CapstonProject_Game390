using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RatSpawnSystem : MonoBehaviour
{
    [Header("Variables")]
    public int numberOfRats;
    public int maxNumberOfRats;
    public Transform[] TargetContainer; //Empty GameObject holding all possible targets the room
    public System.Random random = new System.Random(); //Used to get a random target

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
            if (numberOfRats < maxNumberOfRats && GameManager.cookBookActive)
            {
                numberOfRats++;
                spawnIndex = Random.Range(0, ratSpawnTransform.Length);

                //int targetNum = TargetContainer.transform.childCount;
                //Debug.Log(targetNum);
                //Get a random number between 0 and the number of targets 
                //int randomIndex = random.Next(0, targetNum);
                //Debug.Log(randomIndex);
                //Create an array of the targets
                //Transform[] targetList = TargetContainer.GetComponentsInChildren<Transform>();
                //Set the target for the rat
                //ratPrefab.GetComponent<RatScript>().SetTarget(targetList[randomIndex].gameObject);

                Instantiate(ratPrefab, ratSpawnTransform[spawnIndex].position, Quaternion.identity);
            }
        }
    }
}
