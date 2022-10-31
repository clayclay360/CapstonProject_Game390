using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderManager : MonoBehaviour
{
    public string[] orderNames;
    public bool startingOrders;
    public float maxTimeInBetweenOrders, minTimeInBetweenOrders;
    public static int currentOrders;

    private float timeInBetweenOrders;
    private int maxOrders;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(GameManager.gameStarted && !startingOrders)
        {
            Debug.Log("Orders of go");
            startingOrders = true;
            StartCoroutine(Orders());
        }
    }

    IEnumerator Orders()
    {
        switch (GameManager.currentLevel)
        {
            case 0:
                maxOrders = 3;
                break;
        }

        while (GameManager.gameStarted)
        {
            yield return new WaitForSeconds(2);

            if(GameManager.currentLevel == 0 && maxOrders > currentOrders)
            {
                Debug.Log("Waiting for Order");
                timeInBetweenOrders = Random.Range(minTimeInBetweenOrders, maxTimeInBetweenOrders);
                

                int orderNumber = Random.Range(0, 2);

                Debug.Log("Order Made");
                foreach(PlayerController player in FindObjectsOfType<PlayerController>())
                {
                    player.AddOrder(orderNames[orderNumber], 120);
                }

                currentOrders++;
                yield return new WaitForSeconds(timeInBetweenOrders);
            }
        }
    }
}
