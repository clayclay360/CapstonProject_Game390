using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RatSpawnSystem : MonoBehaviour
{
    [Header("Variables")]
    public int maxNumberOfRats;
    public int minPeeks;
    public int maxPeeks;
    public float peekTime;
    public float timeBetweenPeeks;
    public Transform[] TargetContainer; //Empty GameObject holding all possible targets the room
    public System.Random random = new System.Random(); //Used to get a random target

    [Header("Spawn")]
    public float maxSpawnTime;
    public float minSpawnTime;
    public bool isSpawningRat;
    public GameObject ratPrefab;
    public List<GameObject> ratsList;
    public Transform[] ratSpawnTransform;


    private int spawnIndex;
    private float spawnTime;
    private bool canSpawnRat = false;
    private GameObject ratHole;
    private GameObject ratPeek;
    private Transform peekPosition;
    private Transform startPosition;

    // Start is called before the first frame update
    void Start()
    {
        isSpawningRat = false;
    }

    private void Update()
    {
        if (GameManager.recipeIsOpenP1)
        {
            canSpawnRat = true;
        }
        if (canSpawnRat && !isSpawningRat)
        {
            StartCoroutine(SpawnRat());
        }
    }

    IEnumerator SpawnRat()
    {
        while (true)
        {
            isSpawningRat = true;
            spawnTime = Random.Range(minSpawnTime, maxSpawnTime);
            yield return new WaitForSeconds(spawnTime);
            if (ratsList.Count < maxNumberOfRats)
            {
                spawnIndex = Random.Range(0, ratSpawnTransform.Length);

                yield return StartCoroutine(RatPeek());

                GameObject rat = Instantiate(ratPrefab, ratSpawnTransform[spawnIndex].position, Quaternion.identity);
                ratsList.Add(rat);
                rat.name = "Rat " + ratsList.Count.ToString();
                isSpawningRat = false;
            }
        }
    }

    IEnumerator RatPeek()
    {
        int totalPeeks = Random.Range(minPeeks, maxPeeks + 1);
        //Debug.Log(totalPeeks.ToString());
        int peekCount = 0;
        ratPeek = ratSpawnTransform[spawnIndex].parent.Find("RatPeek").gameObject;
        while (peekCount < totalPeeks)
        {
            peekCount++;
            ratPeek.SetActive(true);
            yield return new WaitForSeconds(peekTime);
            ratPeek.SetActive(false);
            if (totalPeeks > 1 && peekCount < totalPeeks)
            {
                yield return new WaitForSeconds(timeBetweenPeeks);
            }
        }
        ratPeek.SetActive(false);
        yield return null;
    }
}
