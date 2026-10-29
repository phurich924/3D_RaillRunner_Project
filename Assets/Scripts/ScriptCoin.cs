using UnityEngine;

public class ScriptCoin : MonoBehaviour
{
    [Header("Rotation")]
    public float rotateSpeed = 120f;

    [Header("Bobbing Effect")]
    public float bobHeight = 0.15f;
    public float bobSpeed = 2f;

    private float baseLocalY;
    private bool isCollected = false;

    void Start()
    {
        baseLocalY = transform.localPosition.y;
    }

    void Update()
    {
        transform.Rotate(Vector3.up, rotateSpeed * Time.deltaTime, Space.Self);

        float newY = baseLocalY + Mathf.Sin(Time.time * bobSpeed) * bobHeight;
        Vector3 pos = transform.localPosition;
        pos.y = newY;
        transform.localPosition = pos;
    }

    // This ensures the coin collects itself if the player touches it!
    private void OnTriggerEnter(Collider other)
    {
        if (isCollected) return;

        if (other.CompareTag("Player"))
        {
            isCollected = true;
            if (ScriptGameManager.Instance != null)
            {
                ScriptGameManager.Instance.AddCoin();
            }
            Destroy(gameObject);
        }
    }
}