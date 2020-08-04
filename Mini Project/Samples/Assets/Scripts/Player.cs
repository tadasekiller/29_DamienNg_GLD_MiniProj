using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player thisPlayer;

    private float NextAttack = 0;
    public float AttackInterval = 1;
    public GameObject Prefab_Bullet = null;
    public float MoveSpeed = 10;
    public bool UseMouseLook = true;

    void Start()
    {
        thisPlayer = this;
    }

    void Update()
    {
        if (GameManager.CurrentState == GameManager.GameState.GameInProgress)
        {
            Movement();
            MouseLook();
            Shoot();
        }
    }

    private void Movement()
    {
        //get input from player for Horizontal axes only
        float horizontal = Input.GetAxis("Horizontal");
        //get input for Vertical axix : disabled for now
        float Vertical = Input.GetAxis("Vertical");
        //if you want the player to move forward, replace MoveDirection.Z below with Vertical
        //if you want the player to move up/down, replace MoveDirection.Y below with Vertical
        //Note all these are base on world space, if your camera is not align with the world space, the direction will be different
        Vector3 MoveDirection = new Vector3(horizontal, 0, 0);
        transform.position += MoveDirection * Time.deltaTime * MoveSpeed;
    }

    private void MouseLook()
    {
        if (!UseMouseLook)
            return;

        Ray r = Camera.main.ScreenPointToRay(Input.mousePosition);

        float enter = 0;
        Plane p = new Plane(Vector3.down, transform.position.y);

        if (p.Raycast(r, out enter))
        {
            Vector3 hit = r.GetPoint(enter);

            Vector3 direction = hit - transform.position;
            Vector3 rotateDirection = Vector3.RotateTowards(direction, transform.forward, 1 * Time.deltaTime, 0);
            transform.rotation = Quaternion.LookRotation(rotateDirection);
        }
    }

    private void Shoot()
    {
        if (Input.GetMouseButton(0) && Time.time >= NextAttack)
        {
            NextAttack = Time.time + AttackInterval;
            Instantiate(Prefab_Bullet, transform.position +  transform.forward, transform.rotation);
        }
    }
}
