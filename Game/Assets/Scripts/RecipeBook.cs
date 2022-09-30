using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RecipeBook: MonoBehaviour
{
    public string theName;
    public GameObject recipeTextbox1;
    public GameObject recipeTextbox2;
    public GameObject recipeTextbox3;
    public GameObject backgroundImage;
    public bool recipeIsOpenP1;
    public int[] pages;
    public int[] steps;
    public int currentStep;
    public int currentPage;
    public bool isTouchingBook;

    void Start()
    {
        backgroundImage.SetActive(false); 
        pages = new int[] { 1, 2, 3 };
        steps = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
        currentPage = 1;
        currentStep = 2;

        isTouchingBook = false;
    }

    void Update()
    {
        if (Input.GetKeyDown("e") && isTouchingBook)
        {
            //variable telling the game that the recipe for player one is open
            recipeIsOpenP1 = true;

            //puts in the information for the first page when the book first opens
            recipeTextbox1.GetComponent<Text>().text = "Turn on Stove to medium.";
            recipeTextbox2.GetComponent<Text>().text = "Place Pan on Stove.";
            recipeTextbox3.GetComponent<Text>().text = "Beat the eggs (Bowl on countertop).";
            //is checking the current step to determine if it should gray out the first 3 steps on open or not
            if (currentStep >= 1) { recipeTextbox1.GetComponent<Text>().color = Color.gray; }
            else { recipeTextbox1.GetComponent<Text>().color = Color.black; }

            if (currentStep >= 2) { recipeTextbox2.GetComponent<Text>().color = Color.gray; }
            else { recipeTextbox2.GetComponent<Text>().color = Color.black; }

            if (currentStep >= 3) { recipeTextbox3.GetComponent<Text>().color = Color.gray; }
            else { recipeTextbox3.GetComponent<Text>().color = Color.black; }
            //sets the background image to show
            backgroundImage.SetActive(true);

            currentPage = 1;
        }
        else if (Input.GetKeyUp("e") || !isTouchingBook)
        {
            //variable telling the game that the recipe for player one is closed
            recipeIsOpenP1 = false;
            //empties out any information in the textboxes
            recipeTextbox1.GetComponent<Text>().text = " ";
            recipeTextbox2.GetComponent<Text>().text = " ";
            recipeTextbox3.GetComponent<Text>().text = " ";
            //hides the background image
            backgroundImage.SetActive(false);
        }

        if (Input.GetMouseButtonDown(0) && recipeIsOpenP1)
        {
            if (currentPage != pages.Length - 1)
            {
                if (currentPage == 0)
                {
                    //this is putting in the information for the first page while player 1 is clicking through the pages
                    recipeTextbox1.GetComponent<Text>().text = "Turn on Stove to medium.";
                    recipeTextbox2.GetComponent<Text>().text = "Place pan on stove.";
                    recipeTextbox3.GetComponent<Text>().text = "Beat the eggs (Bowl on countertop).";
                    //is checking the current step to determine if it should gray out the first 3 steps while player 1 is clicking through the pages
                    if (currentStep >= 1) { recipeTextbox1.GetComponent<Text>().color = Color.gray; }
                    else { recipeTextbox1.GetComponent<Text>().color = Color.black; }

                    if (currentStep >= 2) { recipeTextbox2.GetComponent<Text>().color = Color.gray; }
                    else { recipeTextbox2.GetComponent<Text>().color = Color.black; }

                    if (currentStep >= 3) { recipeTextbox3.GetComponent<Text>().color = Color.gray; }
                    else { recipeTextbox3.GetComponent<Text>().color = Color.black; }


                }
                else if (currentPage == 1)
                {
                    //this is putting in the information for the second page while player 1 is clicking through the pages
                    recipeTextbox1.GetComponent<Text>().text = "Melt Butter in the pan.";
                    recipeTextbox2.GetComponent<Text>().text = "Add eggs to pan.";
                    recipeTextbox3.GetComponent<Text>().text = "Lift and tilt eggs with spatula.";
                    //is checking the current step to determine if it should gray out the next 3 while player 1 is clicking through the pages
                    if (currentStep >= 4) { recipeTextbox1.GetComponent<Text>().color = Color.gray; }
                    else { recipeTextbox1.GetComponent<Text>().color = Color.black; }

                    if (currentStep >= 5) { recipeTextbox2.GetComponent<Text>().color = Color.gray; }
                    else { recipeTextbox2.GetComponent<Text>().color = Color.black; }

                    if (currentStep >= 6) { recipeTextbox3.GetComponent<Text>().color = Color.gray; }
                    else { recipeTextbox3.GetComponent<Text>().color = Color.black; }

                }

                currentPage++;
            } else
            {
                if (currentPage == 2)
                {
                    //this is putting in the information for the third page while player 1 is clicking through the pages
                    recipeTextbox1.GetComponent<Text>().text = "Add cheese to pan.";
                    recipeTextbox2.GetComponent<Text>().text = "Fold eggs with spatula";
                    recipeTextbox3.GetComponent<Text>().text = " ";
                    //is checking the current step to determine if it should gray out the last3 3 while player 1 is clicking through the pages
                    if (currentStep >= 7) { recipeTextbox1.GetComponent<Text>().color = Color.gray; }
                    else { recipeTextbox1.GetComponent<Text>().color = Color.black; }

                    if (currentStep >= 8) { recipeTextbox2.GetComponent<Text>().color = Color.gray; }
                    else { recipeTextbox2.GetComponent<Text>().color = Color.black; }

                    if (currentStep >= 9) { recipeTextbox3.GetComponent<Text>().color = Color.gray; }
                    else { recipeTextbox3.GetComponent<Text>().color = Color.black; }

                }
                currentPage = 0;
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "CookBook")
        {
            Debug.Log("Touching cook book");
            isTouchingBook = true;
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "CookBook")
        {
            Debug.Log("Not touching cook book");
            isTouchingBook = false;
        }
    }
}
