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
        targetPos = new Vector2(over.position.x, over.position.y + 150); // ���ӿ��� ������ ���� ��ǥ������ ��ġ. y��ǥ�� 150���� �ö󰡵��� �ߴ�.

        foreach (var button in overButtons) // ���ӿ��� ������ �ö󰡴� ������ ��ư ��Ȱ��ȭ
        {
            button.SetActive(false);
        }

        StartCoroutine(MoveToTarget(targetPos, duration));
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private IEnumerator MoveToTarget(Vector2 target, float duration) // ���ӿ��� ������ ���� �����̵��� �ϴ� �ڷ�ƾ
    {
        Vector2 startPosition = over.position; // ���� ��ġ
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            // �ð��� ���� ��ġ ���
            float t = elapsedTime / duration;
            over.position = Vector2.Lerp(startPosition, target, t); // �ε巴�� �̵�

            elapsedTime += Time.deltaTime; // ��� �ð� ������Ʈ
            yield return null; // ���� �����ӱ��� ���
        }

        // ���� ��ġ ����
        transform.position = target; // ��Ȯ�� ��ǥ ��ġ�� �̵�


        yield return new WaitForSeconds(0.5f);

        foreach (var button in overButtons) // ���ӿ��� ������ ��ǥ ��ġ�� �̵�������, �ٽ� ��ư�� Ȱ��ȭ ������
        {
            button.SetActive(true);
        }

    }


}
