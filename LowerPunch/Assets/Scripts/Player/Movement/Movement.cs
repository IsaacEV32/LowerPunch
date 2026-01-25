using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class Movement : MonoBehaviour
{
    public Vector2 MoveDir = Vector2.zero;
    private CharacterController controller;
    [SerializeField] float speed = 4;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
    }
    public void OnMoveInput(InputAction.CallbackContext context)
    {
        MoveDir = context.ReadValue<Vector2>();
    }
    private void Update()
    {
        Vector3 move = new Vector3(MoveDir.x, 0, MoveDir.y);
        controller.Move(move.normalized * speed * Time.deltaTime);
    }
}
