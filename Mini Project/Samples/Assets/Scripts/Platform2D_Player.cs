using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform2D_Player : MonoBehaviour
{
    public float MoveSpeed = 10;        //duh...you should know
    public float JumpForce = 10;        //how fast the upward momentum of the jump
    public float JumpHeight = 1.5f;     //maximum height the player should be able to jump
    public float fallSpeedRate = 10;    //how fast the player falls
    public float AirTime = 0.25f;       //how long before the player starts falling after a jump

    private CharacterController thisController = null;
    private Vector3 MoveDirection = Vector3.zero;
    private bool Jumped = false;
    private float fallSpeed = 0;    
    private float JumpedY = 0;
    private float JumpTimeOut = 0;

    [Space]
    public GameObject Bullet;
    public float ShootInterval = 0.1f;
    private float NextShoot = 0;

    void Start()
    {
        thisController = GetComponent<CharacterController>();
    }

    void Update()
    {
        if(GameManager.CurrentState == GameManager.GameState.GameInProgress)
        {
            Movement();
            Shoot();
        }
    }

    private void Movement()
    {
        MoveDirection.x = Input.GetAxis("Horizontal") * Time.deltaTime * MoveSpeed;

        if (thisController.isGrounded)
        {
            MoveDirection.y = 0;

            if (Jumped) Jumped = false;

            if (Input.GetKey(KeyCode.Space) && !Jumped)
            {
                JumpedY = transform.position.y + JumpHeight;
                JumpTimeOut = Time.time + AirTime;
                fallSpeed = 0;
                Jumped = true;
                MoveDirection.y = JumpForce * Time.deltaTime;
            }
        }

        else
        {
            if (!Jumped)
            {
                if (fallSpeed > -10)
                    fallSpeed -= Time.deltaTime * fallSpeedRate;

                MoveDirection.y = fallSpeed * Time.deltaTime;
            }

            else if (CheckIfHitTop() || transform.position.y >= JumpedY || Time.time >= JumpTimeOut)
                Jumped = false;
        }

        MoveDirection = new Vector3(MoveDirection.x, MoveDirection.y, 0);
        thisController.Move(MoveDirection);
    }

    private bool CheckIfHitTop()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.up, out hit, 1))
        {
            if (hit.collider.gameObject.isStatic)
                return true;
        }

        return false;
    }

    private void Shoot()
    {
        if(Time.time >= NextShoot && Input.GetMouseButton(0))
        {
            NextShoot = Time.time + ShootInterval;

            Vector3 mp = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mp = new Vector3(mp.x, mp.y, transform.position.z);

            Vector3 direction = (mp - transform.position).normalized;
            GameObject b = Instantiate(Bullet, transform.position + direction.normalized, Quaternion.identity);
            b.GetComponent<Platform2D_Bullet>().InitiateBullet(direction);
        }
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        //add your logic here for collision detection on player
    }
}
