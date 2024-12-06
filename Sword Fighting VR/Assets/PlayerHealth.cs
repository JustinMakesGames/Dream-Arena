using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int health;

    [SerializeField] private float endTime;

    [SerializeField] private float jumpForce;
    [SerializeField] private float knockbackForce;
    [SerializeField] private float knockbackDuration;
    private bool _isHit;
    private float _time;

    private Rigidbody _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        TimePunishment();
        
    }

    private void TimePunishment()
    {
        if (_isHit)
        {
            _time += Time.deltaTime;

            if (_time > endTime)
            {
                _isHit = false;
                _time = 0;
            }
        }
    }

    public void TakeDamage(int damage)
    {
        if (_isHit) return; 
        health -= damage;
        Knockback();
        if (health <= 0)
        {
            GameManager.Instance.HandleGameOver();
        }
    }

    private void Knockback()
    {
        Vector3 direction = (transform.position - (transform.position + transform.forward)).normalized;
        direction.y = jumpForce;
        print("jumped");
        _rb.AddForce(direction * knockbackForce, ForceMode.VelocityChange);
        print(direction * knockbackForce);

        Invoke(nameof(ResetKnockback), knockbackDuration);


    }

    private void ResetKnockback()
    {
        _rb.velocity = Vector3.zero;
    }


}
