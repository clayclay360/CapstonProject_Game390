using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CookBook : MonoBehaviour
{
    [Header("Variables")]
    public int lives;

    // Start is called before the first frame update
    void Start()
    {
        GameManager.cookBookActive = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(lives == 0)
        {
            GameManager.cookBookActive = false;
            Destroy(gameObject);
        }
    }
}
