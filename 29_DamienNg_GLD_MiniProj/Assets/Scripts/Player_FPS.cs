using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class Player_FPS : MonoBehaviour
{
    public static Player_FPS thisPlayer;
    public float MoveSpeed = 10;
    public float MouseSensitivity { get; set; } = 5f;
    public Camera FPSCamera;
    private float RotateY = 0;
    private CharacterController thisController = null;
    public float interactDist = 2.5f;
    private Animator animator;
    public bool crouchToggle = false;

    public bool PunchUpgrade = false;
    public bool GunUpgrade = false;
    public bool keycard = false;

    public float HPMax = 100;
    public float HP = 100;
    public float Sanity = 100;
    public float SanityMax = 100;
    public float SanitySpeed = 0;
    public float Arms = 2;
    public float Legs = 2;
    public float Limbs = 4;
    public float LimbSlots = 6;
    public bool LimbMax = false;
    public float Torsos = 0;
    public float MaxTorsos = 0;

    public Transform Gun;
    public GameObject Bullet;
    

    private Vector3 playerVelocity;
    public float JumpHeight = 1;
    private float gravity = -20;
    private RaycastHit hit;
    private Ray ray;
    // Start is called before the first frame update
    void Start()
    {
        thisPlayer = this;
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
            ShootCheck();
            PunchCall();
            SanityCall();
            Interact();
        }
    }
    private void SanityCall()
    {
        if (Sanity > 0 )
        {
            Sanity -= SanitySpeed* Time.deltaTime;
        }
        else
        {
            SanityMax -= SanitySpeed * Time.deltaTime;
            if (SanityMax <= 0)
            {
                Death();
            }
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

        if (OnGround && playerVelocity.y <= 0)
        {
            playerVelocity.y = 0;
            thisController.slopeLimit = 45;
        }

        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        Vector3 direction = (transform.forward * v + transform.right * h);
        thisController.Move(direction * Time.deltaTime * MoveSpeed);

        if (Input.GetKey(KeyCode.Space) && OnGround)
        {
            playerVelocity.y += Mathf.Sqrt(JumpHeight * -3.0f * gravity);
            thisController.slopeLimit = 90;
        }

        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            thisController.height = 0.8f;
        }
        else if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            thisController.height = 2;
        }
        
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
                if (prevHit.tag == "FallenPart")
                {
                    prevHit.GetComponent<Renderer>().material.SetFloat("_enable1", 0f);
                    GameManager.thisManager.Stats.SetActive(false);
                }
                if (prevHit.tag == "Door")
                {
                    for (int i = 0; i < 2; i++)
                    {
                        GameObject door = prevHit.transform.GetChild(i).gameObject;
                        door.GetComponent<Renderer>().material.SetFloat("_enable1", 0f);
                        GameManager.thisManager.Stats.SetActive(false);
                    }
                }
                if (prevHit.tag == "PunchDoor")
                {
                    for (int i = 0; i < 2; i++)
                    {
                        GameObject door = prevHit.transform.GetChild(i).gameObject;
                        door.GetComponent<Renderer>().material.SetFloat("_enable1", 0f);
                    }
                    GameManager.thisManager.Stats.SetActive(false);
                }
                if (prevHit.tag == "Pickup")
                {
                    prevHit.GetComponent<Renderer>().material.SetFloat("_enable1", 0f);
                }
            }
            if (Vector3.Distance(transform.position, hit.transform.position) <= interactDist)
            {
                if (hitGO.tag == "FallenPart")
                {
                    hitGO.GetComponent<Renderer>().material.SetFloat("_enable1", 1f);
                    prevHit = hitGO;
                    GameManager.thisManager.SetStats(hitGO.name,0);
                }
                if (hitGO.tag == "Door")
                {
                    for (int i = 0; i < 2; i++)
                    {
                        GameObject door = hitGO.transform.GetChild(i).gameObject;
                        door.GetComponent<Renderer>().material.SetFloat("_enable1", 1f);
                    }
                    if (!keycard)
                    {
                        GameManager.thisManager.SetStats(hitGO.tag, 0);
                    }
                    prevHit = hitGO;
                }
                if (hitGO.tag == "PunchDoor")
                {
                    for (int i = 0; i < 2; i++)
                    {
                        GameObject door = hitGO.transform.GetChild(i).gameObject;
                        door.GetComponent<Renderer>().material.SetFloat("_enable1", 1f);
                    }
                    prevHit = hitGO;
                    GameManager.thisManager.SetStats(hitGO.name,hitGO.GetComponentInParent<DoorScript>().ArmReq);
                }
                if (hitGO.tag == "Pickup")
                {
                    hitGO.GetComponent<Renderer>().material.SetFloat("_enable1", 1f);
                    prevHit = hitGO;
                }
            }
        }
    }
    
    private void Interact()
    {
        if (Input.GetKeyUp(KeyCode.E))
        {
            if (Physics.Raycast(ray, out hit) && Vector3.Distance(transform.position, hit.transform.position) <= interactDist)
            {
                switch (hit.transform.tag)
                {
                    
                    case "FallenPart":
                        {
                            bool destroy = true;
                            switch (hit.transform.name)
                            {
                                case "Head":
                                    if (Sanity >= 100)
                                    {
                                        destroy = false;
                                        GameManager.thisManager.DisplayError("MaxSanity");
                                    }
                                    else
                                    {
                                        Sanity = SanityMax;
                                    }
                                    break;
                                case "Torso":
                                    if (Torsos >= MaxTorsos)
                                    {
                                        destroy = false;
                                        GameManager.thisManager.DisplayError("MaxTorsos");
                                    }
                                    else
                                    {
                                        HPMax += 5;
                                        HP += 10;
                                        if (HP > HPMax)
                                            HP = HPMax;
                                        LimbSlots++;
                                        LimbMax = false;
                                    }
                                    break;
                                case "Right Arm":
                                    if (LimbMax == true && Legs == 2)
                                    {
                                        destroy = false;
                                        GameManager.thisManager.DisplayError("MinLimb");
                                    }
                                    else if (LimbMax == true)
                                    {
                                        Legs--;
                                        Arms++;
                                    }
                                    else
                                    {
                                        Limbs++;
                                        Arms++;
                                        if (Limbs == LimbSlots)
                                        {
                                            LimbMax = true;
                                        }
                                    }
                                    break;
                                case "Left Arm":
                                    if (LimbMax == true && Legs == 2)
                                    {
                                        destroy = false;
                                        GameManager.thisManager.DisplayError("MinLimb");
                                    }
                                    else if (LimbMax == true)
                                    {
                                        Legs--;
                                        Arms++;
                                    }
                                    else
                                    {
                                        Limbs++;
                                        Arms++;
                                        if (Limbs == LimbSlots)
                                        {
                                            LimbMax = true;
                                        }
                                    }
                                    break;
                                case "Right Leg":
                                    if (LimbMax == true && Arms == 2)
                                    {
                                        destroy = false;
                                        GameManager.thisManager.DisplayError("MinLimb");
                                    }
                                    else if (LimbMax == true)
                                    {
                                        Arms--;
                                        Legs++;
                                    }
                                    else
                                    {
                                        Limbs++;
                                        Legs++;
                                        if (Limbs == LimbSlots)
                                        {
                                            LimbMax = true;
                                        }
                                    }
                                    break;
                                case "Left Leg":
                                    if (LimbMax == true && Arms == 2)
                                    {
                                        destroy = false;
                                        GameManager.thisManager.DisplayError("MinLimb");
                                    }
                                    else if (LimbMax == true)
                                    {
                                        Arms--;
                                        Legs++;
                                    }
                                    else
                                    {
                                        Limbs++;
                                        Legs++;
                                        if (Limbs == LimbSlots)
                                        {
                                            LimbMax = true;
                                        }
                                    }
                                    break;
                                case "ChargeDoor":
                                    GameManager.thisManager.DisplayError("ChargeDoor");
                                    destroy = false;
                                    break;
                                case "KeyCard":
                                    keycard = true;
                                    break;
                                case "Switch":
                                    {
                                        hit.transform.GetComponentInParent<Switch>().hitSwitch();
                                        destroy = false;
                                        break;
                                    }
                            }
                            if (destroy)
                            {
                                Destroy(hit.transform.parent.gameObject);
                                GameManager.thisManager.Stats.SetActive(false);
                            }
                            JumpHeight = Legs * 0.5f;
                            MoveSpeed = 6 + Legs * 2;
                            break;
                        }
                    case "Door":
                        {
                            if (keycard)
                            hit.transform.parent.GetComponent<DoorScript>().Open();
                            break;
                        }
                    case "PunchDoor":
                        {
                            GameManager.thisManager.DisplayError("PunchDoor");
                            break;
                        }
                    case "Pickup":
                        {
                            switch (hit.transform.name)
                            {
                                case "Gun Pickup":
                                    {
                                        GunUpgrade = true;
                                        MaxTorsos = 6;
                                        Destroy(hit.transform.parent.gameObject);
                                        if (PlayerPrefs.GetInt("SanityTut", 0) == 0)
                                        {
                                            GameManager.thisManager.SanityTut();
                                        }
                                        SanitySpeed = 1;
                                        break;
                                    }
                            }
                            break;
                        }
                }
                
            }
        }
    }
    private void Shoot()
    {
        Vector3 targetpoint;
        if (Physics.Raycast(ray, out hit))
            targetpoint = hit.point;
        else
            targetpoint = ray.GetPoint(1000);
        Debug.DrawRay(ray.origin, ray.direction, Color.red, 1);
        GameObject bulletinst = Instantiate(Bullet, Gun.position, Gun.transform.rotation);
        bulletinst.GetComponentInChildren<Rigidbody>().velocity = (targetpoint - Gun.position).normalized * 25;
        Destroy(bulletinst, 5);
    }
    private void ShootCheck()
    {
        if (GunUpgrade == true)
        {
            if (Input.GetMouseButtonDown(1) && !animator.GetCurrentAnimatorStateInfo(0).IsName("Shooting"))
            {
                if (Sanity > 20)
                {
                    animator.Play("ShootCharge");
                    Sanity -= 20;
                }
                else
                {
                    GameManager.thisManager.DisplayError("NoSanity");
                }
            }
            if (Input.GetMouseButtonUp(1) && animator.GetCurrentAnimatorStateInfo(0).IsName("ShootCharge"))
            {
                animator.Play("Shooting");
            }
        }
    }
    public float shootcharge = 0;
    private void ShootChargeAdd()
    {
        if (shootcharge < 3)
        {
            shootcharge++;
        }
    }
    public float punchcharge = 0;
    private void PunchChargeAdd()
    {
        if (punchcharge < 3)
        {
            punchcharge++;
        }
    }
    private void PunchCall()
    {
        if (!PunchUpgrade)
        {
            if (Input.GetMouseButton(0) && !animator.GetCurrentAnimatorStateInfo(0).IsName("Punch"))
            {
                animator.Play("Punch");
            }
        }
        else
        {
            if (Input.GetMouseButtonDown(0) && !animator.GetCurrentAnimatorStateInfo(0).IsName("Punch"))
            {
                animator.Play("PunchCharge");
            }
            if (Input.GetMouseButtonUp(0) && animator.GetCurrentAnimatorStateInfo(0).IsName("PunchCharge"))
            {
                animator.Play("Punch");
            }
        }
    }
    private void Punched()
    {
        if (Physics.Raycast(ray, out hit, 2.8f))
        {
            if (hit.transform.tag == "EnemyPart")
            {
                hit.transform.GetComponentInParent<Enemy_Base>().Damage(hit.transform,(5*(3+Arms))*punchcharge);
                //i want knockbackkk!!!!
                //hit.transform.GetComponentInParent<NavMeshAgent>().Stop();
                //hit.transform.GetComponentInParent<>
            }
            if (hit.transform.tag == "PunchDoor")
            {
                hit.transform.GetComponentInParent<DoorScript>().Punchy(Arms,punchcharge);
                //i want knockbackkk!!!!
                //hit.transform.GetComponentInParent<NavMeshAgent>().Stop();
                //hit.transform.GetComponentInParent<>
            }
        }
        punchcharge = 0;
    }
    public void ReceiveDamage(float dmg)
    {
        HP -= dmg;
        if (HP <= 0)
        {
            Death();
        }
    }
    private void Death()
    {
        GameManager.CurrentState = GameManager.GameState.GameOver;
        animator.Play("Death");
    }
    private void totheGM()
    {
        GameManager.thisManager.Death();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Exit")
        {
            SceneManager.LoadScene(1);
            PlayerPrefs.SetString("TimeSet", (Time.time - GameManager.thisManager.TimerStart).ToString("F2"));
            Cursor.lockState = CursorLockMode.None;
        }
    }
}
