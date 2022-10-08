using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Item : MonoBehaviour, IInteractable<PlayerController.ItemInMainHand, PlayerController>
{
    public string Name;
    public string Type;
    public float Height;
    
    //I'll definetly have to make a food and tool child
    public enum Status {uncooked, cooked, burnt}
    [HideInInspector] public Status status;
    [HideInInspector] public bool Occupied;
    [HideInInspector] public bool Prone;
    [HideInInspector] public string Interaction;
    [HideInInspector] public Utility utilityItemIsOccupying;
    [HideInInspector] public Item toolItemIsOccupying;
    public void Properties() { }

    public bool Interact(PlayerController chef)
    {
        return false /*chef.OnInteract()*/;
    }

    public virtual void CheckHand(PlayerController.ItemInMainHand item, PlayerController chef)
    {

    }

    public void Occupancy()
    {
        if (utilityItemIsOccupying != null)
        {
            utilityItemIsOccupying.Occupied = false;
            utilityItemIsOccupying = null;
        }
        Interaction = "";
        gameObject.SetActive(false);
    }
}
