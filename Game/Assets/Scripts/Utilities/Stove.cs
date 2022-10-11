using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stove :Utility
{
    [Header("States")]
    public GameObject stoveOn;
    public GameObject stoveOff;

    [Header("Item Placement")]
    public Transform placement;

    public void Start()
    {
        //ps.GetComponent<ParticleSystem>();
    }

    public Stove()
    {
        Name = "Stove";
        On = false;
        Occupied = false;
        Interaction = "";
    }

    public void Update()
    {
        CheckOccupancy();
    }

    public override void CheckHand(PlayerController.ItemInMainHand item, PlayerController chef)
    {
        if (!On)
        {
            switch (item)
            {
                default:
                    Interaction = "Turn On Stove";
                    if (chef.isInteracting)
                    {
                        On = true;
                        State(On);
                        chef.isInteracting = false;
                    }
                    break;
            }
        }
        else
        {
            switch (item)
            {
                case PlayerController.ItemInMainHand.empty:
                    Interaction = "Turn Off Stove";
                    if (chef.isInteracting)
                    {
                        On = false;
                        State(On);
                    }
                    break;
                case PlayerController.ItemInMainHand.spatula:
                    Interaction = "Turn Off Stove";
                    if (chef.isInteracting)
                    {
                        On = false;
                        State(On);
                    }
                    break;
                case PlayerController.ItemInMainHand.egg:
                    Interaction = "Turn Off Stove";
                    if (chef.isInteracting)
                    {
                        On = false;
                        State(On);
                    }
                    break;
                case PlayerController.ItemInMainHand.pan:
                    if (!Occupied)
                    {
                        Interaction = "Place Pan On Stove";
                        if (chef.isInteracting)
                        {
                            chef.hand[0].gameObject.SetActive(true);
                            chef.hand[0].gameObject.transform.position = placement.position;
                            chef.hand[0].utilityItemIsOccupying = this;
                            chef.hand[0].GetComponent<Pan>().state = Pan.State.hot;
                            chef.hand[0] = null;
                            chef.itemInMainHand = PlayerController.ItemInMainHand.empty;
                            chef.isInteracting = false;
                            Occupied = true;
                            Interaction = "";
                        }
                    }
                    break;
                default:
                    Interaction = "";
                    break;
            }
        }
    }

    public void CheckOccupancy()
    {
        if (!Occupied)
        {
            tag = "Interactable";
        }
        else
        {
            tag = "Untagged";
        }
    }

    public void State(bool condition)
    {
        if (condition)
        {
            stoveOn.SetActive(true);
            stoveOff.SetActive(false);
        }
        else
        {
            stoveOn.SetActive(false);
            stoveOff.SetActive(true);
        }
    }
}
