using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashCan : Utility
{
    public TrashCan()
    {
        Name = "Trash Can";
    }

    public override void CheckHand(PlayerController.ItemInMainHand item, PlayerController chef)
    {
        if (chef.hand[0] != null)
        {
            Interaction = "Throw " + chef.hand[0].Name + " Away";
            if (chef.isInteracting)
            {
                chef.hand[0] = null;
                chef.itemInMainHand = PlayerController.ItemInMainHand.empty;
                Interaction = "";
            }
        }
    }
}
