using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BodyPart : MonoBehaviour
{
    public Camera playerCam;
    //public Canvas canvas;
    //public GameObject panel;
    // Start is called before the first frame update
    void Start()
    {
        //canvas.enabled = false;
        playerCam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        //panel.transform.LookAt(playerCam.transform);
        if (transform.position.y <= -10)
            Destroy(gameObject);
    }
    public void MousedOver()
    {
        //canvas.enabled = true;
    }
    public void MouseOff()
    {
        //canvas.enabled = false;
    }
}
