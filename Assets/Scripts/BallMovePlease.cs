using System;
using UnityEngine;

public class BallMovePlease : MonoBehaviour
{
    [SerializeField] private float ballForce;
    [SerializeField] private float jumpForce;
    [SerializeField] private float maxDragDistance;
    [SerializeField] private Camera mainCamera;

    public LineRenderer lineRenderer;
    private Vector3 movement;
    private Rigidbody rb;
    private Vector3 startPosition;
    private Vector3 lastDragVector;
    private bool isDragging;

    private float moveH;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        startPosition = transform.position;
        rb.isKinematic = true;
    }

    // Update is called once per frame
    void Update()
    {
        Jump();
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

    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    private void OnMouseDown()
    {
        isDragging = true;
        rb.isKinematic = true;
    }

    private void OnMouseDrag()
    {
        if (!isDragging) return;
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        
        // Plano vertical en la posición de la bola, mirando hacia la cámara
        Plane dragPlane = new Plane(-mainCamera.transform.forward, startPosition);

        if (dragPlane.Raycast(ray, out float distance))
        {
            Vector3 mouseWorldPos = ray.GetPoint(distance);
    
            // Solo usamos X e Y, mantenemos la Z original
            Vector3 dragVector = mouseWorldPos - startPosition;
            dragVector.z = 0; // ignorar cualquier desviación en Z
            
            Debug.Log($"mouseWorldPos: {mouseWorldPos}, dragVector: {dragVector}");

            // Limitar distancia máxima
            if (dragVector.magnitude > maxDragDistance)
                dragVector = dragVector.normalized * maxDragDistance;
            
            lastDragVector = startPosition + dragVector;
            DrawLine();
        }
    }

    private void OnMouseUp()
    {
        if (!isDragging) return;
        isDragging = false;
        
        float forceMagnitude = (lastDragVector.magnitude / maxDragDistance) * ballForce;
        
        rb.isKinematic = false;
        rb.AddForce(-lastDragVector.normalized * forceMagnitude, ForceMode.Impulse);
    }
    
    void DrawLine()
    {
        if (lineRenderer == null) return;

        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, startPosition);
        lineRenderer.SetPosition(1, startPosition + lastDragVector);
    }
}
