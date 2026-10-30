
using UnityEngine;

public class ScriptTileHandler : MonoBehaviour
{
    public Transform[] spawmPoints;
    public GameObject[] ObstaclePrefab;
    public GameObject coinPrefab;
    public GameObject[] PowerUpPrefab;
    public GameObject boxcoinPrefab;

    [Header("Spawn Probabilities (%)")]
    [Range(0, 100)] public int obstaclePercent = 25;
    [Range(0, 100)] public int powerUpPercent = 25;
    [Range(0, 100)] public int coinPercent = 40;
    [Range(0, 100)] public int boxcoinPercent = 15;

    public void Populate(bool allowSpawns)
    {
        if (!allowSpawns) return;
        if (spawmPoints == null || spawmPoints.Length == 0) return;

        foreach (Transform pt in spawmPoints)
        {
            if (pt == null) continue;

            int roll = Random.Range(1, 101);
            GameObject prefab = null;
            float yOffset = 0.6f;

            if (roll <= obstaclePercent &&
                ObstaclePrefab != null && ObstaclePrefab.Length > 0)
            {
                prefab = ObstaclePrefab[Random.Range(0, ObstaclePrefab.Length)];
                yOffset = 0f;
            }
            else if (roll <= obstaclePercent + powerUpPercent &&
                     PowerUpPrefab != null && PowerUpPrefab.Length > 0)
            {
                prefab = PowerUpPrefab[Random.Range(0, PowerUpPrefab.Length)];
            }
            else if (roll <= obstaclePercent + powerUpPercent + coinPercent &&
                     coinPrefab != null)
            {
                prefab = coinPrefab;
            }
            else if (roll <= obstaclePercent + powerUpPercent + coinPercent + boxcoinPercent &&
                     boxcoinPrefab != null)
            {
                prefab = boxcoinPrefab;
            }

            if (prefab == null) continue;

            int lane = Random.Range(0, GameConstants.LaneCount);
            float laneX = GameConstants.LaneToX(lane);

            Vector3 spawnPos = new Vector3(
                laneX,
                pt.position.y + yOffset,
                pt.position.z
            );

            Instantiate(prefab, spawnPos, pt.rotation, transform);
        }
    }
}
