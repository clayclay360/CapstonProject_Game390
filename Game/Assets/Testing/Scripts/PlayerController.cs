using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public enum Player {PlayerOne,PlayerTwo};
    public Player player;
    public float speed;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(player.ToString());
    }

    // Update is called once per frame
    void Update()
    {
        if (Player.PlayerOne == player)
        {
            if (Input.GetKey(KeyCode.W))
            {
                transform.Translate(Vector3.forward * speed * Time.deltaTime);
            }
            if (Input.GetKey(KeyCode.S))
            {
                transform.Translate(Vector3.back * speed * Time.deltaTime);
            }
            if (Input.GetKey(KeyCode.A))
            {
                transform.Translate(Vector3.left * speed * Time.deltaTime);
            }
            if (Input.GetKey(KeyCode.D))
            {
                transform.Translate(Vector3.right * speed * Time.deltaTime);
            }
        }
        else
        {
            if (Input.GetKey(KeyCode.UpArrow))
            {
                transform.Translate(Vector3.forward * speed * Time.deltaTime);
            }
            if (Input.GetKey(KeyCode.DownArrow))
            {
                transform.Translate(Vector3.back * speed * Time.deltaTime);
            }
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                transform.Translate(Vector3.left * speed * Time.deltaTime);
            }
            if (Input.GetKey(KeyCode.RightArrow))
            {
                transform.Translate(Vector3.right * speed * Time.deltaTime);
            }
        }
    }
}
