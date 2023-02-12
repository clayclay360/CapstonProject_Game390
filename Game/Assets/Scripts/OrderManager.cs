using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

public class OrderManager : MonoBehaviour
{
    [Header("Variables")]
    public string[] orderNames;
    public bool startingOrders;
    public float maxTimeInBetweenOrders, minTimeInBetweenOrders;
    public static int currentOrders;

    [Header("Orders")]
    public GameObject platePrefab;
    public Transform[] spawnPosition;
    public static Dictionary<int, Plate> Order = new Dictionary<int, Plate>();
    public int orderNumber;

    private float timeInBetweenOrders;
    private int maxOrders;

    [Header("Menu")]
    public Menu menuObject;
    
    // Update is called once per frame
    void Update()
    {
        if(GameManager.gameStarted && !startingOrders)
        {
            startingOrders = true;
            StartCoroutine(Orders());
            menuObject.gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.GetComponent<PlayerController>() != null)
        {
            Debug.Log(other.gameObject.name);
            menuObject.gameObject.SetActive(true);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<PlayerController>() != null)
        {
            menuObject.gameObject.SetActive(false);
        }
    }

    public void RemoveOrder(int orderNum)
    {
        currentOrders--;
        menuObject.RemoveOrder(Order[orderNum].name);
        Order.Remove(orderNum);
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
                timeInBetweenOrders = Random.Range(minTimeInBetweenOrders, maxTimeInBetweenOrders);

                int orderIndex = Random.Range(0, 2);
                
                //reset
                if(orderNumber > 2)
                {
                    orderNumber = 0;
                }

                //temporary 
                GameObject plate = Instantiate(platePrefab);
                Plate plateScript = plate.GetComponent<Plate>();
                Order.Add(orderNumber, plateScript);
                plate.transform.position = spawnPosition[orderNumber].position;
                plateScript.menuOrder = menuObject;
                plate.GetComponent<Plate>().orderName = orderNames[orderIndex];
                plate.GetComponent<Plate>().timer = 120;
                plate.GetComponent<Plate>().orderNumber = orderNumber;
                plate.GetComponent<Plate>().StartTimer();
                currentOrders++;
                orderNumber++;

                //Add the order to the menu
                menuObject.PlaceOrder(orderNames[orderIndex]);

                yield return new WaitForSeconds(timeInBetweenOrders);
            }
        }
    }
}
