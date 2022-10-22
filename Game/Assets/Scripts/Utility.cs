using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utility : MonoBehaviour, IInteractable<PlayerController.ItemInMainHand,PlayerController>
{

    //declares interaction type: 1 for hold button for event, 2 for rapidly press button for event, or 3 for dispose of item
    public int interactionType;

    //checks item needed; checks item returned
    public int itemNeed;
    public int itemGive;

    //tracks progress
    private int progress = 0;
    private int complete = 10;

    public string Name;
    public float Height;
    public bool On;
    public bool Occupied;
    [HideInInspector] public string Interaction;

    public virtual void Properties() { }

    public bool Interact(PlayerController chef)
    {
        return false/*chef.OnInteract()*/ ;
    }

    public virtual void CheckHand(PlayerController.ItemInMainHand item, PlayerController chef) { }

    //public override int Interact()
    //{
    //    return interactionType;
    //}

    //on correct interaction, raise progress
    public bool makeProgress(int x)
    {
        progress += x;
        return progress == complete;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
