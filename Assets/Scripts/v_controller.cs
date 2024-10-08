using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class v_controller : MonoBehaviour
{
    AudioSource mainBGM;

    Slider v_slider;

    // Start is called before the first frame update
    void Start()
    {
        mainBGM = GameObject.Find("GameDirector").GetComponent<AudioSource>();
        v_slider = GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        // �����̴��� ���� ���� ���� mainBGM�� ���� ���� ����
        v_slider.value = mainBGM.volume;
        
        // �����̴��� ���� ����� �� ȣ��Ǵ� �޼���
        v_slider.onValueChanged.AddListener(SetVolume);
    }

    public void SetVolume(float volume)
    {
        // ���� ���� �о, mainBGM�� ������ ���� ����
        mainBGM.volume = volume;
    }
}
