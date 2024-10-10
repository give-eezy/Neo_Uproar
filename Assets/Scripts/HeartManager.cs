using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class HeartManager : MonoBehaviour
{
    public Image[] heartImages;
    public GameObject[] niceStamp;
    private int maxLives = 3; // �ִ� ���
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

        currentLives = maxLives; // �ʱ� ��� ����
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


    

    private void UpdateHearts() // ��Ʈ �̹��� UI�� ������Ʈ �ϴ� �޼���
    {
        for (int i = 0; i < heartImages.Length; i++)
        {
            if (i < currentLives)
            {
                heartImages[i].enabled = true; // ��Ʈ ǥ��
            }
            else
            {
                heartImages[i].enabled = false; // ��Ʈ ����
            }
        }
    }

    private void gameOver() // ���ӿ����� �Ǿ��� �� �ߵ��� �޼���
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

        // Alpha ���� 0���� 1�� ������ ��ȭ��ŵ�ϴ�.
        for (float t = 0; t < fadeTime; t += Time.deltaTime)
        {
            color.a = Mathf.Lerp(0, 0.8f, t / fadeTime);
            panelImage.color = color;
            yield return null; // ���� �����ӱ��� ��ٸ��ϴ�.
        }

        // ���������� Alpha ���� 1�� ����
        color.a = 0.8f;
        panelImage.color = color;

        
    }

    private IEnumerator FadeIn(Text panelText)
    {
        yield return new WaitForSeconds(1f);

        overText.SetActive(true);

        Color color = panelText.color;

        // Alpha ���� 0���� 1�� ������ ��ȭ��ŵ�ϴ�.
        for (float t = 0; t < fadeTime; t += Time.deltaTime)
        {
            color.a = Mathf.Lerp(0, 1, t / fadeTime);
            panelText.color = color;
            yield return null; // ���� �����ӱ��� ��ٸ��ϴ�.
        }

        // ���������� Alpha ���� 1�� ����
        color.a = 1;
        panelText.color = color;

        

        yield return new WaitForSeconds(2f);

        SceneManager.LoadScene("overScene");
    }

    private IEnumerator overBGM()
    {
        float startVolume = mainBGM.volume; // ���� ���� ����
        float duration = 2f;

        for (float t = 0; t < duration; t += Time.deltaTime)
        {
            // ������ ���� ����
            mainBGM.volume = Mathf.Lerp(startVolume, 0, t / duration);
            yield return null; // ���� �����ӱ��� ���
        }

        mainBGM.volume = 0; // ������ ���� ����
    }

    public IEnumerator niceScore()
    {

        yield return new WaitForSeconds(2f);

        int lives = this.currentLives;

        for (int i = 0; i < lives; i++)
        {
            // ���� ������ ����
            GameObject star = Instantiate(niceStamp[i], stampBox);
            // ������ ��ġ ���� (��: ���� ��ġ)
            

            // 0.3�� ���
            yield return new WaitForSeconds(0.3f);
        }

    }

}
