using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class DirectorScript : MonoBehaviour
{
    public GameObject[] prefabs; // 캐릭터 프리팹 배열
    public GameObject[] signs; // 맞고 틀림을 판단하는 프리팹 배열
    private GameObject currentPrefab; // 현재 생성된 캐릭터 프리팹

    public GameObject LButton;
    public GameObject RButton;

    Transform Buttons;


    Text cdText; // 문구를 나타내기 위함
    Text scText; // 점수판
    

    bool isInput = false;
    

    float fadeDuration = 1f; // 페이드아웃 시간
    int score = 0;
   

    // Start is called before the first frame update
    void Start()
    {
        Buttons = GameObject.Find("Buttons").GetComponent<Transform>();
        cdText = GameObject.Find("cdText").GetComponent<Text>();
        scText = GameObject.Find("Scoreboard").GetComponent<Text>();
        StartCoroutine(RemoveAndSpawnPrefab()); // 처음에 코루틴을 시작. 처음에 프리팹이 생성되도록 하는 기능을 겸한다
    }

    // Update is called once per frame
    void Update()
    {
        scText.text = "현재 점수 : " + this.score;
        
        if(!isInput)
        {
            GameInput(); // 모든 인풋을 담당하는 코드를 함수로 작성함 
        }       

        
    }

    public void GameInput()
    {
        if(Input.GetKeyDown(KeyCode.LeftArrow) && currentPrefab != null) // 아플때
        {

            currentPrefab.GetComponent<PrefabBehavior>().checkDir = "left";

            this.isInput = true; // 다중입력을 막아줌
            checkingLogic();

            //signCheck(checkNormal(currentPrefab)); // 참거짓을 판별하기 위한 함수에 숫자 조건을 판별하는 함수를 넣어줌. 조건판별 함수의 참거짓에 따라 signCheck 함수의 동작이 달라짐
            
            //StartCoroutine(RemoveAndSpawnPrefab()); // 동작을 완료했으니 코루틴을 다시 호출, 프리팹을 없애고 다시 생성해준다

        }

        if(Input.GetKeyDown(KeyCode.RightArrow) && currentPrefab != null) // 1,2,3 일 때는 오른쪽
        {

            currentPrefab.GetComponent<PrefabBehavior>().checkDir = "right";

            this.isInput = true; // 다중입력을 막아줌
            checkingLogic();
        }
    }

    public bool checkNormal(GameObject c_prefab) // 현재 프리팹을 매개변수로 받고, 그 내용에 따라 참거짓을 출력하는 함수
    {

        // isProperty 가 참일때는 아픈거
        // 거짓일 때는 멀쩡한거
        // 순서를 써보기
        // 처음 생성될 때 -> isProperty의 참거짓이 정해짐(아픈지 안아픈지)
        // 아프면 왼쪽으로, 안아프면 오른쪽으로 보내는게 맞는거
        // 만약에 isProperty가 참인데 오른쪽으로 보내면, 문제가 있음
        // 그렇다면 왼쪽을 받았는지, 오른쪽을 받았는지를 조건부에 넣어두면 함수 하나로 퉁칠 수 있나?


        string lrChecker = c_prefab.GetComponent<PrefabBehavior>().checkDir;

        

        if (c_prefab.GetComponent<PrefabBehavior>().isProperty) // 참일 때, 아픈게 맞는 경우
        {
            if (lrChecker == "left") // 왼쪽이 눌렸는지 확인하고
            {
                return true; // 맞으면 true
            }
            else return false;
        }
        else // 거짓일 때, 멀쩡한거
        {
            if (lrChecker == "right")
            {
                return true;
            }
            else return false;
        }
           
    }

   
    // 여기부터 코루틴
    private IEnumerator RemoveAndSpawnPrefab() // 프리팹의 생성과 삭제를 담당하는 코루틴
    {

        //yield return new WaitForSeconds(0.2f); // 다음 동작 전 0.2초 대기

        cdText.text = "";

        if (currentPrefab != null)
        {
            Renderer renderer = currentPrefab.GetComponent<Renderer>(); // 프리팹의 renderer 컴포넌트를 불러옴
            if (renderer != null)
            {
                Color color = renderer.material.color;
                float elapsedTime = 0f;

                while (elapsedTime < fadeDuration)
                {
                    // 서서히 투명해짐
                    color.a = Mathf.Lerp(1, 0, elapsedTime / fadeDuration);
                    renderer.material.color = color;

                    elapsedTime += Time.deltaTime;
                    yield return null; // 다음 프레임까지 대기
                }
            }
        }
        

        // 현재 프리팹 삭제
        Destroy(currentPrefab);

        // 1초 대기
        yield return new WaitForSeconds(0.8f);

        // 랜덤 인덱스 생성
        int randomIndex = Random.Range(0, prefabs.Length);
        currentPrefab = Instantiate(prefabs[randomIndex]); // 랜덤한 프리팹을 생성

        
        // 프리팹이 생성될 때 페이드인 하면서 만들어지도록

        Renderer newRenderer = currentPrefab.GetComponent<Renderer>();
        if (newRenderer != null)
        {
            Color newColor = newRenderer.material.color;
            newColor.a = 0; // 초기 투명도 설정
            newRenderer.material.color = newColor;

            float elapsedTime = 0f;

            // 서서히 나타남
            while (elapsedTime < fadeDuration)
            {
                newColor.a = Mathf.Lerp(0, 1, elapsedTime / fadeDuration);
                newRenderer.material.color = newColor;

                elapsedTime += Time.deltaTime;
                yield return null; // 다음 프레임까지 대기
            }
        }


        
        cdText.text = currentPrefab.GetComponent<PrefabBehavior>().showWord; // 현재 프리팹의 condition 변수를 UI에 출력
        Debug.Log(currentPrefab.GetComponent<PrefabBehavior>().showWord);

        GameObject lb = Instantiate(LButton, Buttons);
        GameObject rb = Instantiate(RButton, Buttons);
        
        this.isInput = false; // 프리팹이 생성되었으니, 다시 입력을 받는 상태로 만들어줌
        
    
    
    }

    // 여기까지 코루틴




    // 이건 맞고 틀림을 알려주는 표시를 띄워주는 함수
    // 1초 유예를 주고 삭제되도록 만들었음
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


    // 버튼 기능 함수
    // 근데 이건 버튼 프리팹 쪽으로 기능을 뺄수도 있음
    
    
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

