using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpawnerHearts : MonoBehaviour
{
    [SerializeField] private float radiusMin = 10f;
    [SerializeField] private float radiusMax = 30f;
    [SerializeField] private GameObject heartPrefab;

    [Header("Timing")]
    [SerializeField] private float spawnInterval = 15f;
    [SerializeField] private float spawnChance = 0.3f;

    [Header("Spawn Area")]
    [SerializeField] private int maxAngle = 360;
    [SerializeField] private float spawnHeight = 1f;

    void Start()
    {
        StartCoroutine(SpawnLoop());
    }

    IEnumerator SpawnLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);

			// Spawn with a probability
			if (Random.value < spawnChance)
            {
                SpawnHeart();
            }
        }
    }

    void SpawnHeart()
    {
        Vector3 spawnPosition = GetRandomPosition(maxAngle);
        Instantiate(heartPrefab, spawnPosition, Quaternion.identity);
    }

    Vector3 GetRandomPosition(int angleMax)
    {
        float randomRadius = Random.Range(radiusMin, radiusMax);
        int randomAngle = Random.Range(0, angleMax);
        float angle = 2 * Mathf.PI * randomAngle / 360f;

        return new Vector3(
            randomRadius * Mathf.Cos(angle),
            spawnHeight,
            randomRadius * Mathf.Sin(angle)
        );
    }
}