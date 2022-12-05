using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Sink : Utility
{
    public enum invSlots { none, one, two, three }
    public invSlots currClean; //enum to track which dish is being washed

    [Header("ProgressBar")]
    public bool isCleaning;
    public float cleanTime = 5f;
    public float cleanProgress = 0f;
    public Slider progressSlider;
    

    [Header("States")]
    public GameObject sinkEmpty;
    public GameObject sinkFull;
    public bool waterFull;

    [Header("Dishes")]
    public Item cleaningDish; //This will be whichever dish is currently being washed
    public Item sinkInv1, sinkInv2, sinkInv3;

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
        isCleaning = false;

        FillSink(false);
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
            //Empty mainhand means we're seeing if we can take anything out of the sink
            case PlayerController.ItemInMainHand.empty:
                Item[] dishes = { sinkInv1, sinkInv2, sinkInv3 };
                for (int i = 0; i < dishes.Length; i++)
                {
                    if (dishes[i] != null && dishes[i].status == Item.Status.clean)
                    {
                        Interaction = "Pick up " + dishes[i].Name;
                        if (chef.isInteracting)
                        {
                            chef.hand[0] = dishes[i];
                            chef.Inv1.text = dishes[i].Name;
                            //Set sinkInv slot to empty
                            switch (i)
                            {
                                case 0:
                                    sinkInv1 = null;
                                    break;
                                case 1:
                                    sinkInv2 = null;
                                    break;
                                case 2:
                                    sinkInv3 = null;
                                    break;
                            }

                            //If the sink is empty, drain the water
                            if (!sinkInv1 && !sinkInv2 && !sinkInv3)
                            {
                                FillSink(false);
                            }
                        }
                        return;
                    }
                }
                break;

            //Now check for the three items that can be dirty
            //Using fallthrough for all of them to be a little safer
            case PlayerController.ItemInMainHand.spatula:
            case PlayerController.ItemInMainHand.pan:
            case PlayerController.ItemInMainHand.plate:
                if (chef.hand[0].status == Item.Status.dirty)
                {
                    if (sinkInv1 && sinkInv2 && sinkInv3) //Can't put anything in the sink if all sink areas are full
                    {
                        Interaction = "Sink is full!";
                        return;
                    }

                    //Check if there's food in the pan (if there is a pan)
                    if(chef.hand[0].TryGetComponent<Pan>(out Pan pan))
                    {
                        if (pan.foodInPan != null)
                        {
                            Interaction = "Pan has food in it!";
                            return;
                        }
                    }
                    //Set interaction text
                    Interaction = "Put " + chef.hand[0].Name + " in sink";
                    if (chef.hand[0].TryGetComponent(out Item dish))
                    {
                        if (!chef.isInteracting) { break; }
                        AddSinkInv(dish);
                        chef.itemInMainHand = PlayerController.ItemInMainHand.empty;
                        chef.hand[0] = null;
                        Interaction = "";
                    }
                    else { Debug.LogError("Could not get Item component!"); }
                }
                break;
            default:
                Debug.LogWarning("Unaccounted for dirty item in Sink.cs! switch statement");
                break;
        }
        chef.isInteracting = false;
    }

    //Add a new dish to the sink inventory
    public void AddSinkInv(Item dish)
    {
        if (sinkInv1 == null)
        {
            sinkInv1 = dish;
            currClean = invSlots.one;
        }
        else if (sinkInv2 == null)
        {
            sinkInv2 = dish;
            currClean = invSlots.two;
        }
        else if (sinkInv3 == null)
        {
            sinkInv3 = dish;
            currClean = invSlots.three;
        }
        else { Debug.LogWarning("Tried to put a dish in the sink but the sink is full!"); }
    }

    //Helper function for when we start cleaning a dish
    public void ActivateCleaning()
    {
        isCleaning = true;
        progressSlider.gameObject.SetActive(true);
        FillSink(true);
        cleanProgress = 0f;
        progressSlider.value = 0f;
    }

    public void ProcessCleaning()
    {
        if (isCleaning) 
        {
            CleaningDish();
            return; 
        }

        //Set the dish to be cleaned
        if (!cleaningDish)
        {
            Item[] sinkInv = { sinkInv1, sinkInv2, sinkInv3 };
            for (int i = 0; i < sinkInv.Length; i++)
            {
                if (sinkInv[i] != null && sinkInv[i].status == Item.Status.dirty)
                {
                    cleaningDish = sinkInv[i];
                    ActivateCleaning();
                    return;
                }
            }
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
        //Add to the progress slider
        if (cleanProgress + Time.deltaTime < cleanTime)
        {
            cleanProgress += Time.deltaTime;
            progressSlider.value = cleanProgress;
        }
        else //We go here when the dish is finished being cleaned
        {
            currClean = invSlots.none;
            progressSlider.gameObject.SetActive(false);
            cleaningDish.dirtySelf.SetActive(false);
            cleaningDish.cleanSelf.SetActive(true);
            cleaningDish.status = Item.Status.clean;
            cleaningDish.currUses = 0;
            cleaningDish = null;
            isCleaning = false;   
        }
    }

    //Helper function to switch sink models
    public void FillSink(bool fillSink)
    {
        if (fillSink)
        {
            sinkFull.SetActive(true);
            sinkEmpty.SetActive(false);
        }
        else
        {
            sinkFull.SetActive(false);
            sinkEmpty.SetActive(true);
        }
    }
}
