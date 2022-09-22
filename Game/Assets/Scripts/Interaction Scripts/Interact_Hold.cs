using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interact_Hold : Interact_Parent
{

    private string interact_Tag = "hold";

    //tracks progress on task
    private int task_progress;

    // Start is called before the first frame update
    void Start()
    {
        task_progress = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
