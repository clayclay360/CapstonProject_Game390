using System.Collections;
using UnityEngine;

public class LineRend : MonoBehaviour
{
    
    private float aimLineLength = 5f;
    private Transform aimLinePoint;
    private bool toggleAimPoint = true;

    LineRenderer aimLine;


    // Start is called before the first frame update
    void Start()
    {
        aimLine = GetComponent<LineRenderer>();
        aimLinePoint = transform.Find("AttackPoint");
    }

    // Update is called once per frame
    void Update()
    {

        toggleAimPoint = false;

        //Debug.Log("AimLine on");
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit))
        {
            if (hit.collider)
            {
                aimLine.SetPosition(1, new Vector3(0, 0, hit.distance) * aimLineLength);

            }
            else
            {
                aimLine.SetPosition(1, new Vector3(0, 0, 5000));
            }

        }
        
    }

}
