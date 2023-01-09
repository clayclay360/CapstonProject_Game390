using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Item : MonoBehaviour, IInteractable<PlayerController.ItemInMainHand, PlayerController>
{
    public string Name;
    public string Type;
    public float despawnTime;

    //I'll definetly have to make a food and tool child
    public enum Status { uncooked, cooked, burnt, clean, dirty, spoiled }
    public Status status;

    [Header("Models")]
    public GameObject cleanSelf;
    public GameObject dirtySelf;

    [HideInInspector]
    public Vector3 startPosition;
    [HideInInspector]
    public Quaternion startRotation;

    [HideInInspector] public bool Occupied;
    [HideInInspector] public bool prone;
    [HideInInspector] public bool isActive;
    [HideInInspector] public bool isTarget;
    [HideInInspector] public string Interaction;
    [HideInInspector] public Utility utilityItemIsOccupying;
    [HideInInspector] public Item toolItemIsOccupying;
    //Sink
    [HideInInspector] public int usesUntilDirty;
    [HideInInspector] public int currUses;
    [HideInInspector] public GameObject counterInUse;


    //IF YOU ARE READING THIS DON'T YOU DARE ADD AN AWAKE METHOD
    //TO AN ITEM INHERITING FROM THIS CLASS DO YOU HEAR ME?
    //FOD GOD'S SAKES IF YOU DO MAKE SURE YOU CALL base.Awake()
    //IN IT BECAUSE THIS ISSUE CAUSED SO MANY ERRORS. FML.
    public void Awake()
    {
        startPosition = transform.position;
        startRotation = transform.rotation;
        isActive = true;
    }

    public void Start()
    {

    }

    //Function to manage dishes and stuff getting dirty. currUses and usesUntilDirty
    //should be set from the Awake() or Start() functions of the individual items
    public void CheckIfDirty()
    {
        
        currUses += 1;
        if (currUses >= usesUntilDirty)
        {
            status = Status.dirty;
            cleanSelf.SetActive(false);
            dirtySelf.SetActive(true);
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

    public void DespawnItem(GameObject item)
    {
        isActive = false;
        Debug.Log("Despawning " + item.name);
        item.GetComponent<Collider>().enabled = false;
        MeshRenderer[] meshRenderers = item.GetComponentsInChildren<MeshRenderer>();
        foreach (MeshRenderer meshRenderer in meshRenderers)
        {
            meshRenderer.enabled = false;
        }
    }

    public void RespawnItem(GameObject item)

    {
        isActive = true;
        Debug.Log("Respawning " + item.name);
        item.GetComponent<Collider>().enabled = true;
        MeshRenderer[] meshRenderers = item.GetComponentsInChildren<MeshRenderer>();
        foreach (MeshRenderer meshRenderer in meshRenderers)
        {
            meshRenderer.enabled = true;
        }
    }

    public void CheckCounter()
    {
        for (int i = 0; i <= GameManager.counterItems.Length; i++)
        {
            if (i >= GameManager.counterItems.Length)
            {
                return;
            }

            if (gameObject.name == GameManager.counterItems[i])
            {
                GameManager.counterItems[i] = "";
                return;
            }
        }
    }

    public void PlaceOnCounter(GameObject counter)
    {
        transform.position = counter.transform.position;
        gameObject.SetActive(true);

        CounterTop counterScript = counter.GetComponentInChildren<CounterTop>();
        counterScript.inUse = true;
    }

    public void CheckIndividualCounters(GameObject counter)
    {
        CounterTop counterScript = counter.GetComponentInChildren<CounterTop>();
        if (counterScript.inUse)
        {
            counterScript.inUse = false;
        }
        else
        {
            return;
        }

    }

    public void HelpRunItemCode(GameObject counter)
    {
        counterInUse = counter;
    }

    public void HelpDeleteGameObject()
    {
        counterInUse = null;
    }
}
