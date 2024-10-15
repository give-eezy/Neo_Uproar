using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class overSceneScript : MonoBehaviour
{

    public GameObject[] overButtons;

    Transform over;

    Vector2 targetPos;
    float duration = 1.2f;


    // Start is called before the first frame update
    void Start()
    {
        over = GameObject.Find("overControl").GetComponent<Transform>(); ;
        targetPos = new Vector2(over.position.x, over.position.y + 150); // 게임오버 문구의 최종 목표지점의 위치. y좌표로 150정도 올라가도록 했다.

        foreach (var button in overButtons) // 게임오버 문구가 올라가는 동안은 버튼 비활성화
        {
            button.SetActive(false);
        }

        StartCoroutine(MoveToTarget(targetPos, duration));
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private IEnumerator MoveToTarget(Vector2 target, float duration) // 게임오버 문구가 위로 움직이도록 하는 코루틴
    {
        Vector2 startPosition = over.position; // 시작 위치
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            // 시간에 따라 위치 계산
            float t = elapsedTime / duration;
            over.position = Vector2.Lerp(startPosition, target, t); // 부드럽게 이동

            elapsedTime += Time.deltaTime; // 경과 시간 업데이트
            yield return null; // 다음 프레임까지 대기
        }

        // 최종 위치 설정
        transform.position = target; // 정확한 목표 위치로 이동


        yield return new WaitForSeconds(0.5f);

        foreach (var button in overButtons) // 게임오버 문구가 목표 위치로 이동했으니, 다시 버튼을 활성화 시켜줌
        {
            button.SetActive(true);
        }

    }


}
