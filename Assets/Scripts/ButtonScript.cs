using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class ButtonScript : MonoBehaviour
{
    Transform Buttons;
    DirectorScript Director;

    

    // Start is called before the first frame update
    void Start()
    {
        
        Buttons = GameObject.Find("Buttons").GetComponent<Transform>();
        Director = GameObject.Find("GameDirector").GetComponent<DirectorScript>();
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

    public void testClick()
    {
        Debug.Log("눌리지롱");
    }

    public void closeHowTo()
    {
        GameObject howTo = GameObject.Find("howTo");
        howTo.SetActive(false);
        Time.timeScale = 1;
    }

}
