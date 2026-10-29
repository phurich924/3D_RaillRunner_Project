using UnityEngine;

public class ScriptPowerUp : MonoBehaviour
{
    public enum PowerUpType 
    { 
        Invincibility, 
        CoinMagnet 
    }

    [Header("Power-Up Settings")]
    public PowerUpType type = PowerUpType.Invincibility;
    public float rotateSpeed = 80f;
    public float bobHeight = 0.15f;
    public float bobSpeed = 2f;

    private float startLocalY;
    private bool isCollected = false;

    void Start()
    {
        startLocalY = transform.localPosition.y;

        // Force disable gravity and lock kinematic on start as a safeguard
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.useGravity = false;
            rb.isKinematic = true;
        }
    }

    void Update()
    {
        // Smooth rotation
        transform.Rotate(Vector3.up, rotateSpeed * Time.deltaTime, Space.Self);

        // Smooth hover up and down relative to the tile
        float newY = startLocalY + Mathf.Sin(Time.time * bobSpeed) * bobHeight;
        Vector3 localPos = transform.localPosition;
        localPos.y = newY;
        transform.localPosition = localPos;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isCollected) return;

        if (other.CompareTag("Player"))
        {
            isCollected = true;

            ScriptPlayerController player = other.GetComponent<ScriptPlayerController>();
            if (player != null)
            {
                Apply(player);
            }

            Destroy(gameObject);
        }
    }

    public void Apply(ScriptPlayerController player)
    {
        if (player == null) return;

        switch (type)
        {
            case PowerUpType.Invincibility:
                player.ActivateInvincibility();
                break;

            case PowerUpType.CoinMagnet:
                player.ActivateMagnet();
                break;
        }
    }
}