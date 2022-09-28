using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnifeAddon : MonoBehaviour
{

    public int damage;

    //private Rigidbody rb;

    private bool targetHit;
    // Start is called before the first frame update
    void Start()
    {
        //rb = GetComponent<RigidBody>();
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
            RatScript enemy = collision.gameObject.GetComponent<RatScript>();

            enemy.TakeDamage(damage);

            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
