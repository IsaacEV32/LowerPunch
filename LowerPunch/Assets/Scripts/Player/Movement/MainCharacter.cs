using System;
using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class MainCharacter : MonoBehaviour
{
    internal float health = 100;
    internal float specialPoints = 0;

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

    private bool strongPunchChronometer = false;
    private float strongPunchChronometerDelay = 0;
    private float strongPunchDelay = 0.6f;

    private bool specialActivated = false;
    private float specialDamageTimer;
    private float specialDamageDuration = 3;

    internal bool increaseSpecialBar = false;
    internal bool changeHealthPoints = false;
    private void Awake()
    {
        HUD.SetReferencePlayer(this);
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position + new Vector3(1, 0, 0), new Vector3(1f, 1.25f, 1f));
    }
    private void Start()
    {
        controller = GetComponent<CharacterController>();
    }
    public void OnMoveInput(InputAction.CallbackContext contextMove)
    {
        if (contextMove.started && !specialActivated)
        {
            _movementInputPressed = true;
        }
        else if (contextMove.canceled)
        {
            _movementInputPressed = false;
        }
        MoveDir = contextMove.ReadValue<Vector2>();
        if (Input.GetKeyDown(KeyCode.A) && !specialActivated)
        {
            lookLeft = true;
        }
        else if (Input.GetKeyDown(KeyCode.D) && !specialActivated)
        {
            lookLeft = false;
        }
    }
    public void OnJumpInput(InputAction.CallbackContext contextJump)
    {
        if (contextJump.performed && controller.isGrounded && !specialActivated)
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
        if (contextPunch.performed && controller.isGrounded && !Weakchronometer && !specialActivated)
        {
            chronometer = true;
            Weakchronometer = true;
            if (lookLeft)
            {
                Collider[] colliders = Physics.OverlapBox(transform.position + new Vector3(-1, 0, 0), new Vector3(1f, 1.25f, 1f), Quaternion.identity);
                foreach (Collider collider in colliders)
                {
                    if (collider.TryGetComponent<Enemy>(out Enemy E) && specialPoints < HUD.maxSpecial)
                    {
                        E.ReceiveDamage(5);
                        increaseSpecialBar = true;
                        specialPoints += 5;
                    }
                }
            }
            else if (!lookLeft)
            {
                Collider[] colliders = Physics.OverlapBox(transform.position + new Vector3(1, 0, 0), new Vector3(1f, 1.25f, 1f), Quaternion.identity);
                foreach (Collider collider in colliders)
                {
                    if (collider.TryGetComponent<Enemy>(out Enemy E) && specialPoints < HUD.maxSpecial)
                    {
                        E.ReceiveDamage(5);
                        increaseSpecialBar = true;
                        specialPoints += 5;
                    }
                }
            }
            numberOfWeakPunches++;
        }
        else if (contextPunch.performed && !controller.isGrounded && !airPunchChronometer && !specialActivated)
        {
            airPunchChronometer = true;
            if (lookLeft)
            {
                Collider[] colliders = Physics.OverlapBox(transform.position + new Vector3(-1, -1, 0), new Vector3(1f, 1f, 1f), Quaternion.identity);
                foreach (Collider collider in colliders)
                {
                    if (collider.TryGetComponent<Enemy>(out Enemy E) && specialPoints < HUD.maxSpecial)
                    {
                        E.ReceiveDamage(5);
                        increaseSpecialBar = true;
                        specialPoints += 5;
                    }
                }
            }
            else if (!lookLeft)
            {
                Collider[] colliders = Physics.OverlapBox(transform.position + new Vector3(1, -1, 0), new Vector3(1f, 1f, 1f), Quaternion.identity);
                foreach (Collider collider in colliders)
                {
                    if (collider.TryGetComponent<Enemy>(out Enemy E) && specialPoints < HUD.maxSpecial)
                    {
                        E.ReceiveDamage(5);
                        increaseSpecialBar = true;
                        specialPoints += 5;
                    }
                }
            }
        }
    }
    public void OnStrongPunch(InputAction.CallbackContext contextHardPunch)
    {
        if (contextHardPunch.performed && controller.isGrounded && contextHardPunch.duration > 0.3f && !specialActivated)
        {
            strongPunchChronometer = true;
            Weakchronometer = false;
            chronometer = false;
            chronometerWeakDelay = 0;
            numberOfWeakPunches = 0;

            if (lookLeft)
            {
                Collider[] colliders = Physics.OverlapBox(transform.position + new Vector3(-1, 0, 0), new Vector3(2f, 1.25f, 1f), Quaternion.identity);
                foreach (Collider collider in colliders)
                {
                    if (collider.TryGetComponent<Enemy>(out Enemy E) && specialPoints < HUD.maxSpecial)
                    {
                        E.ReceiveDamage(10);
                        increaseSpecialBar = true;
                        specialPoints += 10;
                    }
                }
            }
            else if (!lookLeft)
            {
                Collider[] colliders = Physics.OverlapBox(transform.position + new Vector3(1, 0, 0), new Vector3(2f, 1.25f, 1f), Quaternion.identity);
                foreach (Collider collider in colliders)
                {
                    if (collider.TryGetComponent<Enemy>(out Enemy E) && specialPoints < HUD.maxSpecial)
                    {
                        E.ReceiveDamage(10);
                        increaseSpecialBar = true;
                        specialPoints += 10;
                    }
                }
            }
        }
    }
    public void OnSpecialPunch(InputAction.CallbackContext contextSpecial)
    {
        if (contextSpecial.performed && controller.isGrounded && contextSpecial.duration > 0.4f && specialPoints >= 50)
        {
            specialActivated = true;
        }
    }
    private void SpecialPunch()
    {
        if (lookLeft)
        {
            Collider[] colliders = Physics.OverlapBox(transform.position + new Vector3(4f, 0, 0), new Vector3(7f, 1.25f, 1f), Quaternion.identity);
            foreach (Collider collider in colliders)
            {
                if (collider.TryGetComponent<Enemy>(out Enemy E))
                {
                    E.ReceiveDamage(15);
                }
            }
        }
        else if (!lookLeft)
        {
            Collider[] colliders = Physics.OverlapBox(transform.position + new Vector3(-4f, 0, 0), new Vector3(7f, 1.25f, 1f), Quaternion.identity);
            foreach (Collider collider in colliders)
            {
                if (collider.TryGetComponent<Enemy>(out Enemy E))
                {
                    E.ReceiveDamage(15);
                }
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
        }

    }
    private void DelayForStrongPunches()
    {
        strongPunchChronometerDelay += Time.deltaTime;
        if (strongPunchChronometerDelay > strongPunchDelay)
        {
            strongPunchChronometer = false;
            strongPunchChronometerDelay = 0;
        }
    }
    private void HandlerAttacksDelay()
    {
        if (chronometer)
        {
            chronometerWeakDelay += Time.deltaTime;
            if (chronometerForLimitPunchWeak < chronometerWeakDelay)
            {
                Weakchronometer = false;
                chronometer = false;
                chronometerWeakDelay = 0;
                numberOfWeakPunches = 0;
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
        else if (strongPunchChronometer)
        {
            DelayForStrongPunches();
        }
    }
    private void Update()
    {
        if (_movementInputPressed && !specialActivated)
        {
            Vector3 move = new Vector3(MoveDir.x, 0, MoveDir.y);
            controller.Move(move.normalized * speed * Time.deltaTime);
        }
        if (!specialActivated)
        {
            velocity.y += gravity * Time.deltaTime;
            controller.Move(velocity * Time.deltaTime);
        }
        HandlerAttacksDelay();
        if (specialActivated)
        {
            if (specialDamageTimer <= 0)
            {
                specialDamageTimer = specialDamageDuration;
                specialActivated = false;
            }
            else
            {
                SpecialPunch();
                specialDamageTimer -= Time.deltaTime;
            }
            ResetSpecialPoints(true);
        }
    }
    public virtual void ReceiveDamage(int damagePoints)
    {
        if (health > 0)
        {
            health -= damagePoints;
            changeHealthPoints = true;
            Debug.Log(health);
        }
    }
    private void ResetSpecialPoints(bool check)
    {
        if (check && specialDamageTimer <= 0)
        {
            increaseSpecialBar = true;
            specialPoints = 0;
        }
    }
}
