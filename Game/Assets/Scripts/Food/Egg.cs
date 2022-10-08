using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.PlayerLoop;

public class Egg : Item
{
    public GameObject[] Form;
    public enum State { shell, yoke, omelet };
    public State state;

    PlayerController player;
    public Egg()
    {
        Name = "Egg";
        Type = "Food";
        Interaction = "";
        status = Status.uncooked;
        Prone = false;
        state = State.shell;
    }

    public void Update()
    {
        GetState(state);

        if (toolItemIsOccupying == null)
        {
            tag = "Interactable";
        }
        else
        {
            tag = "Untagged";
        }

        if(player != null)
        {
            //player.Interact();
        }
    }

    public override void CheckHand(PlayerController.ItemInMainHand item, PlayerController chef)
    {
        player = chef;

        if (chef.inventoryFull)
        {
            Interaction = "Inventory Full";
            return;
        }

        switch (item)
        {
            case PlayerController.ItemInMainHand.empty:
                Interaction = "Grab Egg";
                if (chef.isInteracting)
                {
                    Debug.Log("Egg Grabbed");
                    gameObject.SetActive(false);
                    Interaction = "";
                }
                break;
            case PlayerController.ItemInMainHand.spatula:
                Interaction = "Grab Egg";
                if (chef.isInteracting)
                {
                    gameObject.SetActive(false);
                    Interaction = "";
                }
                break;
            case PlayerController.ItemInMainHand.pan:
                Interaction = "Grab Egg";
                if (chef.isInteracting)
                {
                    gameObject.SetActive(false);
                    Interaction = "";
                }
                break;
        }
    }

    public void GetState(State state)
    {
        switch (state)
        {
            case State.shell:
                Form[0].SetActive(true);
                break;
            case State.yoke:
                Form[0].SetActive(false);
                Form[1].SetActive(true);
                break;
        }
    }
}
