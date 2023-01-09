using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entryway : MonoBehaviour
{
    bool hasTriggered = false;


    private void OnTriggerStay(Collider other)
    {
        //Don't enter this code unless the object is a rat
        if (other.gameObject.GetComponentInParent<RatScript>() != null  && !hasTriggered)
        {
            RatScript rat = other.gameObject.GetComponentInParent<RatScript>();

            //Don't continue if the rat is hiding, carrying an item, returning to a vent, or has already gone through this trigger
            if (!rat.hiding && !rat.objectiveComplete && !rat.isCarryingItem && !hasTriggered)
            {
                hasTriggered = true;
                rat.CrossEntryway();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        //Once object passes through, allow other objects to activate trigger
        hasTriggered = false;
    }
}
