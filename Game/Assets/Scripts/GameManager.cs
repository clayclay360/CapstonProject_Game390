using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager
{
    public static bool gameStarted;
    public static bool assigningOrders;
    public static int currentLevel;
    public static float rating;

    public static bool isTouchingTrashCan;
    public static bool passItemsReady;
    public static string[] counterItems = { "", "", ""};
    public static bool putOnCounter;
    public static Bacon bacon;
    public static Egg egg;

    public static bool recipeIsOpenP1;
    public static bool isTouchingBook;
    public static List<int> isStepCompleted = new List<int>();

    //We can't use dictionaries in the GameManager :( Maybe use a Main in the level?
    public List<string> recipeReq = new List<string>();
    public string[] recipeReqs;
    public Item foodReq;
    public bool reqsClear;

    public static int numberOfPlayers = 0;
    public static bool cookBookActive = true;

    public int playerScore;
    public int scoreMultiplier;

    public static PlayerController playerOne;
    public static PlayerController playerTwo;
}
