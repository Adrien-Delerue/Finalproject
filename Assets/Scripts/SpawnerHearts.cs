using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpawnerHearts : MonoBehaviour
{
    [SerializeField] private float radiusMin = 10f;
    [SerializeField] private float radiusMax = 30f;
    [SerializeField] public GameObject heartPrefab;

    [Header("Timing")]
    [SerializeField] private float spawnInterval = 10f;
    [SerializeField] private float spawnChance = 0.3f; 

    [Header("Spawn Area")]
    [SerializeField] private int maxAngle = 360; 
	[SerializeField] private float spawnYOffset = 1.25f;

	void Start()
    {
        StartCoroutine(SpawnLoop());
    }

    IEnumerator SpawnLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);

            if (Random.value < spawnChance)
            {
                SpawnHeart();
            }
        }
    }

    void SpawnHeart()
    {
        Vector3 spawnPosition = SpawnUtils.GetRandomPosition(radiusMin, radiusMax, maxAngle, 1f, spawnYOffset);
        GameObject heart = Instantiate(heartPrefab, spawnPosition, Quaternion.identity);
    }
}