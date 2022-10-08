using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Pan : Item
{
    public Slider progressSlider;
    public enum State { cold, hot }
    public bool cooking;
    public State state;

    public float cookTime;
    public float progressMeterMin, progressMeterMax;

    private float progressMeter;
    private float interactionMeterStart, interactionMeterEnd;
    private float finishMeterStart, finishMeterEnd;
    public Pan()
    {
        Name = "Pan";
        Type = "Tool";
        Interaction = "";
        state = State.cold;
    }

    private void Start()
    {
        progressSlider.GetComponent<Slider>();
    }

    public override void CheckHand(PlayerController.ItemInMainHand item, PlayerController chef)
    {
        //figure out inventory full

        switch (item)
        {
            case PlayerController.ItemInMainHand.empty:
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
                break;
            case PlayerController.ItemInMainHand.egg:

                switch (chef.hand[0].GetComponent<Egg>().state)
                {
                    case Egg.State.shell:
                        Interaction = "Crack Egg";

                        if (chef.isInteracting)
                        {
                            chef.hand[0].GetComponent<Egg>().state = Egg.State.yoke;
                            chef.hand[0].GetComponent<Egg>().toolItemIsOccupying = this;
                            chef.hand[0].GetComponent<Egg>().gameObject.transform.parent = transform;
                            chef.hand[0].GetComponent<Egg>().gameObject.transform.localPosition = new Vector3(0, .15f, 0);
                            chef.hand[0].GetComponent<Egg>().gameObject.SetActive(true);
                            chef.hand[0] = null;
                            chef.itemInMainHand = PlayerController.ItemInMainHand.empty;
                            Occupied = true;
                            Prone = true;
                        }
                        break;
                }
                break;
            case PlayerController.ItemInMainHand.spatula:
                if (Occupied)
                {
                    Interaction = "Use Spatula";
                    if (chef.isInteracting)
                    {

                    }
                }
                else
                {
                    if (chef.inventoryFull)
                    {
                        Interaction = "Inventory Full";
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

    public void StartCooking()
    {
        if(Occupied && !cooking && state == State.hot)
        {
            Debug.Log("Cooking TIme");
            progressSlider.gameObject.SetActive(true);
            progressMeter = progressMeterMin;
            progressSlider.maxValue = progressMeterMax;
            progressSlider.minValue = progressMeterMin;  
            progressSlider.value = progressMeter;
            cooking = true;
            StartCoroutine(Cooking(cookTime));
        }
    }

    IEnumerator Cooking(float time)
    {
        while(progressMeter < progressMeterMax)
        {
            float reference = 0;
            progressMeter = Mathf.SmoothDamp(progressMeter, progressMeterMax, ref reference, time);
            progressSlider.value = progressMeter;
            yield return null;
        }
        progressSlider.gameObject.SetActive(false);
    }
}
