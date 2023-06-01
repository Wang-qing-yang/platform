using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
///
///</summary>
public class GetTarget : MonoBehaviour
{
    private Button mButton;
    public InputField xinputField;
    public InputField zinputField;
    public Transform target;

    private void Awake()
    {
        mButton=GetComponent<Button>();
    }

    private void Start()
    {
        mButton.onClick.AddListener(onClick);
    }

    public void onClick()
    {
      target.position =new Vector3( float.Parse(xinputField.text),0.1f,float.Parse(zinputField.text) );   
    }



}
