using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Window : Utility
{

    public override void CheckHand(PlayerController.ItemInMainHand item, PlayerController chef)
    {
        for (int i = 0; i < GameManager.counterItems.Length; i++)
        {
            if (GameManager.counterItems[i] == "" && chef.hand[0])
            {
                Interaction = "Place " + chef.hand[0].name + " in Window";
                return;
            }
        }
        Interaction = "";
    }
}
