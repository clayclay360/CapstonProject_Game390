using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public GameObject target;
    private Vector3 offset = new Vector3(0, 5, -1);
    private Vector3 rotation = new Vector3(70, 0, 0);

    void Awake()
    {
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.position = offset;
        transform.eulerAngles = rotation;
    }
}
