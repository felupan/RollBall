using System;
using UnityEngine;

public class MouseFollow : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private BallMovePlease dani;
    private Vector3 mouseWorldPos;
    private bool isOnDrag;
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        mouseWorldPos = Input.mousePosition;
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

        Plane dragPlane = new Plane(mainCamera.transform.forward, dani.transform.position);

        if (dragPlane.Raycast(ray, out float distance))
        {
            mouseWorldPos = ray.GetPoint(distance);
        }
        
        transform.position = mouseWorldPos;
    }
}