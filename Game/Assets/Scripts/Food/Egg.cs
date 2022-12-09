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
    private GameObject passItems;
    public Vector3 origPos;

    PlayerController player;
    public Egg()
    {
        Name = "Egg";
        Type = "Food";
        Interaction = "";
        status = Status.uncooked;
        prone = false;
        state = State.shell;
    }

    new void Start()
    {
        base.Start();
        passItems = GameObject.Find("PassItems");
        GameManager.egg = GameObject.Find("Egg(Clone)").GetComponentInChildren<Egg>(); //This line returns an error every time the game is started.
        origPos = transform.position;
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
            Interaction = "Hands Full";
            return;
        }

        switch (item)
        {
            case PlayerController.ItemInMainHand.empty:
                Interaction = "Grab Egg";
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
                Interaction = "Grab Egg";
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
                Interaction = "Grab Egg";
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
            case PlayerController.ItemInMainHand.bacon:
                Interaction = "Grab Egg";
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
            case State.omelet:
                Name = "Omelet";
                Form[1].SetActive(false);
                Form[2].SetActive(true);
                break;
        }
    }

    public void PassEgg(int passLocation)
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

    public void DropEggOnGround(GameObject player)
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
