using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnifeAddon : MonoBehaviour
{

    public int damage;

    private Rigidbody rb;

    private bool targetHit;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
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

            Destroy(enemy.gameObject);
            Destroy(gameObject);
        }

        Invoke(nameof(DestroyKnife), 2f);

    }

    private void DestroyKnife()
    {
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
