using System;
using UnityEngine;

public class CoinScript : MonoBehaviour
{
    [SerializeField] private float rotationSpeed;
    
    private void Update()
    {
        transform.Rotate(0, rotationSpeed * Time.deltaTime,0);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.Instance.RegisterCoin();
            Destroy(gameObject);
        }
    }
}
