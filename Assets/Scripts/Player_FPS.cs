using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class Player_FPS : MonoBehaviour
{
    public float MoveSpeed = 10;
    public float MouseSensitivity = 1;
    public Camera FPSCamera;
    private float RotateY = 0;
    private CharacterController thisController = null;
    public float interactDist = 10;
    private Animator animator;

    public Transform Gun;
    public GameObject Bullet;
    public float ShootInterval = 1f;
    private float NextShoot = 1;
    

    private Vector3 playerVelocity;
    public float JumpHeight = 1;
    private float gravity = -20;
    private RaycastHit hit;
    private Ray ray;

    public GameObject Gamemanager;
    // Start is called before the first frame update
    void Start()
    {
        thisController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(GameManager.CurrentState == GameManager.GameState.GameInProgress)
        {
            ray = FPSCamera.ViewportPointToRay(new Vector3(0.5F, 0.5F, 0));
            MouseLook();
            Movement();
            CheckCrosshair();
            Shoot();
            PunchCall();
        }
    }

    private void MouseLook()
    {
        float rotateX = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * MouseSensitivity;

        RotateY -= Input.GetAxis("Mouse Y") * MouseSensitivity;
        RotateY = Mathf.Clamp(RotateY, -60, 60);
        FPSCamera.transform.localEulerAngles = new Vector3( RotateY,  0, 0);
        transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, rotateX, transform.localEulerAngles.z);
    }

    private void Movement()
    {


        bool OnGround = thisController.isGrounded;

        if (OnGround && playerVelocity.y < 0)
            playerVelocity.y = 0;

        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        Vector3 direction = (transform.forward * v + transform.right * h);
        thisController.Move(direction * Time.deltaTime * MoveSpeed);

        if (Input.GetKeyDown(KeyCode.Space) && OnGround)
            playerVelocity.y += Mathf.Sqrt(JumpHeight * -3.0f * gravity);

        playerVelocity.y += gravity * Time.deltaTime;
        thisController.Move(playerVelocity * Time.deltaTime);
    }

    GameObject prevHit = null;
    private void CheckCrosshair()
    {
        if (Physics.Raycast(ray, out hit)) 
        {
            GameObject hitGO = hit.transform.gameObject;

            if (prevHit != hitGO && prevHit != null)
            {
                prevHit.GetComponent<Renderer>().material.SetFloat("_enable1", 0f);
                prevHit.GetComponent<BodyPart>().MouseOff();
                GameManager.thisManager.Stats.SetActive(false);
            }
            if (Vector3.Distance(transform.position, hit.transform.position) <= interactDist)
            {
                if (hitGO.tag == "FallenPart")
                {
                    hitGO.GetComponent<Renderer>().material.SetFloat("_enable1", 1f);
                    hitGO.GetComponent<BodyPart>().MousedOver();
                    prevHit = hitGO;
                    GameManager.thisManager.SetStats(hitGO.name);
                }
            }
        }
    }
    private void PartPickup()
    {
        if (Input.GetKeyUp(KeyCode.E))
        {
            if (Physics.Raycast(ray, out hit))
            {
                switch (hit.transform.tag)
                {
                    case "FallenPart":
                        {
                            break;
                        }
                    case "Switch":
                        {
                            break;
                        }
                }
                
            }
        }
    }

    private void Shoot()
    {
        if (Input.GetMouseButton(1) && Time.time >= NextShoot)
        {
            NextShoot = Time.time + ShootInterval;
            Vector3 targetpoint;
            if (Physics.Raycast(ray, out hit))
                targetpoint = hit.point;
            else
                targetpoint = ray.GetPoint(1000);
            Debug.DrawRay(ray.origin,ray.direction ,Color.red,1);
            GameObject bullet = Instantiate(Bullet, Gun.position, Gun.transform.rotation);
            bullet.GetComponentInChildren<Rigidbody>().velocity = (targetpoint - Gun.position).normalized * 25;
            Destroy(bullet, 5);
        }
    }
    private void PunchCall()
    {
        if (Input.GetMouseButton(0) && animator.GetCurrentAnimatorStateInfo(0).IsName("Punch"))
        {
        }
        else if (Input.GetMouseButton(0))
        {
            animator.Play("Punch");
        }
    }
    private void Punched()
    {
        if (Physics.Raycast(ray, out hit, 2.5f))
        {
            if (hit.transform.tag == "EnemyPart")
            {
                hit.transform.GetComponentInParent<Enemy_Base>().Damage(hit.transform);
                //i want knockbackkk!!!!
                //hit.transform.GetComponentInParent<NavMeshAgent>().Stop();
                //hit.transform.GetComponentInParent<>
            }
        }
    }
}
