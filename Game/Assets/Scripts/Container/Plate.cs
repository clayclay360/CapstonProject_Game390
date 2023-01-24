using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Plate : Item
{
    [HideInInspector]
    public Item foodOnPlate;
    public string orderName;
    public float timer;
    public int orderNumber;
    public Slider sliderTimer;
    public PrefabReferences pr;
    //Bacon baconRespawn;
    //Egg eggRespawn;
    Menu menuOrder;

    public Plate()
    {
        Name = "Plate";
        Type = "Tool";
        status = Status.clean;
    }

    public void Awake()
    {
        currUses = 0;
        usesUntilDirty = 1;
    }

    public void Start()
    {
        menuOrder = GameObject.Find("MenuWindow").GetComponentInChildren<Menu>(); //This line returns an error every time the game is started
        menuOrder.PlaceOrder(orderName);
    }

    public void Update()
    {
        //baconRespawn = GameManager.bacon;
        //eggRespawn = GameManager.egg;
    }

    public override void CheckHand(PlayerController.ItemInMainHand item, PlayerController chef)
    {
        
        switch (item)
        {
            case PlayerController.ItemInMainHand.pan:

                Interaction = orderName;
                sliderTimer.gameObject.SetActive(true);
                Menu menuOrder = GameObject.Find("MenuWindow").GetComponentInChildren<Menu>();

                if (chef.hand[0].GetComponent<Pan>() != null && chef.hand[0].GetComponent<Pan>().Occupied && chef.hand[0].GetComponent<Pan>().foodInPan.status == Status.cooked)
                {
                    Interaction = "Place food on plate";
                    if (chef.isInteracting)
                    {
                        Pan pan = chef.hand[0].GetComponent<Pan>();
                        //if whether the order is correct or not
                        if(orderName == pan.foodInPan.Name)
                        {
                            Debug.Log("Order Complete");
                            GameManager.rating += .2f;
                            OrderManager.currentOrders--;
                            OrderManager.Order.Remove(orderNumber);
                            RespawnItem(pan.foodInPan);
                            Destroy(gameObject);
                        }
                        else
                        {
                            Debug.Log("Wrong Order");
                            GameManager.rating -= .2f;
                            OrderManager.currentOrders--;
                            OrderManager.Order.Remove(orderNumber);
                            RespawnItem(pan.foodInPan);
                            Destroy(gameObject);
                        }
                        //if (orderName.Contains("Omelet"))
                        //{
                        //    Debug.Log("RespawnItem");
                        //    eggRespawn.Respawn();
                        //}
                        //else if (orderName.Contains("Bacon"))
                        //{
                        //    Debug.Log("RespawnItem");
                        //    baconRespawn.Respawn();
                        //}

                        menuOrder.RemoveOrder(orderName);

                        foodOnPlate = chef.hand[0].GetComponent<Pan>().foodInPan;
                        foodOnPlate.toolItemIsOccupying = this;
                        foodOnPlate.gameObject.transform.parent = transform;
                        foodOnPlate.gameObject.transform.localPosition = new Vector3(0, .15f, 0);
                        chef.hand[0].GetComponent<Pan>().foodInPan = null;
                        chef.hand[0].GetComponent<Pan>().Occupied = false;
                        chef.hand[0].GetComponent<Pan>().CheckIfDirty();
                        chef.isInteracting = false;

                        //Checkcompletion
                        //GameManager.Instance.CheckLevelCompletion(foodOnPlate);
                    }
                }
                else if(chef.inventoryFull)
                    {
                        Interaction = "Hands Full";
                        return;
                    }
                break;
            default:
                Interaction = orderName;
                sliderTimer.gameObject.SetActive(true);
                break;
        }
    }

    private void RespawnItem(Item item)
    {
        //Find and instance the object
        GameObject prefab = pr.FindPrefab(item);
        if (prefab)
        {
            GameObject newFood = (GameObject)Instantiate(prefab, item.startPosition, item.startRotation, transform.parent);
            newFood.name = item.name;
        }
    }

    public void StartTimer()
    {
        StartCoroutine(Timer());
    }

    public IEnumerator Timer()
    {
        sliderTimer.maxValue = timer;
        float maxTime = sliderTimer.maxValue;
        float currentTime = Time.unscaledTime;
        float orderTime = 0;
        sliderTimer.value = maxTime;

        while (maxTime - orderTime > 0)
        {
            orderTime = Time.unscaledTime - currentTime;
            yield return null;
            sliderTimer.value = maxTime - orderTime;
        }

        OrderManager.currentOrders--;
        OrderManager.Order.Remove(orderNumber);
        menuOrder.RemoveOrder(orderName);
        Destroy(gameObject);
    }
}
