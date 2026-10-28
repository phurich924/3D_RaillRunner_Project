using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class ScriptPlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float forwardSpeed = 8f;
    public float laneChangeSpeed = 12f;

    [Header("Jump & Gravity")]
    public float jumpHeight = 1.5f;
    public float gravity = -25f;
    private float verticalVelocity;

    private CharacterController controller;
    private int currentLane;
    private float targetX;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        currentLane = GameConstants.CenterLane;
        targetX = GameConstants.LaneToX(currentLane);
    }

    void Update()
    {
        HandleLaneInput();
        Move();
    }

    void HandleLaneInput()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
        {
            currentLane = Mathf.Max(0, currentLane - 1);
        }

        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        {
            currentLane = Mathf.Min(GameConstants.LaneCount - 1, currentLane + 1);
        }

        targetX = GameConstants.LaneToX(currentLane);
    }

    void Move()
    {
        Vector3 move = Vector3.zero;

        // วิ่งไปข้างหน้า
        move.z = forwardSpeed;

        // การเปลี่ยนเลนซ้าย-ขวา
        float newX = Mathf.MoveTowards(transform.position.x, targetX, laneChangeSpeed * Time.deltaTime);
        move.x = (newX - transform.position.x) / Time.deltaTime;

        // ระบบกระโดดและแรงโน้มถ่วง
        if (controller.isGrounded)
        {
            verticalVelocity = -2f; // แรงกดตัวละครให้เกาะติดพื้นสม่ำเสมอ
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W))
            {
                verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);
            }
        }
        else
        {
            verticalVelocity += gravity * Time.deltaTime;
        }

        move.y = verticalVelocity;

        // สั่งเคลื่อนที่
        controller.Move(move * Time.deltaTime);
    }
}