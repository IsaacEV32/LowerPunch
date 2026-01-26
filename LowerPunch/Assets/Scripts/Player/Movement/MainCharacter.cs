using System;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class MainCharacter : MonoBehaviour
{
    public Vector2 MoveDir = Vector2.zero;
    private CharacterController controller;
    [SerializeField] float speed = 4f;
    //For the jump control
    [SerializeField] Vector3 velocity;
    [SerializeField] float jumpForce = 4f;
    [SerializeField] float gravity = -9.8f;

    private float jumpTimeStamp;
    private float jumpTime = 0.2f;
    private bool jumpButtonPressed = false;
    private bool _movementInputPressed = false;
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
    private void Update()
    {
        if (_movementInputPressed)
        {
            Vector3 move = new Vector3(MoveDir.x, 0, MoveDir.y);
            controller.Move(move.normalized * speed * Time.deltaTime);
        }
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}
