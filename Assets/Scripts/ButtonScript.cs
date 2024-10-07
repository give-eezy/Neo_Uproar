using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class ButtonScript : MonoBehaviour
{
    Transform Buttons;
    DirectorScript Director;
    GameObject p_panel;

    Button p_button;

    AudioSource mainBGM;


    private void Awake()
    {
        p_panel = GameObject.Find("pausePanel");
    }


    // Start is called before the first frame update
    void Start()
    {
        
        Buttons = GameObject.Find("Buttons").GetComponent<Transform>();
        Director = GameObject.Find("GameDirector").GetComponent<DirectorScript>();
        mainBGM = Director.GetComponent<AudioSource>();
        p_button = GameObject.Find("pauseButton").GetComponent<Button>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void leftOnclick()
    {
        if (Time.timeScale == 0)
        {
            return; // 일시 정지 상태일 때 클릭 무시
        }
        Director.buttonListener(true);
    }

    public void rightOnclick()
    {
        if (Time.timeScale == 0)
        {
            return; // 일시 정지 상태일 때 클릭 무시
        }

        Director.buttonListener(false);
    }


    public void closeHowTo()
    {
        GameObject howTo = GameObject.Find("howTo");
        howTo.SetActive(false);
        p_button.interactable = true;
        Time.timeScale = 1;
        mainBGM.Play();
    }

    public void restart()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void gameQuit()
    {
        Application.Quit();
    }

    public void pauseFunc()
    {
        
        Time.timeScale = 0;
        Debug.Log(p_panel);
        p_panel.SetActive(true);
    }

    public void resumeFunc()
    {
        
        Time.timeScale = 1;
        p_panel.SetActive(false);
    }
}
