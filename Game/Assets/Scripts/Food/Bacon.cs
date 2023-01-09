using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bacon : Item
{
    public GameObject[] Form;
    private GameObject passItems;
    public Vector3 origPos;

    PlayerController player;
    public Bacon()
    {
        Name = "Bacon";
        Type = "Food";
        Interaction = "";
        status = Status.uncooked;
        prone = false;
    }

    private new void Start()
    {
        passItems = GameObject.Find("PassItems");
        GameManager.bacon = GameObject.Find("Bacon(Clone)").GetComponentInChildren<Bacon>(); //This line returns an error every time the game is started
        origPos = transform.position;
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
                    CheckCounter();
                    if (counterInUse != null)
                    {
                        CheckIndividualCounters(counterInUse);
                    }
                }
                break;
            case PlayerController.ItemInMainHand.spatula:
                Interaction = "Grab Bacon";
                if (chef.isInteracting)
                {
                    gameObject.SetActive(false);
                    Interaction = "";
                    CheckCounter();
                    if (counterInUse != null)
                    {
                        CheckIndividualCounters(counterInUse);
                    }
                }
                break;
            case PlayerController.ItemInMainHand.pan:
                Interaction = "Grab Bacon";
                if (chef.isInteracting)
                {
                    gameObject.SetActive(false);
                    Interaction = "";
                    CheckCounter();
                    if (counterInUse != null)
                    {
                        CheckIndividualCounters(counterInUse);
                    }
                }
                break;
            case PlayerController.ItemInMainHand.egg:
                Interaction = "Grab Bacon";
                if (chef.isInteracting)
                {
                    gameObject.SetActive(false);
                    Interaction = "";
                    CheckCounter();
                    if (counterInUse != null)
                    {
                        CheckIndividualCounters(counterInUse);
                    }
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
            case Status.burnt:
                Name = "Burnt Bacon";
                Form[1].SetActive(false);
                Form[2].SetActive(true);
                break;
        }
    }

    public void PassBacon(int passLocation)
    {
        if (passLocation == 0)
        {
            transform.position = passItems.transform.position + new Vector3(0, 0, 0.5f);
            gameObject.SetActive(true);
        }
        else if (passLocation == 1)
        {
            transform.position = passItems.transform.position;
            gameObject.SetActive(true);
        }
        else if (passLocation == 2)
        {
            transform.position = passItems.transform.position + new Vector3(0, 0, -0.5f);
            gameObject.SetActive(true);
        }
    }

    public void DropBaconOnGround(GameObject player)
    {
        transform.position = player.transform.position;
        gameObject.SetActive(true);
    }

    public void Respawn()
    {
        transform.position = origPos;
        gameObject.SetActive(true);
    }
}
