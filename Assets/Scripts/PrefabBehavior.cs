using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabBehavior : MonoBehaviour
{


    public bool isMove; // 캐릭터가 움직이면서 사라지게 하기 위한 플래그

    public string showWord; // 말풍선에 띄워줄 문구

    public string[] testWord;


    // 아픈 문구 랜덤으로 하기 위한 배열
    private string[] sickWord = 
        {"이가 썩은 것 같아요....", "이가 아파요. 진료 받고 싶어요", "엉엉... 이가 아프니까 자꾸 눈물이 나요", "아야야... 이가 너무 찌릿찌릿해요.", "이가 빠졌어요! 얼른 봐주세요!"};
    

    // 멀쩡한 문구 랜덤으로 하기 위한 배열
    private string[] normalWord = 
        { "볼이 빵빵하니까 귀여워진 것 같아요! 럭키네오잖앙??", "헉, 볼이 부어올랐어요... 다들 저한테 귀여운 말 해줘요!", "...", "아야... 배가 아파요. 화장실이 어디죠?", "입이 쩍 벌어지네 이젠 분위기가 아닌 나의 입 안을 파악~♪"};

   
    public bool isSick; // 아픈지 아닌지 판단하기 위한 플래그

    float speed = 2.5f; // 캐릭터가 삭제될 때 이동 속도

    

    void Awake()
    {
        // Director 에서는 isSick 값에 따라 행동해야 한다
        // 만약 아프면 왼쪽으로 보내고, 안아프면 오른쪽으로 보내야 한다

        // 태그 별로 나눠서, 아픈지 아프지 않은지를 isSick 으로 나누기
        // 
            

        if(gameObject.CompareTag("sick")) // 아픔 태그가 달려 있다면
        {
            this.isSick = true; // 아픔은 참
            this.showWord = sickWord[Random.Range(0, sickWord.Length)]; // 아픈 멘트를 랜덤으로 부여하기. 일단은 
        }
        else if(gameObject.CompareTag("good")) // 멀쩡함 태그가 달려 있다면
        {
            this.isSick = false; // 아픔은 거짓
            this.showWord = normalWord[Random.Range(0, normalWord.Length)]; // 아프지 않은 문구 랜덤 부여
        }

        Debug.Log(testWord);
        Debug.Log(gameObject.name);

        

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
            StartCoroutine(movePrefab()); // 이동 관련 코루틴 실행
        }
        
    }


    private IEnumerator movePrefab()
    {
        yield return new WaitForSeconds(1f); // 1초 대기했다가

        transform.Translate(-speed * Time.deltaTime, 0, 0); // 좌측으로 움직이도록 설정

    }

}



