using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public AudioSource bgm;
    public GameObject OptionsMenu;
    public Text timer;
    public void ExitBtn()
    {
        Application.Quit();
    }
    public void RestartBtn()
    {
        SceneManager.LoadScene(0);
        GameManager.CurrentState = GameManager.GameState.GameStart;
    }
    public void OptionsBtn()
    {
        OptionsMenu.SetActive(true);
        gameObject.SetActive(false);
    }
    private void Start()
    {
        if (timer != null)
        {
            timer.text = "Time Taken: " + PlayerPrefs.GetString("TimeSet","err0r")+"s";
        }
    }

}
