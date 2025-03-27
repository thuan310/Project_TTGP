using UnityEngine;
using UnityEngine.Windows;

public class PlayerController : MonoBehaviour
{

    public Transform cam;
    public Rigidbody rb;

    public float speed = 6.0f;

    public float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;

    private Vector2 moveInput;

    private void OnEnable()
    {
        EventManager.instance.inputEvents.onMovePressed += HandleMovement;
    }

    private void OnDisable()
    {
        EventManager.instance.inputEvents.onMovePressed -= HandleMovement;
    }

    private void Awake()
    {
        rb = GetComponentInChildren<Rigidbody>();
    }

    private void HandleMovement(Vector2 input)
    {
        moveInput = input;
    }

    private void Update()
    {
        Vector3 direction = new Vector3(moveInput.x, 0f, moveInput.y).normalized;

        if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            rb.linearVelocity = moveDir * speed;
        }
        else
        {
            rb.linearVelocity = Vector3.zero;
        }
    }
}
