using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pan : Item
{
    public Pan()
    {
        Name = "Pan";
        Type = "Tool";
        Interaction = "";
    }

    public override void CheckHand(PlayerController.ItemInMainHand item, PlayerController chef)
    {
        //figure out inventory full

        switch (item)
        {
            case PlayerController.ItemInMainHand.empty:
                Interaction = "Grab Pan";
                if (chef.isInteracting)
                {
                    if (utilityItemIsOccupying != null)
                    {
                        utilityItemIsOccupying.Occupied = false;
                        utilityItemIsOccupying = null;
                    }
                    Interaction = "";
                    gameObject.SetActive(false);
                }
                break;
            case PlayerController.ItemInMainHand.egg:

                switch (chef.hand[0].GetComponent<Egg>().state)
                {
                    case Egg.State.shell:
                        Interaction = "Crack Egg";

                        if (chef.isInteracting)
                        {
                            chef.hand[0].GetComponent<Egg>().state = Egg.State.yoke;
                            chef.hand[0].GetComponent<Egg>().toolItemIsOccupying = this;
                            chef.hand[0].GetComponent<Egg>().gameObject.transform.parent = transform;
                            chef.hand[0].GetComponent<Egg>().gameObject.transform.localPosition = new Vector3(0, .15f, 0);
                            chef.hand[0].GetComponent<Egg>().gameObject.SetActive(true);
                            chef.hand[0] = null;
                            chef.itemInMainHand = PlayerController.ItemInMainHand.empty;
                            Occupied = true;
                            Prone = true;
                        }
                        break;
                }
                break;
            case PlayerController.ItemInMainHand.spatula:
                if (Occupied)
                {
                    Interaction = "Use Spatula";
                    if (chef.isInteracting)
                    {

                    }
                }
                else
                {
                    if (chef.inventoryFull)
                    {
                        Interaction = "Inventory Full";
                        return;
                    }

                    Interaction = "Grab Pan";

                    if (chef.isInteracting)
                    {
                        if (utilityItemIsOccupying != null)
                        {
                            utilityItemIsOccupying.Occupied = false;
                            utilityItemIsOccupying = null;
                        }
                        Interaction = "";
                        gameObject.SetActive(false);
                    }
                }
                break;
        }
    }
}
