using System.Collections;
using UnityEngine;
using UnityEngine.ProBuilder;

public class LineRend : MonoBehaviour
{
    private float aimLineLength = 1f;
    private Transform aimLinePoint;
    LineRenderer aimLine;
    //public Vector3 forward;
    private bool ToggleAimLine = true;


    // Start is called before the first frame update
    void Start()
    {
        aimLine = GetComponent<LineRenderer>();
        //aimLinePoint = transform.Find("Attackpoint");
    }

    // Update is called once per frame
    void Update()
    {
        ToggleAimLine = false;


        Debug.Log("AimLine on");
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit))
        {
            if (hit.collider)
            {
                aimLine.SetPosition(1, new Vector3(0, 0, hit.distance) * aimLineLength);

            }
            else
            {
                aimLine.SetPosition(1, new Vector3(0, 0, 5));
            }

        }
        
    }
    
}
