using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utility : Interactable
{

    //declares interaction type: 1 for hold button for event, 2 for rapidly press button for event, or 3 for dispose of item
    public int interactionType;

    private int progress = 0;
    private int complete = 10;

    public override int Interact()
    {
        return interactionType;
    }

    //on correct interaction, raise progress
    public void makeProgress(int x)
    {
        progress += x;
        if (progress == complete)
        {
            //when complete, update event
        }
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
