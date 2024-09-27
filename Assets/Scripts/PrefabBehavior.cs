using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabBehavior : MonoBehaviour
{


    public string checkDir;

    public string showWord;

    private string[] sickWord = {"����", "��¥����", "��ݴ�", "�մ�", "�Ű�ġ��", "��ġ"};
    private string[] normalWord = {"�����", "����ô", "�ƹ����� ����", "������", "�ϴ� �׽�Ʈ", "������°"};

    public bool isProperty;

    float speed = 2.5f;

    

    void Awake()
    {
        // Director ������ isProperty ���� ���� �ൿ�ؾ� �Ѵ�
        // ���� ������ �������� ������, �Ⱦ����� ���������� ������ �Ѵ�
        this.isProperty = Random.value < 0.5f; // ������, ������ ������ �Ǻ��ϱ�

        

        if (isProperty) // ���϶� ����
        {
            this.showWord = sickWord[Random.Range(0, sickWord.Length)];

        }
        else if (!isProperty) // �����϶� ������
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
        if (checkDir == "left") // ���� üũ�ؼ� �����̸鼭 ����������(�������°� �ٸ� �ڵ�)
        {
            transform.Translate(-speed * Time.deltaTime, 0, 0);
        }
        else if (checkDir == "right")
        {
            transform.Translate(speed * Time.deltaTime, 0, 0);
        }
    }

    
}



