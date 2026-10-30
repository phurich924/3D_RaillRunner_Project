
using UnityEngine;

public class ScriptPowerUp : MonoBehaviour
{
    public enum PowerUpType
    {
        Invincibility,
        CoinMagnet,
        DoubleCoin
    }

    [Header("Power-Up Settings")]
    public PowerUpType type = PowerUpType.Invincibility;
    public float rotateSpeed = 80f;
    public float bobHeight = 0.15f;
    public float bobSpeed = 2f;

    private float startLocalY;

    void Start()
    {
        startLocalY = transform.localPosition.y;

        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.useGravity = false;
            rb.isKinematic = true;
        }
    }

    void Update()
    {
        transform.Rotate(Vector3.up, rotateSpeed * Time.deltaTime, Space.Self);

        float newY = startLocalY + Mathf.Sin(Time.time * bobSpeed) * bobHeight;
        Vector3 localPos = transform.localPosition;
        localPos.y = newY;
        transform.localPosition = localPos;
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

            case PowerUpType.DoubleCoin:
                player.ActivateDoubleCoin();
                break;
        }
    }
}
