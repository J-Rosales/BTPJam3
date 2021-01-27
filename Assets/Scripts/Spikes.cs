using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spikes : MonoBehaviour
{
    Rigidbody2D myRigidbody;

    private void Awake()
    {
        myRigidbody = GetComponent<Rigidbody2D>();    
    }
    
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.GetComponent<PlayerMovement>())
            FallOut();
    }

    void FallOut()
    {
        Vector2 direction = new Vector2(Random.Range(-5, 5), Random.Range(1, 5));
        transform.SetParent(null);
        myRigidbody.isKinematic = false;
        myRigidbody.AddTorque(20f, ForceMode2D.Impulse);
        myRigidbody.AddForce(direction, ForceMode2D.Impulse);
    }
}
