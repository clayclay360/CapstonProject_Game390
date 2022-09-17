using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{

    public enum Player {PlayerOne,PlayerTwo};
    public Player player;
    public float movingSpeed;
    public float turningSpeed;
    public Vector2 moveVal;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
            transform.Translate(new Vector3(0, 0, moveVal.y) * movingSpeed * Time.deltaTime);
            transform.Rotate(new Vector3(0, moveVal.x, 0) * turningSpeed * Time.deltaTime);
    }

    private void OnMove(InputValue value)
    {
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
    }
}
