using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabBehavior : MonoBehaviour
{


    public bool isMove; // ĳ���Ͱ� �����̸鼭 ������� �ϱ� ���� �÷���

    public string showWord; // ��ǳ���� ����� ����

    public string[] testWord;


    // ���� ���� �������� �ϱ� ���� �迭
    private string[] sickWord = 
        {"�̰� ���� �� ���ƿ�....", "�̰� ���Ŀ�. ���� �ް� �;��", "����... �̰� �����ϱ� �ڲ� ������ ����", "�ƾ߾�... �̰� �ʹ� ���ؿ�.", "�̰� �������! �� ���ּ���!"};
    

    // ������ ���� �������� �ϱ� ���� �迭
    private string[] normalWord = 
        { "���� �����ϴϱ� �Ϳ����� �� ���ƿ�! ��Ű�׿��ݾ�??", "��, ���� �ξ�ö����... �ٵ� ������ �Ϳ��� �� �����!", "...", "�ƾ�... �谡 ���Ŀ�. ȭ����� �����?", "���� ½ �������� ���� �����Ⱑ �ƴ� ���� �� ���� �ľ�~��"};

   
    public bool isSick; // ������ �ƴ��� �Ǵ��ϱ� ���� �÷���

    float speed = 2.5f; // ĳ���Ͱ� ������ �� �̵� �ӵ�

    

    void Awake()
    {
        // Director ������ isSick ���� ���� �ൿ�ؾ� �Ѵ�
        // ���� ������ �������� ������, �Ⱦ����� ���������� ������ �Ѵ�

        // �±� ���� ������, ������ ������ �������� isSick ���� ������
        // 
            

        if(gameObject.CompareTag("sick")) // ���� �±װ� �޷� �ִٸ�
        {
            this.isSick = true; // ������ ��
            this.showWord = sickWord[Random.Range(0, sickWord.Length)]; // ���� ��Ʈ�� �������� �ο��ϱ�. �ϴ��� 
        }
        else if(gameObject.CompareTag("good")) // ������ �±װ� �޷� �ִٸ�
        {
            this.isSick = false; // ������ ����
            this.showWord = normalWord[Random.Range(0, normalWord.Length)]; // ������ ���� ���� ���� �ο�
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



