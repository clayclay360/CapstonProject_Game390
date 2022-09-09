using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public enum Player {PlayerOne,PlayerTwo};
    public Player player;
    public float movingSpeed;
    public float turningSpeed;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Movement();   
    }

    private void Movement()
    {
        if (Player.PlayerOne == player)
        {
            if (Input.GetKey(KeyCode.W))
            {
                transform.Translate(Vector3.forward * movingSpeed * Time.deltaTime);
            }
            if (Input.GetKey(KeyCode.S))
            {
                transform.Translate(Vector3.back * movingSpeed * Time.deltaTime);
            }
            if (Input.GetKey(KeyCode.A))
            {
                transform.Rotate(Vector3.down * turningSpeed * Time.deltaTime);
            }
            if (Input.GetKey(KeyCode.D))
            {
                transform.Rotate(Vector3.up * turningSpeed * Time.deltaTime);
            }
        }
        else
        {
            if (Input.GetKey(KeyCode.UpArrow))
            {
                transform.Translate(Vector3.forward * movingSpeed * Time.deltaTime);
            }
            if (Input.GetKey(KeyCode.DownArrow))
            {
                transform.Translate(Vector3.back * movingSpeed * Time.deltaTime);
            }
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                transform.Translate(Vector3.left * movingSpeed * Time.deltaTime);
            }
            if (Input.GetKey(KeyCode.RightArrow))
            {
                transform.Translate(Vector3.right * movingSpeed * Time.deltaTime);
            }
        }
    }
}
