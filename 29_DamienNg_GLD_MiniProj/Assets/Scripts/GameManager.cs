using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public enum GameState { GameOver, GameInProgress, GamePaused,GameStart,Tutorial };
    public static GameState CurrentState = GameState.GameStart;
    

    public static GameManager thisManager;
    private Animator animator;
    public Text Startmsg;
    public Text Breakmsg;
    public GameObject Stats;
    public GameObject Error;
    public Text bodyPart;
    public Text bodyText;
    public Text bodyText2;
    public Text ErrorText;
    public Image healthbar;
    public Image healthbarback;
    public Image sanitybar;
    public Image sanitybarback;
    public Image BasicTutorial;
    public Image GunTutorial;
    public Text deathmsg;
    public Text deathmsg2;
    private Outline healthout;
    private Outline healthoutback;
    private Outline sanityout;
    private Outline sanityoutback;
    public AudioSource bgm;
    public GameObject PauseMenu;
    public GameObject OptionsMenu;
    public float MusicVolume { get; set; } = 0.5f;
    public float TimerStart;

    public float breakout = 0;

    void Start()
    {
        thisManager = this;
        animator = GetComponent<Animator>();
        Stats.SetActive(false);
        PauseMenu.SetActive(false);
        OptionsMenu.SetActive(false);
        bodyText2.enabled = false;
        BasicTutorial.enabled = false;
        GunTutorial.enabled = false;
        deathmsg.enabled = false;
        deathmsg2.enabled = false;
        healthout = healthbar.GetComponent<Outline>();
        healthoutback = healthbarback.GetComponent<Outline>();
        sanityout = sanitybar.GetComponent<Outline>();
        sanityoutback = sanitybarback.GetComponent<Outline>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        if (CurrentState == GameState.GameStart)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (breakout >= 3)
                {
                    Startmsg.enabled = false;
                    Breakmsg.enabled = false;
                    if (PlayerPrefs.GetInt("BasicTut", 0) == 0)
                    {
                        BasicTutorial.enabled = true;
                        CurrentState = GameState.Tutorial;
                        PlayerPrefs.SetInt("BasicTut", 1);
                    }
                    else
                    {
                        CurrentState = GameState.GameInProgress;
                    }
                    bgm.Play();
                    TimerStart = Time.time;
                }
                breakout++;
                animator.SetFloat("breakstate", breakout);
            }
        }
        if (CurrentState == GameState.GameInProgress)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                CurrentState = GameState.GamePaused;
                Cursor.lockState = CursorLockMode.None;
                PauseMenu.SetActive(true);
            }

        }
        else if (CurrentState == GameState.GamePaused)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                CurrentState = GameState.GameInProgress;
                Cursor.lockState = CursorLockMode.Locked;
                PauseMenu.SetActive(false);
                OptionsMenu.SetActive(false);
            }
        }
        else if (CurrentState == GameState.Tutorial)
        {
            if (Input.GetKeyDown(KeyCode.T))
            {
                CurrentState = GameState.GameInProgress;
                BasicTutorial.enabled = false;
                GunTutorial.enabled = false;

            }
        }
        else if (CurrentState == GameState.GameOver)
        {
            if (Input.GetKeyDown(KeyCode.R) && dead)
            {
                SceneManager.LoadScene(0);
                CurrentState = GameState.GameStart;
            }
        }
        bgm.volume = MusicVolume;
        BarCheck();
        //restart the current scene
        if (Input.GetKeyDown(KeyCode.R)&&Input.GetKey(KeyCode.LeftShift))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            PlayerPrefs.SetInt("BasicTut", 0);
            PlayerPrefs.SetInt("SanityTut", 0);
            CurrentState = GameState.GameStart;
        }
    }
    public void SanityTut()
    {
        CurrentState = GameState.Tutorial;
        GunTutorial.enabled = true;
        PlayerPrefs.SetInt("SanityTut", 1);
        OptionsMenu.SetActive(false);
    }
    public void BasicTut()
    {
        CurrentState = GameState.Tutorial;
        BasicTutorial.enabled = true;
        PlayerPrefs.SetInt("BasicTut", 1);
        OptionsMenu.SetActive(false);
    }
    public bool dead;
    public void Death()
    {
        deathmsg.enabled = true;
        deathmsg2.enabled = true;
        bgm.Stop();
        dead = true;
    }
    public void BarCheck()
    {
        healthbar.rectTransform.sizeDelta = new Vector2(Player_FPS.thisPlayer.HP * 2, 20);
        healthbarback.rectTransform.sizeDelta = new Vector2(Player_FPS.thisPlayer.HPMax * 2, 20);
        sanitybar.rectTransform.sizeDelta = new Vector2(Player_FPS.thisPlayer.Sanity * 2, 20);
        sanitybarback.rectTransform.sizeDelta = new Vector2(Player_FPS.thisPlayer.SanityMax * 2, 20);
        //MaxSanitybar
        if (Player_FPS.thisPlayer.Sanity <= 0)
        {
            sanityoutback.effectColor = Color.red;
        }
        else
        {
            sanityoutback.effectColor = Color.white;
        }
        //Sanitybar
        if (Player_FPS.thisPlayer.Sanity <= 25)
        {
            sanityout.effectColor = Color.red;
        }
        else if (Player_FPS.thisPlayer.Sanity <= 50)
        {
            sanityout.effectColor = Color.yellow;
        }
        else
        {
            sanityout.effectColor = Color.white;
        }
        if (Player_FPS.thisPlayer.Sanity <= 25)
        {
            sanityout.effectColor = Color.red;
        }
        else if (Player_FPS.thisPlayer.Sanity <= 50)
        {
            sanityout.effectColor = Color.yellow;
        }
        else
        {
            sanityout.effectColor = Color.white;
        }

    }
    public void SetStats(string name,float numbie)
    {
        Stats.SetActive(true);
        switch (name)
        {
            case "Head":
                bodyPart.text = name;
                bodyText.text = "Regenerates Sanity";
                bodyText2.enabled = false;
                return;
            case "Torso":
                bodyPart.text = name;
                bodyText.text = "Increases Limb Cap";
                bodyText2.enabled = false;
                return;
            case "Right Arm":
                bodyPart.text = name;
                bodyText.text = "Improve Strength";
                if (Player_FPS.thisPlayer.LimbMax)
                {
                    bodyText2.enabled = true;
                    bodyText2.text = "Removes Leg";
                }
                else
                {
                    bodyText2.enabled = false;
                }
                return;
            case "Left Arm":
                bodyPart.text = name;
                bodyText.text = "Improve Strength";
                if (Player_FPS.thisPlayer.LimbMax)
                {
                    bodyText2.enabled = true;
                    bodyText2.text = "Removes Leg";
                }
                else
                {
                    bodyText2.enabled = false;
                }
                return;
            case "Right Leg":
                bodyPart.text = name;
                bodyText.text = "Improve Speed/Jump";
                if (Player_FPS.thisPlayer.LimbMax)
                {
                    bodyText2.enabled = true;
                    bodyText2.text = "Removes Arm";
                }
                else
                {
                    bodyText2.enabled = false;
                }
                return;
            case "Left Leg":
                bodyPart.text = name;
                bodyText.text = "Improve Speed/Jump";
                if (Player_FPS.thisPlayer.LimbMax)
                {
                    bodyText2.enabled = true;
                    bodyText2.text = "Removes Arm";
                }
                else
                {
                    bodyText2.enabled = false;
                }
                return;
            case "Punch Doors":
                bodyPart.text = "Broken Door";
                bodyText.text = "Charge punch to break";
                bodyText2.enabled = true;
                bodyText2.text = "Requires "+numbie+" Arms or more";
                return;
            case "ChargeDoor":
                bodyPart.text = "Powered Door";
                bodyText.text = "Needs some sort of charge";
                bodyText2.enabled = false;
                return;
            case "Door":
                bodyPart.text = "Locked Door";
                bodyText.text = "Needs a keycard of sorts";
                bodyText2.enabled = false;
                return;
            case "KeyCard":
                bodyPart.text = "A Keycard!";
                bodyText.text = "You probably need this";
                bodyText2.enabled = false;
                return;
            case "Switch":
                bodyPart.text = "Switch for a door";
                bodyText.text = "Hit it!";
                bodyText2.enabled = false;
                return;
            case "Cylinder":
                bodyPart.text = "??????";
                bodyText.text = "Charge punch to break";
                bodyText2.enabled = true;
                bodyText2.text = "Need. This. Want. This.";
                return;
        }
    }
    public void DisplayError (string name)
    {
        Error.SetActive(true);
        switch (name)
        {
            case "MaxTorsos":
                ErrorText.text = "Can't. Fit. More. Body..";

                animator.Play("ErrorFade");
                
                break;
            case "Not Enough Strength":
                ErrorText.text = "Not. Strong. Enough..";

                animator.Play("ErrorFade");

                break;
            case "NoSanity":
                ErrorText.text = "Can't. Focus...";

                animator.Play("ErrorFade");

                break;
            case "MinLimb":
                ErrorText.text = "Can't. Lose. Anymore...";

                animator.Play("ErrorFade");

                break;
            case "MaxSanity":
                ErrorText.text = "There is no use for this.";
                animator.Play("ErrorFade");
                break;
            case "PunchDoor":
                ErrorText.text = "Needs. Brute. Force.";
                animator.Play("ErrorFade");
                break;
            case "ChargeDoor":
                ErrorText.text = "Can't open. Needs. Power?";
                animator.Play("ErrorFade");
                break;
        }
    }
}
