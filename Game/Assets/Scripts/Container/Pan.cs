using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Pan : Item
{
    public bool canCheck = true;

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
    public float overCookTime;
    public float cookOffset;
    public float progressMeterMin, progressMeterMax;
    public float[] interactionMeterStart, interactionMeterEnd;
    RecipeBook cookBook; //Added by Owen for changing the steps
    private GameObject passItems;


    [Header("Item Placement")]
    public Transform placement;

    private float progressMeter;
    private int interactionIndex = 0;
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
        status = Status.clean;

    }

    private void Awake()
    {
        base.Awake();
        progressSlider.GetComponent<Slider>();
        interactionAttemptReady = new bool[interactionMeterEnd.Length];
        cookBook = GameObject.Find("CookBook").GetComponentInChildren<RecipeBook>();
        attempt = new Attempt[interactionMeterEnd.Length];
        attempt[0] = Attempt.None;
        attempt[1] = Attempt.None;
        usesUntilDirty = 1;
        currUses = 0;
        passItems = GameObject.Find("PassItems");
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
                        state = State.cold;
                        Interaction = "";
                        gameObject.SetActive(false);
                        CheckCounter();
                        if (counterInUse != null)
                        {
                            CheckIndividualCounters(counterInUse);
                        }
                    }
                }
                else
                {
                    Interaction = "";
                }
                break;
            case PlayerController.ItemInMainHand.egg:

                if (status == Status.dirty)
                {
                    Interaction = "Pan is dirty!";
                    break;
                }

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
                            chef.hand[0].GetComponent<Collider>().enabled = false;
                            chef.hand[0].GetComponent<Egg>().state = Egg.State.yoke;
                            chef.hand[0].GetComponent<Egg>().toolItemIsOccupying = this;
                            chef.hand[0].GetComponent<Egg>().gameObject.transform.parent = transform;
                            chef.hand[0].GetComponent<Egg>().gameObject.transform.position = placement.position;
                            chef.hand[0].GetComponent<Egg>().gameObject.SetActive(true);
                            foodInPan = chef.hand[0];
                            chef.hand[0] = null;
                            chef.itemInMainHand = PlayerController.ItemInMainHand.empty;
                            Occupied = true;
                            prone = true;
                            chef.isInteracting = false;
                        }
                        break;
                }
                break;
            case PlayerController.ItemInMainHand.bacon:
                if (status == Status.dirty)
                {
                    Interaction = "Pan is dirty!";
                    break;
                }

                Interaction = "Place Bacon";
                if (chef.isInteracting)
                {
                    chef.hand[0].GetComponent<Collider>().enabled = false;
                    chef.hand[0].GetComponent<Bacon>().toolItemIsOccupying = this;
                    chef.hand[0].GetComponent<Bacon>().gameObject.transform.parent = transform;
                    chef.hand[0].GetComponent<Bacon>().gameObject.transform.position = placement.position;
                    chef.hand[0].GetComponent<Bacon>().gameObject.SetActive(true);
                    foodInPan = chef.hand[0];
                    chef.hand[0] = null;
                    chef.itemInMainHand = PlayerController.ItemInMainHand.empty;
                    Occupied = true;
                    prone = true;
                    chef.isInteracting = false;
                }
                break;
            case PlayerController.ItemInMainHand.spatula:
                if (Occupied && state == State.hot && cooking)
                {
                    if (chef.hand[0].status == Status.dirty)
                    {
                        Interaction = "Spatula is Dirty!";
                        return;
                    }
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
                                interactionIndex++;
                            }
                        }
                        
                        //Attempts
                        if(progressMeter > progressMeterMax / 4)
                        {
                            switch (foodInPan.Name)
                            {
                                case "Egg":
                                    foodInPan.GetComponent<Egg>().state = Egg.State.omelet;
                                    break;
                                case "Bacon":
                                    foodInPan.GetComponent<Bacon>().status = Bacon.Status.cooked;
                                    break;
                            }
                        }
                        else
                        {
                            switch (foodInPan.Name)
                            {
                                case "Burnt Bacon":
                                    foodInPan.GetComponent<Bacon>().status = Bacon.Status.burnt;
                                    break;
                            }
                        }
                        if (interactionIndex < attempt.Length)
                        {
                            if (interactionAttemptReady[interactionIndex])
                            {
                                if (progressMeter > interactionMeterStart[interactionIndex] && progressMeter < interactionMeterEnd[interactionIndex])
                                {
                                    completeMark[interactionIndex].sprite = checkMark;
                                    completeMark[interactionIndex].gameObject.SetActive(true);
                                    attempt[interactionIndex] = Attempt.Completed;
                                    
                                }
                                else if (progressMeter < interactionMeterStart[interactionIndex])
                                {
                                    completeMark[interactionIndex].sprite = xMark;
                                    completeMark[interactionIndex].gameObject.SetActive(true);
                                    attempt[interactionIndex] = Attempt.Failed;
                                }
                                else if (progressMeter > interactionMeterEnd[interactionIndex])
                                {
                                    completeMark[interactionIndex].sprite = xMark;
                                    completeMark[interactionIndex].gameObject.SetActive(true);
                                    attempt[interactionIndex] = Attempt.Failed;
                                }
                                interactionAttemptReady[interactionIndex] = false;
                            }
                        }
                        chef.hand[0].CheckIfDirty();
                        chef.isInteracting = false;
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
                        CheckCounter();
                        if (counterInUse != null)
                        {
                            CheckIndividualCounters(counterInUse);
                        }
                    }
                }
                break;
        }
    }


    private IEnumerator CheckStatus()
    {
        yield return new WaitForSeconds(5f);
        canCheck = true;
    }

    private void Update()
    {
        StartCooking();

        if (cooking)
        {
            prone = true;
        }
        else
        {
            prone = false;
        }

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
            foreach(Image img in completeMark)
            {
                img.gameObject.SetActive(false);
            }
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
        float deltaTime = Time.unscaledTime;

        while (progressMeter < progressMeterMax)
        {
            progressMeter = (Time.unscaledTime - deltaTime) / time;
            progressSlider.value = progressMeter;

            for (int i = 0; i < interactionMeterEnd.Length; i++)
            {
                //Could also add a or to check if the attempt is complet or uncompleted
                if (interactionMeterEnd[i] < progressMeter)
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

            yield return null;
               
        }
        progressSlider.gameObject.SetActive(false);
        cooking = false;
        foodInPan.status = Status.cooked;

        //CHeck if the player failed all attempts if so, food is burnt
        for(int i = 0; i < attempt.Length; i++)
        {
            if (attempt[i] == Attempt.Completed)
            {
                foodInPan.status = Status.cooked;
                break;
            }

            foodInPan.status = Status.burnt;
        }

        StartCoroutine(OverCooked(overCookTime));

        if (Attempt.Failed == attempt[0])
        {
            Debug.Log("Burnt");
            progressSlider.gameObject.SetActive(false);
            cooking = false;
            foodInPan.status = Status.burnt;
            CheckIfDirty();
        }

    }

    public IEnumerator OverCooked(float timer)
    {
        Debug.Log("Starting OverCooked");
        yield return new WaitForSeconds(2);

        float deltaTime = Time.unscaledTime;
        float time = 0;

        while (state == State.hot)
        {
            time = (Time.unscaledTime - deltaTime);
            if(timer < time)
            {
                Debug.Log("Food Burnt");
                foodInPan.GetComponent<Item>().status = Status.burnt;
                state = State.cold;
            }
            yield return null;
        }
    }

    public void PassPan(int passLocation)
    {
        if (passLocation == 0)
        {
            transform.position = passItems.transform.position + new Vector3(0, 0, 0.5f);
            gameObject.SetActive(true);
        }
        else if (passLocation == 1)
        {
            transform.position = passItems.transform.position;
            gameObject.SetActive(true);
        }
        else if (passLocation == 2)
        {
            transform.position = passItems.transform.position + new Vector3(0, 0, -0.5f);
            gameObject.SetActive(true);
        }
    }

    public void DropPanOnGround(GameObject player)
    {
        transform.position = player.transform.position;
        gameObject.SetActive(true);
    }

}
