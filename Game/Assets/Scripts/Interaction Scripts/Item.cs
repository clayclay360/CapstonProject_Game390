using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Item : MonoBehaviour, IInteractable<PlayerController.ItemInMainHand, PlayerController>
{
    public string Name;
    public string Type;
    public float despawnTime;

    [HideInInspector]
    public Vector3 startPosition;
    [HideInInspector]
    public Quaternion startRotation;

    //I'll definetly have to make a food and tool child
    public enum Status {uncooked, cooked, burnt, clean, dirty}
    [HideInInspector] public Status status;
    [HideInInspector] public bool Occupied;
    [HideInInspector] public bool prone;
    [HideInInspector] public bool isActive;
    [HideInInspector] public string Interaction;
    [HideInInspector] public Utility utilityItemIsOccupying;
    [HideInInspector] public Item toolItemIsOccupying;
    //Sink
    [HideInInspector] public int usesUntilDirty;
    [HideInInspector] public int currUses;
    public void Start()
    {
        startPosition = transform.position;
        startRotation = transform.rotation;
        isActive = true;
    }

    //Function to manage dishes and stuff getting dirty. currUses and usesUntilDirty
    //should be set from the Awake() or Start() functions of the individual items
    public void CheckIfDirty()
    {
        
        currUses += 1;
        if (currUses >= usesUntilDirty)
        {
            status = Status.dirty;
        }
        //Debug.LogError(currUses + " // " + usesUntilDirty);
    }

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

    public IEnumerator Despawn(GameObject item)
    {
        Debug.Log(item.name);
        yield return new WaitForSeconds(despawnTime);
        isActive = true;
        Debug.Log("Respawning Item");
        item.SetActive(true);
    }
}
