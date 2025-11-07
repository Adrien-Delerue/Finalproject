using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class SpawnerMobs : MonoBehaviour
{
    [SerializeField] private float radiusMin;
    [SerializeField] private float radiusMax; 
    [SerializeField] private GameObject mobObject;
    [SerializeField] private GameObject ammoObject;
	[SerializeField] public Transform flagTarget;

	// Time between 2 waves
	private float timeBetweenWaves = 5;

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

			int nbMob = ScoreManager.instance.score / 500 + 1;
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

			GameObject mob = Instantiate(mobObject, position, rotation);
			mob.SetActive(false);
			Enemy enemy = mob.GetComponent<Enemy>();
            if (enemy != null && flagTarget != null)
            {
                enemy.Init(flagTarget);
                mob.SetActive(true);
            }
		}

		// Spawn ammo with 50% chance
		if (Random.value < 0.5f){
            Vector3 ammoPosition = SpawnUtils.GetRandomPosition(radiusMin, radiusMax, angleMax, 0.5f);
            Instantiate(ammoObject, ammoPosition, Quaternion.identity);
        }
    }
}
