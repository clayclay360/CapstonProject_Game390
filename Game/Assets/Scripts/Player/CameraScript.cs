using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public GameObject target;
    private Vector3 offset = new Vector3(0, 10, -3);
    private Vector3 rotation = new Vector3(70, 0, 0);

    void Awake()
    {
        transform.position = offset;
        transform.eulerAngles = rotation;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.position = target.gameObject.transform.position + offset;
    }
}
