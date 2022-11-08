using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Window : Utility
{

    public override void CheckHand(PlayerController.ItemInMainHand item, PlayerController chef)
    {
        for (int i = 0; i < gm.counterItems.Length; i++)
        {
            if (gm.counterItems[i] == "" && chef.hand[0])
            {
                Debug.LogError("Pepsi");
                Interaction = "Place " + chef.hand[0].name + " in Window";
                return;
            }
        }
        Interaction = "";
    }
}
