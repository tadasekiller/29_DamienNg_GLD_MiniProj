using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour
{
    private Animator animator;
    public Collider doorcollider;
    public float ArmReq;
    public int switched;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Open()
    {
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("DoorOpen")) //|| !animator.GetCurrentAnimatorStateInfo(0).IsName("DoorClose"))
        {
            animator.Play("DoorOpen");
            doorcollider.enabled = false;
            //Invoke("Close", 10);
        }
    }public void Charged()
    {
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("ChargeDoorOpen")) //|| !animator.GetCurrentAnimatorStateInfo(0).IsName("DoorClose"))
        {
            animator.Play("ChargeDoorOpen");
            //Invoke("Close", 10);
        }
    }
    public void Close()
    {
        animator.Play("DoorClose");
    }
    public void Punchy(float Arms, float punchcharge)
    {
        if (punchcharge >= 3 && Arms >= ArmReq) {
            GameManager.thisManager.Stats.SetActive(false);
            Destroy(transform.GetChild(3).gameObject); }
    }
    public void switches()
    {
        switched++;
        if (switched >= 2)
        {
            animator.Play("DoorOpen");
            doorcollider.enabled = false;
        }
    }
}
