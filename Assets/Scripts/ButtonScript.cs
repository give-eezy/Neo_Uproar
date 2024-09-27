using System.Collections;
using System.Collections.Generic;
using UnityEditor.VisionOS;
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
        Director.checkingLogic("left");
    }

    public void rightOnclick()
    {

        Director.checkingLogic("right");

    }

}
