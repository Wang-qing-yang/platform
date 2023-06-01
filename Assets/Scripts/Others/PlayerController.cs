using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
///
///</summary>
public class PlayerController : MonoBehaviour
{
    private Rigidbody rb;
    public float moveSpeed;
    public Text countText;
    public Text winText;
    private int count;
    public void Start()
    {
        rb = GetComponent<Rigidbody>();
        SetCountText();
        winText.text = "";
    }



    private void FixedUpdate()
    {
        float moveHorizontal = Input.GetAxis("Horizontal")*moveSpeed;
        float moveVertical = Input.GetAxis("Vertical")*moveSpeed;
        rb.AddForce(moveHorizontal,0,moveVertical);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Pick Up"))
        {
            other.gameObject.SetActive(false);
            count++;
            SetCountText();
            if (count >= 16)
                winText.text = "You Win!";
        }
    }
    private void SetCountText()
    {
        countText.text = "Count:" + count.ToString();
    }

}
