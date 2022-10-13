using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
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

    public int life = 3;

    public AudioClip deathSound;
    public AudioSource aS;
    public bool canJump = true;

    public GroundCheck groundCheck;

    private void Start()
    {
        anim = GetComponent<Animator>();
        _playerRigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        IsGrounded();


        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            if (!canJump)
            {
                return;
            }

            Jump();
        }
    }

    private void FixedUpdate()
    {
        MovePlayer();
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


        _playerRigidbody.velocity =
            new Vector2(horizontalInput * playerSpeed, _playerRigidbody.velocity.y);


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
        _playerRigidbody.AddForce(new Vector3(0, jumpPower, 0), ForceMode.Impulse);
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

    public void ReSpawn()
    {
        this.transform.gameObject.transform.position = GameManager.Instance.start.transform.position;
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.transform.gameObject.CompareTag("Death"))
        {
            this.transform.gameObject.transform.position = GameManager.Instance.start.transform.position;
            aS.clip = deathSound;
            aS.Play();
        }

        if (collision.gameObject.name == "TrampolineUp")
        {
            if (!isGrounded)
            {
                return;
            }

            canJump = false;
            _playerRigidbody.AddForce(new Vector3(playerSpeed * direction * 3.25f, jumpPower * 3f, 0),
                ForceMode.Impulse);

            _playerRigidbody.velocity =
                new Vector2(direction * playerSpeed, _playerRigidbody.velocity.y);
        }
        else
        {
            canJump = true;
        }

        if (collision.gameObject.name == "TrampolineDown")
        {
            if (!isGrounded)
            {
                return;
            }

            canJump = false;
            _playerRigidbody.AddForce(new Vector3(playerSpeed * direction * 3.25f, -jumpPower * 3f, 0),
                ForceMode.Impulse);
            _playerRigidbody.velocity =
                new Vector2(direction * playerSpeed, _playerRigidbody.velocity.y);
        }
        else
        {
            canJump = true;
        }
        
        
        
        if (collision.transform.gameObject.CompareTag("Death"))
        {
            this.transform.gameObject.transform.position = GameManager.Instance.start.transform.position;
            aS.clip = deathSound;
            aS.Play();
            SubLife();
        }


        if (collision.transform.gameObject.name == "Goal")
        {
            if (GameManager.Instance._gameState == GameManager.GameState.Editor)
            {
                GameManager.Instance.StopPlayTest();
            }

            GameManager.Instance.PlayerWins();
            this.transform.GetComponent<Rigidbody>().isKinematic = true;
        }
        
    }


    public void SubLife()
    {
        if (GameManager.Instance._gameState == GameManager.GameState.Editor)
        {
            return;
        }


        if (life <= 0)
        {
            this.transform.GetComponent<Rigidbody>().isKinematic = true;
            GameManager.Instance.PlayerLoses();
        }

        life -= 1;
    }

    private void OnTriggerStay(Collider collision)
    {
        if (collision.transform.gameObject.CompareTag("Death"))
        {
            this.transform.gameObject.transform.position = GameManager.Instance.start.transform.position;
            aS.clip = deathSound;
            aS.Play();
        }

        if (collision.gameObject.name == "TrampolineUp")
        {
            if (!isGrounded)
            {
                return;
            }

            canJump = false;
            _playerRigidbody.AddForce(new Vector3(playerSpeed * direction * 3.25f, jumpPower * 3f, 0),
                ForceMode.Impulse);

            _playerRigidbody.velocity =
                new Vector2(direction * playerSpeed, _playerRigidbody.velocity.y);
        }
        else
        {
            canJump = true;
        }

        if (collision.gameObject.name == "TrampolineDown")
        {
            if (!isGrounded)
            {
                return;
            }

            canJump = false;
            _playerRigidbody.AddForce(new Vector3(playerSpeed * direction * 3.25f, -jumpPower * 3f, 0),
                ForceMode.Impulse);
            _playerRigidbody.velocity =
                new Vector2(direction * playerSpeed, _playerRigidbody.velocity.y);
        }
        else
        {
            canJump = true;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {

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