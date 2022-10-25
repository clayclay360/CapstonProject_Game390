using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;

public class Spatula : Item
{
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
                }
                break;
            case PlayerController.ItemInMainHand.egg:
                Interaction = "Grab Spatula";
                if (chef.isInteracting)
                {
                    gameObject.SetActive(false);
                    Interaction = "";
                }
                break;
            case PlayerController.ItemInMainHand.pan:
                Interaction = "Grab Spatula";
                if (chef.isInteracting)
                {
                    gameObject.SetActive(false);
                    Interaction = "";
                }
                break;
            case PlayerController.ItemInMainHand.bacon:
                Interaction = "Grab Spatula";
                if (chef.isInteracting)
                {
                    gameObject.SetActive(false);
                    Interaction = "";
                }
                break;
        }
    }
}
