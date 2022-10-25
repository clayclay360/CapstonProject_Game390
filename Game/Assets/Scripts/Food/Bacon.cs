using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bacon : Item
{
    public GameObject[] Form;

    PlayerController player;
    public Bacon()
    {
        Name = "Bacon";
        Type = "Food";
        Interaction = "";
        status = Status.uncooked;
        prone = false;
    }

    public void Update()
    {
        GetState(status);

        if (toolItemIsOccupying == null)
        {
            tag = "Interactable";
        }
        else
        {
            tag = "Untagged";
        }

        if (player != null)
        {
            //player.Interact();
        }
    }

    public override void CheckHand(PlayerController.ItemInMainHand item, PlayerController chef)
    {
        player = chef;

        if (chef.inventoryFull)
        {
            Interaction = "Hands Full";
            return;
        }

        switch (item)
        {
            case PlayerController.ItemInMainHand.empty:
                Interaction = "Grab Bacon";
                if (chef.isInteracting)
                {
                    gameObject.SetActive(false);
                    Interaction = "";
                }
                break;
            case PlayerController.ItemInMainHand.spatula:
                Interaction = "Grab Bacon";
                if (chef.isInteracting)
                {
                    gameObject.SetActive(false);
                    Interaction = "";
                }
                break;
            case PlayerController.ItemInMainHand.pan:
                Interaction = "Grab Bacon";
                if (chef.isInteracting)
                {
                    gameObject.SetActive(false);
                    Interaction = "";
                }
                break;
            case PlayerController.ItemInMainHand.egg:
                Interaction = "Grab Bacon";
                if (chef.isInteracting)
                {
                    gameObject.SetActive(false);
                    Interaction = "";
                }
                break;
        }
    }

    public void GetState(Status status)
    {
        switch (status)
        {
            case Status.uncooked:
                Form[0].SetActive(true);
                break;
            case Status.cooked:
                Form[0].SetActive(false);
                Form[1].SetActive(true);
                break;
        }
    }

    public void HitByRat()
    {
        isActive = false;
        StartCoroutine(Despawn(gameObject));
        Debug.Log("Despawning Bacon");
        gameObject.SetActive(false);
    }
}
