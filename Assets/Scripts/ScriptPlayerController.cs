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

    public float hitCooldown = 0.6f;
    public float hitTimer;

    public float invincibleDuration = 10f, magnetDuration = 10f;
    public float magnetRadius = 3.5f, magnetPullSpeed = 14f;
    public float invincibleTimer, magnetTimer;

    public bool IsInvincible => invincibleTimer > 0f;
    public bool IsMagnetActive => magnetTimer > 0f;

    public void ActivateInvincibility() => invincibleTimer = invincibleDuration;
    public void ActivateMagnet() => magnetTimer = magnetDuration;

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

        if (invincibleTimer > 0f) invincibleTimer -= Time.deltaTime;
        if (magnetTimer > 0f) magnetTimer -= Time.deltaTime;
        HandleMagnet();

        if (hitTimer > 0f) hitTimer -= Time.deltaTime;
    }
    
    void HandleMagnet()
    {
        if (magnetTimer <= 0f) return;
        foreach (Collider c in Physics.OverlapSphere(transform.position, magnetRadius))
        {
            if (c.CompareTag("Coin"))
            {
                c.transform.position = Vector3.MoveTowards(
                    c.transform.position,
                    transform.position,
                    magnetPullSpeed * Time.deltaTime
                );
            }
        }
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

        // Run forward
        move.z = forwardSpeed;

        // Lane change left-right
        float newX = Mathf.MoveTowards(transform.position.x, targetX, laneChangeSpeed * Time.deltaTime);
        move.x = (newX - transform.position.x) / Time.deltaTime;

        // Jump and gravity
        if (controller.isGrounded)
        {
            verticalVelocity = -2f;
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

        controller.Move(move * Time.deltaTime);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Coin"))
        {
            if (ScriptGameManager.Instance != null)
            {
                ScriptGameManager.Instance.AddCoin();
            }
            else
            {
                Debug.LogWarning("ScriptGameManager.Instance is null! Make sure ScriptGameManager is in the scene and assigns Instance in Awake().");
            }

            Destroy(other.gameObject);
        }
        else if (other.CompareTag("PowerUp"))
        {
            if (other.TryGetComponent<ScriptPowerUp>(out var p))
            {
                p.Apply(this);
            }
            Destroy(other.gameObject);
        }
        else if (other.CompareTag("Obstacle"))
        {
            if (IsInvincible || hitTimer > 0f) return;
            hitTimer = hitCooldown;

            if (ScriptGameManager.Instance != null)
            {
                ScriptGameManager.Instance.TakeDamage(1);
            }
            else
            {
                Debug.LogWarning("ScriptGameManager.Instance is null when hitting an obstacle!");
            }
        }
    }
}