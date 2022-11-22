using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CounterTop : Utility
{
    Pan counterPan;
    Bacon counterBacon;
    Egg counterEgg;
    Spatula counterSpatula;
    CookBookPages counterPages;

    public bool inUse;

    void Start()
    {
        counterPan = GameObject.Find("Pan").GetComponentInChildren<Pan>();
        counterBacon = GameObject.Find("Bacon").GetComponentInChildren<Bacon>();
        counterEgg = GameObject.Find("Egg").GetComponentInChildren<Egg>();
        counterSpatula = GameObject.Find("Spatula").GetComponentInChildren<Spatula>();
        counterPages = GameObject.Find("Pages").GetComponentInChildren<CookBookPages>();

        //Debug.LogError(counterBacon);
        inUse = false;
    }

    public void AddToCounterTop(string item)
    {

        if (item.Contains("Spatula"))
        {
            counterSpatula.PlaceOnCounter(gameObject);
        }
        else if (item.Contains("Pan"))
        {
            counterPan.PlaceOnCounter(gameObject);
        }
        else if (item.Contains("Egg"))
        {
            counterEgg.PlaceOnCounter(gameObject);
        }
        else if (item.Contains("Bacon"))
        {
            counterBacon.PlaceOnCounter(gameObject);
        }


        Debug.LogError(inUse);
    }

    public void CheckIfInUse()
    {
        counterSpatula.HelpRunItemCode(gameObject);
        counterPan.HelpRunItemCode(gameObject);
        counterBacon.HelpRunItemCode(gameObject);
        counterEgg.HelpRunItemCode(gameObject);
        counterPages.HelpRunItemCode(gameObject);
    }

    public void DeleteGameObject()
    {
        counterSpatula.HelpDeleteGameObject();
        counterPan.HelpDeleteGameObject();
        counterBacon.HelpDeleteGameObject();
        counterEgg.HelpDeleteGameObject();
        counterPages.HelpDeleteGameObject();
    }

    public override void CheckHand(PlayerController.ItemInMainHand item, PlayerController chef)
    {
        if (GameManager.putOnCounter && chef.hand[0])
        {
            Interaction = "Place " + chef.hand[0].name + " on counter";
            return;
        }
        Interaction = "";
    }
}
