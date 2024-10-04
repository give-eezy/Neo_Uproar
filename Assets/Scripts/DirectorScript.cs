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

    HeartManager heart;
    AudioSource mainBGM;


    public GameObject LButton;
    public GameObject RButton;

    GameObject howTo;

    Transform Buttons;


    Text cdText; // 문구를 나타내기 위함
    Text scText; // 점수판


    private Coroutine typeCoroutine; // 타이핑쪽 코루틴 객체를 다루기 위한... 뭐시기
    

    float outDuration = 1f; // 페이드아웃 시간
    float inDuration = 0.5f;
    float typingSpeed = 0.05f;

    public bool stopCo = false;


    int score = 0; // 점수
    
    

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
        // 처음에 코루틴을 시작. RemoveAndSpawnPrefab()은 생성된 프리팹을 삭제 후 재생성 해주는 코루틴 이지만, 현재 생성된 프리팹이 없다면 삭제를 건너뛰고 생성만을 담당.
        // 현재 상태는 스크립트가 실행되기 시작하는 상태이므로 프리팹이 존재하지 않아서 생성만을 담당한다.
     
        
   
    }

    // Update is called once per frame
    void Update()
    {
        scText.text = "현재 점수 : " + this.score;
        
        if(Input.GetKeyDown(KeyCode.Space)) 
        {
            Time.timeScale = 0;
        }
        if(Input.GetKeyDown(KeyCode.Return))
        {
            Time.timeScale = 1;
        }
    }


    // 여기부터 코루틴
    private IEnumerator RemoveAndSpawnPrefab() // 프리팹의 생성과 삭제를 담당하는 코루틴
    {
        

            if (currentPrefab != null) // 프리팹이 삭제되기 전 문구 나타내기 기능. 프리팹이 없다면(최초 생성 시)는 작동하지 않는다.
            {


                if (typeCoroutine != null) // 텍스트 타이핑이 다 되기 전에 버튼을 누르면, 그 전의 값이 여전히 입력중이다. 그것을 방지하기 위한 코드
                {
                    StopCoroutine(this.typeCoroutine); // 객체에 저장되었던 코루틴을 멈춤
                    this.typeCoroutine = null;
                }
            //if (currentPrefab.CompareTag("Fake"))
            //    {
            //        StartCoroutine(TypeDialogue(currentPrefab.GetComponent<PrefabBehavior>().fakeWord));
            //    }
            //    else
            //    {
            //        if(typeCoroutine != null) // 텍스트 타이핑이 다 되기 전에 버튼을 누르면, 그 전의 값이 여전히 입력중이다. 그것을 방지하기 위한 코드
            //        {
            //            StopCoroutine(this.typeCoroutine); // 객체에 저장되었던 코루틴을 멈춤
            //            this.typeCoroutine = null;
            //        }
            //        StartCoroutine(TypeDialogue(currentPrefab.GetComponent<PrefabBehavior>().showWord));
            //    }
            }


            yield return new WaitForSeconds(1.2f); // 다음 동작 전 1초 대기





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


            if(!this.stopCo) // 조건을 만족했을 경우, 코루틴 작동하되 프리팹 생성은 되지 않도록
            {
                 yield return new WaitForSeconds(0.8f);

                 // 랜덤 인덱스 생성
                int randomIndex = Random.Range(0, prefabs.Length);
                currentPrefab = Instantiate(prefabs[randomIndex]); // 랜덤한 프리팹을 생성
                // 프리팹의 위치 좌표를 설정하려면 여기서 건드리면 될 듯


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



            this.typeCoroutine = StartCoroutine(TypeDialogue(currentPrefab.GetComponent<PrefabBehavior>().showWord)); // 글자가 타이핑되듯이



            // 버튼 생성부
            // 버튼의 좌표를 여기서 결정해주면 될 듯 하다.
            // LButton 은 아픔, RButton은 멀쩡함

            GameObject lb = Instantiate(LButton, Buttons); // Lbutton을 생성하고, Buttons의 자식 오브젝트로 생성
            GameObject rb = Instantiate(RButton, Buttons); // Rbutton을 생성하고, Buttons의 자식 오브젝트로 생성




            }

     
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
            heart.LoseLife();
        }

        
    }


    // 텍스트를 한글자씩 타이핑되듯이 하는 기능

    public IEnumerator TypeDialogue(string dialogue)
    {
        cdText.text = ""; // 먼저 한번 비워줌

        foreach (char letter in dialogue.ToCharArray())
        {
            cdText.text += letter; // 한 글자씩 추가
            yield return new WaitForSeconds(typingSpeed); // 타이핑 속도 만큼 대기
        }
    }

    // 버튼 기능 함수
    // 로직을 간단하게 수정함


    public void buttonListener(bool sickChecker)
    {
        Debug.Log("버튼이 눌렸고, 받은 값은 " + sickChecker + " 입니다");

        currentPrefab.GetComponent<PrefabBehavior>().isMove = true;

        foreach (Transform child in Buttons)
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


        
        
        StartCoroutine(RemoveAndSpawnPrefab());
        
        

    }

    private void popHowTo()
    {
        howTo.SetActive(true);
        Time.timeScale = 0;
    }


    

}

