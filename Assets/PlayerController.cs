using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float playerSpeed = 5.0f;
    [SerializeField] private float jumpPower = 5.0f;

    private Rigidbody _playerRigidbody;
    public float groundCast = 1f;

    public bool isGrounded = false;

    private void Start()
    {
        _playerRigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        IsGrounded();
        MovePlayer();

        if (Input.GetButton("Jump") && isGrounded)
        {
            Jump();
        }
    }

    private void MovePlayer()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");

        _playerRigidbody.velocity = new Vector2(horizontalInput * playerSpeed, _playerRigidbody.velocity.y);
        
    }

    private void Jump()
    {
        _playerRigidbody.AddForce(new Vector3(0, jumpPower, 0));
    }

    private void IsGrounded()
    {
        var groundCheck = Physics.Raycast(transform.position, Vector2.down, groundCast);

        if (groundCheck)
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
    }
}