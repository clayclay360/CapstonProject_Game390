using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Main : MonoBehaviour
{
    public Text ratingText;

    public void Update()
    {
        DisplayRating();
    }

    public void DisplayRating()
    {
        ratingText.text = "Rating: " + GameManager.rating.ToString("F1");
    }

}
