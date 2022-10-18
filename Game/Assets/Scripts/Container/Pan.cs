using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Pan : Item
{
    [Header("UI")]
    public Slider progressSlider;
    public Image[] completeMark;
    public Sprite checkMark;
    public Sprite xMark;

    public enum State { cold, hot }

    [Header("Variables")]
    public bool cooking;
    public State state;
    public float cookTime;
    public float cookOffset;
    public float progressMeterMin, progressMeterMax;
    public float[] interactionMeterStart, interactionMeterEnd;
    RecipeBook cookBook; //Added by Owen for changing the steps

    private float progressMeter;
    int interactionIndex = 0;
    private bool[] interactionAttemptReady;
    [HideInInspector]
    public Item foodInPan;
    private enum Attempt { None, Failed, Completed };
    private Attempt[] attempt;

    public Pan()
    {
        Name = "Pan";
        Type = "Tool";
        Interaction = "";
        state = State.cold;
    }

    private void Awake()
    {
        progressSlider.GetComponent<Slider>();
        interactionAttemptReady = new bool[interactionMeterEnd.Length];
        cookBook = GameObject.Find("DetectCollision").GetComponent<RecipeBook>();
        attempt = new Attempt[interactionMeterEnd.Length];
        attempt[0] = Attempt.None;
        attempt[0] = Attempt.None;
    }

    public override void CheckHand(PlayerController.ItemInMainHand item, PlayerController chef)
    {
        //figure out inventory full

        switch (item)
        {
            case PlayerController.ItemInMainHand.empty:
                if(!cooking){
                    Interaction = "Grab Pan";
                    if (chef.isInteracting)
                    {
                        if (utilityItemIsOccupying != null)
                        {
                            utilityItemIsOccupying.Occupied = false;
                            utilityItemIsOccupying = null;
                        }
                        Interaction = "";
                        gameObject.SetActive(false);
                    }
                }
                break;
            case PlayerController.ItemInMainHand.egg:

                switch (chef.hand[0].GetComponent<Egg>().state)
                {
                    case Egg.State.shell:
                        Interaction = "Crack Egg";

                        if (chef.isInteracting)
                        {
                            if (!GameManager.isStepCompleted.Contains(5))
                            {
                                GameManager.isStepCompleted.Add(5);
                                cookBook.printRecipeBookText("Add eggs to pan.", "Lift and tilt eggs with spatula.", 5, 6);
                            }
                            chef.hand[0].GetComponent<Egg>().state = Egg.State.yoke;
                            chef.hand[0].GetComponent<Egg>().toolItemIsOccupying = this;
                            chef.hand[0].GetComponent<Egg>().gameObject.transform.parent = transform;
                            chef.hand[0].GetComponent<Egg>().gameObject.transform.localPosition = new Vector3(0, .15f, 0);
                            chef.hand[0].GetComponent<Egg>().gameObject.SetActive(true);
                            foodInPan = chef.hand[0];
                            chef.hand[0] = null;
                            chef.itemInMainHand = PlayerController.ItemInMainHand.empty;
                            Occupied = true;
                            Prone = true;
                            chef.isInteracting = false;
                        }
                        break;
                }
                break;
            case PlayerController.ItemInMainHand.spatula:
                if (Occupied && state == State.hot && cooking)
                {
                    Interaction = "Use Spatula";
                    if (chef.isInteracting)
                    {
                        if (!GameManager.isStepCompleted.Contains(6))
                        {
                            GameManager.isStepCompleted.Add(6);
                            cookBook.printRecipeBookText("Add eggs to pan.", "Lift and tilt eggs with spatula.", 5, 6);
                        }
                        chef.isInteracting = false;
                        interactionIndex = 0;

                        for(int i = 0; i < interactionMeterEnd.Length; i++)
                        {
                            if(interactionMeterEnd[i] < progressMeter)
                            {
                                Debug.Log("Attempts: " + attempt[interactionIndex]);
                                interactionIndex++;
                            }
                        }
                        
                        //Attempts
                        Debug.Log("Index: " + interactionIndex);

                        if(progressMeter > progressMeterMax / 4)
                        {
                            foodInPan.GetComponent<Egg>().state = Egg.State.omelet;
                        }
                        if (interactionIndex < attempt.Length)
                        {
                            if (interactionAttemptReady[interactionIndex])
                            {
                                if (progressMeter > interactionMeterStart[interactionIndex] && progressMeter < interactionMeterEnd[interactionIndex])
                                {
                                    Debug.Log("Great Job!");
                                    completeMark[interactionIndex].sprite = checkMark;
                                    completeMark[interactionIndex].gameObject.SetActive(true);
                                    attempt[interactionIndex] = Attempt.Completed;
                                }
                                else if (progressMeter < interactionMeterStart[interactionIndex])
                                {
                                    completeMark[interactionIndex].sprite = xMark;
                                    completeMark[interactionIndex].gameObject.SetActive(true);
                                    attempt[interactionIndex] = Attempt.Failed;
                                    Debug.Log("Too Early");
                                }
                                else if (progressMeter > interactionMeterEnd[interactionIndex])
                                {
                                    completeMark[interactionIndex].sprite = xMark;
                                    completeMark[interactionIndex].gameObject.SetActive(true);
                                    attempt[interactionIndex] = Attempt.Failed;
                                    Debug.Log("Too Late");
                                }
                                interactionAttemptReady[interactionIndex] = false;
                            }
                        }
                    }
                    else
                    {
                        for (int i = 0; i < interactionMeterEnd.Length; i++)
                        {
                            //Could also add a or to check if the attempt is complet or uncompleted
                            if(interactionMeterEnd[i] < progressMeter) 
                            {
                                completeMark[i].gameObject.SetActive(true);
                                switch (attempt[i])
                                {
                                    case Attempt.None:
                                        completeMark[i].sprite = xMark;
                                        break;
                                    case Attempt.Failed:
                                        completeMark[i].sprite = xMark;
                                        break;
                                    case Attempt.Completed:
                                        completeMark[i].sprite = checkMark;
                                        break;
                                    default:
                                        Debug.Log(attempt);
                                        break;
                                }
                            }
                        }
                    }
                }
                else
                {
                    if(chef.inventoryFull)
                    {
                        Interaction = "Hands Full";
                        return;
                    }

                    Interaction = "Grab Pan";

                    if (chef.isInteracting)
                    {
                        if (utilityItemIsOccupying != null)
                        {
                            utilityItemIsOccupying.Occupied = false;
                            utilityItemIsOccupying = null;
                        }
                        Interaction = "";
                        gameObject.SetActive(false);
                    }
                }
                break;
        }
    }

    private void Update()
    {
        StartCooking();
    }

    public void ResetAttempts()
    {
        for(int i = 0; i < interactionAttemptReady.Length; i++)
        {
            interactionAttemptReady[i] = true;
        }
    }
    public void StartCooking()
    {
        if(Occupied && !cooking && state == State.hot && foodInPan.status == Status.uncooked)
        {
            progressSlider.gameObject.SetActive(true);
            progressMeter = progressMeterMin;
            progressSlider.maxValue = progressMeterMax;
            progressSlider.minValue = progressMeterMin;  
            progressSlider.value = progressMeter;
            cooking = true;
            ResetAttempts();
            StartCoroutine(Cooking(cookTime, cookOffset));
        }
    }

    IEnumerator Cooking(float time, float offset)
    {
        while(progressMeter + offset < progressMeterMax)
        {
            progressMeter = Mathf.Lerp(progressMeter, progressMeterMax, time);
            progressSlider.value = progressMeter;
            yield return null;
        }
        progressSlider.gameObject.SetActive(false);
        cooking = false;
        foodInPan.status = Status.cooked;
    }
}
