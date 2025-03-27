using UnityEngine;
using System.Collections;
using UnityEngine.AI;
using System.Collections.Generic;

public class StudentMovement : MonoBehaviour
{
    private NavMeshAgent agent;
    private Vector3 seatPosition; // 학생의 지정된 자리
    private BoxCollider hallwayCollider; // 복도 영역
    private bool isWandering = false; // 복도에서 서성이는지 여부
    private StudentMovement leader; // 따라갈 리더
    private bool isLeader = false; //리더 여부


    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        seatPosition = transform.position;

        // 복도 영역 찾기
        GameObject hallway = GameObject.FindGameObjectWithTag("Hallway");
        if (hallway != null)
            hallwayCollider = hallway.GetComponent<BoxCollider>();

        // TimeManager 이벤트 구독
        TimeManager.Instance.OnBreakTimeStart += GoToHallway;
        TimeManager.Instance.OnClassTimeStart += ReturnToSeat;

        // 처음에는 자리에서 시작
        SitAtSeat();
    }

    void SitAtSeat()
    {
        agent.SetDestination(seatPosition);
    }

    void GoToHallway()
    {
        bool isLeader = Random.value < 0.2f; // 20% 확률로 리더가 됨.

        if (isLeader)
        {
            this.isLeader = true;
            Vector3 randomPoint = GetRandomPointInHallway();
            agent.SetDestination(randomPoint);
        }

        else
        {
            // 30% 확률로 서성이는 학생이 됨.
            isWandering = Random.value < 0.3f;

            if (isWandering)
            {
                StartCoroutine(WanderAround());
            }
            else
            {
                // 리더를 찾아 따라감
                leader = FindRandomLeader();
                if (leader != null)
                {
                    StartCoroutine(FollowLeader());
                }
            }
        }
    }

    void ReturnToSeat()
    {
        isWandering = false;
        StopAllCoroutines(); // 모든 행동 중지
        agent.SetDestination(seatPosition);
    }

    IEnumerator WanderAround()
    {
        while (true)
        {
            Vector3 randomWanderPoint = GetRandomPointInHallway();
            agent.SetDestination(randomWanderPoint);
            yield return new WaitForSeconds(Random.Range(2f, 5f));
        }
    }

    IEnumerator FollowLeader()
    {
        while (leader != null)
        {
            agent.SetDestination(leader.transform.position);
            yield return new WaitForSeconds(1f);
        }
    }

    StudentMovement FindRandomLeader()
    {
        StudentMovement[] students = FindObjectsOfType<StudentMovement>();
        if (students == null || students.Length == 0)
        {
            Debug.LogWarning("학생 리스트가 비어 있습니다. 리더를 찾을 수 없습니다.");
            return null;
        }

        // isLeader인 학생 중 랜덤 선택
        List<StudentMovement> leaders = new List<StudentMovement>();
        foreach (var student in students)
        {
            if (student != null && student.isLeader)
            {
                leaders.Add(student);
            }
        }

        if (leaders.Count == 0)
        {
            Debug.LogWarning("적절한 리더를 찾지 못했습니다.");
            return null;
        }

        return leaders[Random.Range(0, leaders.Count)];
    }


    Vector3 GetRandomPointInHallway()
    {
        if (hallwayCollider == null) return seatPosition;

        // 복도 내부의 랜덤한 위치 생성
        Vector3 randomPoint = new Vector3(
            Random.Range(hallwayCollider.bounds.min.x, hallwayCollider.bounds.max.x),
            transform.position.y,
            Random.Range(hallwayCollider.bounds.min.z, hallwayCollider.bounds.max.z)
        );

        return randomPoint;
    }
}
