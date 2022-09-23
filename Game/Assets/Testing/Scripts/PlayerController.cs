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
    private Vector3 moveVec;
    private Vector2 mousePos;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Movement
        transform.position += moveVec * movingSpeed * Time.deltaTime;

        //Rotation
        //transform.Rotate(new Vector3(0, moveVal.x, 0) * turningSpeed * Time.deltaTime);
        //mousePos = Display.RelativeMouseAt(Input.mousePosition);
        //transform.LookAt(mousePos);
    }

    public void OnMove(InputValue value)
    {
        //Debug.Log("OnMove Called!");
        moveVal = value.Get<Vector2>();
        moveVec = new Vector3(moveVal.x, 0, moveVal.y);

    }

    public void OnLook(InputValue value)
    {
        
        //OnLook is connected to the mouse's delta value
        mousePos = value.Get<Vector2>();
        //God help me
        //Uses a raycast to make the player look towards the mouse
        Ray ray = Camera.main.ScreenPointToRay(mousePos);

        if (Physics.Raycast(ray, out RaycastHit raycastHit))
        {
            Vector3 lookDir = raycastHit.point;
            lookDir -= transform.position;
            //lookDir.y = transform.rotation.y;
            lookDir.y = 0f;
            Debug.Log(lookDir);
            transform.LookAt(lookDir);
            //transform.rotation.eulerAngles.y = 0f;
        }
    }
}
