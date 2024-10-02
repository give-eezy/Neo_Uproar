using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class HeartManager : MonoBehaviour
{
    public Image[] heartImages;
    private int maxLives = 3; // �ִ� ���
    private int currentLives;

    float fadeTime = 2f;


    GameObject overPanel;
    GameObject overText;

    GameObject director;

    

    // Start is called before the first frame update
    void Start()
    {
        director = GameObject.Find("GameDirector");

        overPanel = GameObject.Find("overPanel");
        overText = GameObject.Find("overText");
        

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

}
