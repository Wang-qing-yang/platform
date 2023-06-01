using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
///
///</summary>
public class Pathing : MonoBehaviour
{
    public GameObject lr;
    private GameObject[] cars;

    private void Awake()
    {
        cars = GameObject.FindGameObjectsWithTag("Player");
    }

    private void Start()
    {
        for (int i = 0; i < cars.Length; i++)
        {
            cars[i].GetComponentInParent<NavMeshAgent>().enabled = true;
        }
    }

}
