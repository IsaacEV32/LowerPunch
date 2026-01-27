using System;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class MainCharacter : MonoBehaviour
{
    internal int health = 100;
    internal int specialPoints = 0;

    public HUDSystem HUD;

    public Vector2 MoveDir = Vector2.zero;
    private CharacterController controller;
    [SerializeField] float speed = 4f;
    //For the jump control
    [SerializeField] Vector3 velocity;
    [SerializeField] float jumpForce = 4f;
    [SerializeField] float gravity = -9.8f;

    private float jumpTimeStamp;
    private float jumpTime = 0.2f;

    private bool _movementInputPressed = false;

    private bool lookLeft = true;

    private bool chronometer = false;

    private bool Weakchronometer = false;
    private float chronometerWeakDelay = 0;
    private float punchWeakDelay = 0.5f;
    private float chronometerForLimitPunchWeak = 0.6f;
    private int numberOfWeakPunches = 0;


    private bool airPunchChronometer = false;
    private float airPunchChronometerDelay = 0;
    private float airPunchDelay = 1f;

    private void Awake()
    {
        HUD.SetReferencePlayer(this);
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position + new Vector3(1, -1, 0), new Vector3(1f, 1f, 1f));
    }
    private void Start()
    {
        controller = GetComponent<CharacterController>();
    }
    public void OnMoveInput(InputAction.CallbackContext contextMove)
    {
        if (contextMove.started)
        {
            _movementInputPressed = true;
        }
        else if (contextMove.canceled)
        {
            _movementInputPressed = false;
        }
        MoveDir = contextMove.ReadValue<Vector2>();
        if (Input.GetKeyDown(KeyCode.A))
        {
            lookLeft = true;
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            lookLeft = false;
        }
    }
    public void OnJumpInput(InputAction.CallbackContext contextJump)
    {
        if (contextJump.performed && controller.isGrounded)
        {
            jumpTimeStamp = Time.time;
            //Ayuda a establecer la máxima altura a la que el jugador quiere llegar
            velocity.y = MathF.Sqrt(jumpForce * -3 * gravity);
        }
        else if (contextJump.canceled)
        {
            if (Time.time - jumpTimeStamp < jumpTime)
            {
                velocity.y = 0;
            }
        }
    }
    public void OnWeakPunch(InputAction.CallbackContext contextPunch)
    {
        if (contextPunch.performed && controller.isGrounded && !Weakchronometer)
        {
            chronometer = true;
            Weakchronometer = true;
            if (lookLeft)
            {
                Collider[] colliders = Physics.OverlapBox(transform.position + new Vector3(-1, 0, 0), new Vector3(1f, 1.25f, 1f), Quaternion.identity);
                Debug.Log("Golpe suelo izquierda");
            }
            else if (!lookLeft)
            {
                Collider[] colliders = Physics.OverlapBox(transform.position + new Vector3(1, 0, 0), new Vector3(1f, 1.25f, 1f), Quaternion.identity);
                Debug.Log("Golpe suelo derecha");
            }
            numberOfWeakPunches++;
            Debug.Log("Golpe suelo derecha : " + numberOfWeakPunches);
        }
        else if (contextPunch.performed && !controller.isGrounded && !airPunchChronometer)
        {
            airPunchChronometer = true;
            if (lookLeft)
            {
                Collider[] colliders = Physics.OverlapBox(transform.position + new Vector3(-1, -1, 0), new Vector3(1f, 1f, 1f), Quaternion.identity);
                Debug.Log("Golpe aire izquierda");
            }
            else if (!lookLeft)
            {
                Collider[] colliders = Physics.OverlapBox(transform.position + new Vector3(1, -1, 0), new Vector3(1f, 1f, 1f), Quaternion.identity);
                Debug.Log("Golpe aire derecha");
            }
        }

    }
    private void DelayForWeakPunches()
    {
        if (numberOfWeakPunches == 3)
        {
            Weakchronometer = true;
            punchWeakDelay = 0.5f;
            if (chronometerWeakDelay > punchWeakDelay)
            {
                Weakchronometer = false;
                chronometer = false;
                chronometerWeakDelay = 0;
                numberOfWeakPunches = 0;
                Debug.Log("Reinicio del combo de golpes");
            }
        }
        else if (numberOfWeakPunches <= 3)
        {
            punchWeakDelay = 0.2f;
            if (chronometerWeakDelay > punchWeakDelay)
            {
                Weakchronometer = false;
                chronometerWeakDelay = 0;
            }
        }
    }
    private void DelayForAirPunches()
    {
        airPunchChronometerDelay += Time.deltaTime;
        airPunchDelay = 1f;
        if (airPunchChronometerDelay > airPunchDelay)
        {
            airPunchChronometer = false;
            airPunchChronometerDelay = 0;
            Debug.Log("Delay en el aire completado");
        }

    }
    private void Update()
    {
        if (_movementInputPressed)
        {
            Vector3 move = new Vector3(MoveDir.x, 0, MoveDir.y);
            controller.Move(move.normalized * speed * Time.deltaTime);
        }
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
        if (chronometer)
        {
            chronometerWeakDelay += Time.deltaTime;
            if (chronometerForLimitPunchWeak < chronometerWeakDelay)
            {
                Weakchronometer = false;
                chronometer = false;
                chronometerWeakDelay = 0;
                numberOfWeakPunches = 0;
                Debug.Log("Combo Cancelado");
            }
            if (Weakchronometer)
            {
                DelayForWeakPunches();
            }
            else if (airPunchChronometer)
            {
                DelayForAirPunches();
            }
        }


    }
}
