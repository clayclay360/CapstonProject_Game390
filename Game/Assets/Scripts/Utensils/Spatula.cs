using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;

public class Spatula : Item
{
    public GameObject passItems;

    public Spatula()
    {
        Name = "Spatula";
        Type = "Tool";
        Interaction = "";
        status = Status.clean;
         
    }

    private new void Start()
    {

        currUses = 0;
        usesUntilDirty = 2;
        passItems = GameObject.Find("PassItems");
    }

    public void Update()
    {
        switch (status)
        {
            case Status.clean:
                main = clean;
                break;

            case Status.dirty:
                main = dirty;
                break;
        }
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
                Interaction = "Grab Spatula";
                if (chef.isInteracting)
                {
                    chef.readyToInteract = false;
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
                Interaction = "Grab Spatula";
                if (chef.isInteracting)
                {


                    

                    chef.readyToInteract = false;


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
                Interaction = "Grab Spatula";
                if (chef.isInteracting)
                {


                   

                    chef.readyToInteract = false;

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
                Interaction = "Grab Spatula";
                if (chef.isInteracting)
                {



                    chef.readyToInteract = false;


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

    public void PassSpatula(int passLocation)
    {
        if (passLocation == 0)
        {
            transform.position = passItems.transform.position + new Vector3(0.35f, 0, 0.5f);
            gameObject.SetActive(true);
        } else if (passLocation == 1)
        {
            transform.position = passItems.transform.position + new Vector3(0.35f, 0, 0);
            gameObject.SetActive(true);
        } else if (passLocation == 2)
        {
            transform.position = passItems.transform.position + new Vector3(0.35f, 0, -0.5f);
            gameObject.SetActive(true);
        }
    }

    public void DropSpatulaOnGround(GameObject player)
    {
        transform.position = player.transform.position;
        gameObject.SetActive(true);
    }
}
