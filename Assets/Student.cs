using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Student : MonoBehaviour
{
    public enum InfectionStatus { Healthy, Infected, Recovered }
    public InfectionStatus status = InfectionStatus.Healthy;
    public float infectionRadius = 0.5f; // Á¢ÃË °¨¿° °Å¸®
    public float infectionChance = 0.3f; // °¨¿° È®·ü (30%)

    private void Update()
    {
        //heckForInfection();
    }


    

    private void OnTriggerEnter(Collider other)
    {
        Student otherStudent = other.GetComponent<Student>();
        if (otherStudent != null && otherStudent.status == InfectionStatus.Infected && status == InfectionStatus.Healthy)
        {
            if (Random.value < infectionChance)
            {
                status = InfectionStatus.Infected;
                Debug.Log(gameObject.name + " °¨¿°µÊ!");
            }
        }
    }
}




