using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnifeAddon : MonoBehaviour
{

    public int damage;

    public int speed;
    public Vector3 forward;

    private Rigidbody rb;

    private bool targetHit;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        rb.velocity = forward * speed;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (targetHit)
        {
        return;
        }
        else
        targetHit = true;

        

        if(collision.gameObject.GetComponent<RatScript>() != null)
        {
            Debug.Log("Rat hit");
            RatScript enemy = collision.gameObject.GetComponent<RatScript>();

            enemy.TakeDamage(damage);

            Destroy(gameObject);
        }

        Invoke(nameof(DestroyKnife), 0.7f);

    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (targetHit)
    //    {
    //        return;
    //    }
    //    else
    //        targetHit = true;



    //    if (other.gameObject.GetComponent<RatScript>() != null)
    //    {
    //        Debug.Log("Rat scared");
    //        RatScript enemy = other.gameObject.GetComponent<RatScript>();

    //        //enemy.TakeDamage(damage);

    //        Destroy(gameObject);
    //    }

    //    Invoke(nameof(DestroyKnife), 0.7f);
    //}

    private void DestroyKnife()
    {
        Destroy(gameObject);
    }
}
