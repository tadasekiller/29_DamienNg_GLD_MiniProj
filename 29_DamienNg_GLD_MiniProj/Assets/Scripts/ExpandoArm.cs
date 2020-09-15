using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpandoArm : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.localScale = Vector3.one * (1+Player_FPS.thisPlayer.punchcharge * 0.3f);
    }
}
