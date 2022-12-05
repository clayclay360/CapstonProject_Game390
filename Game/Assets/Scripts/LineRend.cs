using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineRend : MonoBehaviour
{
    LineRenderer al;
    private float aimLineLength = 1f;
    private Transform aimLinePoint;
    //private bool toggleAimPoint = true;

    // Start is called before the first frame update
    void Start()
    {
        //checks if the lineRenderer component is there, then finds the transform of the gameobject "Attackpoint"
        al = GetComponent<LineRenderer>();
        aimLinePoint = transform.Find("AttackPoint");
    }

    void Update()
    {
        aimLineOn();
    }

    // Update is called once per frame
    public void aimLineOn()
    {

        //toggleAimPoint = false;

        //Debug.Log("AimLine on");


        //To see if we hit any collider in the scene
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit))
        {
            if (hit.collider.gameObject.GetComponent<RatScript>())
            {
                Debug.Log("Rat hit");
                RatScript enemy = hit.collider.gameObject.GetComponent<RatScript>();
                al.SetPosition(1, new Vector3(0, 0, hit.distance) * aimLineLength);

            }
            else
            {
                al.SetPosition(0, new Vector3(0, 0, 9));
            }

        }
        
    }

   

}
