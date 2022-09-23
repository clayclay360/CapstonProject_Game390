using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{

    public enum Player {PlayerOne,PlayerTwo};
    public Player player;
    private float movingSpeed = 3;
    private float turningSpeed = 300;
    private Vector2 moveVal;
    public Vector3 mousePos;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(new Vector3(moveVal.x, 0, moveVal.y) * movingSpeed * Time.deltaTime);
        //transform.Rotate(new Vector3(0, moveVal.x, 0) * turningSpeed * Time.deltaTime);
        //mousePos = Display.RelativeMouseAt(Input.mousePosition);
        //transform.LookAt(mousePos);
    }

    public void OnMove(InputValue value)
    {
        //Debug.Log("OnMove Called!");
        moveVal = value.Get<Vector2>();

        if (moveVal.x > 0)
        {
            moveVal.x = 1;
        }
        else if (moveVal.x < 0)
        {
            moveVal.x = -1;
        }
        if (moveVal.y > 0)
        {
            moveVal.y = 1;
        }
        else if (moveVal.y < 0)
        {
            moveVal.y = -1;
        }

        //Debug.Log(moveVal);
    }

    //public void onlook(inputvalue value)
    //{
    //    mousepos = value.get<vector2>();
    //}
}
