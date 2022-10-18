using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineRend : MonoBehaviour
{
    LineRenderer al;
    private float aimLineLength = 2f;
    private Transform aimLinePoint;
    //private bool toggleAimPoint = true;

    // Start is called before the first frame update
    void Start()
    {
        //checks if the lineRenderer component is there, then finds the transform of the gameobject "Attackpoint"
        al = GetComponent<LineRenderer>();
        aimLinePoint = transform.Find("AttackPoint");
    }

    // Update is called once per frame
    void Update()
    {

        //toggleAimPoint = false;

        //Debug.Log("AimLine on");


        //To see if we hit any collider in the scene
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit))
        {
            if (hit.collider)
            {
                al.SetPosition(1, new Vector3(0, 0, hit.distance) * aimLineLength);

            }
            else
            {
                al.SetPosition(1, new Vector3(0, 0, 50));
            }

        }
        
    }

   

}
