using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabBehavior : MonoBehaviour
{

    Animator myAnim;


    public bool isMove;

    public string showWord;

    private string[] sickWord = {"아픔", "진짜아픔", "어금니", "앞니", "신경치료", "충치"};
    private string[] normalWord = {"구경꾼", "아픈척", "아무생각 없음", "뭐쓰지", "일단 테스트", "여섯개째"};

    public string fakeWord = "페이크다 이 xx들아";

    public bool isSick;

    float speed = 2.5f;

    

    void Awake()
    {
        // Director 에서는 isProperty 값에 따라 행동해야 한다
        // 만약 아프면 왼쪽으로 보내고, 안아프면 오른쪽으로 보내야 한다

        // 태그 별로 나눠서, 아픈지 아프지 않은지를 isSick 으로 나누기
        // 
            

        if(gameObject.CompareTag("sick")) // 아픔 태그가 달려 있다면
        {
            this.isSick = true; // 아픔 상태를 부여하고
            this.showWord = sickWord[Random.Range(0, sickWord.Length)]; // 아픈 멘트를 랜덤으로 부여하기. 일단은 
        }
        else if(gameObject.CompareTag("good"))
        {
            this.isSick = false;
            this.showWord = normalWord[Random.Range(0, normalWord.Length)];
        }
        


        // fake 의 경우에는 문구를 따로 적용해줘야 한다
        // 그렇게 하려면... 일단은 생각을 해보자
        // 아니지 이것도 그냥 태그로 따로 정리를 해버릴까?

        if(gameObject.name == "fake_prefab(Clone)")
        {
            this.showWord = ".....";
        }
       
        

        Debug.Log(gameObject.name);

        if(gameObject.name == "fake_prefab(Clone)")
        {
            myAnim = GetComponent<Animator>();
            Debug.Log("s2프리팹입니다. 애니메이터 불러옴");
        }

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        
        
        if (isMove) // 움직이라는 코드를 받았다면
        {
            StartCoroutine(movePrefab());
        }
        

        
    }


    private IEnumerator movePrefab()
    {
        yield return new WaitForSeconds(1f);

        transform.Translate(speed * Time.deltaTime, 0, 0);

    }

}



