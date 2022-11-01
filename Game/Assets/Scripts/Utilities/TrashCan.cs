using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashCan : Utility
{
    [Header("Parent Transform")]
    public GameObject parentTransform;

    [Header("Prefabs")]
    public GameObject eggPrefab;
    public GameObject spatulaPrefab;
    public GameObject panPrefab;

    public TrashCan()
    {
        Name = "Trash Can";
    }

    public override void CheckHand(PlayerController.ItemInMainHand item, PlayerController chef)
    {
        if (chef.hand[0] != null)
        {
            Interaction = "Throw " + chef.hand[0].Name + " Away";
            GameManager.isTouchingTrashCan = true;

            if (chef.isInteracting)
            {
                //spawning the item that in the player hand to its original position
                switch (item)
                {
                case PlayerController.ItemInMainHand.egg:
                    Instantiate(eggPrefab, chef.hand[0].startPosition, chef.hand[0].startRotation, parentTransform.transform);
                    break;
                case PlayerController.ItemInMainHand.pan:
                    Instantiate(panPrefab, chef.hand[0].startPosition, chef.hand[0].startRotation, parentTransform.transform);
                    break;
                case PlayerController.ItemInMainHand.spatula:
                    Instantiate(spatulaPrefab, chef.hand[0].startPosition, chef.hand[0].startRotation, parentTransform.transform);
                    break;
                }
                       
                chef.hand[0] = null;
                chef.itemInMainHand = PlayerController.ItemInMainHand.empty;
                Interaction = "";
                chef.isInteracting = false;
            }
        }
    }
}
