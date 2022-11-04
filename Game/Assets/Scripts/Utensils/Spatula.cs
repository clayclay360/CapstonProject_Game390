using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;

public class Spatula : Item
{
    private GameObject passItems;

    public Spatula()
    {
        Name = "Spatula";
        Type = "Tool";
        Interaction = "";
        status = Status.clean;
         
    }

    public new void Start()
    {
        currUses = 0;
        usesUntilDirty = 2;
        passItems = GameObject.Find("PassItems");
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
                    gameObject.SetActive(false);
                    Interaction = "";
                    CheckCounter();
                }
                break;
            case PlayerController.ItemInMainHand.egg:
                Interaction = "Grab Spatula";
                if (chef.isInteracting)
                {
                    gameObject.SetActive(false);
                    Interaction = "";
                    CheckCounter();
                }
                break;
            case PlayerController.ItemInMainHand.pan:
                Interaction = "Grab Spatula";
                if (chef.isInteracting)
                {
                    gameObject.SetActive(false);
                    Interaction = "";
                    CheckCounter();
                }
                break;
            case PlayerController.ItemInMainHand.bacon:
                Interaction = "Grab Spatula";
                if (chef.isInteracting)
                {
                    gameObject.SetActive(false);
                    Interaction = "";
                    CheckCounter();
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

    public void CheckCounter()
    {
        for (int i = 0; i <= GameManager.counterItems.Length; i++)
        {
            if (i >= GameManager.counterItems.Length)
            {
                return;
            }

            if (gameObject.name == GameManager.counterItems[i])
            {
                GameManager.counterItems[i] = "";
                return;
            }
        }
    }
}
