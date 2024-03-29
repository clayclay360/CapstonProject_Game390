using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public class CookBook : Utility
{
    public enum Status { Undamaged, Destroyed}
    

    [Header("Variables")]
    public int lives;
    public Status status;
    public GameObject[] Form;

    private bool destroying = false; //Variable used for testing

    RecipeBook setCookBookActive; //put in by Owen to activate and deactivate the cook book

    public CookBook()
    {
        Name = "Cookbook";
        Interaction = "";
        status = Status.Undamaged;
    }

    // Start is called before the first frame update
    void Start()
    {
        GameManager.cookBookActive = true;
        setCookBookActive = GameObject.Find("CookBook").GetComponentInChildren<RecipeBook>();
    }

    public void GetState(Status status)
    {
        switch (status)
        {
            case Status.Undamaged:
                Form[0].SetActive(true);
                Form[1].SetActive(false);
                break;

            case Status.Destroyed:
                Form[0].SetActive(false);
                Form[1].SetActive(true);
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.isTouchingBook)
        {
            setCookBookActive.setActiveFalseFunc();
        }
        if (lives == 0)
        {
            GameManager.cookBookActive = false;
            setCookBookActive.setActiveFalseFunc();
            status = Status.Destroyed;

            //Set to desroyed book
        }
        else if (!GameManager.cookBookActive)
        {
            lives = 0;
        }


        GetState(status);
    }

    public override void CheckHand(PlayerController.ItemInMainHand item, PlayerController chef)
    {
        if (setCookBookActive.isBookOpen)
        {
            Interaction = "Close Book";
            return;
        }
        else if (lives > 0 && GameManager.isTouchingBook)
        {
            Interaction = "Open Book";
            return;
        } 


        

        if (item == PlayerController.ItemInMainHand.pages)
        {
            Interaction = "Repair Book";
        }
        else
        {
            Interaction = "Cookbook is Destroyed.";
        }
        if (chef.isInteracting)
        {
            if (item == PlayerController.ItemInMainHand.pages) {
                lives = 3;
                GameManager.cookBookActive = true;
                setCookBookActive.setActiveTrueFunc();
                Debug.LogWarning("Cookbook repaired!");
                chef.hand[0] = null;
                if (chef.hand[1] != null)
                {
                    chef.hand[0] = chef.hand[1];
                    chef.hand[1] = null;
                }
                chef.Inv1.text = "";
                status = Status.Undamaged;
                //Restore book model
            }
        }
    }

    //TEST FUNCTION FOR DESTROYING COOKBOOK
    //private IEnumerator DestroyCookbook()
    //{
    //    if (lives <= 0) { yield return null; }
    //    yield return new WaitForSeconds(2);
    //    lives--;
    //    Debug.Log("Cookbook lives: " + lives);
    //    destroying = false;
    //}
}
