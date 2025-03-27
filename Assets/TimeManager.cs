using System;
using System.Collections;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public static TimeManager Instance; // �̱��� ���� ����
    public event Action OnBreakTimeStart; // ���� �ð� �̺�Ʈ
    public event Action OnClassTimeStart; // ���� �ð� �̺�Ʈ

    [SerializeField] private float breakDuration = 10f; // ���� �ð�(�� ����)
    [SerializeField] private float classDuration = 30f; // ���� �ð�(�� ����)

    private bool isBreakTime = false;

    void Awake()
    {
        // �̱��� ���� ���� (�ϳ��� TimeManager�� �����ϵ��� ��)
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        StartCoroutine(SchoolRoutine());
    }

    IEnumerator SchoolRoutine()
    {
        while (true)
        {
            yield return StartCoroutine(StartBreakTime());
            yield return StartCoroutine(StartClassTime());
        }
    }

    IEnumerator StartBreakTime()
    {
        isBreakTime = true;
        Debug.Log("���� �ð��� ���۵Ǿ����ϴ�!");
        OnBreakTimeStart?.Invoke(); // ��� �л����� �˸�
        yield return new WaitForSeconds(breakDuration);
    }

    IEnumerator StartClassTime()
    {
        isBreakTime = false;
        Debug.Log("���� �ð��� ���۵Ǿ����ϴ�!");
        OnClassTimeStart?.Invoke(); // ��� �л����� �˸�
        yield return new WaitForSeconds(classDuration);
    }
}

