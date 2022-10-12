using System.Collections;
using UnityEngine;

public class Spawner : MonoBehaviour
{

    public Transform[] spawnLocation;
    public GameObject[] foodPrefab;
    public GameObject[] foodClone;
    // Start is called before the first frame update
    void Start()
    {
        spawn();
    }

    void spawn()
    {
        foodClone[0] = Instantiate(foodPrefab[0], spawnLocation[0].transform.position, Quaternion.Euler(0, 0, 0)) as GameObject;
        foodClone[1] = Instantiate(foodPrefab[1], spawnLocation[1].transform.position, Quaternion.Euler(0, 0, 0)) as GameObject;
        //foodClone[2] = Instantiate(foodPrefab[2], spawnLocation[2].transform.position, Quaternion.Euler(0, 0, 0)) as GameObject;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
