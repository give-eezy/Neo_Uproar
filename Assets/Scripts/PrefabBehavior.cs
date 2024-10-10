using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabBehavior : MonoBehaviour
{


    public bool isMove; // 캐릭터가 움직이면서 사라지게 하기 위한 플래그

    public string showWord; // 말풍선에 띄워줄 문구

    public string[] testWord; // 캐릭터 문구 배열

    public bool isSick; // 아픈지 아닌지 판단하기 위한 플래그

    float speed = 2.5f; // 캐릭터가 삭제될 때 이동 속도

    

    void Awake()
    {
        // Director 에서는 isSick 값에 따라 행동해야 한다
        // 만약 아프면 왼쪽으로 보내고, 안아프면 오른쪽으로 보내야 한다

        // 태그 별로 나눠서, 아픈지 아프지 않은지를 isSick 으로 나누기
        // 

        this.showWord = testWord[Random.Range(0, testWord.Length)];

        if(gameObject.CompareTag("sick")) // 아픔 태그가 달려 있다면
        {
            this.isSick = true; // 아픔은 참
            
            
        }
        else if(gameObject.CompareTag("good")) // 멀쩡함 태그가 달려 있다면
        {
            this.isSick = false; // 아픔은 거짓
            
            
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
            StartCoroutine(movePrefab()); // 이동 관련 코루틴 실행
        }
        
    }


    private IEnumerator movePrefab()
    {
        yield return new WaitForSeconds(1f); // 1초 대기했다가

        transform.Translate(-speed * Time.deltaTime, 0, 0); // 좌측으로 움직이도록 설정

    }

}



