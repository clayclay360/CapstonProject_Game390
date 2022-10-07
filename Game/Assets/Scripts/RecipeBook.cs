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
    //public GameObject taskManager;
    //public TaskManager currentStepTaskMang;
    public bool recipeIsOpenP1;
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

        GameManager.isTouchingBook = true;

        //currentStepTaskMang = taskManager.GetComponent<TaskManager>();
    }

    void Update()
    {
       

        if (Input.GetKeyDown("e") && GameManager.isTouchingBook)
        {
            //variable telling the game that the recipe for player one is open
            recipeIsOpenP1 = true;
            Debug.LogError(recipeIsOpenP1);

            //currentStepTaskMang = taskManager.GetComponent<TaskManager>();
            //sets the background image to show
            backgroundImage.SetActive(true);

            currentPage = 1;

            //puts in the information for the first page when the book first opens
            recipeTextbox1.GetComponent<Text>().text = "Turn on Stove to medium.";
            recipeTextbox2.GetComponent<Text>().text = "Place Pan on Stove.";
            recipeTextbox3.GetComponent<Text>().text = "Beat the eggs (Bowl on countertop).";
            //is checking the current step to determine if it should gray out the first 3 steps on open or not
            if (checkIfStepCompleted(1)) { recipeTextbox1.GetComponent<Text>().color = Color.gray; }
            else { recipeTextbox1.GetComponent<Text>().color = Color.black; }

            if (checkIfStepCompleted(2)) { recipeTextbox2.GetComponent<Text>().color = Color.gray; }
            else { recipeTextbox2.GetComponent<Text>().color = Color.black; }

            if (checkIfStepCompleted(3)) { recipeTextbox3.GetComponent<Text>().color = Color.gray; }
            else { recipeTextbox3.GetComponent<Text>().color = Color.black; }
            
        }
        else if (Input.GetKeyUp("e") || !GameManager.isTouchingBook)
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
            //currentStepTaskMang = taskManager.GetComponent<TaskManager>();

            if (currentPage != pages.Length - 1)
            {
                if (currentPage == 0)
                {
                    //this is putting in the information for the first page while player 1 is clicking through the pages
                    recipeTextbox1.GetComponent<Text>().text = "Turn on Stove to medium.";
                    recipeTextbox2.GetComponent<Text>().text = "Place pan on stove.";
                    recipeTextbox3.GetComponent<Text>().text = "Beat the eggs (Bowl on countertop).";
                    //is checking the current step to determine if it should gray out the first 3 steps while player 1 is clicking through the pages
                    if (checkIfStepCompleted(1)) { recipeTextbox1.GetComponent<Text>().color = Color.gray; }
                    else { recipeTextbox1.GetComponent<Text>().color = Color.black; }

                    if (checkIfStepCompleted(2)) { recipeTextbox2.GetComponent<Text>().color = Color.gray; }
                    else { recipeTextbox2.GetComponent<Text>().color = Color.black; }

                    if (checkIfStepCompleted(3)) { recipeTextbox3.GetComponent<Text>().color = Color.gray; }
                    else { recipeTextbox3.GetComponent<Text>().color = Color.black; }


                }
                else if (currentPage == 1)
                {
                    //this is putting in the information for the second page while player 1 is clicking through the pages
                    recipeTextbox1.GetComponent<Text>().text = "Melt Butter in the pan.";
                    recipeTextbox2.GetComponent<Text>().text = "Add eggs to pan.";
                    recipeTextbox3.GetComponent<Text>().text = "Lift and tilt eggs with spatula.";
                    //is checking the current step to determine if it should gray out the next 3 while player 1 is clicking through the pages
                    if (checkIfStepCompleted(4)) { recipeTextbox1.GetComponent<Text>().color = Color.gray; }
                    else { recipeTextbox1.GetComponent<Text>().color = Color.black; }

                    if (checkIfStepCompleted(5)) { recipeTextbox2.GetComponent<Text>().color = Color.gray; }
                    else { recipeTextbox2.GetComponent<Text>().color = Color.black; }

                    if (checkIfStepCompleted(6)) { recipeTextbox3.GetComponent<Text>().color = Color.gray; }
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
                    if (checkIfStepCompleted(7)) { recipeTextbox1.GetComponent<Text>().color = Color.gray; }
                    else { recipeTextbox1.GetComponent<Text>().color = Color.black; }

                    if (checkIfStepCompleted(8)) { recipeTextbox2.GetComponent<Text>().color = Color.gray; }
                    else { recipeTextbox2.GetComponent<Text>().color = Color.black; }

                    if (checkIfStepCompleted(9)) { recipeTextbox3.GetComponent<Text>().color = Color.gray; }
                    else { recipeTextbox3.GetComponent<Text>().color = Color.black; }

                }
                currentPage = 0;
            }
        }
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
}
