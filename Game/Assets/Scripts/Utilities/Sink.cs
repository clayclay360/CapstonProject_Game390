using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sink : Utility
{
    
    [Header("States")]
    public GameObject sinkEmpty;
    public GameObject sinkFull;

    [Header("Dishes")]
    public Pan sinkPan;
    public Spatula sinkSpatula;
    public Plate sinkPlate;

    public Sink()
    {
        Name = "Sink";
        Interaction = "";
        Occupied = false;
        On = false;
    }

    public override void CheckHand(PlayerController.ItemInMainHand item, PlayerController chef)
    {
        //Stop now if the player's item isn't dirty
        if (chef.hand[0] != null && chef.hand[0].status != Item.Status.dirty)
        {
            Interaction = "";
            return;
        }


        //Checking the player's mainhand item
        switch (item)
        {
            case PlayerController.ItemInMainHand.empty:
                Item[] dishes = { sinkPan, sinkPlate, sinkSpatula };
                foreach (Item dish in dishes)
                {
                    if (dish != null && dish.status == Item.Status.clean)
                    {
                        Interaction = "Pick up " + dish.Name;
                        if (chef.isInteracting)
                        {
                            chef.hand[0] = dish;
                            chef.Inv1.text = dish.Name;
                            //chef.itemInMainHand = PlayerController.ItemInMainHand.
                        }
                        return;
                    }
                }
                break;


            //Now check for the three items that can be dirty
            //Spatula
            case PlayerController.ItemInMainHand.spatula:
                Interaction = "Put spatula in sink";
                //Try and get spatula component, clear play hand and set spatula to sink
                if (chef.hand[0].TryGetComponent(out Spatula spatula))
                {
                    if (!chef.isInteracting) { break; }
                    sinkSpatula = spatula;
                    chef.itemInMainHand = PlayerController.ItemInMainHand.empty;
                    chef.hand[0] = null;
                    StartCoroutine(WaitForClean(sinkSpatula));
                    Interaction = "";
                }
                else { Debug.LogError("Could not get Spatula component with ItemInMainHand.spatula!");}
                break;
            //Pan
            case PlayerController.ItemInMainHand.pan:
                Interaction = "Put pan in sink";
                //We do the same thing for the other possible items
                if (chef.hand[0].TryGetComponent(out Pan pan))
                {
                    if (!chef.isInteracting) { break; }
                    sinkPan = pan;
                    chef.itemInMainHand = PlayerController.ItemInMainHand.empty;
                    chef.hand[0] = null;
                    StartCoroutine(WaitForClean(sinkPan));
                    Interaction = "";
                }
                else { Debug.LogError("Could not get Pan component with ItemInMainHand.pan!"); }
                break;
            //Plate
            case PlayerController.ItemInMainHand.plate:
                Interaction = "Put plate in sink";
                if (chef.hand[0].TryGetComponent(out Plate plate))
                {
                    if (!chef.isInteracting) { break; }
                    sinkPlate = plate;
                    chef.itemInMainHand = PlayerController.ItemInMainHand.empty;
                    chef.hand[0] = null;
                    StartCoroutine(WaitForClean(sinkPlate));
                    Interaction = "";
                }
                else { Debug.LogError("Could not get Plate component with ItemInMainHand.plate!"); }
                break;
            default:
                Debug.LogWarning("Unaccounted for dirty item in Sink.cs! switch statement");
                break;
        }
    }

    public IEnumerator WaitForClean(Item item)
    {
        yield return new WaitForSeconds(5f);
        item.status = Item.Status.clean;
        Debug.Log(item.Name + " is clean!");
        yield return null;
    }
}
