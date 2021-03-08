using BrawlServer;
using GameClient;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Range(0.0f, 50.0f)]
    public float PlayerSpeed;
    [Range(0.0f, 50.0f)]
    public float JumpForce;
    [Range(0.0f, 50.0f)]
    public float Gravity;

    public PlayerManager m_manager;

    public GroundChecker m_footSensor;
    public bool m_isGrounded;

    private CharacterController m_controller;
    private Vector3 m_playerVelocity;

    private void Start()
    {
        m_controller = GetComponent<CharacterController>() ?? throw new MissingComponentException(nameof(m_controller));
        m_footSensor = GetComponentInChildren<GroundChecker>() ?? throw new MissingComponentException(nameof(m_footSensor));
        m_manager = GetComponent<PlayerManager>() ?? throw new MissingComponentException(nameof(m_manager));
        if (m_manager.id != Client.instance.myId) Destroy(this);
    }

    private void Update()
    {
        m_isGrounded = m_footSensor.IsOverlapping;
        if (m_isGrounded && m_playerVelocity.y < 0)
        {
            m_playerVelocity.y = 0f;
        }

        Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        m_controller.Move(move * Time.deltaTime * PlayerSpeed);

        if (move != Vector3.zero)
        {
            gameObject.transform.forward = move;
        }

        // Changes the height position of the player..
        if (Input.GetButtonDown("Jump") && m_isGrounded)
        {
            m_playerVelocity.y += Mathf.Sqrt(JumpForce * 3.0f * Gravity);
        }

        m_playerVelocity.y -= Gravity * Time.deltaTime;
        m_controller.Move(m_playerVelocity * Time.deltaTime);
    }

    private void FixedUpdate()
    {
        SendInputToServer();
    }

    private void SendInputToServer()
    {
        bool[] inputs = new bool[]
        {
            Input.GetKey(KeyCode.W),
            Input.GetKey(KeyCode.S),
            Input.GetKey(KeyCode.A),    
            Input.GetKey(KeyCode.D)
        };
        ((BrawlClientSender)Client._sender).PlayerMovement(inputs);
    }
}
