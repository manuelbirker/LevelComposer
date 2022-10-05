using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float playerSpeed = 5.0f;
    [SerializeField] private float jumpPower = 5.0f;

    private Rigidbody _playerRigidbody;
    public float groundCast = 1f;

    public bool isGrounded = false;


    public int direction = 1;

    public Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
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


        anim.SetFloat("speed", _playerRigidbody.velocity.x);


        if (_playerRigidbody.velocity.x > 0)
        {
            direction = 1;
        }
        else
        {
            direction = -1;
        }


        transform.rotation = Quaternion.LookRotation(new Vector3(_playerRigidbody.velocity.x, 0, 0));


        if (_playerRigidbody.velocity.x == 0)
        {
            if (direction == 1)
            {
                transform.rotation = Quaternion.LookRotation(new Vector3(90, 0, 0));
            }

            if (direction == -1)
            {
                transform.rotation = Quaternion.LookRotation(new Vector3(-90, 0, 0));
            }
        }
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