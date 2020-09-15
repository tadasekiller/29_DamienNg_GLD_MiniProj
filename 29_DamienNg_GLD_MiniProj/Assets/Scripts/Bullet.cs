using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private float chargelevel;
    private void Start()
    {
        chargelevel = Player_FPS.thisPlayer.shootcharge;
        transform.localScale = new Vector3(0.3f * chargelevel, 0.3f * chargelevel, 0.3f * chargelevel);
        Player_FPS.thisPlayer.shootcharge = 0;
    }
    private void Update()
    {

    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "EnemyPart")
        {
            collision.gameObject.GetComponentInParent<Enemy_Base>().Damage(collision.transform, 35*chargelevel);
        }
        else if (collision.gameObject.tag == "EnemyCloth")
        {
            Destroy(collision.gameObject);
        }
        else if (collision.gameObject.name == "ChargeDoor")
        {
           collision.transform.GetComponentInParent<DoorScript>().Charged();
        }
        Destroy(gameObject);
    }
    
}
