using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public enum GameState { GameOver, GameInProgress, GamePaused };
    public static GameState CurrentState = GameState.GamePaused;

    public static GameManager thisManager;
    public Text Txt_Message;
    public GameObject Stats;
    public Text bodyPart;
    public Text bodyText;
    void Start()
    {
        thisManager = this;
        Stats.SetActive(false);
    }

    void Update()
    {
        if (CurrentState == GameState.GamePaused)
        {
            if (Input.GetKey(KeyCode.Space))
            {
                CurrentState = GameState.GameInProgress;
                Txt_Message.gameObject.SetActive(false);
            }
        }


        //restart the current scene
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
    public void SetStats(string name)
    {
        Stats.SetActive(true);
        switch (name)
        {
            case "Head":
                bodyPart.text = name;
                bodyText.text = "Regenerates Sanity";
                return;
            case "Torso":
                bodyPart.text = name;
                bodyText.text = "Restores Health";
                return;
            case "RightArm":
                bodyPart.text = name;
                bodyText.text = "Improve Strength";
                return;
            case "LeftArm":
                bodyPart.text = name;
                bodyText.text = "Improve Strength";
                return;
            case "RightLeg":
                bodyPart.text = name;
                bodyText.text = "Improve Speed/Jump";
                return;
            case "LeftLeg":
                bodyPart.text = name;
                bodyText.text = "Improve Speed/Jump";
                return;
        }
    }
}
