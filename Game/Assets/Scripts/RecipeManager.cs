using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecipeManager : MonoBehaviour
{
    public Recipe[] recipe;

    private void Update()
    {
        if (GameManager.assigningOrders)
        {
            AssignOrder(GameManager.currentLevel);
            GameManager.assigningOrders = false;
        }
    }

    private void AssignOrder(int level)
    {
        switch (GameManager.currentLevel)
        {
            case 0:
                
                for (int i = 0; i < recipe[0].Steps.Length; i++)
                {
                    GameManager.playerOne.taskManager.CreateTask();
                    GameManager.playerOne.taskManager.tasks[i].taskName = recipe[GameManager.currentLevel].Steps[i];
                }

                Debug.Log(GameManager.numberOfPlayers);

                for (int i = 0; i < recipe[1].Steps.Length; i++)
                {
                    GameManager.playerTwo.taskManager.CreateTask();
                    GameManager.playerTwo.taskManager.tasks[i].taskName = recipe[GameManager.currentLevel + 1].Steps[i];
                }
                break;
        }
        GameManager.assigningOrders = false;
    }
}
