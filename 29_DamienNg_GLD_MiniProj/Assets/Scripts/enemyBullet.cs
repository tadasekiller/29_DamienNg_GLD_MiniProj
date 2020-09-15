using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyBullet : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        //destroy this bullet when it hit something
        if (other.tag == "Player")
        {
            Player_FPS.thisPlayer.ReceiveDamage(10);
            Destroy(gameObject);
        }
        if (other.tag == "Obstacle")
        {
            Destroy(gameObject);
        }
    }
    /*private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "EnemyPart")
        {
            collision.gameObject.GetComponentInParent<Enemy_Base>().Damage(collision.transform, 35 * chargelevel);
        }
        else if (collision.gameObject.tag == "EnemyCloth")
        {
            Destroy(collision.gameObject);
        }
        Destroy(gameObject);
    }*/
}
