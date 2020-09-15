using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet1 : MonoBehaviour
{
    private void Start()
    {
    }
    private void Update()
    {
        transform.localScale = new Vector3(0.3f * Player_FPS.thisPlayer.shootcharge, 0.3f * Player_FPS.thisPlayer.shootcharge, 0.3f * Player_FPS.thisPlayer.shootcharge);
    }

}
