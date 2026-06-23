using UnityEngine;
using UnityEngine.InputSystem;

public class InputPlayer : MonoBehaviour
{
    [SerializeField] public InputActionAsset input;
    [SerializeField] private string mapName = "Player";
    [SerializeField] private float walkSpeed = 5f;
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private Rigidbody rb;

    [SerializeField] private int turnSpeed = 100;
    private bool isGrounded = true;

    InputActionMap map;
    InputAction moveAction;
    InputAction jumpAction;
    InputAction sprintAction;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        map = input.FindActionMap(mapName);
        moveAction = map.FindAction("Move");
        jumpAction = map.FindAction("Jump");
        sprintAction = map.FindAction("Sprint");
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 moveInput = moveAction.ReadValue<Vector2>();

        // Vooruit/achteruit bewegen
        float speed = walkSpeed * moveInput.y;
        if (sprintAction.IsPressed()) speed *= 2f;
        Vector3 movement = transform.forward * speed * Time.deltaTime;
        float angle = moveInput.x * turnSpeed * Time.deltaTime;
        transform.Rotate(0f, angle, 0f, Space.World);
        transform.Translate(movement, Space.World);
        if (jumpAction.WasPressedThisFrame() && isGrounded)
        {
         rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
         isGrounded = false;
        }
    }
}
