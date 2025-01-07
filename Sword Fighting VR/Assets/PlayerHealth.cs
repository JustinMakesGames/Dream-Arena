using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public int health;
    public int maxHealth;

    [SerializeField] private float endTime;

    [SerializeField] private float jumpForce;
    [SerializeField] private float knockbackForce;
    [SerializeField] private float knockbackDuration;
    private bool _isHit;
    private float _time;

    private Rigidbody _rb;
    private RawImage _image;


    private void Awake()
    {
        _image = GetComponentInChildren<RawImage>();
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
        if (health <= 0)
        {
            GameManager.Instance.HandleGameOver();
        }

        _image.color = new Color(_image.color.r, _image.color.g, _image.color.b, CalculateColor());
        Knockback();
    }

    private float CalculateColor()
    {
        float color = 1 - ((float)health / (float)maxHealth);
        print(color);
        return color;
    }

    private void Knockback()
    {
        _isHit = true;
        Vector3 direction = (transform.position - (transform.position + transform.forward)).normalized;
        direction.y = jumpForce;


        Invoke(nameof(ResetKnockback), knockbackDuration);


    }

    private void ResetKnockback()
    {
        _isHit = false;
    }


}
