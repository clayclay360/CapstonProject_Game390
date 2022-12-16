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
    private float hbarTimer = 0f;
    private const float HBARTIME = 3f;
    public bool hbarVisible = false;

    //Update healthbar position every frame
    private void Update()
    {
        if (!rat) { return; }
        transform.position = rat.transform.position + hbarOffset;

        //Healthbar timer—disappears if it has been visible for more than 3 seconds
        if (hbarVisible)
        {
            hbarTimer += Time.deltaTime;
            if (hbarTimer >= HBARTIME)
            {
                gameObject.SetActive(false);
                hbarTimer = 0f;
                hbarVisible = false;
            }
        }
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
