using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager
{
    //We can't use dictionaries in the GameManager :( Maybe use a Main in the level?
    public string[] recipeReqs;
    public Item foodReq;
    public bool reqsClear;

    public static int numberOfPlayers = 0;
    public static bool cookBookActive = true;

    public int playerScore;
    public int scoreMultiplier;

    public void Start()
    {
        reqsClear = false;
        playerScore = 0;
        scoreMultiplier = 1;

        recipeReqs = new string[0];
        recipeReqs[0] = "omelet";
    }

    public void Update() 
    {
        if (reqsClear)
        {
            Debug.Log("Omelet Complete!");
            Time.timeScale = 0;
        }
    }

    public void AddScore(int points, bool increaseMultiplier = false)
    {
        if (increaseMultiplier) { scoreMultiplier += 1; }
        playerScore += scoreMultiplier * points;
    }

    public void CheckLevelCompletion(Item food)
    {
        foreach(string req in recipeReqs)
        {
            switch (req)
            {
                case "omelet":
                    food.TryGetComponent(out Egg egg);
                    reqsClear = omeletStatus(egg);
                    break;
            }

        }
    }

    public bool omeletStatus(Egg egg)
    {
        if (egg.state == Egg.State.omelet) 
        {
            AddScore(50, true);
            return true;
        }
        return false;
    }
}
