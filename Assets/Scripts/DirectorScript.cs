using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class DirectorScript : MonoBehaviour
{
    public GameObject[] prefabs; // ĳ���� ������ �迭
    public GameObject[] signs; // �°� Ʋ���� �Ǵ��ϴ� ������ �迭
    private GameObject currentPrefab; // ���� ������ ĳ���� ������

    HeartManager heart;
    AudioSource mainBGM;


    public GameObject LButton;
    public GameObject RButton;

    GameObject howTo;

    Transform Buttons;


    Text cdText; // ������ ��Ÿ���� ����
    Text scText; // ������


    private Coroutine typeCoroutine; // Ÿ������ �ڷ�ƾ ��ü�� �ٷ�� ����... ���ñ�
    

    float outDuration = 1f; // ���̵�ƿ� �ð�
    float inDuration = 0.5f;
    float typingSpeed = 0.05f;

    public bool stopCo = false;


    int score = 0; // ����
    
    

    // Start is called before the first frame update
    void Start()
    {
        heart = GameObject.Find("heartBox").GetComponent<HeartManager>();

        Buttons = GameObject.Find("Buttons").GetComponent<Transform>();

        cdText = GameObject.Find("cdText").GetComponent<Text>();
        scText = GameObject.Find("Scoreboard").GetComponent<Text>();

        howTo = GameObject.Find("howTo");

        mainBGM = GetComponent<AudioSource>();

        

        popHowTo();    
        
        StartCoroutine(RemoveAndSpawnPrefab()); 
        // ó���� �ڷ�ƾ�� ����. RemoveAndSpawnPrefab()�� ������ �������� ���� �� ����� ���ִ� �ڷ�ƾ ������, ���� ������ �������� ���ٸ� ������ �ǳʶٰ� �������� ���.
        // ���� ���´� ��ũ��Ʈ�� ����Ǳ� �����ϴ� �����̹Ƿ� �������� �������� �ʾƼ� �������� ����Ѵ�.
     
        
   
    }

    // Update is called once per frame
    void Update()
    {
        scText.text = "���� ���� : " + this.score;
        
        if(Input.GetKeyDown(KeyCode.Space)) 
        {
            Time.timeScale = 0;
        }
        if(Input.GetKeyDown(KeyCode.Return))
        {
            Time.timeScale = 1;
        }
    }


    // ������� �ڷ�ƾ
    private IEnumerator RemoveAndSpawnPrefab() // �������� ������ ������ ����ϴ� �ڷ�ƾ
    {
        

            if (currentPrefab != null) // �������� �����Ǳ� �� ���� ��Ÿ���� ���. �������� ���ٸ�(���� ���� ��)�� �۵����� �ʴ´�.
            {


                if (typeCoroutine != null) // �ؽ�Ʈ Ÿ������ �� �Ǳ� ���� ��ư�� ������, �� ���� ���� ������ �Է����̴�. �װ��� �����ϱ� ���� �ڵ�
                {
                    StopCoroutine(this.typeCoroutine); // ��ü�� ����Ǿ��� �ڷ�ƾ�� ����
                    this.typeCoroutine = null;
                }
            //if (currentPrefab.CompareTag("Fake"))
            //    {
            //        StartCoroutine(TypeDialogue(currentPrefab.GetComponent<PrefabBehavior>().fakeWord));
            //    }
            //    else
            //    {
            //        if(typeCoroutine != null) // �ؽ�Ʈ Ÿ������ �� �Ǳ� ���� ��ư�� ������, �� ���� ���� ������ �Է����̴�. �װ��� �����ϱ� ���� �ڵ�
            //        {
            //            StopCoroutine(this.typeCoroutine); // ��ü�� ����Ǿ��� �ڷ�ƾ�� ����
            //            this.typeCoroutine = null;
            //        }
            //        StartCoroutine(TypeDialogue(currentPrefab.GetComponent<PrefabBehavior>().showWord));
            //    }
            }


            yield return new WaitForSeconds(1.2f); // ���� ���� �� 1�� ���





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


            if(!this.stopCo) // ������ �������� ���, �ڷ�ƾ �۵��ϵ� ������ ������ ���� �ʵ���
            {
                 yield return new WaitForSeconds(0.8f);

                 // ���� �ε��� ����
                int randomIndex = Random.Range(0, prefabs.Length);
                currentPrefab = Instantiate(prefabs[randomIndex]); // ������ �������� ����
                // �������� ��ġ ��ǥ�� �����Ϸ��� ���⼭ �ǵ帮�� �� ��


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



            this.typeCoroutine = StartCoroutine(TypeDialogue(currentPrefab.GetComponent<PrefabBehavior>().showWord)); // ���ڰ� Ÿ���εǵ���



            // ��ư ������
            // ��ư�� ��ǥ�� ���⼭ �������ָ� �� �� �ϴ�.
            // LButton �� ����, RButton�� ������

            GameObject lb = Instantiate(LButton, Buttons); // Lbutton�� �����ϰ�, Buttons�� �ڽ� ������Ʈ�� ����
            GameObject rb = Instantiate(RButton, Buttons); // Rbutton�� �����ϰ�, Buttons�� �ڽ� ������Ʈ�� ����




            }

     
    }

    // ������� �ڷ�ƾ




    // �̰� �°� Ʋ���� �˷��ִ� ǥ�ø� ����ִ� �Լ�
    // 1�� ������ �ְ� �����ǵ��� �������
    public void signCheck(bool isSign)
    {
        if (isSign)
        {
            GameObject right = Instantiate(signs[0]);
            Destroy(right, 1f);
            this.score++;
        }
        else if (!isSign)
        {
            GameObject wrong = Instantiate(signs[1]);
            Destroy(wrong, 1f);
            heart.LoseLife();
        }

        
    }


    // �ؽ�Ʈ�� �ѱ��ھ� Ÿ���εǵ��� �ϴ� ���

    public IEnumerator TypeDialogue(string dialogue)
    {
        cdText.text = ""; // ���� �ѹ� �����

        foreach (char letter in dialogue.ToCharArray())
        {
            cdText.text += letter; // �� ���ھ� �߰�
            yield return new WaitForSeconds(typingSpeed); // Ÿ���� �ӵ� ��ŭ ���
        }
    }

    // ��ư ��� �Լ�
    // ������ �����ϰ� ������


    public void buttonListener(bool sickChecker)
    {
        Debug.Log("��ư�� ���Ȱ�, ���� ���� " + sickChecker + " �Դϴ�");

        currentPrefab.GetComponent<PrefabBehavior>().isMove = true;

        foreach (Transform child in Buttons)
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


        
        
        StartCoroutine(RemoveAndSpawnPrefab());
        
        

    }

    private void popHowTo()
    {
        howTo.SetActive(true);
        Time.timeScale = 0;
    }


    

}

