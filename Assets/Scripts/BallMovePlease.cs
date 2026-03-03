using System;
using UnityEngine;

public class BallMovePlease : MonoBehaviour
{
    [SerializeField] private float ballForce;
    [SerializeField] private float jumpForce;
    [SerializeField] private Camera mainCamera;
    
    private Vector3 movement;
    private Rigidbody rb;
    private Vector3 currentPosition;
    private Vector3 lastDragVector;
    private bool isDragging;

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
        //Jump();
    }

    private void FixedUpdate()
    {
        //MovePlease();
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
    
            // Solo usamos X e Y, mantenemos la Z original
            Vector3 dragVector = mouseWorldPos - currentPosition;
            dragVector.z = 0; // ignorar cualquier desviación en Z
            
            Debug.Log($"mouseWorldPos: {mouseWorldPos}, dragVector: {dragVector}");
            
            lastDragVector = dragVector;
        }
    }

    private void OnMouseUp()
    {
        if (!isDragging) return;
        isDragging = false;
        
        rb.isKinematic = false;
        rb.AddForce(lastDragVector.normalized * ballForce, ForceMode.Impulse);
    }
    
    private void MovePlease()
    {
        moveH = Input.GetAxisRaw("Horizontal");
        movement = new Vector3(moveH, 0, 0);
        rb.AddForce(movement.normalized * ballForce, ForceMode.Acceleration);
    }

    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }
}
