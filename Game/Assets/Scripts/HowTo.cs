using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HowTo : MonoBehaviour
{
    public Page[] pages;
    public Image img;
    public Text txt;

    [HideInInspector]
    public int pageNumber;

    // Start is called before the first frame update
    void Start()
    {
        img.GetComponent<Image>();
        txt.GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        //loop
        if(pageNumber <= -1)
        {
            pageNumber = pages.Length - 1;
        }
        if(pageNumber >= pages.Length)
        {
            pageNumber = 0;
        }

        Pages();
    }
    
    //get the page image and description
    public void Pages()
    {
        img.sprite = pages[pageNumber].image;
        txt.text = pages[pageNumber].descrtiption;
    }
}
