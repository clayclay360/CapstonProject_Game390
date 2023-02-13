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
                        RespawnItem(pan.foodInPan, chef);
                        Destroy(pan.foodInPan.gameObject);
                        pan.foodInPan = null;
                        pan.Occupied = false;
                        pan.Name = "Pan";
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
                RespawnItem(chef.hand[0], chef);
                //Clear the player character
                Destroy(chef.hand[0].gameObject);
                chef.hand[0] = null;
                if (chef.hand[1] != null)
                {
                    chef.hand[0] = chef.hand[1];
                    chef.hand[1] = null;
                }
                chef.itemInMainHand = PlayerController.ItemInMainHand.empty;
                Interaction = "";
                chef.isInteracting = false;
            }
        }
    }

    //Helper function to respawn items
    private void RespawnItem(Item item, PlayerController chef)
    {
        //Find and instance the object
        GameObject prefab = pr.FindPrefab(item);
        if (prefab)
        {
            GameObject newFood = (GameObject)Instantiate(prefab, item.startPosition, item.startRotation, transform.parent);
            newFood.name = item.name;
            //newFood.GetComponent<Item>().passItems = GameObject.Find("PassItems");

            switch (newFood.name)
            {
                case "Pan":
                    chef.passPan = GameObject.Find("Pan").GetComponentInChildren<Pan>();
                    newFood.GetComponent<Pan>().passItems = GameObject.Find("PassItems");
                    break;

                case "Spatula":
                    chef.passSpatula = GameObject.Find("Spatula").GetComponentInChildren<Spatula>();
                    newFood.GetComponent<Spatula>().passItems = GameObject.Find("PassItems");
                    break;

                case "Egg":
                    chef.passEgg = GameObject.Find("Egg").GetComponentInChildren<Egg>();
                    newFood.GetComponent<Egg>().passItems = GameObject.Find("PassItems");
                    break;

                case "Bacon":
                    chef.passBacon = GameObject.Find("Bacon").GetComponentInChildren<Bacon>();
                    newFood.GetComponent<Bacon>().passItems = GameObject.Find("PassItems");
                    break;
            }
                
        }
    }
}
