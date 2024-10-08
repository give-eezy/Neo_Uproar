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
    float blinkDuration = 1f; // �����̴� ���� �ð�
    float blinkInterval = 0.1f; // ������ ����

    Coroutine startCo; // �ڷ�ƾ�� ���߰ų� �� �� ��ü�� ����ؾ� StopCoroutine�� ���ڷ� ������ �����ϴ�

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
            Debug.Log("Ű�� �ԷµǾ����ϴ�!");
            StopCoroutine(startCo);
            StartCoroutine(gameStart());
        }
    }

    private IEnumerator gameStart()
    {
        isClicked = true;
        float elapsedTime = 0f;

        Debug.Log("��������");

        while (elapsedTime < blinkDuration)
        {
            startText.enabled = !startText.enabled; // �ؽ�Ʈ ǥ��/�����
            elapsedTime += blinkInterval;
            yield return new WaitForSeconds(blinkInterval);
        }

        startText.enabled = true; // ���������� �ؽ�Ʈ�� �ٽ� ǥ��
        this.isClicked = false;
        

        yield return new WaitForSeconds(1.8f);

        SceneManager.LoadScene("GameScene");
    }

    private IEnumerator n_BlinkCo()
    {
        while (true)
        {
            Debug.Log("�븻 ����");
            startText.enabled = !startText.enabled; // �ؽ�Ʈ ǥ��/�����
            yield return new WaitForSeconds(normalBlink);
        }
    }
}
