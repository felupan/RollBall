using System;
using Managers;
using UnityEngine;

public class CoinScript : MonoBehaviour
{
    [SerializeField] private float rotationSpeed;
    [SerializeField] private AudioClip coinSound;
    
    private void Update()
    {
        transform.Rotate(0, rotationSpeed * Time.deltaTime,0);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.Instance.RegisterCoin();
            AudioManager.Instance.PlaySfx(coinSound, 0.6f);
            Destroy(gameObject);
        }
    }
}
