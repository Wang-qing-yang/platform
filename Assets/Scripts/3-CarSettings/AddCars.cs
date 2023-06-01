using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
///添加无人车
///</summary>
public class AddCars : MonoBehaviour
{
    private Button add;
    //无人车的父物体
    public GameObject cars;
    //无人车
    public GameObject car;

    private void Awake()
    {
        add = GetComponent<Button>();
    }
    private void Start()
    {
        add.onClick.AddListener(Add);
    }


    private void Add()
    {
        GameObject a = GameObject.Instantiate(car);
        DataSaver.Instance.count++;
        a.transform.localPosition = new Vector3(DataSaver.Instance.count, 0.15f, DataSaver.Instance.count);
        a.transform.SetParent(cars.transform);

    }


}
