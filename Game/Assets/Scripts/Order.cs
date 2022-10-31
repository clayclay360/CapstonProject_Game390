using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.UI;
public class Order : MonoBehaviour
{
    public bool Complete;

    public Text orderNameText;
    public Slider sliderTimer;

    public void AssignOrder(string name, int time)
    {
        orderNameText.text = name;
        sliderTimer.maxValue = time;
        StartCoroutine(Timer());
    }

    IEnumerator Timer()
    {
        float maxTime = sliderTimer.maxValue;
        float currentTime = Time.unscaledTime;
        float orderTime = 0;
        sliderTimer.value = maxTime;

        while(maxTime - orderTime > 0)
        {
            orderTime = Time.unscaledTime - currentTime;
            yield return null;
            sliderTimer.value = maxTime - orderTime;
        }

        OrderManager.currentOrders--;
        Destroy(gameObject);
    }
}
