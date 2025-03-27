using System;
using System.Collections;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public static TimeManager Instance; // 싱글톤 패턴 적용
    public event Action OnBreakTimeStart; // 쉬는 시간 이벤트
    public event Action OnClassTimeStart; // 수업 시간 이벤트

    [SerializeField] private float breakDuration = 10f; // 쉬는 시간(초 단위)
    [SerializeField] private float classDuration = 30f; // 수업 시간(초 단위)

    private bool isBreakTime = false;

    void Awake()
    {
        // 싱글톤 패턴 적용 (하나의 TimeManager만 존재하도록 함)
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
        Debug.Log("쉬는 시간이 시작되었습니다!");
        OnBreakTimeStart?.Invoke(); // 모든 학생에게 알림
        yield return new WaitForSeconds(breakDuration);
    }

    IEnumerator StartClassTime()
    {
        isBreakTime = false;
        Debug.Log("수업 시간이 시작되었습니다!");
        OnClassTimeStart?.Invoke(); // 모든 학생에게 알림
        yield return new WaitForSeconds(classDuration);
    }
}

