using System;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class Movement : MonoBehaviour
{
    public Vector2 MoveDir = Vector2.zero;
    private CharacterController controller;
    [SerializeField] float speed = 4f;
    //For the jump control
    [SerializeField] Vector3 velocity;
    [SerializeField] float jumpForce = 4f;
    [SerializeField] float gravity = -9.8f;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
    }
    public void OnMoveInput(InputAction.CallbackContext contextMove)
    {
        MoveDir = contextMove.ReadValue<Vector2>();
    }
    public void OnJumpInput(InputAction.CallbackContext contextJump)
    {
        if (contextJump.performed && controller.isGrounded)
        {
            velocity.y = MathF.Sqrt(jumpForce * -2 * gravity);
        }
    }
    private void Update()
    {
        Vector3 move = new Vector3(MoveDir.x, 0, MoveDir.y);
        controller.Move(move.normalized * speed * Time.deltaTime);

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}
