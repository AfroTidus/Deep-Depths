using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 1.0f;
    public float turnSpeed = 1.0f;
    private Rigidbody2D _rigidbody;
    private bool launching;
    private float turnDirection;

    void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        launching = (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow));

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            turnDirection = 1.0f;
        } else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            turnDirection = -1.0f;
        } else {
        
            turnDirection = 0.0f;
        }
    }

    private void FixedUpdate()
    {
        if (launching)
        {
            _rigidbody.AddForce(this.transform.up * this.speed);
        }

        if (turnDirection != 0.0f)
        {
            _rigidbody.AddTorque(turnDirection * this.turnSpeed);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag =="Wall")
        {
            print("hit");
        }
    }
}
