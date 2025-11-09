using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class SpawnerMobs : MonoBehaviour
{
	[System.Serializable]
	public class EnemySpawnData
	{
		public GameObject prefab;
		[Range(0, 100)] public float spawnWeight;
	}

	[SerializeField] private float radiusMin;
    [SerializeField] private float radiusMax; 
	[SerializeField] private EnemySpawnData[] enemies;
	[SerializeField] private GameObject ammoPrefab;
	[SerializeField] private Transform flagTarget;
	[SerializeField] private int maxMobPerWave = 7;

	// Time between 2 waves
	private float timeBetweenWaves = 5f;

	void Start()
    {
        if (flagTarget == null) {
            Debug.LogError("Flag is not assigned in SpawnerMobs.");
			return;
        }

		// Start the coroutine that spawns mobs
		StartCoroutine(SpawnLoop());
    }

    IEnumerator SpawnLoop()
    {
		while (true)
        {
		    yield return new WaitForSeconds(timeBetweenWaves);
            if (timeBetweenWaves < 20f)
            {
                timeBetweenWaves += 5f;
			}

			int nbMob = Math.Min(ScoreManager.instance.score / 500 + 1, maxMobPerWave);
			int angleMax = Mathf.Min(Mathf.Max(45, Mathf.RoundToInt(ScoreManager.instance.score * 0.1f)), 360);

			SpawnWave(nbMob, angleMax);
        }
    }

    void SpawnWave(int nbMob, int angleMax)
    {
		// Spawn mobs
		for (int i = 0; i < nbMob; i++)
        {
            Vector3 position = SpawnUtils.GetRandomPosition(radiusMin, radiusMax, angleMax, 2f);

			// Create the rotation toward the center
			Quaternion rotation = Quaternion.LookRotation(-position.normalized, Vector3.up);

			// Spawn a random enemy
			SpawnRandomEnemy(position, rotation);
		}

		// Spawn nbAmmo with 50% chance
		if (Random.value < 0.5f){
            int nbAmmo = nbMob / 3 + 1;
            for (int i = 0; i < nbAmmo; i++)
            {
			    Vector3 ammoPosition = SpawnUtils.GetRandomPosition(radiusMin, radiusMax, angleMax, 0.5f);
                Instantiate(ammoPrefab, ammoPosition, Quaternion.identity);
			}
        }
    }

    private void SpawnRandomEnemy(Vector3 position, Quaternion rotation)
    {
		float totalWeight = 0;
		foreach (var e in enemies) totalWeight += e.spawnWeight;

		float random = Random.Range(0, totalWeight);
		float current = 0;

		foreach (var e in enemies)
		{
			current += e.spawnWeight;
			if (random <= current)
			{
				GameObject newEnemy = Instantiate(e.prefab, position, rotation);
				newEnemy.SetActive(false);
				Enemy enemy = newEnemy.GetComponent<Enemy>();
				if (enemy != null && flagTarget != null)
				{
					enemy.Init(flagTarget);
					newEnemy.SetActive(true);
				}
				break;
			}
		}
	}
}
