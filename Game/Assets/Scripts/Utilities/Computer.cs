using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Computer : Utility
{
    CookBookPages pages;
    private GameObject passItems;

    public Computer()
    {
        Name = "Computer";
        Interaction = "";
    }

    public void Start()
    {
        pages = GameObject.Find("Pages2").GetComponentInChildren<CookBookPages>();
    }

    public override void CheckHand(PlayerController.ItemInMainHand item, PlayerController chef)
    {
        if (GameManager.cookBookActive)
        {
            Interaction = "Destroy Cookbook";
            if (chef.isInteracting)
            {
                GameManager.cookBookActive = false;
                chef.isInteracting = false;
                Interaction = "";
            }
        }

        else
        {
            Interaction = "Print new cookbook";
            if (chef.isInteracting)
            {
                //Tell the player inv full when it's full
                if (chef.inventoryFull)
                {
                    Interaction = "Hands Full";
                    return;
                }
                //Add the item to main or offhand
                if (item == PlayerController.ItemInMainHand.empty)
                {
                    chef.Inv1.text = "Cookbook pages";
                    pages.TurnOnSetActive();
                    chef.hand[0] = pages;
                }
                else
                {
                    chef.Inv2.text = "Cookbook pages";
                    pages.TurnOnSetActive();
                    chef.hand[1] = pages;
                }
                chef.isInteracting = false;
            }
        }


    }

}
