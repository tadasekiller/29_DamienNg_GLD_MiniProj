using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform2D_Camera : MonoBehaviour
{
    public Transform target;
    public Vector3 Offset;
    public float damper = 1;
    private Vector3 v = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(target != null)
        {
            Vector3 offsetPos = target.position + Offset;
            transform.position = Vector3.SmoothDamp(transform.position, offsetPos, ref v, damper);
        }
    }
}
