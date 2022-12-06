using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashCan : Utility
{

    [Header("Prefabs")]
    public GameObject eggPrefab;
    public GameObject spatulaPrefab;
    public GameObject panPrefab;
    public PrefabReferences pr;

    public TrashCan()
    {
        Name = "Trash Can";
    }

    public override void CheckHand(PlayerController.ItemInMainHand item, PlayerController chef)
    {
        if (chef.hand[0] != null)
        {
            //Check for the pan and throw out food in it instead of the pan itself
            //This will have to be refactored if we add other "container" items
            //or we'll have to manually add matching functionality like this.
            if (chef.hand[0].TryGetComponent<Pan>(out Pan pan))
            {
                if (pan.foodInPan) 
                {
                    Interaction = "Throw " + pan.foodInPan.name + " Away";
                    GameManager.isTouchingTrashCan = true; //I don't even know what this line does but I'm copying it here

                    if (chef.isInteracting)
                    {
                        //Respawn the food and empty the pan
                        RespawnItem(pan.foodInPan);
                        Destroy(pan.foodInPan.gameObject);
                        pan.foodInPan = null;
                        Interaction = "";
                        chef.isInteracting = false;
                    }
                    return; //Stop here because the pan work is done.
                }
            }

            //No food in pan
            Interaction = "Throw " + chef.hand[0].Name + " Away";
            GameManager.isTouchingTrashCan = true;

            if (chef.isInteracting)
            {
                //Instance the prefab if it is on our list of respawnable prefabs
                RespawnItem(chef.hand[0]);
                //Clear the player character
                Destroy(chef.hand[0].gameObject);
                chef.hand[0] = null;
                chef.itemInMainHand = PlayerController.ItemInMainHand.empty;
                Interaction = "";
                chef.isInteracting = false;
            }
        }
    }

    //Helper function to respawn items
    private void RespawnItem(Item item)
    {
        //Find and instance the object
        GameObject prefab = pr.FindPrefab(item);
        if (prefab)
        {
            Instantiate(prefab, item.startPosition, item.startRotation, transform.parent);
        }
    }
}
