using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform2D_Bullet: MonoBehaviour
{
    public float Force = 1000;

    void Start()
    {
        Destroy(gameObject, 2);
    }

    //called from Player_Platformer.cs when bullet is spawned to indicate the direction of the bullet
    public void InitiateBullet(Vector3 direction)
    {
        GetComponent<Rigidbody>().AddForce(direction * Force);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.isStatic)
            Destroy(gameObject);
    }
}
