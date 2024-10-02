using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabBehavior : MonoBehaviour
{

    


    public bool isMove;

    public string showWord;

    private string[] sickWord = 
        {"이가 썩은 것 같아요....", "이가 아파요. 진료 받고 싶어요", "엉엉... 이가 아프니까 자꾸 눈물이 나요", "아야야... 이가 너무 찌릿찌릿해요.", "이가 빠졌어요! 얼른 봐주세요!"};
    
    private string[] normalWord = 
        { "볼이 빵빵하니까 귀여워진 것 같아요! 럭키네오잖앙??", "헉, 볼이 부어올랐어요... 다들 저한테 귀여운 말 해줘요!", "...", "아야... 배가 아파요. 화장실이 어디죠?", "입이 쩍 벌어지네 이젠 분위기가 아닌 나의 입 안을 파악~♪"};

    public string fakeWord = "페이크다 이 xx들아";

    public bool isSick;

    float speed = 2.5f;

    

    void Awake()
    {
        // Director 에서는 isSick 값에 따라 행동해야 한다
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
            this.showWord = normalWord[Random.Range(0, normalWord.Length)]; // 아프지 않은 문구 랜덤 부여
        }
        


        // fake 의 경우에는 문구를 따로 적용해줘야 한다
        // 그렇게 하려면... 일단은 생각을 해보자
        // 아니지 이것도 그냥 태그로 따로 정리를 해버릴까?

        if(gameObject.name == "s_prefab(Clone)")
        {
            this.showWord = ".....";
        }
       
        

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
            StartCoroutine(movePrefab());
        }
        

        
    }


    private IEnumerator movePrefab()
    {
        yield return new WaitForSeconds(1f);

        transform.Translate(speed * Time.deltaTime, 0, 0);

    }

}



