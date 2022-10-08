using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineRend : MonoBehaviour
{
    //private Controls m_Controls;
    [SerializeField] private float aimLineLength = 5f;
    public Transform aimLinePoint;

    LineRenderer aimLine;


    // Start is called before the first frame update
    void Start()
    {
        aimLine = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {


        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit))
        {
            if (hit.collider)
            {
                aimLine.SetPosition(1, new Vector3(0, 0, hit.distance));

            }
            else
            {
                aimLine.SetPosition(1, new Vector3(0, 0, 5000));
            }

        }
        Debug.Log("AimLine on");
    }

}
