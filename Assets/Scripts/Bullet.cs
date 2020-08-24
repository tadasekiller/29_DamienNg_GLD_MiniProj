using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private void Start()
    {
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "EnemyPart")
        {
            collision.gameObject.GetComponentInParent<Enemy_Base>().Death();
        }
        else if (collision.gameObject.tag == "EnemyCloth")
        {
            Destroy(collision.gameObject);
        }
        Destroy(gameObject);
    }
}
