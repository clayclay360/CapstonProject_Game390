using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CookBookPages : Item
{
    private GameObject passItems;

    public CookBookPages()
    {
        Name = "Cookbook Pages";
    }

    public void Start()
    {
        passItems = GameObject.Find("PassItems");
        transform.position = transform.position + new Vector3(0, 0, 2);
    }

    public override void CheckHand(PlayerController.ItemInMainHand item, PlayerController chef)
    {
        if (chef.inventoryFull)
        {
            Interaction = "Hands Full";
            return;
        }

        switch (item)
        {
            case PlayerController.ItemInMainHand.empty:
                Interaction = "Grab Pages";
                if (chef.isInteracting)
                {
                    gameObject.SetActive(false);
                    Interaction = "";
                    CheckCounter();
                    if (counterInUse != null)
                    {
                        CheckIndividualCounters(counterInUse);
                    }
                }
                break;
            case PlayerController.ItemInMainHand.egg:
                Interaction = "Grab Pages";
                if (chef.isInteracting)
                {
                    gameObject.SetActive(false);
                    Interaction = "";
                    CheckCounter();
                    if (counterInUse != null)
                    {
                        CheckIndividualCounters(counterInUse);
                    }
                }
                break;
            case PlayerController.ItemInMainHand.pan:
                Interaction = "Grab Pages";
                if (chef.isInteracting)
                {
                    gameObject.SetActive(false);
                    Interaction = "";
                    CheckCounter();
                    if (counterInUse != null)
                    {
                        CheckIndividualCounters(counterInUse);
                    }
                }
                break;
            case PlayerController.ItemInMainHand.bacon:
                Interaction = "Grab Pages";
                if (chef.isInteracting)
                {
                    gameObject.SetActive(false);
                    Interaction = "";
                    CheckCounter();
                    if (counterInUse != null)
                    {
                        CheckIndividualCounters(counterInUse);
                    }
                }
                break;
        }
    }

    public void PassPages(int passLocation)
    {
        if (passLocation == 0)
        {
            transform.position = passItems.transform.position + new Vector3(0, 0, 0.5f);
            gameObject.SetActive(true);
        }
        else if (passLocation == 1)
        {
            transform.position = passItems.transform.position;
            gameObject.SetActive(true);
        }
        else if (passLocation == 2)
        {
            transform.position = passItems.transform.position + new Vector3(0, 0, -0.5f);
            gameObject.SetActive(true);
        }
    }

    public void DropPagesOnGround(GameObject player)
    {
        transform.position = player.transform.position + new Vector3(0, 0.2f, 0);
        gameObject.SetActive(true);
    }

    public void TurnOnSetActive()
    {
        gameObject.SetActive(false);
    }
}
