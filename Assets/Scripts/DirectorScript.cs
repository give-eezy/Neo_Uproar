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

    public GameObject LButton;
    public GameObject RButton;

    Transform Buttons;


    Text cdText; // ������ ��Ÿ���� ����
    Text scText; // ������
    

    bool isInput = false;
    

    float fadeDuration = 1f; // ���̵�ƿ� �ð�
    int score = 0;
   

    // Start is called before the first frame update
    void Start()
    {
        Buttons = GameObject.Find("Buttons").GetComponent<Transform>();
        cdText = GameObject.Find("cdText").GetComponent<Text>();
        scText = GameObject.Find("Scoreboard").GetComponent<Text>();
        StartCoroutine(RemoveAndSpawnPrefab()); // ó���� �ڷ�ƾ�� ����. ó���� �������� �����ǵ��� �ϴ� ����� ���Ѵ�
    }

    // Update is called once per frame
    void Update()
    {
        scText.text = "���� ���� : " + this.score;
        
        if(!isInput)
        {
            GameInput(); // ��� ��ǲ�� ����ϴ� �ڵ带 �Լ��� �ۼ��� 
        }       

        
    }

    public void GameInput()
    {
        if(Input.GetKeyDown(KeyCode.LeftArrow) && currentPrefab != null) // ���ö�
        {

            currentPrefab.GetComponent<PrefabBehavior>().checkDir = "left";

            this.isInput = true; // �����Է��� ������
            checkingLogic();

            //signCheck(checkNormal(currentPrefab)); // �������� �Ǻ��ϱ� ���� �Լ��� ���� ������ �Ǻ��ϴ� �Լ��� �־���. �����Ǻ� �Լ��� �������� ���� signCheck �Լ��� ������ �޶���
            
            //StartCoroutine(RemoveAndSpawnPrefab()); // ������ �Ϸ������� �ڷ�ƾ�� �ٽ� ȣ��, �������� ���ְ� �ٽ� �������ش�

        }

        if(Input.GetKeyDown(KeyCode.RightArrow) && currentPrefab != null) // 1,2,3 �� ���� ������
        {

            currentPrefab.GetComponent<PrefabBehavior>().checkDir = "right";

            this.isInput = true; // �����Է��� ������
            checkingLogic();
        }
    }

    public bool checkNormal(GameObject c_prefab) // ���� �������� �Ű������� �ް�, �� ���뿡 ���� �������� ����ϴ� �Լ�
    {

        // isProperty �� ���϶��� ���°�
        // ������ ���� �����Ѱ�
        // ������ �Ẹ��
        // ó�� ������ �� -> isProperty�� �������� ������(������ �Ⱦ�����)
        // ������ ��������, �Ⱦ����� ���������� �����°� �´°�
        // ���࿡ isProperty�� ���ε� ���������� ������, ������ ����
        // �׷��ٸ� ������ �޾Ҵ���, �������� �޾Ҵ����� ���Ǻο� �־�θ� �Լ� �ϳ��� ��ĥ �� �ֳ�?


        string lrChecker = c_prefab.GetComponent<PrefabBehavior>().checkDir;

        

        if (c_prefab.GetComponent<PrefabBehavior>().isProperty) // ���� ��, ���°� �´� ���
        {
            if (lrChecker == "left") // ������ ���ȴ��� Ȯ���ϰ�
            {
                return true; // ������ true
            }
            else return false;
        }
        else // ������ ��, �����Ѱ�
        {
            if (lrChecker == "right")
            {
                return true;
            }
            else return false;
        }
           
    }

   
    // ������� �ڷ�ƾ
    private IEnumerator RemoveAndSpawnPrefab() // �������� ������ ������ ����ϴ� �ڷ�ƾ
    {

        //yield return new WaitForSeconds(0.2f); // ���� ���� �� 0.2�� ���

        cdText.text = "";

        if (currentPrefab != null)
        {
            Renderer renderer = currentPrefab.GetComponent<Renderer>(); // �������� renderer ������Ʈ�� �ҷ���
            if (renderer != null)
            {
                Color color = renderer.material.color;
                float elapsedTime = 0f;

                while (elapsedTime < fadeDuration)
                {
                    // ������ ��������
                    color.a = Mathf.Lerp(1, 0, elapsedTime / fadeDuration);
                    renderer.material.color = color;

                    elapsedTime += Time.deltaTime;
                    yield return null; // ���� �����ӱ��� ���
                }
            }
        }
        

        // ���� ������ ����
        Destroy(currentPrefab);

        // 1�� ���
        yield return new WaitForSeconds(0.8f);

        // ���� �ε��� ����
        int randomIndex = Random.Range(0, prefabs.Length);
        currentPrefab = Instantiate(prefabs[randomIndex]); // ������ �������� ����

        
        // �������� ������ �� ���̵��� �ϸ鼭 �����������

        Renderer newRenderer = currentPrefab.GetComponent<Renderer>();
        if (newRenderer != null)
        {
            Color newColor = newRenderer.material.color;
            newColor.a = 0; // �ʱ� ���� ����
            newRenderer.material.color = newColor;

            float elapsedTime = 0f;

            // ������ ��Ÿ��
            while (elapsedTime < fadeDuration)
            {
                newColor.a = Mathf.Lerp(0, 1, elapsedTime / fadeDuration);
                newRenderer.material.color = newColor;

                elapsedTime += Time.deltaTime;
                yield return null; // ���� �����ӱ��� ���
            }
        }


        
        cdText.text = currentPrefab.GetComponent<PrefabBehavior>().showWord; // ���� �������� condition ������ UI�� ���
        Debug.Log(currentPrefab.GetComponent<PrefabBehavior>().showWord);

        GameObject lb = Instantiate(LButton, Buttons);
        GameObject rb = Instantiate(RButton, Buttons);
        
        this.isInput = false; // �������� �����Ǿ�����, �ٽ� �Է��� �޴� ���·� �������
        
    
    
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
            
        }

        
    }


    // ��ư ��� �Լ�
    // �ٵ� �̰� ��ư ������ ������ ����� ������ ����
    
    
    public void checkingLogic()
    {
        signCheck(checkNormal(currentPrefab));

        StartCoroutine(RemoveAndSpawnPrefab());

        foreach (Transform child in Buttons)
        {
            Destroy(child.gameObject);
        }
    }

    public void checkingLogic(string dir)
    {
        currentPrefab.GameObject().GetComponent<PrefabBehavior>().checkDir = dir;


        signCheck(checkNormal(currentPrefab));

        StartCoroutine(RemoveAndSpawnPrefab());

        foreach (Transform child in Buttons)
        {
            Destroy(child.gameObject);
        }
    }
   


}

