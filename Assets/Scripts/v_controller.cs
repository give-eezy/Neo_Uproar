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
        // 슬라이더의 현재 볼륨 값에 mainBGM의 볼륨 값을 대입
        v_slider.value = mainBGM.volume;
        
        // 슬라이더의 값이 변경될 때 호출되는 메서드
        v_slider.onValueChanged.AddListener(SetVolume);
    }

    public void SetVolume(float volume)
    {
        // 볼륨 값을 읽어서, mainBGM의 볼륨의 값에 대입
        mainBGM.volume = volume;
    }
}
