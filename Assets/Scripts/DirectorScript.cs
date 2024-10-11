using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;


public class DirectorScript : MonoBehaviour
{
    public GameObject[] prefabs; // 캐릭터 프리팹 배열
    public GameObject[] signs; // 맞고 틀림을 판단하는 프리팹 배열
    public GameObject stampPrefab; // 클리어 했을 때 나타낼 이미지
    

    private GameObject currentPrefab; // 현재 생성된 캐릭터 프리팹

    HeartManager heart; // 목숨이 깎이는 함수를 쓰기 위해서
    AudioSource mainBGM; // 배경음악


    public GameObject result_Rb;
    public GameObject result_Qb;

    // 상호작용 버튼. 아픈지 아닌지 판단하기 위한 버튼들이 들어간다
    public GameObject LButton; 
    public GameObject RButton;

    GameObject howTo; // 게임 개요 화면
    GameObject s_bubble; // 말풍선
    GameObject p_panel; // 일시정지 화면
    GameObject r_panel;
    GameObject result_paper;


    Button p_button; // 일시정지 겸 메뉴 버튼

    Transform Buttons; // 게임 메인 버튼들(아픈지 아닌지 상호작용) 의 부모를 불러와서 캐릭터 프리팹과 함께 Destroy 하기 위해
    Transform resultBs;

    GameObject disolveP;

    Text cdText; // 문구를 나타내기 위함
    Text scText; // 점수판


    private Coroutine typeCoroutine; // 타이핑쪽 코루틴 객체를 다루기 위한... 뭐시기
    

    float outDuration = 1f; // 페이드아웃 시간
    float inDuration = 0.5f; // 페이드인 시간
    float typingSpeed = 0.05f; // 타이핑 효과의 시간
    float totalTime = 0f;
    int guestCnt = 0;

    public bool stopCo = false; // 조건을 만족했을 때 프리팹의 삭제 기능은 작동하되, 생성 기능은 작동하지 않도록 막아주기 위해 플래그를 설정해준다


    int score = 0; // 점수
    int c_point = 20; // 클리어 기준 점수
    
    

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

        p_panel.SetActive(false); // 시작했을 때는 일시정지 화면 Deactive
        s_bubble.SetActive(false); // 처음엔 말풍선 Deactive

        r_panel.SetActive(false);

        StartCoroutine(panel_Disolve());

        //popHowTo();    // 처음에 게임 개요를 띄워준다
        
        StartCoroutine(RemoveAndSpawnPrefab()); 
        // 처음에 코루틴을 시작. RemoveAndSpawnPrefab()은 생성된 프리팹을 삭제 후 재생성 해주는 코루틴 이지만, 현재 생성된 프리팹이 없다면 삭제를 건너뛰고 생성만을 담당.
        // 현재 상태는 스크립트가 실행되기 시작하는 상태이므로 프리팹이 존재하지 않아서 생성만을 담당한다.
     
        
   
    }

    // Update is called once per frame
    void Update()
    {
        scText.text = this.score + " / " + this.c_point; // 항상 점수 화면이 상단에 뜨도록 Update문에 적용함
        
        if(!this.stopCo)
        {
            this.totalTime += Time.deltaTime;
        }
   
    }


    // 여기부터 코루틴
    private IEnumerator RemoveAndSpawnPrefab() // 프리팹의 생성과 삭제를 담당하는 코루틴
    {
        // 일단은 삭제를 먼저 시작해준다. 항상 생성하기 전에는 삭제를 해야하기 때문.
        // 하지만 처음에는 삭제를 할 것이 없기 때문에, if != null 조건을 추가해준다

        s_bubble.SetActive(false); // 프리팹이 삭제되기 전, 말풍선 또한 Deactive 해준다
            
        if(this.score >= c_point) // 클리어 점수와 같거나 넘겼을 때
        {
            StartCoroutine(clearStamp()); // Clear 관련 코루틴을 호출해줌
        } 
        


        if (currentPrefab != null) // 프리팹이 삭제되기 전 문구 나타내기 기능. 프리팹이 없다면(최초 생성 시)는 작동하지 않는다.
        {


            if (typeCoroutine != null) // 텍스트 타이핑이 다 되기 전에 버튼을 누르면, 그 전의 값이 여전히 입력중이다. 그것을 방지하기 위한 코드
            {
                StopCoroutine(this.typeCoroutine); // 객체에 저장되었던 코루틴을 멈춤
                this.typeCoroutine = null; // 다음에 사용하기 위해 객체를 다시 비워준다
            }
                
        }


        yield return new WaitForSeconds(1.2f); // 다음 동작 전 지정한 시간동안 대기


        if (currentPrefab != null) // 페이드아웃 하면서 삭제되도록 하는 부분. 아래쪽의 Destroy가 삭제기능, 이 if문 안쪽은 페이드아웃 기능
        {

            Renderer renderer = currentPrefab.GetComponent<Renderer>(); // 프리팹의 renderer 컴포넌트를 불러옴
            if (renderer != null)
            {
                Color color = renderer.material.color;
                float elapsedTime = 0f;

                while (elapsedTime < outDuration)
                {
                    // 서서히 투명해짐
                    color.a = Mathf.Lerp(1, 0, elapsedTime / outDuration);
                    renderer.material.color = color;

                    elapsedTime += Time.deltaTime;
                    yield return null; // 다음 프레임까지 대기
                }
            }
        }


        // 현재 프리팹 삭제
        Destroy(currentPrefab);
        cdText.text = "";


        if (!this.stopCo) // 조건을 만족했을 경우, 코루틴 작동하되 프리팹 생성은 되지 않도록
        {
            yield return new WaitForSeconds(0.8f);

            // 랜덤 인덱스 생성
            int randomIndex = UnityEngine.Random.Range(0, prefabs.Length); // public으로 설정한 프리팹 배열에서 랜덤한 인덱스를 지정
            currentPrefab = Instantiate(prefabs[randomIndex]); // 랜덤한 프리팹을 생성
            this.guestCnt++;                                                       


            // 프리팹이 생성될 때 페이드인 하면서 만들어지도록

            Renderer newRenderer = currentPrefab.GetComponent<Renderer>();
            if (newRenderer != null)
            {
                Color newColor = newRenderer.material.color;
                newColor.a = 0; // 초기 투명도 설정
                newRenderer.material.color = newColor;

                float elapsedTime = 0f;

                // 서서히 나타남
                while (elapsedTime < inDuration)
                {
                    newColor.a = Mathf.Lerp(0, 1, elapsedTime / inDuration);
                    newRenderer.material.color = newColor;

                    elapsedTime += Time.deltaTime;
                    yield return null; // 다음 프레임까지 대기
                }
            }


            s_bubble.SetActive(true); // 말풍선이 나타나도록
            this.typeCoroutine = StartCoroutine(TypeDialogue(currentPrefab.GetComponent<PrefabBehavior>().showWord)); // 글자가 타이핑되듯이 하는 코루틴 호출

            // 버튼 생성부
            // 버튼의 좌표를 여기서 결정해주면 될 듯 하다.
            // LButton 은 아픔, RButton은 멀쩡함

            GameObject lb = Instantiate(LButton, Buttons); // Lbutton을 생성하고, Buttons의 자식 오브젝트로 생성
            GameObject rb = Instantiate(RButton, Buttons); // Rbutton을 생성하고, Buttons의 자식 오브젝트로 생성

        }
        

    }

    // 여기까지 캐릭터 프리팹의 삭제와 생성을 담당하는 코루틴




    // 이건 맞고 틀림을 알려주는 표시를 띄워주는 함수
    // 1초 유예를 주고 삭제되도록 만들었음
    public void signCheck(bool isSign)
    {
        if (isSign)
        {
            GameObject right = Instantiate(signs[0]);
            right.GetComponent<AudioSource>().volume = mainBGM.volume; // 음악과 같이 볼륨이 조정되도록 설정
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


    // 텍스트를 한글자씩 타이핑되듯이 하는 기능

    public IEnumerator TypeDialogue(string dialogue)
    {
        cdText.text = ""; // 먼저 한번 비워줌

        foreach (char letter in dialogue.ToCharArray()) // 매개인자로 받은 dialogue 라는 텍스트를 한글자씩 배열에 넣어줌
        {
            cdText.text += letter; // 텍스트 배열을 읽으면서 cdText.text에 채워줌
            yield return new WaitForSeconds(typingSpeed); // 지정해둔 타이핑 속도만큼 대기. 글자를 입력하는 간격이라고 생각하면 됨
        }
    }


    // 버튼 기능 함수
    // 로직을 간단하게 수정함


    public void buttonListener(bool sickChecker)
    {
        Debug.Log("버튼이 눌렸고, 받은 값은 " + sickChecker + " 입니다");

        cdText.text = ""; // 말풍선의 문구를 비워줌

        currentPrefab.GetComponent<PrefabBehavior>().isMove = true; // 캐릭터 프리팹이 움직이면서 없어지도록 움직이라는 플래그를 전달

        foreach (Transform child in Buttons) // 상호작용 버튼들을 지워준다
        {
            Destroy(child.gameObject);
        }

        if (sickChecker == currentPrefab.GetComponent<PrefabBehavior>().isSick) // sickChecker와 isSick이 같을 때, 즉 맞췄을 때
        {
            signCheck(true);
        }
        else // 같지 않다면
        {
            signCheck(false);
        }
   
        
        StartCoroutine(RemoveAndSpawnPrefab()); // 캐릭터 프리팹의 삭제,생성 코루틴을 호출해줌

    }


    private IEnumerator clearStamp() // 약간 스탬프효과 내듯이... 큰 상태에서 작아지기
    {

        this.stopCo = true; // 클리어 했으니 캐릭터 프리팹 생성 코루틴에 중단 플래그를 전달함

        yield return new WaitForSeconds(3f);

        p_button.interactable = false; // 일시정지 버튼이 작동하지 않도록

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
        p_button.interactable = false; // 일시정지 버튼이 눌리지 않도록

        Color color = disolveP.GetComponent<Image>().color;
        float elapsedTime = 0f;

        while (elapsedTime < 1.5f)
        {
            // 서서히 투명해짐
            color.a = Mathf.Lerp(1, 0, elapsedTime / 1.5f);
            disolveP.GetComponent<Image>().color = color;

            elapsedTime += Time.deltaTime;
            yield return null; // 다음 프레임까지 대기
        }

        disolveP.SetActive(false);

        popHowTo();

    }



    private void popHowTo() // 게임개요 화면 띄우는 함수
    {
        howTo.SetActive(true); // 패널을 Active
        
        Time.timeScale = 0; // 게임 시간을 멈춰둔다.
    }


    private IEnumerator popResult()
    {

        Debug.Log("결과창이 나올겁니다!");
        r_panel.SetActive(true);
        result_paper.SetActive(true);

        DateTime today = DateTime.Today;

        yield return new WaitForSeconds(0.8f);
        
        Text r_text = GameObject.Find("resultTime").GetComponent<Text>();
        r_text.text = this.totalTime.ToString("F1") + "s";

        yield return new WaitForSeconds(0.8f);

        Text g_text = GameObject.Find("resultGuest").GetComponent<Text>();
        g_text.text = "총 " + this.guestCnt + " 명";

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

