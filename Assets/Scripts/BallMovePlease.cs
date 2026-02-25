using System;
using UnityEngine;

public class BallMovePlease : MonoBehaviour
{
    [SerializeField] private float ballForce;

    private Vector3 movement;
    private Rigidbody rb;

    private float moveH;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        MovePlease();
    }

    
    
    
    
    
    
    
    
    
    
    private void MovePlease()
    {
        moveH = Input.GetAxisRaw("Horizontal");
        movement = new Vector3(moveH, 0, 0);
        rb.AddForce(movement.normalized * ballForce, ForceMode.Acceleration);
    }
}
