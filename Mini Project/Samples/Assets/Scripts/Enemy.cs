using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int HP = 5;
    public int MoveSpeed = 10;
    //change the value for x, y, z of MoveDirection to observe the direction the enemy is moving relative to camera direction.
    public Vector3 MoveDirection = Vector3.zero;

    void Update()
    {
        //add codes for enemy behaviour inside this block : enemy should only run when game is in progress
        if(GameManager.CurrentState == GameManager.GameState.GameInProgress)
        {
            transform.Translate(MoveDirection * MoveSpeed * Time.deltaTime);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "PlayerBullet")
        {
            Destroy(other.gameObject);
            RecieveDamage();
        }
    }

    public void RecieveDamage()
    {
        HP--;
        if (HP <= 0)
        {
            Destroy(gameObject);
        }
    }
}
