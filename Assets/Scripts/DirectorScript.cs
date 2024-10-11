using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;


public class DirectorScript : MonoBehaviour
{
    public GameObject[] prefabs; // ĳ���� ������ �迭
    public GameObject[] signs; // �°� Ʋ���� �Ǵ��ϴ� ������ �迭
    public GameObject stampPrefab; // Ŭ���� ���� �� ��Ÿ�� �̹���
    

    private GameObject currentPrefab; // ���� ������ ĳ���� ������

    HeartManager heart; // ����� ���̴� �Լ��� ���� ���ؼ�
    AudioSource mainBGM; // �������


    public GameObject result_Rb;
    public GameObject result_Qb;

    // ��ȣ�ۿ� ��ư. ������ �ƴ��� �Ǵ��ϱ� ���� ��ư���� ����
    public GameObject LButton; 
    public GameObject RButton;

    GameObject howTo; // ���� ���� ȭ��
    GameObject s_bubble; // ��ǳ��
    GameObject p_panel; // �Ͻ����� ȭ��
    GameObject r_panel;
    GameObject result_paper;


    Button p_button; // �Ͻ����� �� �޴� ��ư

    Transform Buttons; // ���� ���� ��ư��(������ �ƴ��� ��ȣ�ۿ�) �� �θ� �ҷ��ͼ� ĳ���� �����հ� �Բ� Destroy �ϱ� ����
    Transform resultBs;

    GameObject disolveP;

    Text cdText; // ������ ��Ÿ���� ����
    Text scText; // ������


    private Coroutine typeCoroutine; // Ÿ������ �ڷ�ƾ ��ü�� �ٷ�� ����... ���ñ�
    

    float outDuration = 1f; // ���̵�ƿ� �ð�
    float inDuration = 0.5f; // ���̵��� �ð�
    float typingSpeed = 0.05f; // Ÿ���� ȿ���� �ð�
    float totalTime = 0f;
    int guestCnt = 0;

    public bool stopCo = false; // ������ �������� �� �������� ���� ����� �۵��ϵ�, ���� ����� �۵����� �ʵ��� �����ֱ� ���� �÷��׸� �������ش�


    int score = 0; // ����
    int c_point = 20; // Ŭ���� ���� ����
    
    

    // Start is called before the first frame update
    void Start()
    {
        disolveP = GameObject.Find("disolveP");


        heart = GameObject.Find("heartBox").GetComponent<HeartManager>();

        Buttons = GameObject.Find("Buttons").GetComponent<Transform>();
        resultBs = GameObject.Find("resultBs").GetComponent<Transform>();

        cdText = GameObject.Find("cdText").GetComponent<Text>();
        scText = GameObject.Find("Scoreboard").GetComponent<Text>();

        howTo = GameObject.Find("howTo");
        s_bubble = GameObject.Find("s_bubble");
        p_panel = GameObject.Find("pausePanel");
        p_button = GameObject.Find("pauseButton").GetComponent<Button>();

        r_panel = GameObject.Find("resultPanel");

        result_paper = GameObject.Find("neo_result");

        result_paper.SetActive(false);


        mainBGM = GetComponent<AudioSource>();

        p_panel.SetActive(false); // �������� ���� �Ͻ����� ȭ�� Deactive
        s_bubble.SetActive(false); // ó���� ��ǳ�� Deactive

        r_panel.SetActive(false);

        StartCoroutine(panel_Disolve());

        //popHowTo();    // ó���� ���� ���並 ����ش�
        
        StartCoroutine(RemoveAndSpawnPrefab()); 
        // ó���� �ڷ�ƾ�� ����. RemoveAndSpawnPrefab()�� ������ �������� ���� �� ����� ���ִ� �ڷ�ƾ ������, ���� ������ �������� ���ٸ� ������ �ǳʶٰ� �������� ���.
        // ���� ���´� ��ũ��Ʈ�� ����Ǳ� �����ϴ� �����̹Ƿ� �������� �������� �ʾƼ� �������� ����Ѵ�.
     
        
   
    }

    // Update is called once per frame
    void Update()
    {
        scText.text = this.score + " / " + this.c_point; // �׻� ���� ȭ���� ��ܿ� �ߵ��� Update���� ������
        
        if(!this.stopCo)
        {
            this.totalTime += Time.deltaTime;
        }
   
    }


    // ������� �ڷ�ƾ
    private IEnumerator RemoveAndSpawnPrefab() // �������� ������ ������ ����ϴ� �ڷ�ƾ
    {
        // �ϴ��� ������ ���� �������ش�. �׻� �����ϱ� ������ ������ �ؾ��ϱ� ����.
        // ������ ó������ ������ �� ���� ���� ������, if != null ������ �߰����ش�

        s_bubble.SetActive(false); // �������� �����Ǳ� ��, ��ǳ�� ���� Deactive ���ش�
            
        if(this.score >= c_point) // Ŭ���� ������ ���ų� �Ѱ��� ��
        {
            StartCoroutine(clearStamp()); // Clear ���� �ڷ�ƾ�� ȣ������
        } 
        


        if (currentPrefab != null) // �������� �����Ǳ� �� ���� ��Ÿ���� ���. �������� ���ٸ�(���� ���� ��)�� �۵����� �ʴ´�.
        {


            if (typeCoroutine != null) // �ؽ�Ʈ Ÿ������ �� �Ǳ� ���� ��ư�� ������, �� ���� ���� ������ �Է����̴�. �װ��� �����ϱ� ���� �ڵ�
            {
                StopCoroutine(this.typeCoroutine); // ��ü�� ����Ǿ��� �ڷ�ƾ�� ����
                this.typeCoroutine = null; // ������ ����ϱ� ���� ��ü�� �ٽ� ����ش�
            }
                
        }


        yield return new WaitForSeconds(1.2f); // ���� ���� �� ������ �ð����� ���


        if (currentPrefab != null) // ���̵�ƿ� �ϸ鼭 �����ǵ��� �ϴ� �κ�. �Ʒ����� Destroy�� �������, �� if�� ������ ���̵�ƿ� ���
        {

            Renderer renderer = currentPrefab.GetComponent<Renderer>(); // �������� renderer ������Ʈ�� �ҷ���
            if (renderer != null)
            {
                Color color = renderer.material.color;
                float elapsedTime = 0f;

                while (elapsedTime < outDuration)
                {
                    // ������ ��������
                    color.a = Mathf.Lerp(1, 0, elapsedTime / outDuration);
                    renderer.material.color = color;

                    elapsedTime += Time.deltaTime;
                    yield return null; // ���� �����ӱ��� ���
                }
            }
        }


        // ���� ������ ����
        Destroy(currentPrefab);
        cdText.text = "";


        if (!this.stopCo) // ������ �������� ���, �ڷ�ƾ �۵��ϵ� ������ ������ ���� �ʵ���
        {
            yield return new WaitForSeconds(0.8f);

            // ���� �ε��� ����
            int randomIndex = UnityEngine.Random.Range(0, prefabs.Length); // public���� ������ ������ �迭���� ������ �ε����� ����
            currentPrefab = Instantiate(prefabs[randomIndex]); // ������ �������� ����
            this.guestCnt++;                                                       


            // �������� ������ �� ���̵��� �ϸ鼭 �����������

            Renderer newRenderer = currentPrefab.GetComponent<Renderer>();
            if (newRenderer != null)
            {
                Color newColor = newRenderer.material.color;
                newColor.a = 0; // �ʱ� ���� ����
                newRenderer.material.color = newColor;

                float elapsedTime = 0f;

                // ������ ��Ÿ��
                while (elapsedTime < inDuration)
                {
                    newColor.a = Mathf.Lerp(0, 1, elapsedTime / inDuration);
                    newRenderer.material.color = newColor;

                    elapsedTime += Time.deltaTime;
                    yield return null; // ���� �����ӱ��� ���
                }
            }


            s_bubble.SetActive(true); // ��ǳ���� ��Ÿ������
            this.typeCoroutine = StartCoroutine(TypeDialogue(currentPrefab.GetComponent<PrefabBehavior>().showWord)); // ���ڰ� Ÿ���εǵ��� �ϴ� �ڷ�ƾ ȣ��

            // ��ư ������
            // ��ư�� ��ǥ�� ���⼭ �������ָ� �� �� �ϴ�.
            // LButton �� ����, RButton�� ������

            GameObject lb = Instantiate(LButton, Buttons); // Lbutton�� �����ϰ�, Buttons�� �ڽ� ������Ʈ�� ����
            GameObject rb = Instantiate(RButton, Buttons); // Rbutton�� �����ϰ�, Buttons�� �ڽ� ������Ʈ�� ����

        }
        

    }

    // ������� ĳ���� �������� ������ ������ ����ϴ� �ڷ�ƾ




    // �̰� �°� Ʋ���� �˷��ִ� ǥ�ø� ����ִ� �Լ�
    // 1�� ������ �ְ� �����ǵ��� �������
    public void signCheck(bool isSign)
    {
        if (isSign)
        {
            GameObject right = Instantiate(signs[0]);
            right.GetComponent<AudioSource>().volume = mainBGM.volume; // ���ǰ� ���� ������ �����ǵ��� ����
            Destroy(right, 1f);
            this.score++;
        }
        else if (!isSign)
        {
            GameObject wrong = Instantiate(signs[1]);
            wrong.GetComponent<AudioSource>().volume = mainBGM.volume;
            Destroy(wrong, 1f);
            heart.LoseLife();
        }

        
    }


    // �ؽ�Ʈ�� �ѱ��ھ� Ÿ���εǵ��� �ϴ� ���

    public IEnumerator TypeDialogue(string dialogue)
    {
        cdText.text = ""; // ���� �ѹ� �����

        foreach (char letter in dialogue.ToCharArray()) // �Ű����ڷ� ���� dialogue ��� �ؽ�Ʈ�� �ѱ��ھ� �迭�� �־���
        {
            cdText.text += letter; // �ؽ�Ʈ �迭�� �����鼭 cdText.text�� ä����
            yield return new WaitForSeconds(typingSpeed); // �����ص� Ÿ���� �ӵ���ŭ ���. ���ڸ� �Է��ϴ� �����̶�� �����ϸ� ��
        }
    }


    // ��ư ��� �Լ�
    // ������ �����ϰ� ������


    public void buttonListener(bool sickChecker)
    {
        Debug.Log("��ư�� ���Ȱ�, ���� ���� " + sickChecker + " �Դϴ�");

        cdText.text = ""; // ��ǳ���� ������ �����

        currentPrefab.GetComponent<PrefabBehavior>().isMove = true; // ĳ���� �������� �����̸鼭 ���������� �����̶�� �÷��׸� ����

        foreach (Transform child in Buttons) // ��ȣ�ۿ� ��ư���� �����ش�
        {
            Destroy(child.gameObject);
        }

        if (sickChecker == currentPrefab.GetComponent<PrefabBehavior>().isSick) // sickChecker�� isSick�� ���� ��, �� ������ ��
        {
            signCheck(true);
        }
        else // ���� �ʴٸ�
        {
            signCheck(false);
        }
   
        
        StartCoroutine(RemoveAndSpawnPrefab()); // ĳ���� �������� ����,���� �ڷ�ƾ�� ȣ������

    }


    private IEnumerator clearStamp() // �ణ ������ȿ�� ������... ū ���¿��� �۾�����
    {

        this.stopCo = true; // Ŭ���� ������ ĳ���� ������ ���� �ڷ�ƾ�� �ߴ� �÷��׸� ������

        yield return new WaitForSeconds(3f);

        p_button.interactable = false; // �Ͻ����� ��ư�� �۵����� �ʵ���

        float elapsedTime = 0f;
        float scaleDuration = 0.32f;
        float scaleMutiplier = 3.5f;
        Vector3 finalScale = new Vector3(1f, 1f, 1f);
        
        GameObject stamp = Instantiate(stampPrefab);
        stamp.transform.localScale = finalScale * scaleMutiplier;

        

        while (elapsedTime < scaleDuration)
        {
            float scale = Mathf.Lerp(finalScale.x * scaleMutiplier, finalScale.x, elapsedTime / scaleDuration);
            stamp.transform.localScale = new Vector3(scale, scale, 1);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        stamp.transform.localScale = finalScale;


        yield return new WaitForSeconds(1.5f);

        StartCoroutine(popResult());
 
    }


    private IEnumerator panel_Disolve()
    {
        p_button.interactable = false; // �Ͻ����� ��ư�� ������ �ʵ���

        Color color = disolveP.GetComponent<Image>().color;
        float elapsedTime = 0f;

        while (elapsedTime < 1.5f)
        {
            // ������ ��������
            color.a = Mathf.Lerp(1, 0, elapsedTime / 1.5f);
            disolveP.GetComponent<Image>().color = color;

            elapsedTime += Time.deltaTime;
            yield return null; // ���� �����ӱ��� ���
        }

        disolveP.SetActive(false);

        popHowTo();

    }



    private void popHowTo() // ���Ӱ��� ȭ�� ���� �Լ�
    {
        howTo.SetActive(true); // �г��� Active
        
        Time.timeScale = 0; // ���� �ð��� ����д�.
    }


    private IEnumerator popResult()
    {

        Debug.Log("���â�� ���ð̴ϴ�!");
        r_panel.SetActive(true);
        result_paper.SetActive(true);

        DateTime today = DateTime.Today;

        yield return new WaitForSeconds(0.8f);
        
        Text r_text = GameObject.Find("resultTime").GetComponent<Text>();
        r_text.text = this.totalTime.ToString("F1") + "s";

        yield return new WaitForSeconds(0.8f);

        Text g_text = GameObject.Find("resultGuest").GetComponent<Text>();
        g_text.text = "�� " + this.guestCnt + " ��";

        yield return new WaitForSeconds(0.8f);
        Text d_text = GameObject.Find("resultDay").GetComponent<Text>();
        d_text.text = today.ToString("yy.MM.dd");

        yield return new WaitForSeconds(1.2f);

        StartCoroutine(heart.niceScore());

        yield return new WaitForSeconds(1.5f);

        GameObject reB = Instantiate(result_Rb, resultBs);
        GameObject qB = Instantiate(result_Qb, resultBs);
    }


    

}

