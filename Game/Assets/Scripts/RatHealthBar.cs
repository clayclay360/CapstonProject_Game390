using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RatHealthBar : MonoBehaviour
{
    public Slider slider;
    public Text itemText;

    public void SetMaxHealth(int health)
    {
        slider.maxValue = health;
        slider.value = health;
    }

    public void SetHealth(int health)
    {
        slider.value = health;
    }

    public void SetItemText(string item)
    {
        itemText.text = item;
    }
}
