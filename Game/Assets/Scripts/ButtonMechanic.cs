using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonMechanic : MonoBehaviour
{
 
    public float timeInBetween;

    private int tapCount;
    private int amountOfTaps { get; set; }
    private bool doublePressActive;
    private KeyCode storagedKey;

    // Update is called once per frame
    void Update()
    {
        //Testing//
        KeyPress(KeyCode.A);
        DoubleTap(KeyCode.S);
    }

    bool KeyPress(KeyCode key)
    {
        if (Input.GetKeyDown(key))
        {
            Debug.Log("Press");
            return true;
        }
        return false;
    }

    bool DoubleTap(KeyCode key)
    {
        tapCount = 1;

        if (Input.GetKeyDown(key))
        {
            if (storagedKey == key && doublePressActive && amountOfTaps == tapCount)
            {
                Debug.Log("DoublePress");
                amountOfTaps = 0;
                return true;
            }
            else
            {
                storagedKey = key;
                doublePressActive = true;
                amountOfTaps = 1;
                StartCoroutine(Timer());
            }
        }
        return false;
    }

    IEnumerator Timer()
    {
        yield return new WaitForSeconds(timeInBetween);
        doublePressActive = false;
        amountOfTaps = 0;
    }
}
