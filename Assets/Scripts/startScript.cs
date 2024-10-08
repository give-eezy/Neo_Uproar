using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class startScript : MonoBehaviour
{

    Text startText;


    bool isClicked = false;


    float normalBlink = 0.8f;
    float blinkDuration = 1f; // 깜빡이는 지속 시간
    float blinkInterval = 0.1f; // 깜빡임 간격

    Coroutine startCo; // 코루틴을 멈추거나 할 때 객체를 사용해야 StopCoroutine에 인자로 전달이 가능하다

    // Start is called before the first frame update
    void Start()
    {
        startText = GameObject.Find("startText").GetComponent<Text>();

        startCo = StartCoroutine(n_BlinkCo());
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.anyKeyDown && !isClicked)
        {
            Debug.Log("키가 입력되었습니다!");
            StopCoroutine(startCo);
            StartCoroutine(gameStart());
        }
    }

    private IEnumerator gameStart()
    {
        isClicked = true;
        float elapsedTime = 0f;

        Debug.Log("깜빡깜빡");

        while (elapsedTime < blinkDuration)
        {
            startText.enabled = !startText.enabled; // 텍스트 표시/숨기기
            elapsedTime += blinkInterval;
            yield return new WaitForSeconds(blinkInterval);
        }

        startText.enabled = true; // 마지막에는 텍스트를 다시 표시
        this.isClicked = false;
        

        yield return new WaitForSeconds(1.8f);

        SceneManager.LoadScene("GameScene");
    }

    private IEnumerator n_BlinkCo()
    {
        while (true)
        {
            Debug.Log("노말 깜빡");
            startText.enabled = !startText.enabled; // 텍스트 표시/숨기기
            yield return new WaitForSeconds(normalBlink);
        }
    }
}
