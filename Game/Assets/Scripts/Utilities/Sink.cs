using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Sink : Utility
{
    public enum dishBeingCleaned { none, pan, spatula, plate }
    public dishBeingCleaned currentDish; //enum to track which dish is being washed

    [Header("ProgressBar")]
    public bool isCleaning;
    public float cleanTime = 5f;
    public float cleanProgress = 0f;
    public Slider progressSlider;
    

    [Header("States")]
    public GameObject sinkEmpty;
    public GameObject sinkFull;

    [Header("Dishes")]
    
    public Pan sinkPan;
    public Spatula sinkSpatula;
    public Plate sinkPlate;
    public Item cleaningDish; //This will be whichever dish is currently being washed

    public Sink()
    {
        Name = "Sink";
        Interaction = "";
        Occupied = false;
        On = false;
    }

    private void Awake()
    {
        progressSlider.GetComponent<Slider>();
        currentDish = 0; //none
        isCleaning = false;
    }

    public void Update()
    {
        ProcessCleaning();
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
                            //Now this is an awful way to do this and I don't want to see anything like this in the future
                            switch (dish.Name)
                            {
                                case "Pan":
                                    sinkPan = null;
                                    break;
                                case "Plate":
                                    sinkPlate = null;
                                    break;
                                case "Spatula":
                                    sinkSpatula = null;
                                    break;
                            }
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
                    Interaction = "";
                }
                else { Debug.LogError("Could not get Plate component with ItemInMainHand.plate!"); }
                break;
            default:
                Debug.LogWarning("Unaccounted for dirty item in Sink.cs! switch statement");
                break;
        }
        chef.isInteracting = false;
    }

    //Helper function for when we start cleaning a dish
    public void ActivateCleaning()
    {
        isCleaning = true;
        progressSlider.gameObject.SetActive(true);
        cleanProgress = 0f;
        progressSlider.value = 0f;
    }

    public void ProcessCleaning()
    {
        //Process a dish being cleaned
        if (isCleaning) {
            CleaningDish();
            return; 
        }


        
        switch (currentDish)
        {
            //Check for other dishes and set them to be cleaned
            //Right now, this will check in the order they are listed
            //which means technically there is a priority list
            case dishBeingCleaned.none:

                if (sinkPan != null && sinkPan.status == Item.Status.dirty)
                {
                    cleaningDish = sinkPan;
                    ActivateCleaning();
                    currentDish = dishBeingCleaned.pan;
                }
                else if (sinkSpatula != null && sinkSpatula.status == Item.Status.dirty)
                {
                    cleaningDish = sinkSpatula;
                    ActivateCleaning();
                    currentDish = dishBeingCleaned.spatula;
                }
                else if (sinkPlate != null && sinkPlate.status == Item.Status.dirty)
                {
                    cleaningDish = sinkPlate;
                    ActivateCleaning();
                    currentDish = dishBeingCleaned.plate;
                }
                if (currentDish != dishBeingCleaned.none)
                {
                    progressSlider.gameObject.SetActive(true);
                }
                break;
            default:

                break;
        }
    }

    //Runs every frame a dish is being cleaned, adding to the progressbar
    public void CleaningDish()
    {
        if (cleaningDish == null)
        {
            Debug.LogError("IsCleaning but no dish in the sink!");
            return;
        }
        if (cleanProgress + Time.deltaTime < cleanTime)
        {
            //Debug.Log("Adding Progress!!");
            cleanProgress += Time.deltaTime;
            progressSlider.value = cleanProgress;
        }
        else //We go here when the dish is finished being cleaned
        {
            //Debug.LogWarning(cleaningDish.Name +  " is clean!");
            progressSlider.gameObject.SetActive(false);
            cleaningDish.status = Item.Status.clean;
            cleaningDish.currUses = 0;
            currentDish = dishBeingCleaned.none;
            isCleaning = false;
        }
    }
}
