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
    float blinkDuration = 0.6f; // 깜빡이는 지속 시간
    float blinkInterval = 0.1f; // 깜빡임 간격

    Coroutine startCo; // 코루틴을 멈추거나 할 때 객체를 사용해야 StopCoroutine에 인자로 전달이 가능하다

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
            Debug.Log("키가 입력되었습니다!");

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
            startText.enabled = !startText.enabled; // 텍스트 표시/숨기기
            elapsedTime += blinkInterval;
            yield return new WaitForSeconds(blinkInterval);
        }

        startText.enabled = true; // 마지막에는 텍스트를 다시 표시

        float startVolume = startAudi[0].volume; // 현재 볼륨 저장

        for (float t = 0; t < 2f; t += Time.deltaTime)
        {
            // 서서히 볼륨 조정
            startAudi[0].volume = Mathf.Lerp(startVolume, 0, t / 2f);
            yield return null; // 다음 프레임까지 대기
        }

        startAudi[0].volume = 0; // 마지막 볼륨 설정


        yield return new WaitForSeconds(0.2f);

        SceneManager.LoadScene("GameScene");
    }

    private IEnumerator n_BlinkCo()
    {
        this.isClicked = false;
        startText.text = "-Press Any Key-";

        while (true)
        {
            startText.enabled = !startText.enabled; // 텍스트 표시/숨기기
            yield return new WaitForSeconds(normalBlink);
        }
    }

    private IEnumerator startInit(Vector2 target, float duration)
    {
        Vector2 startPosition = new Vector2(0, startObj[0].transform.position.y - 100); // 시작 위치
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            // 시간에 따라 위치 계산
            float t = elapsedTime / duration;
            startObj[0].transform.position = Vector2.Lerp(startPosition, target, t); // 부드럽게 이동

            elapsedTime += Time.deltaTime; // 경과 시간 업데이트
            yield return null; // 다음 프레임까지 대기
        }

        // 최종 위치 설정
        transform.position = target; // 정확한 목표 위치로 이동


        yield return new WaitForSeconds(0.2f);

        StartCoroutine(startCreator(startObj[1]));

        

        StartCoroutine(startCreator(startObj[2]));

        yield return new WaitForSeconds(0.3f);

        StartCoroutine(titleCreator(startObj[3]));

        yield return new WaitForSeconds(0.2f);

        startCo = StartCoroutine(n_BlinkCo());

    }


    private IEnumerator startCreator(GameObject obj) // 프리팹 생성용
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

    private IEnumerator titleCreator(GameObject obj) // 별이 뒤에서 뿜어져 나오는 듯한 이펙트로
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
