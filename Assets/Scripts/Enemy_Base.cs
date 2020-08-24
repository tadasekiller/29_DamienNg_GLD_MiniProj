using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy_Base : MonoBehaviour
{
    private Transform player;
    private NavMeshAgent NavAgent;
    public float range = 30;
    public int multiplier = 1;
    public bool chase = false;
    public GameObject model;
    public float visionCone = 25;
    public float HP = 3;
    private Color defaultcolor;

    private RaycastHit hit;
    private Ray ray;
    [SerializeField] private Renderer[] thisRender;
    private Transform lasthitted;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        NavAgent = GetComponent<NavMeshAgent>();
        defaultcolor = thisRender[0].material.GetColor("_Color");
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.CurrentState == GameManager.GameState.GameInProgress)
        {
            Vector3 targetDir = player.position - transform.position;
            if (Vector3.Angle(targetDir, transform.forward) < visionCone)
            {
                if (Physics.Raycast(ray, out hit, range))
                {

                }
                else
                {

                }
            }
            if (chase)
            {
               // 
            }
            else
            {
                Vector3 runTo = transform.position + ((transform.position - player.position) * multiplier);
                float dist = Vector3.Distance(transform.position, player.position);
                if (dist < range)
                {
                    NavAgent.SetDestination(runTo);
                }
            }
        }
    }
    public void Damage(Transform hitted)
    {
        foreach (Renderer renderer in thisRender)
        {
            renderer.material.SetColor("_Color", Color.red);
        }
        HP--;
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
        transform.DetachChildren();
        Destroy(gameObject,1);
        Destroy(lasthitted.gameObject);
    }
    public void ResetRend()
    {
        foreach (Renderer renderer in thisRender)
        {   
            if (renderer != null)
            renderer.material.SetColor("_Color", defaultcolor);
        }
    }
}
