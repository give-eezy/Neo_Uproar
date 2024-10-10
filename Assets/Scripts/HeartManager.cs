using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class HeartManager : MonoBehaviour
{
    public Image[] heartImages;
    public GameObject[] niceStamp;
    private int maxLives = 3; // 최대 목숨
    private int currentLives;

    float fadeTime = 2f;

    GameObject popUp;
    GameObject overPanel;
    GameObject overText;

    GameObject director;

    Transform stampBox;


    AudioSource mainBGM;


    private void Awake()
    {
        stampBox = GameObject.Find("resultPanel").GetComponent<Transform>();
    }

    // Start is called before the first frame update
    void Start()
    {
        director = GameObject.Find("GameDirector");

        popUp = GameObject.Find("popUp");
        overPanel = GameObject.Find("overPanel");
        overText = GameObject.Find("overText");

       

        mainBGM = director.GetComponent<AudioSource>();

        popUp.SetActive(false);
        overPanel.SetActive(false);
        overText.SetActive(false);

        currentLives = maxLives; // 초기 목숨 설정
        UpdateHearts();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoseLife()
    {
        if (currentLives > 0)
        {
            currentLives--;
            UpdateHearts();
        }

        if (currentLives <= 0)
        {
            gameOver();
        }
    }


    

    private void UpdateHearts() // 하트 이미지 UI를 업데이트 하는 메서드
    {
        for (int i = 0; i < heartImages.Length; i++)
        {
            if (i < currentLives)
            {
                heartImages[i].enabled = true; // 하트 표시
            }
            else
            {
                heartImages[i].enabled = false; // 하트 숨김
            }
        }
    }

    private void gameOver() // 게임오버가 되었을 때 발동할 메서드
    {

        director.GetComponent<DirectorScript>().stopCo = true;

        StartCoroutine(overBGM());

        popUp.SetActive(true);

        StartCoroutine(FadeIn(overPanel.GetComponent<Image>()));
        StartCoroutine(FadeIn(overText.GetComponent<Text>()));

        
    }

    private IEnumerator FadeIn(Image panelImage)
    {

        yield return new WaitForSeconds(1f);
        overPanel.SetActive(true);


        Color color = panelImage.color;

        // Alpha 값을 0에서 1로 서서히 변화시킵니다.
        for (float t = 0; t < fadeTime; t += Time.deltaTime)
        {
            color.a = Mathf.Lerp(0, 0.8f, t / fadeTime);
            panelImage.color = color;
            yield return null; // 다음 프레임까지 기다립니다.
        }

        // 최종적으로 Alpha 값을 1로 설정
        color.a = 0.8f;
        panelImage.color = color;

        
    }

    private IEnumerator FadeIn(Text panelText)
    {
        yield return new WaitForSeconds(1f);

        overText.SetActive(true);

        Color color = panelText.color;

        // Alpha 값을 0에서 1로 서서히 변화시킵니다.
        for (float t = 0; t < fadeTime; t += Time.deltaTime)
        {
            color.a = Mathf.Lerp(0, 1, t / fadeTime);
            panelText.color = color;
            yield return null; // 다음 프레임까지 기다립니다.
        }

        // 최종적으로 Alpha 값을 1로 설정
        color.a = 1;
        panelText.color = color;

        

        yield return new WaitForSeconds(2f);

        SceneManager.LoadScene("overScene");
    }

    private IEnumerator overBGM()
    {
        float startVolume = mainBGM.volume; // 현재 볼륨 저장
        float duration = 2f;

        for (float t = 0; t < duration; t += Time.deltaTime)
        {
            // 서서히 볼륨 조정
            mainBGM.volume = Mathf.Lerp(startVolume, 0, t / duration);
            yield return null; // 다음 프레임까지 대기
        }

        mainBGM.volume = 0; // 마지막 볼륨 설정
    }

    public IEnumerator niceScore()
    {

        yield return new WaitForSeconds(2f);

        int lives = this.currentLives;

        for (int i = 0; i < lives; i++)
        {
            // 별점 프리팹 생성
            GameObject star = Instantiate(niceStamp[i], stampBox);
            // 별점의 위치 조정 (예: 수평 배치)
            

            // 0.3초 대기
            yield return new WaitForSeconds(0.3f);
        }

    }

}
