using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RecipeBook : MonoBehaviour
{
    public string theName;
    public GameObject recipeTextbox1;
    public GameObject recipeTextbox2;
    public GameObject recipeTextbox3;
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
        backgroundImage.SetActive(false);
        pages = new int[] { 1, 2, 3 };
        steps = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
        currentPage = 1;
        currentStep = 2;
        GameManager.isStepCompleted = new int[] { 0, 1, 7, 4, 8 };

        GameManager.isTouchingBook = false;

        //currentStepTaskMang = taskManager.GetComponent<TaskManager>();
    }

    void Update()
    {
        if (!GameManager.isTouchingBook)
        {
            //variable telling the game that the recipe for player one is closed
            GameManager.recipeIsOpenP1 = false;
            //empties out any information in the textboxes
            recipeTextbox1.GetComponent<Text>().text = " ";
            recipeTextbox2.GetComponent<Text>().text = " ";
            recipeTextbox3.GetComponent<Text>().text = " ";
            //hides the background image
            backgroundImage.SetActive(false);
        }

        /*if (GameManager.isTouchingBook)
        {
            //variable telling the game that the recipe for player one is open
            recipeIsOpenP1 = true;
            Debug.LogError(recipeIsOpenP1);

            //currentStepTaskMang = taskManager.GetComponent<TaskManager>();
            //sets the background image to show
            backgroundImage.SetActive(true);

            currentPage = 1;

            printRecipeBookText("Turn on Stove to medium.", "Place Pan on Stove.", "Beat the eggs (Bowl on countertop).", 1, 2, 3);
        }
        else if (!GameManager.isTouchingBook)
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

        if (recipeIsOpenP1)
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
            } else
            {
                if (currentPage == 2)
                {
                    printRecipeBookText("Add cheese to pan.", "Fold eggs with spatula", " ", 7, 8, 9);
                }
                currentPage = 0;
            }
        }*/
    }



    public bool checkIfStepCompleted(int step)
    {

        for (int i = 0; i < GameManager.isStepCompleted.Length; i++)
        {
            if (step == GameManager.isStepCompleted[i])
            {
                return (true);
            }
            else
            {
                isTrue = false;
            }
        }
        return (isTrue);
    }

    public void printRecipeBookText(string recipeText1, string recipeText2, string recipeText3, int checkNum1, int checkNum2, int checkNum3)
    {
        //puts in the information for the first page when the book first opens
        recipeTextbox1.GetComponent<Text>().text = recipeText1;
        recipeTextbox2.GetComponent<Text>().text = recipeText2;
        recipeTextbox3.GetComponent<Text>().text = recipeText3;
        //is checking the current step to determine if it should gray out the first 3 steps on open or not
        if (checkIfStepCompleted(checkNum1)) { recipeTextbox1.GetComponent<Text>().color = Color.gray; }
        else { recipeTextbox1.GetComponent<Text>().color = Color.black; }

        if (checkIfStepCompleted(checkNum2)) { recipeTextbox2.GetComponent<Text>().color = Color.gray; }
        else { recipeTextbox2.GetComponent<Text>().color = Color.black; }

        if (checkIfStepCompleted(checkNum3)) { recipeTextbox3.GetComponent<Text>().color = Color.gray; }
        else { recipeTextbox3.GetComponent<Text>().color = Color.black; }
    }

    public void ClickOnBook()
    {
        if (GameManager.isTouchingBook)
        {
            //variable telling the game that the recipe for player one is open
            GameManager.recipeIsOpenP1 = true;
            Debug.LogError(GameManager.recipeIsOpenP1);

            //currentStepTaskMang = taskManager.GetComponent<TaskManager>();
            //sets the background image to show
            backgroundImage.SetActive(true);

            currentPage = 1;

            printRecipeBookText("Turn on Stove to medium.", "Place Pan on Stove.", "Beat the eggs (Bowl on countertop).", 1, 2, 3);
        }
    }

    public void ClickThroughBook()
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
    }
}
