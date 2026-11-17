using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PurpleHerbSpawner : MonoBehaviour
{
    public GameObject PurpleHerb;

    [Header("Spawn Setting")]
    public float spawnRadius = 15f;

    [Header("Sorting Settings")]
    public int sortingOrder = 1;

    private void Start()
    {
        InvokeRepeating("SpawnHerb", 5f, 10f);
    }

    void SpawnHerb()
    {
        Vector2 randomPoint = Random.insideUnitCircle * spawnRadius;
        Vector2 spawnPosition = new Vector2
            (transform.position.x + randomPoint.x,
            transform.position.y + randomPoint.y);
        Instantiate(PurpleHerb, spawnPosition, Quaternion.identity);
    }
}