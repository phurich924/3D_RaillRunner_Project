using System.Collections.Generic;
using UnityEngine;

public class ScriptTileManager : MonoBehaviour
{
    public Transform player;
    public GameObject[] tilePrefabs;
    public float tileLength = 20f;
    public int tileOnScreen = 6;
    public int safeTiles = 1; // Only tile 0 is empty; tile 1 (20m away) will have items!

    private float nextSpawnZ = 0f;
    private int tileSpawned = 0;
    private readonly Queue<GameObject> activeTiles = new Queue<GameObject>();

    void Start()
    {
        for (int i = 0; i < tileOnScreen; i++) 
        {
            Spawntile();
        }
    }

    void Update()
    {
        float triggerZ = nextSpawnZ - (tileOnScreen - 1) * tileLength;
        if (player.position.z > triggerZ) 
        { 
            Spawntile(); 
            RecycleOldestTile(); 
        }
    }

    void Spawntile()
    {
        if (tilePrefabs == null || tilePrefabs.Length == 0) return;

        GameObject tile = Instantiate(
            tilePrefabs[Random.Range(0, tilePrefabs.Length)],
            new Vector3(0, 0, nextSpawnZ), 
            Quaternion.identity, 
            transform
        );

        // Populate items starting right after the first tile
        tile.GetComponent<ScriptTileHandler>()?.Populate(tileSpawned >= safeTiles);
        activeTiles.Enqueue(tile);

        nextSpawnZ += tileLength; 
        tileSpawned++;
    }

    void RecycleOldestTile()
    {
        if (activeTiles.Count > tileOnScreen) 
        {
            Destroy(activeTiles.Dequeue());
        }
    }
}