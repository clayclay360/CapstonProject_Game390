using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Main : MonoBehaviour
{
    [Header("Variables")]
    public RecipeManager[] recipeManager;
    public int currentMainRecipe;
    
    public string[] orderNames;
    public bool startingOrders;
    public float maxTimeInBetweenOrders, minTimeInBetweenOrders;
    public static int currentNumberOfSides;

    [Header("Orders")]
    public GameObject platePrefab;
    public Transform[] spawnPosition;
    public static Dictionary<int, Plate> Order = new Dictionary<int, Plate>();
    public int orderNumber;

    private float timeInBetweenOrders;
    private int maxOrdersOfSides;

    public void StartGame()
    {
        GameManager.gameStarted = true;
    }

    //public IEnumerator GameLoop()
    //{
    //    while (GameManager.gameStarted)
    //    {
    //        if (GameManager.currentLevel == 0)
    //        {
    //            //recipeManager[GameManager.currentLevel].mainRecipes[currentMainRecipe];
    //        }
    //    }
    //}

    IEnumerator SideOrders()
    {
        while (GameManager.gameStarted)
        {
            yield return new WaitForSeconds(2);

            if (GameManager.currentLevel == 0 && maxOrdersOfSides > currentNumberOfSides)
            {
                timeInBetweenOrders = Random.Range(minTimeInBetweenOrders, maxTimeInBetweenOrders);

                int orderIndex = Random.Range(0, 2);

                currentNumberOfSides++;
                yield return new WaitForSeconds(timeInBetweenOrders);
            }
        }
    }
}
