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
    private bool isTrue;

    void Start()
    {
        pages = new int[] { 1, 2, 3, 4 };
        steps = new int[] { 1, 2, 3, 4, 5, 6, 7, 8 };
        currentPage = 0;
        currentStep = 2;
        GameManager.isStepCompleted.Add(0);
        GameManager.recipeIsOpenP1 = false;
        GameManager.recipeIsOpenP2 = false;
        GameManager.isTouchingBook = false;
        recipeTextbox1.SetActive(false);
        recipeTextbox2.SetActive(false);
        backgroundImage.SetActive(false);

        printRecipeBookText("Turn on Stove to medium.", "Place Pan on Stove.", 1, 2);
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
        else
        {
            isTrue = false;
        }
        return (isTrue);
    }

    public void printRecipeBookText(string recipeText1, string recipeText2, int checkNum1, int checkNum2) //This function is taking in the text and printing it out to the game
    {
        //puts in the information for the first page when the book first opens
        recipeTextbox1.GetComponent<Text>().text = recipeText1;
        recipeTextbox2.GetComponent<Text>().text = recipeText2;
        //is checking the current step to determine if it should gray out the first 3 steps on open or not
        if (checkIfStepCompleted(checkNum1)) { recipeTextbox1.GetComponent<Text>().color = Color.gray; }
        else { recipeTextbox1.GetComponent<Text>().color = Color.black; }

        if (checkIfStepCompleted(checkNum2)) { recipeTextbox2.GetComponent<Text>().color = Color.gray; }
        else { recipeTextbox2.GetComponent<Text>().color = Color.black; }
    }

    public void ClickOnBook(float value)
    {
        //if (!GameManager.cookBookActive) { return; };
        if (GameManager.isTouchingBook && GameManager.cookBookActive) //This is to detect if the player is touching the book
        {
            int val = (int)value;
            currentPage += val;
            if(currentPage > 2)
            {
                currentPage = 0;
            }
            else if(currentPage < 0)
            {
                currentPage = 2;
            }
            UpdateRecipeBookText(currentPage);

        }
    }

    public void SwitchRecipe(float currentRecipe)
    {
        if(GameManager.recipeIsOpenP1 && (currentRecipe == 1 || currentRecipe == -1))
        {
            GameManager.recipeIsOpenP1 = false;
            GameManager.recipeIsOpenP2 = true;
        }
        else if(GameManager.recipeIsOpenP2 && (currentRecipe == 1 || currentRecipe == -1))
        {
            GameManager.recipeIsOpenP2 = false;
            GameManager.recipeIsOpenP1 = true;
        }

        currentPage = 0;
        UpdateRecipeBookText(currentPage);
    }

    public void UpdateRecipeBookText(int currentStep)
    {
        if(GameManager.recipeIsOpenP1)
        {
            switch (currentStep)
            {
                case 0:
                    printRecipeBookText("Turn on Stove.", "Place Pan on Stove.", 1, 2);
                    break;

                case 1:
                    printRecipeBookText("Crack Egg in pan.", "Fold egg with spatula.", 3, 4);
                    break;

                case 2:
                    printRecipeBookText("Serve on plate.", "", 5, 6);
                    break;

            }
                
        }
        else if (GameManager.recipeIsOpenP2)
        {
            switch (currentStep)
            {
                case 0:
                    printRecipeBookText("Turn on Stove.", "Place Pan on Stove.", 1, 2);
                    break;

                case 1:
                    printRecipeBookText("Put bacon in pan.", "Use spatula to make sure bacon doesn't burn.", 3, 4);
                    break;

                case 2:
                    printRecipeBookText("Serve on plate.", "", 5, 6);
                    break;
            }
        }
    }

    public void setActiveFalseFunc()
    {
        recipeTextbox1.SetActive(false);
        recipeTextbox2.SetActive(false);
        backgroundImage.SetActive(false);
    }

    public void setActiveTrueFunc()
    {
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
