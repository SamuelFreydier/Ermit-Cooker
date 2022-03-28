using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    public GameObject prefabToSpawn;
    public float spawnFrequency = 10.0f;
    public int startQuantity = 6;
    private CapsuleCollider2D[] spawnAreas;

    private float t = 0.0f;

    private Vector2 size;
    private Vector2 center;

    private void Awake()
    {
        spawnAreas = GetComponents<CapsuleCollider2D>();
    }

    void Start()
    {
        for( int i = 0; i < startQuantity; ++i )
        {
            SpawnPrefab();
        }
    }

    private void Update()
    {
        t += Time.deltaTime;

        if( t >= spawnFrequency )
        {
            t = 0.0f;
            SpawnPrefab();
        }
    }

    private void SpawnPrefab()
    {
        Instantiate(prefabToSpawn, GetRandomPosition(), Quaternion.identity);
    }

    private Vector2 GetRandomPosition()
    {
        int randomZone = Random.Range(0, spawnAreas.Length);
        CapsuleCollider2D spawnArea = spawnAreas[randomZone];
        center = spawnArea.bounds.center;
        size.x = spawnArea.transform.localScale.x * spawnArea.size.x;
        size.y = spawnArea.transform.localScale.y * spawnArea.size.y;
        Vector2 randomPosition = new Vector2(Random.Range(-size.x / 2, size.x / 2), Random.Range(-size.y / 2, size.y / 2));

        return center + randomPosition;
    }
}
