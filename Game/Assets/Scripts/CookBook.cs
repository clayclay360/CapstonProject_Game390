using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CookBook : Utility
{
    [Header("Variables")]
    public int lives;

    private bool destroying = false; //Variable used for testing

    public CookBook()
    {
        Name = "Cookbook";
        Interaction = "";
    }

    // Start is called before the first frame update
    void Start()
    {
        GameManager.cookBookActive = true;

    }

    // Update is called once per frame
    void Update()
    {
        if (lives == 0)
        {
            GameManager.cookBookActive = false;
            //Set to desroyed book
        }
        //TESTING
        else if (!destroying)
        {
            //StartCoroutine(DestroyCookbook());
            destroying = true;
        }
    }

    public override void CheckHand(PlayerController.ItemInMainHand item, PlayerController chef)
    {
        if (lives > 0)
        {
            Interaction = "Flip page";
            //Move normal interaction here
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
                Debug.LogWarning("Cookbook repaired!");
                chef.itemInMainHand = PlayerController.ItemInMainHand.empty;
                chef.Inv1.text = "";
                //Restore book model
            }
        }
    }

    //TEST FUNCTION FOR DESTROYING COOKBOOK
    /*private IEnumerator DestroyCookbook()
    {
        if (lives <= 0) { yield return null; }
        yield return new WaitForSeconds(2);
        lives--;
        Debug.Log("Cookbook lives: " + lives);
        destroying = false;
    }*/
}
