using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabBehavior : MonoBehaviour
{


    public bool isMove; // ĳ���Ͱ� �����̸鼭 ������� �ϱ� ���� �÷���

    public string showWord; // ��ǳ���� ����� ����

    public string[] testWord; // ĳ���� ���� �迭

    public bool isSick; // ������ �ƴ��� �Ǵ��ϱ� ���� �÷���

    float speed = 2.5f; // ĳ���Ͱ� ������ �� �̵� �ӵ�

    

    void Awake()
    {
        // Director ������ isSick ���� ���� �ൿ�ؾ� �Ѵ�
        // ���� ������ �������� ������, �Ⱦ����� ���������� ������ �Ѵ�

        // �±� ���� ������, ������ ������ �������� isSick ���� ������
        // 

        this.showWord = testWord[Random.Range(0, testWord.Length)];

        if(gameObject.CompareTag("sick")) // ���� �±װ� �޷� �ִٸ�
        {
            this.isSick = true; // ������ ��
            
            
        }
        else if(gameObject.CompareTag("good")) // ������ �±װ� �޷� �ִٸ�
        {
            this.isSick = false; // ������ ����
            
            
        }

  

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
     
        if (isMove) // �����̶�� �ڵ带 �޾Ҵٸ�
        {
            StartCoroutine(movePrefab()); // �̵� ���� �ڷ�ƾ ����
        }
        
    }


    private IEnumerator movePrefab()
    {
        yield return new WaitForSeconds(1f); // 1�� ����ߴٰ�

        transform.Translate(-speed * Time.deltaTime, 0, 0); // �������� �����̵��� ����

    }

}



