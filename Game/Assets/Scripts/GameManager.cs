using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                //Debug.LogError("GameManager is null!");
                Debug.LogWarning("Creating GameManager");

                _instance = new GameObject().AddComponent<GameManager>();

                _instance.name = _instance.GetType().ToString();

                DontDestroyOnLoad(_instance.gameObject);
            }
            return _instance;
        }
    }

    //We can't use dictionaries in the GameManager :( Maybe use a Main in the level?
    public List<string> recipeReq = new List<string>();
    public string[] recipeReqs;
    public Item foodReq;
    public bool reqsClear;

    public static int numberOfPlayers = 0;
    public static bool cookBookActive = true;

    public int playerScore;
    public int scoreMultiplier;

    private void Awake()
    {
        _instance = this;
    }
    private void Start()
    {
        reqsClear = false;
        playerScore = 0;
        scoreMultiplier = 1;

        recipeReq.Add("omelet");
        Debug.LogWarning("GameManager Ready!");
    }

    private void Update() 
    {
        if (reqsClear)
        {
            Debug.LogWarning("Omelet Complete!");
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
        foreach(string req in recipeReq)
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
