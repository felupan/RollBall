using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BallMovePlease : MonoBehaviour
{
    [SerializeField] private float ballForce;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private float groundFriction;
    
    private Vector3 movement;
    private Rigidbody rb;
    private Vector3 currentPosition;
    private Vector3 lastDragVector;
    private bool isDragging;
    private bool isOnBasket;
    private bool isGround;

    private float moveH;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        currentPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (isGround)
        {
            rb.linearDamping = groundFriction;
        }
        else rb.linearDamping = 0;
    }

    private void FixedUpdate()
    {
        // Negative velocity on 'y' means the ball is going down. If 0 means its movement is horizontal
        if (Mathf.Abs(rb.linearVelocity.y) < 0.01f)
        {
            // We check if the ball is standing on the ground
            CheckGround();
        }
        else isGround = false;
    }

    private void OnMouseDown()
    {
        isDragging = true;
        rb.isKinematic = true;
        currentPosition = transform.position;
    }

    private void OnMouseDrag()
    {
        if (!isDragging) return;
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        
        Plane dragPlane = new Plane(mainCamera.transform.forward, currentPosition);

        if (dragPlane.Raycast(ray, out float distance))
        {
            Vector3 mouseWorldPos = ray.GetPoint(distance);
            
            Vector3 dragVector = mouseWorldPos - currentPosition;
            dragVector.z = 0;
            
            lastDragVector = dragVector;
        }
    }

    private void OnMouseUp()
    {
        if (!isDragging) return;
        isDragging = false;
        
        rb.isKinematic = false;
        rb.AddForce(lastDragVector * ballForce, ForceMode.Impulse);
        
        GameManager.Instance.RegisterHit();
    }

    private void CheckGround()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out var hitInfo, 1f))
        {
            if (hitInfo.collider.CompareTag("Ground"))
            {
                isGround = true;
            }
            else
            {
                isGround = false;
            }
        }
        else isGround = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Basket"))
        {
            // If isOnBasket then we go to the next level + add points to the score
            isOnBasket = true;
            GameManager.Instance.CalculateLevelScore();
            
        }
    }
}
