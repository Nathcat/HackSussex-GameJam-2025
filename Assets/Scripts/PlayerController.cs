using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private float walkSpeed = 10;
    [SerializeField] private float runSpeed = 2;

    new private Rigidbody2D rigidbody;
    private InputAction sprintAction;
    private InputAction moveAction;

    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        moveAction = InputSystem.actions.FindAction("Move");
        sprintAction = InputSystem.actions.FindAction("Sprint");
    }

    void Update()
    {
        Vector2 velocity = moveAction.ReadValue<Vector2>();
        velocity *= walkSpeed;

        if (sprintAction.IsPressed()) velocity *= runSpeed;

        rigidbody.linearVelocity = velocity;
    }
}
