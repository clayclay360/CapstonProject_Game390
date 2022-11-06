using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Window : Utility
{

    public override void CheckHand(PlayerController.ItemInMainHand item, PlayerController chef)
    {
        for (int i = 0; i < gm.counterItems.Length; i++)
        {
            if (gm.counterItems[i] != "")
            {
                Interaction = "Place " + gm.counterItems[i] + " in Window";
                return;
            }
        }
    }
}
