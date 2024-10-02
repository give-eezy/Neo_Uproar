using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabBehavior : MonoBehaviour
{

    Animator myAnim;


    public bool isMove;

    public string showWord;

    private string[] sickWord = {"����", "��¥����", "��ݴ�", "�մ�", "�Ű�ġ��", "��ġ"};
    private string[] normalWord = {"�����", "����ô", "�ƹ����� ����", "������", "�ϴ� �׽�Ʈ", "������°"};

    public string fakeWord = "����ũ�� �� xx���";

    public bool isSick;

    float speed = 2.5f;

    

    void Awake()
    {
        // Director ������ isProperty ���� ���� �ൿ�ؾ� �Ѵ�
        // ���� ������ �������� ������, �Ⱦ����� ���������� ������ �Ѵ�

        // �±� ���� ������, ������ ������ �������� isSick ���� ������
        // 
            

        if(gameObject.CompareTag("sick")) // ���� �±װ� �޷� �ִٸ�
        {
            this.isSick = true; // ���� ���¸� �ο��ϰ�
            this.showWord = sickWord[Random.Range(0, sickWord.Length)]; // ���� ��Ʈ�� �������� �ο��ϱ�. �ϴ��� 
        }
        else if(gameObject.CompareTag("good"))
        {
            this.isSick = false;
            this.showWord = normalWord[Random.Range(0, normalWord.Length)];
        }
        


        // fake �� ��쿡�� ������ ���� ��������� �Ѵ�
        // �׷��� �Ϸ���... �ϴ��� ������ �غ���
        // �ƴ��� �̰͵� �׳� �±׷� ���� ������ �ع�����?

        if(gameObject.name == "fake_prefab(Clone)")
        {
            this.showWord = ".....";
        }
       
        

        Debug.Log(gameObject.name);

        if(gameObject.name == "fake_prefab(Clone)")
        {
            myAnim = GetComponent<Animator>();
            Debug.Log("s2�������Դϴ�. �ִϸ����� �ҷ���");
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
            StartCoroutine(movePrefab());
        }
        

        
    }


    private IEnumerator movePrefab()
    {
        yield return new WaitForSeconds(1f);

        transform.Translate(speed * Time.deltaTime, 0, 0);

    }

}



