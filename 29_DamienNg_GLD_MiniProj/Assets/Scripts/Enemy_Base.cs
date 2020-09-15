using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy_Base : MonoBehaviour
{
    private Transform player;
    private NavMeshAgent NavAgent;
    private Animator animator;
    [SerializeField] private Renderer[] thisRender;
    [SerializeField] private Color[] defaultcolor;
    public Transform[] patrollocations;

    public float range = 10;
    public int multiplier = 2;
    public bool chase = false;
    public GameObject model;
    public float visionCone = 30;
    public float HP = 3;

    private RaycastHit hit;
    private Ray ray;
    private Transform lasthitted;
    private Vector3 lastseen;
    public bool seeing;
    public bool seen;
    public bool gunned;
        public GameObject Bullet;
        public Transform Gun;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        NavAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        int i = 0;
        foreach (Renderer renderer in thisRender)
        {
            defaultcolor[i] = renderer.material.GetColor("_Color");
            i++;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.CurrentState == GameManager.GameState.GameInProgress)
        {
            float dist = Vector3.Distance(transform.position, player.position);
            Vision();
            if (seen)
            {
                if (chase)
                {
                    Chase();
                }
                else
                {
                    Cower();
                }
            }
            else
            {
                Patrol();
            }
        }
    }
    private bool running;
    private bool cowering;
    private void Cower()
    {
        if (!running&&!cowering)
        {
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("LookAround"))
                animator.Play("Idle");
            Vector3 runTo = transform.position + ((transform.position - player.position) * multiplier);
            NavAgent.SetDestination(runTo);
            animator.SetBool("moving", true);
            NavAgent.speed = 5;
            running = true;
        }
        else
        {
            if (Vector3.Distance(NavAgent.destination, transform.position) > 0.1f &&running)
            {
                NavAgent.isStopped = false;
                animator.SetBool("moving", true);
            }
            else if (Vector3.Distance(NavAgent.destination, transform.position) <= 0.1f && running)
            {
                NavAgent.isStopped = true;
                animator.SetBool("moving", false);
                animator.Play("Cower");
                running = false;
                cowering = true;
                NavAgent.speed = 8;
            }
        }
        
    }
    private float kalmpoint;
    private void kalming()
    {
        if (kalmpoint >= 5)
        {
            seen = false;
            cowering = false;
            animator.Play("Idle");
            kalmpoint = 0;
        }
        else
        {
            kalmpoint++;
        }
    }
    private bool patrolling;
    private void Patrol()
    {
        if (!patrolling && !animator.GetCurrentAnimatorStateInfo(0).IsName("LookAround"))
        {
            Vector3 patrolpoint = patrollocations[Random.Range(0, 3)].position;
            NavAgent.SetDestination(patrolpoint);
            patrolling = true;
        }
        else
        {
            if (Vector3.Distance(NavAgent.destination, transform.position) > 0.5f && patrolling)
            {
                NavAgent.isStopped = false;
                animator.SetBool("moving", true);
            }
            else if (Vector3.Distance(NavAgent.destination, transform.position) <= 0.5f &&patrolling)
            {
                NavAgent.isStopped = true;
                animator.SetBool("moving", false);
                animator.Play("LookAround");
                patrolling = false;
            }
        }
    }
    private bool reached;
    public float nextAttack = 1;
    public float attackInterval = 3;
    private void Chase()
    {
        if (seeing)
        {
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("LookAround"))
                animator.Play("Idle");
            reached = false;
            NavAgent.isStopped = true;
            animator.SetBool("moving", false);
            //Vector3 rotatedir = transform.position + player.position;
            //rotatedir = new Vector3(rotatedir.x, 0, rotatedir.z);
            //transform.rotation = Quaternion.LookRotation(rotatedir);
            transform.LookAt(new Vector3(player.position.x, transform.position.y, player.position.z));
            
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("Aim") || animator.GetCurrentAnimatorStateInfo(0).IsName("Aiming") || animator.GetCurrentAnimatorStateInfo(0).IsName("Shoot"))
            {
                if (Time.time >= nextAttack)
                {
                    nextAttack = Time.time + attackInterval;
                    animator.SetTrigger("Shoot");
                }
            }
            else
            {
                animator.Play("Aim");
            }
        }
        else
        {
            if (Vector3.Distance(NavAgent.destination, transform.position) > 0.1f && !reached)
            {
                NavAgent.isStopped = false;
                animator.SetBool("moving", true);
                NavAgent.SetDestination(lastseen);
            }
            else if (Vector3.Distance(NavAgent.destination, transform.position) <= 0.1f && !reached)
            {
                NavAgent.isStopped = true;
                animator.SetBool("moving", false);
                animator.Play("LookAround");
                reached = true;
            }
            else
            {
                seen = false;
            }
        }
    }
    private void Shoot()
    {
        GameObject bulletinst = Instantiate(Bullet, Gun.position, Gun.transform.rotation);
        bulletinst.GetComponentInChildren<Rigidbody>().velocity = (player.position - Gun.position).normalized * 20;
        Destroy(bulletinst, 5);
    }
    private void Vision()
    {
        Vector3 targetDir = player.position - transform.position;
        Vector3 targetDir2 = new Vector3(targetDir.x, targetDir.y - 1.3f, targetDir.z);
        if (Vector3.Angle(targetDir, model.transform.forward) < visionCone)
        {
            ray = new Ray(model.transform.position, targetDir2);

            //Debug.DrawRay(model.transform.position, targetDir,Color.red);
            //Debug.DrawRay(model.transform.position, player.transform.position,Color.cyan);
            //
            if (Physics.Raycast(ray, out hit, range))
            {
                if (hit.transform.tag == "Player")
                {
                    Debug.DrawRay(model.transform.position, targetDir2, Color.green);
                    lastseen = player.position;
                    seeing = true;
                    seen = true;
                }
                else
                {
                    seeing = false;
                }
            }
            else
            {
                seeing = false;
            }
        }
    }
    public void Damage(Transform hitted,float damage)
    {
        if (damage > 0)
        {
            foreach (Renderer renderer in thisRender)
            {
                renderer.material.SetColor("_Color", Color.red);
            }
        }
        HP -= damage;
        if (HP <= 0)
        {
            lasthitted = hitted;
            Death();
        }
        Invoke("ResetRend", 0.3f);
    }
    public void Death()
    {
        foreach (Transform child in model.transform)
        {
            child.gameObject.AddComponent<Rigidbody>();
            child.gameObject.tag = "FallenPart";
        }
        Destroy(animator);
        transform.DetachChildren();
        Destroy(gameObject,1);
        Destroy(lasthitted.gameObject);
    }
    public void ResetRend()
    {

        int i = 0;
        foreach (Renderer renderer in thisRender)
        {
            if (renderer != null)
            {
                renderer.material.SetColor("_Color", defaultcolor[i]);
            }
            i++;
        }
    }
}
