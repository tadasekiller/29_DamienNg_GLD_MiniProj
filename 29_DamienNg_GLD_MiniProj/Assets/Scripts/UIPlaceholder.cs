using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPlaceholder : MonoBehaviour
{
    public Text Limbs;
    public Outline LimbsOut;
    public Text Arms;
    public Text Legs;
    // Start is called before the first frame update
    void Start()
    {
        LimbsOut = Limbs.GetComponent<Outline>();
    }

    // Update is called once per frame
    void Update()
    {
        Limbs.text = "Limbs: " + Player_FPS.thisPlayer.Limbs + "/" + Player_FPS.thisPlayer.LimbSlots;
        Arms.text = "Arms: " + Player_FPS.thisPlayer.Arms;
        Legs.text = "Legs: " + Player_FPS.thisPlayer.Legs;
        if (Player_FPS.thisPlayer.LimbMax)
        {
            LimbsOut.effectColor = Color.red;
        }
        else
        {
            LimbsOut.effectColor = Color.white;
        }
    }
}
