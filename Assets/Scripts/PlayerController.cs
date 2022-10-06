using System;
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


    public AudioClip deathSound;
    public AudioSource aS;


    public GroundCheck groundCheck;

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


        if (horizontalInput > 0)
        {
            direction = 1;
        }
        else if (horizontalInput < 0)
        {
            direction = -1;
        }


        _playerRigidbody.velocity = new Vector2(horizontalInput * playerSpeed, _playerRigidbody.velocity.y);


        anim.SetFloat("speed", _playerRigidbody.velocity.x);


        if (horizontalInput == 0)
        {
            if (direction == 1)
            {
                transform.rotation = Quaternion.LookRotation(new Vector3(90, 0, 0));
            }
            else
            {
                transform.rotation = Quaternion.LookRotation(new Vector3(-90, 0, 0));
            }
        }
        else
        {
            transform.rotation = Quaternion.LookRotation(new Vector3(_playerRigidbody.velocity.x, 0, 0));
        }
    }

    private void Jump()
    {
        _playerRigidbody.AddForce(new Vector3(0, jumpPower, 0));
    }

    private void IsGrounded()
    {
        isGrounded = groundCheck.isGrounded;


        /**
        var groundCheck = Physics.Raycast(transform.position, Vector2.down, groundCast);

        if (groundCheck)
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }  **/
    }


    private void OnTriggerEnter(Collider collision)
    {
        if (collision.transform.gameObject.CompareTag("Death"))
        {
            this.transform.gameObject.transform.position = GameManager.Instance.start.transform.position;
            aS.clip = deathSound;
            aS.Play();
        }
    }

    private void OnTriggerStay(Collider collision)
    {
        if (collision.transform.gameObject.CompareTag("Death"))
        {
            this.transform.gameObject.transform.position = GameManager.Instance.start.transform.position;
            aS.clip = deathSound;
            aS.Play();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.gameObject.CompareTag("Death"))
        {
            this.transform.gameObject.transform.position = GameManager.Instance.start.transform.position;
            aS.clip = deathSound;
            aS.Play();
        }
    }


    private void OnCollisionStay(Collision collision)
    {
        if (collision.transform.gameObject.CompareTag("Death"))
        {
            this.transform.gameObject.transform.position = GameManager.Instance.start.transform.position;
            aS.clip = deathSound;
            aS.Play();
        }
    }
}