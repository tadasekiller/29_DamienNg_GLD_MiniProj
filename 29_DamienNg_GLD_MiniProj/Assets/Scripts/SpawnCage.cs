using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class SpawnCage : MonoBehaviour
{
    private Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.CurrentState == GameManager.GameState.GameStart)
        {
            animator.SetFloat("breakstate", GameManager.thisManager.breakout);
        }
    }
}
