using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_ThirdPerson : MonoBehaviour
{
    private CharacterController thisController = null;
    public float MoveSpeed = 10;

    public Transform CameraRig;
    public float CamYSpeed = 1; //how fast the camera rotate for Y axis
    public float CamXSpeed = 1; //how fast the camera rotaet for  X axis
    private float CamY = 0; 
    private float CamX = 0;

    public Transform Gun;
    public GameObject Bullet;
    public float ShootInterval = 0.1f;
    private float NextShoot = 0;

    void Start()
    {
        thisController = GetComponent<CharacterController>();
    }

    void Update()
    {
        Movement();
        CameraControls();
        MouseAimShoot();
    }

    private void Movement()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        Vector3 direction = (transform.forward * v + transform.right * h);

        thisController.Move(direction * MoveSpeed * Time.deltaTime);
    }

    private void CameraControls()
    {
        //update position for CameraRig
        CameraRig.transform.position = transform.position;

        //manage the rotation for CameraRig
        CamY -= Input.GetAxis("Mouse Y") * CamYSpeed;
        CamY = Mathf.Clamp(CamY, -28, 60);
        CamX += Input.GetAxis("Mouse X") * CamXSpeed;
        CameraRig.localEulerAngles = new Vector3(CamY, CamX, 0);

        //ensure camera looks at player
        Camera.main.transform.LookAt(transform.position);

        //determine direction of Camera from player -> required as we want player to rotate to camera facing direction
        Vector3 rotateDirection = transform.position - Camera.main.transform.position;

        //rotate player towards direction where camera is pointing
        rotateDirection = new Vector3(rotateDirection.x, 0, rotateDirection.z);
        Vector3 newDirection = Vector3.RotateTowards(transform.forward, rotateDirection, Time.deltaTime * 10, 0);
        transform.rotation = Quaternion.LookRotation(newDirection);
    }

    private void MouseAimShoot()
    {
        Ray r = Camera.main.ScreenPointToRay(Input.mousePosition);
        Vector3 targetDirection = (r.origin + r.direction * 30) - Gun.transform.position;
        Vector3 newDirection = Vector3.RotateTowards(Gun.transform.forward, targetDirection, Time.deltaTime * 10, 0);
        Gun.transform.rotation = Quaternion.LookRotation(newDirection);

        if(Input.GetMouseButton(0) && Time.time >= NextShoot)
        {
            NextShoot = Time.time + ShootInterval;
            Instantiate(Bullet, Gun.transform.position, Gun.transform.rotation);
        }
    } 
}
