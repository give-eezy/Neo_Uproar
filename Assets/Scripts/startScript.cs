using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class startScript : MonoBehaviour
{
    public GameObject[] startObj;


    Text startText;

    public AudioSource[] startAudi;

    bool isClicked = true;


    float normalBlink = 0.8f;
    float blinkDuration = 0.6f; // �����̴� ���� �ð�
    float blinkInterval = 0.1f; // ������ ����

    Coroutine startCo; // �ڷ�ƾ�� ���߰ų� �� �� ��ü�� ����ؾ� StopCoroutine�� ���ڷ� ������ �����ϴ�

    // Start is called before the first frame update
    void Start()
    {
        startText = GameObject.Find("startText").GetComponent<Text>();
        
   
        StartCoroutine(startInit(startObj[0].transform.position, 1.5f));
        


    }

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKeyDown && !isClicked)
        {
            Debug.Log("Ű�� �ԷµǾ����ϴ�!");

            StopCoroutine(startCo);
            StartCoroutine(gameStart());
        }
        
    }

    private IEnumerator gameStart()
    {
        startAudi[1].Play();

        isClicked = true;
        float elapsedTime = 0f;


        while (elapsedTime < blinkDuration)
        {
            startText.enabled = !startText.enabled; // �ؽ�Ʈ ǥ��/�����
            elapsedTime += blinkInterval;
            yield return new WaitForSeconds(blinkInterval);
        }

        startText.enabled = true; // ���������� �ؽ�Ʈ�� �ٽ� ǥ��

        float startVolume = startAudi[0].volume; // ���� ���� ����

        for (float t = 0; t < 2f; t += Time.deltaTime)
        {
            // ������ ���� ����
            startAudi[0].volume = Mathf.Lerp(startVolume, 0, t / 2f);
            yield return null; // ���� �����ӱ��� ���
        }

        startAudi[0].volume = 0; // ������ ���� ����


        yield return new WaitForSeconds(0.2f);

        SceneManager.LoadScene("GameScene");
    }

    private IEnumerator n_BlinkCo()
    {
        this.isClicked = false;
        startText.text = "-Press Any Key-";

        while (true)
        {
            startText.enabled = !startText.enabled; // �ؽ�Ʈ ǥ��/�����
            yield return new WaitForSeconds(normalBlink);
        }
    }

    private IEnumerator startInit(Vector2 target, float duration)
    {
        Vector2 startPosition = new Vector2(0, startObj[0].transform.position.y - 100); // ���� ��ġ
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            // �ð��� ���� ��ġ ���
            float t = elapsedTime / duration;
            startObj[0].transform.position = Vector2.Lerp(startPosition, target, t); // �ε巴�� �̵�

            elapsedTime += Time.deltaTime; // ��� �ð� ������Ʈ
            yield return null; // ���� �����ӱ��� ���
        }

        // ���� ��ġ ����
        transform.position = target; // ��Ȯ�� ��ǥ ��ġ�� �̵�


        yield return new WaitForSeconds(0.2f);

        StartCoroutine(startCreator(startObj[1]));

        

        StartCoroutine(startCreator(startObj[2]));

        yield return new WaitForSeconds(0.3f);

        StartCoroutine(titleCreator(startObj[3]));

        yield return new WaitForSeconds(0.2f);

        startCo = StartCoroutine(n_BlinkCo());

    }


    private IEnumerator startCreator(GameObject obj) // ������ ������
    {

   

        float elapsedTime = 0f;
        float scaleDuration = 0.2f;
        float scaleMutiplier = 0.3f;
        Vector3 finalScale = new Vector3(2f, 2f, 2f);

        GameObject star = Instantiate(obj);
        star.transform.localScale = finalScale * scaleMutiplier;


        while (elapsedTime < scaleDuration)
        {
            float scale = Mathf.Lerp(finalScale.x * scaleMutiplier, 2.5f, elapsedTime / scaleDuration);
            star.transform.localScale = new Vector3(scale, scale, 1);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        elapsedTime = 0f;

        while (elapsedTime < scaleDuration)
        {
            float scale = Mathf.Lerp(2.5f, finalScale.x, elapsedTime / scaleDuration);
            star.transform.localScale = new Vector3(scale, scale, 1);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
       

        star.transform.localScale = finalScale;


       

    }

    private IEnumerator titleCreator(GameObject obj) // ���� �ڿ��� �վ��� ������ ���� ����Ʈ��
    {

        yield return new WaitForSeconds(0.5f);


        float elapsedTime = 0f;
        float scaleDuration = 0.2f;
        float scaleMutiplier = 5f;
        Vector3 finalScale = new Vector3(1.4f, 1.4f, 1.4f);

        GameObject star = Instantiate(obj);
        star.transform.localScale = finalScale * scaleMutiplier;


        while (elapsedTime < scaleDuration)
        {
            float scale = Mathf.Lerp(finalScale.x * scaleMutiplier, finalScale.x, elapsedTime / scaleDuration);
            star.transform.localScale = new Vector3(scale, scale, 1);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        star.transform.localScale = finalScale;


        yield return new WaitForSeconds(0.5f);

    }




}
