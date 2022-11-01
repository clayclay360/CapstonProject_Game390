using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entryway : MonoBehaviour
{
    bool hasTriggered = false;


    private void OnTriggerEnter(Collider other)
    {
        //Don't enter this code unless the object is a rat that hasn't done this before
        if (other.gameObject.GetComponentInParent<RatScript>() != null  && !hasTriggered)
        {

            RatScript rat = other.gameObject.GetComponentInParent<RatScript>();

            if(!rat.hiding && !rat.objectiveComplete)
            {
                rat.CrossEntryway();
            }
        }
        //Disable trigger
        hasTriggered = true;
    }
    private void OnTriggerExit(Collider other)
    {
        //Once object passes through, allow other objects to activate trigger
        hasTriggered = false;
    }
}
