using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plate : Item
{
    [HideInInspector]
    public Item foodOnPlate;

    public Plate()
    {
        Name = "Plate";
        Type = "Tool";
    }

    public override void CheckHand(PlayerController.ItemInMainHand item, PlayerController chef)
    {
        switch(item)
        {
            case PlayerController.ItemInMainHand.pan:
                if(chef.hand[0].GetComponent<Pan>() != null && chef.hand[0].GetComponent<Pan>().Occupied && chef.hand[0].GetComponent<Pan>().foodInPan.status == Status.cooked)
                {
                    Interaction = "Place food on plate";
                    if(chef.isInteracting)
                    {
                        foodOnPlate = chef.hand[0].GetComponent<Pan>().foodInPan;
                        foodOnPlate.toolItemIsOccupying = this;
                        foodOnPlate.gameObject.transform.parent = transform;
                        foodOnPlate.gameObject.transform.localPosition = new Vector3(0, .15f, 0);
                        chef.hand[0].GetComponent<Pan>().foodInPan = null;
                        chef.hand[0].GetComponent<Pan>().Occupied = false;
                        chef.isInteracting = false;

                        //Checkcompletion
                        GameManager.Instance.CheckLevelCompletion(foodOnPlate);
                    }
                }
                else if(chef.inventoryFull)
                    {
                        Interaction = "Hands Full";
                        return;
                    }
                break;
            default:
                Interaction = "";
                break;
        }
    }
}
