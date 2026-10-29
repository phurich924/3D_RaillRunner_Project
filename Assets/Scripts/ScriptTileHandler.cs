using UnityEngine;

public class ScriptTileHandler : MonoBehaviour
{
    public Transform[] spawmPoints;
    public GameObject[] ObstaclePrefab;
    public GameObject coinPrefab;
    public GameObject[] PowerUpPrefab;

    [Header("Spawn Probabilities (%)")]
    [Range(0, 100)] public int obstaclePercent = 25;
    [Range(0, 100)] public int powerUpPercent = 25;
    [Range(0, 100)] public int coinPercent = 40;

    public void Populate(bool allowSpawns)
    {
        if (!allowSpawns) return;
        if (spawmPoints == null || spawmPoints.Length == 0) return;

        foreach (Transform pt in spawmPoints)
        {
            if (pt == null) continue;

            int roll = Random.Range(1, 101);

            // 1. Obstacle (Carts sit directly on the floor at yOffset = 0)
            if (roll <= obstaclePercent && ObstaclePrefab != null && ObstaclePrefab.Length > 0)
            {
                Spawn(ObstaclePrefab[Random.Range(0, ObstaclePrefab.Length)], pt, 0f);
            }
            // 2. Power-Ups (hover at yOffset = 0.6)
            else if (roll <= (obstaclePercent + powerUpPercent) && PowerUpPrefab != null && PowerUpPrefab.Length > 0)
            {
                Spawn(PowerUpPrefab[Random.Range(0, PowerUpPrefab.Length)], pt, 0.6f);
            }
            // 3. Coins (hover at yOffset = 0.6)
            else if (roll <= (obstaclePercent + powerUpPercent + coinPercent) && coinPrefab != null)
            {
                Spawn(coinPrefab, pt, 0.6f);
            }
        }
    }

    void Spawn(GameObject prefab, Transform pt, float yOffset)
    {
        if (prefab == null || pt == null) return;
        Vector3 spawnPos = pt.position + Vector3.up * yOffset;
        Instantiate(prefab, spawnPos, pt.rotation, transform);
    }
}