using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RatHealthBar : MonoBehaviour
{
    public Slider slider;
    public Text itemText;
    public GameObject rat;
    private Vector3 hbarOffset = new Vector3(0f, 0f, 0.5f);

    //Update healthbar position every frame
    private void Update()
    {
        if (!rat) { return; }
        transform.position = rat.transform.position + hbarOffset;
    }

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
