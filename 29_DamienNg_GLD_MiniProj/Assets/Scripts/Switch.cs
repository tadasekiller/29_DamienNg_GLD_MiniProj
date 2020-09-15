using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : MonoBehaviour
{
    public GameObject Door;
    private DoorScript doorScript;
    private Animator animator;
    public Collider switchobj;
    // Start is called before the first frame update
    void Start()
    {
        doorScript = Door.GetComponent<DoorScript>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void hitSwitch()
    {
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Switch"))
            animator.Play("Switch");
        doorScript.switches();
        switchobj.enabled = false;
    }
}
