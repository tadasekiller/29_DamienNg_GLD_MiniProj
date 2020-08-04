using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Rigidbody thisRigidBody = null;
    public float Force = 600;
    public float TimeOut = 3;

    void Start()
    {
        thisRigidBody = GetComponent<Rigidbody>();
        thisRigidBody.AddForce(transform.forward * Force);
        Destroy(gameObject, TimeOut);
    }

    void OnCollisionEnter(Collision other)
    {
        //add your own codes here
    }

    void OnTriggerEnter(Collider other)
    {
        //add your codes here
        //remember to check if your collider isTrigger

        if (other.gameObject.isStatic)
            Destroy(gameObject);
    }
}
