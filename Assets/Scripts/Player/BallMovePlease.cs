using System;
using Managers;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BallMovePlease : MonoBehaviour
{
    [SerializeField] private float ballForce;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private float groundFriction;
    [SerializeField] private float clickedFriction;
    
    [Header("Sounds")]
    [SerializeField] private AudioClip coilSound;
    [SerializeField] private AudioClip basketSound;
    
    private Vector3 movement;
    private Rigidbody rb;
    private Vector3 currentPosition;
    private Vector3 lastDragVector;
    private bool isDragging;
    private bool isOnBasket;
    private bool isGround;
    private bool isSummary;
    private LineRenderer lineRenderer;

    private float moveH;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        lineRenderer = GetComponent<LineRenderer>();
        currentPosition = transform.position;
        isOnBasket = false;
        isSummary = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isGround)
        {
            rb.linearDamping = groundFriction;
        }
        else if (isDragging)
        {
            rb.linearDamping = clickedFriction;
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
            
            if (GameManager.Instance.HitsLeft <= 0 && !isSummary)
            {
                isSummary = true;
                isOnBasket = true;
                GameManager.Instance.CalculateLevelScore();
            }
        }
        else isGround = false;
    }
    
    private void OnMouseDown()
    {
        if (GameManager.Instance.HitsLeft <= 0) return;
        isDragging = true;
        currentPosition = transform.position;
        lineRenderer.enabled = true;
    }

    private void OnMouseDrag()
    {
        if (!isDragging || GameManager.Instance.HitsLeft <= 0) return;
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        
        Plane dragPlane = new Plane(mainCamera.transform.forward, currentPosition);

        if (dragPlane.Raycast(ray, out float distance))
        {
            Vector3 mouseWorldPos = ray.GetPoint(distance);
            
            Vector3 dragVector = mouseWorldPos - currentPosition;
            dragVector.z = 0;
            
            lastDragVector = dragVector;
        }
        
        int points = 20;
        lineRenderer.positionCount = points;

        Vector3 velocity = lastDragVector * ballForce;

        for (int i = 0; i < points; i++)
        {
            float t = i * 0.05f;
            Vector3 point = transform.position + velocity * t + 0.5f * (Physics.gravity * 0.4f) * t * t;
            lineRenderer.SetPosition(i, point);
        }
    }

    private void OnMouseUp()
    {
        if (!isDragging) return;
        isDragging = false;
        lineRenderer.enabled = false;
        
        rb.AddForce(lastDragVector * ballForce, ForceMode.Impulse);
        AudioManager.Instance.PlaySfx(coilSound,0.6f);
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
        if (other.CompareTag("Basket") && !isOnBasket)
        {
            // If isOnBasket then we go to the next level + add points to the score
            isOnBasket = true;
            isSummary = true;
            AudioManager.Instance.PlaySfx(basketSound, 0.5f);
            GameManager.Instance.CalculateLevelScore();
        }
    }
}
