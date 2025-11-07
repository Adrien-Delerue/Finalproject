using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class SpawnerMobs : MonoBehaviour
{
    [SerializeField] private float RadiusMin;
    [SerializeField] private float RadiusMax; 
    [SerializeField] private GameObject mobObject;
    [SerializeField] private GameObject ammoObject;
	[SerializeField] public Transform flagTarget;

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
		yield return new WaitForSeconds(5f);
		Debug.Log("Beginning to spawn mobs.");

		while (true)
        {
			int nbMob = ScoreManager.instance.score / 500 + 1;
			int angleMax = Mathf.Min(Mathf.Max(45, Mathf.RoundToInt(ScoreManager.instance.score * 0.1f)), 360);

			SpawnWave(nbMob, angleMax);
			yield return new WaitForSeconds(7f);
        }
    }

    void SpawnWave(int nbMob, int angleMax)
    {
        for (int i = 0; i < nbMob; i++)
        {
            GameObject mob = Instantiate(mobObject, GetRandomPosition(angleMax), Quaternion.identity);
			mob.SetActive(false);
			Enemy enemy = mob.GetComponent<Enemy>();
            if (enemy != null && flagTarget != null)
            {
                enemy.Init(flagTarget);
                mob.SetActive(true);
            }
		}
        if (Random.value < 0.5f){
            Vector3 ammoPosition = GetRandomPosition(angleMax);
            ammoPosition.y = 0.5f;
            Instantiate(ammoObject, ammoPosition, Quaternion.identity);
        }
    }

    Vector3 GetRandomPosition(int angleMax) {
        float randomRadius = Random.Range(RadiusMin, RadiusMax);
        int randomAngle = Random.Range(0, angleMax);
        float angle = 2 * Mathf.PI * randomAngle / 360f;

        float x = randomRadius * Mathf.Cos(angle);
        float z = randomRadius * Mathf.Sin(angle);

		Ray ray = new(new Vector3(x, 200f, z), Vector3.down);
		RaycastHit hit;

        float y = Physics.Raycast(ray, out hit) ? (hit.point.y + 0.5f) : 2f;

		return new Vector3(x, y, z);
	}

    void BeginSpawning()
    {
        // This function can be used to trigger any initial setup before spawning begins
        Debug.Log("Beginning to spawn mobs.");
	}
}
