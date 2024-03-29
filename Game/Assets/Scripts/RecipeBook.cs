using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RecipeBook : MonoBehaviour
{
    public string theName;
    public GameObject recipeTextbox1;
    public GameObject recipeTextbox2;
    public GameObject backgroundImage;
    //public GameObject taskManager;
    //public TaskManager currentStepTaskMang;
    public int[] pages;
    public int[] steps;
    public int currentStep;
    public int currentPage;
    public bool isBookOpen = false;

    void Start()
    {
        pages = new int[] { 1, 2, 3, 4 };
        steps = new int[] { 1, 2, 3, 4, 5, 6, 7, 8 };
        currentPage = 0;
        currentStep = 2;
        GameManager.isStepCompleted.Add(0);
        GameManager.recipeIsOpenP1 = true;
        GameManager.recipeIsOpenP2 = false;
        GameManager.isTouchingBook = false;
        recipeTextbox1.SetActive(false);
        recipeTextbox2.SetActive(false);
        backgroundImage.SetActive(false);
        UpdateRecipeBookText(4);

        //printRecipeBookText("Turn on Stove to medium.", "Place Pan on Stove.", 1, 2);
        //currentStepTaskMang = taskManager.GetComponent<TaskManager>();
    }



    void Update()
    {

    }



    public bool checkIfStepCompleted(int step) //This function is to determine if the step in the recipe book should be grayed out or not
    {

        if (GameManager.isStepCompleted.Contains(step)) //This is checking if the step is in the array or not, so as to return true or false
        {
            return (true);
        }
        return false;
        //else
        //{
        //    isTrue = false;
        //}
        //return (isTrue);
    }

    public void printRecipeBookText(string recipeText1, string recipeText2, int checkNum1, int checkNum2) //This function is taking in the text and printing it out to the game
    {
        //puts in the information for the first page when the book first opens
        recipeTextbox1.GetComponent<Text>().text = recipeText1;
        recipeTextbox2.GetComponent<Text>().text = recipeText2;
        //is checking the current step to determine if it should gray out the first 3 steps on open or not
        //if (checkIfStepCompleted(checkNum1)) { recipeTextbox1.GetComponent<Text>().color = Color.gray; }
        //else { recipeTextbox1.GetComponent<Text>().color = Color.black; }

        //if (checkIfStepCompleted(checkNum2)) { recipeTextbox2.GetComponent<Text>().color = Color.gray; }
        //else { recipeTextbox2.GetComponent<Text>().color = Color.black; }
    }

    public void ClickOnBook(float value)
    {
        //if (!GameManager.cookBookActive) { return; };
        //if (GameManager.isTouchingBook && GameManager.cookBookActive) //This is to detect if the player is touching the book
        //{
        //    int val = (int)value;
        //    currentPage += val;
        //    if(currentPage > 2)
        //    {
        //        currentPage = 0;
        //    }
        //    else if(currentPage < 0)
        //    {
        //        currentPage = 2;
        //    }
        //    UpdateRecipeBookText(currentPage);

        //}
        SwitchRecipe(0);
    }

    public void SwitchRecipe(float currentRecipe)
    {
        GameManager.recipeIsOpenP1 = !GameManager.recipeIsOpenP1;
        GameManager.recipeIsOpenP2 = !GameManager.recipeIsOpenP2;

        currentPage = 0;
        UpdateRecipeBookText(currentPage);
    }

    public void UpdateRecipeBookText(int currentStep)
    {
        if(GameManager.recipeIsOpenP1)
        {
            printRecipeBookText("Omelet Recipe\nStep 1: Turn on Stove.\nStep 2: Place Pan on Stove.\nStep 3: Crack Egg in pan.", "\nStep 4: Fold egg with spatula.\nStep 5: Serve Omelet on plate.", 1, 2);
                
        }
        else if (GameManager.recipeIsOpenP2)
        {
            printRecipeBookText("Bacon Recipe\nStep 1: Turn on Stove.\nStep 2: Place Pan on Stove.\nStep 3: Put bacon in pan.", "\nStep 4: Use spatula to make sure bacon doesn't burn.\nStep 5: Serve bacon on plate.", 1, 2);

        }
    }

    public void setActiveFalseFunc()
    {
        isBookOpen = false;
        recipeTextbox1.SetActive(false);
        recipeTextbox2.SetActive(false);
        backgroundImage.SetActive(false);
    }

    public void setActiveTrueFunc()
    {
        isBookOpen = true;
        recipeTextbox1.SetActive(true);
        recipeTextbox2.SetActive(true);
        backgroundImage.SetActive(true);
    }

    /*public void ClickThroughBook()
    {
        if (GameManager.recipeIsOpenP1)
        {
            //currentStepTaskMang = taskManager.GetComponent<TaskManager>();

            if (currentPage != pages.Length - 1)
            {
                if (currentPage == 0)
                {
                    printRecipeBookText("Turn on Stove to medium.", "Place Pan on Stove.", "Beat the eggs (Bowl on countertop).", 1, 2, 3);


                }
                else if (currentPage == 1)
                {
                    printRecipeBookText("Melt Butter in the pan.", "Add eggs to pan.", "Lift and tilt eggs with spatula.", 4, 5, 6);
                }

                currentPage++;
            }
            else
            {
                if (currentPage == 2)
                {
                    printRecipeBookText("Add cheese to pan.", "Fold eggs with spatula", " ", 7, 8, 9);
                }
                currentPage = 0;
            }
        }
    }*/
}
