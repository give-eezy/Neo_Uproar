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
            return; // �Ͻ� ���� ������ �� Ŭ�� ����
        }
        Director.buttonListener(true);
    }

    public void rightOnclick()
    {
        if (Time.timeScale == 0)
        {
            return; // �Ͻ� ���� ������ �� Ŭ�� ����
        }

        Director.buttonListener(false);
    }


    public void closeHowTo() // ���� ���� �ݱ� ��ư ��� �Լ�
    {
        GameObject howTo = GameObject.Find("howTo"); // howTo �г��� ã�Ƽ�
        howTo.SetActive(false); // �г� ��Ȱ��ȭ
        p_button.interactable = true; // �Ͻ����� ��ư�� ��ȣ�ۿ� �����ϵ��� �������
        Time.timeScale = 1; // ������ �ð� �ǵ�����
        mainBGM.Play(); // ����BGM ��ŸƮ
    }

    public void restart() // ����� ��ư ��� �Լ�
    {
        Time.timeScale = 1f; // ������ �ð��� �ٽ� �ǵ����鼭
        SceneManager.LoadScene("GameScene"); // GameScene, �� ���� ������ �̷������ ���� �ҷ��´�
    }

    public void gameQuit() // �������� ��ư ��� �Լ�
    {
        Application.Quit();
    }

    public void pauseFunc() // �Ͻ����� ��ư ��� �Լ�
    {
       
        Time.timeScale = 0; // ������ �ð��� �����ְ�
        p_panel.SetActive(true); // pausePanel �� Ȱ��ȭ �����ش�
    }

    public void resumeFunc() // ���� �簳 ��ư ��� �Լ�
    {
        
        Time.timeScale = 1; // ������ �ð��� �ٽ� �ǵ�����
        p_panel.SetActive(false); // pausePanel�� ��Ȱ��ȭ ��Ų��
    }
}
