using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

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


    public void closeHowTo() // 게임 개요 닫기 버튼 기능 함수
    {
        GameObject howTo = GameObject.Find("howTo"); // howTo 패널을 찾아서
        howTo.SetActive(false); // 패널 비활성화
        p_button.interactable = true; // 일시정지 버튼이 상호작용 가능하도록 만들어줌
        Time.timeScale = 1; // 게임의 시간 되돌리기
        mainBGM.Play(); // 메인BGM 스타트
    }

    public void restart() // 재시작 버튼 기능 함수
    {
        Time.timeScale = 1f; // 게임의 시간을 다시 되돌리면서
        SceneManager.LoadScene("GameScene"); // GameScene, 즉 메인 게임이 이루어지는 씬을 불러온다
    }

    public void gameQuit() // 게임종료 버튼 기능 함수
    {
        Application.Quit();
    }

    public void pauseFunc() // 일시정지 버튼 기능 함수
    {
       
        Time.timeScale = 0; // 게임의 시간을 멈춰주고
        p_panel.SetActive(true); // pausePanel 을 활성화 시켜준다
    }

    public void resumeFunc() // 게임 재개 버튼 기능 함수
    {
        
        Time.timeScale = 1; // 게임의 시간을 다시 되돌리고
        p_panel.SetActive(false); // pausePanel을 비활성화 시킨다
    }
}
