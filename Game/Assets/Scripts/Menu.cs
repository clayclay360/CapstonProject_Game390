using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    public GameObject menuText1;
    public GameObject menuText2;
    public GameObject menuText3;

    void Start()
    {
        menuText1.GetComponent<Text>().text = " ";
        menuText2.GetComponent<Text>().text = " ";
        menuText3.GetComponent<Text>().text = " ";
    }

    // Start is called before the first frame update
    public void PlaceOrder(string orderName)
    {

        if (menuText1.GetComponent<Text>().text == " " && orderName != "")
        {
            menuText1.GetComponent<Text>().text = orderName;
        } else if (menuText2.GetComponent<Text>().text == " " && orderName != "")
        {
            menuText2.GetComponent<Text>().text = orderName;
        } else if (menuText3.GetComponent<Text>().text == " " && orderName != "")
        {
            menuText3.GetComponent<Text>().text = orderName;
        }
    }

    public void RemoveOrder(string orderName)
    {
        if (menuText1.GetComponent<Text>().text == orderName)
        {
            menuText1.GetComponent<Text>().text = menuText2.GetComponent<Text>().text;
            menuText2.GetComponent<Text>().text = menuText3.GetComponent<Text>().text;
            menuText3.GetComponent<Text>().text = " ";
        } else if (menuText2.GetComponent<Text>().text == orderName)
        {
            menuText2.GetComponent<Text>().text = menuText3.GetComponent<Text>().text;
            menuText3.GetComponent<Text>().text = " ";
        } else
        {
            menuText3.GetComponent<Text>().text = " ";
        }
    }
}
