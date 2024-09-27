using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabBehavior : MonoBehaviour
{


    public string checkDir;

    public string showWord;

    private string[] sickWord = {"아픔", "진짜아픔", "어금니", "앞니", "신경치료", "충치"};
    private string[] normalWord = {"구경꾼", "아픈척", "아무생각 없음", "뭐쓰지", "일단 테스트", "여섯개째"};

    public bool isProperty;

    float speed = 2.5f;

    

    void Awake()
    {
        // Director 에서는 isProperty 값에 따라 행동해야 한다
        // 만약 아프면 왼쪽으로 보내고, 안아프면 오른쪽으로 보내야 한다
        this.isProperty = Random.value < 0.5f; // 아픈지, 아프지 않은지 판별하기

        

        if (isProperty) // 참일때 아픔
        {
            this.showWord = sickWord[Random.Range(0, sickWord.Length)];

        }
        else if (!isProperty) // 거짓일때 멀쩡함
        {
            this.showWord = normalWord[Random.Range(0, normalWord.Length)];

        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (checkDir == "left") // 방향 체크해서 움직이면서 없어지도록(없어지는건 다른 코드)
        {
            transform.Translate(-speed * Time.deltaTime, 0, 0);
        }
        else if (checkDir == "right")
        {
            transform.Translate(speed * Time.deltaTime, 0, 0);
        }
    }

    
}



