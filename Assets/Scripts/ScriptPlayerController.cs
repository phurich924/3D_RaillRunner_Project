
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

    [Header("Hit Settings")]
    public float hitCooldown = 0.6f;
    public float hitTimer;

    [Header("Power-Up Duration")]
    public float invincibleDuration = 10f;
    public float magnetDuration = 10f;
    public float doubleCoinDuration = 10f;

    [Header("Magnet Settings")]
    public float magnetRadius = 3.5f;
    public float magnetPullSpeed = 14f;

    [Header("Power-Up Timers")]
    public float invincibleTimer;
    public float magnetTimer;
    public float doubleCoinTimer;

    public bool IsInvincible => invincibleTimer > 0f;
    public bool IsMagnetActive => magnetTimer > 0f;
    public bool IsDoubleCoinActive => doubleCoinTimer > 0f;

    public void ActivateInvincibility()
    {
        invincibleTimer = invincibleDuration;
    }

    public void ActivateMagnet()
    {
        magnetTimer = magnetDuration;
    }

    public void ActivateDoubleCoin()
    {
        doubleCoinTimer = doubleCoinDuration;
    }

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

        if (invincibleTimer > 0f)
            invincibleTimer -= Time.deltaTime;

        if (magnetTimer > 0f)
            magnetTimer -= Time.deltaTime;

        if (doubleCoinTimer > 0f)
            doubleCoinTimer -= Time.deltaTime;

        HandleMagnet();

        if (hitTimer > 0f)
            hitTimer -= Time.deltaTime;
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

        move.z = forwardSpeed;

        float newX = Mathf.MoveTowards(
            transform.position.x,
            targetX,
            laneChangeSpeed * Time.deltaTime
        );

        move.x = (newX - transform.position.x) / Time.deltaTime;

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
                int coinAmount = IsDoubleCoinActive ? 2 : 1;
                ScriptGameManager.Instance.AddCoin(coinAmount);
            }
            else
            {
                Debug.LogWarning("ScriptGameManager.Instance is null!");
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
                Debug.LogWarning("ScriptGameManager.Instance is null!");
            }
        }
        else if (other.CompareTag("boxcoin"))
        {
            if (ScriptGameManager.Instance != null)
            {
                int coinAmount = IsDoubleCoinActive ? 20 : 10;
                ScriptGameManager.Instance.AddCoin(coinAmount);
            }

            Destroy(other.gameObject);
        }
    }
}
