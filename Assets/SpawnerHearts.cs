using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpawnerHearts : MonoBehaviour
{
    [SerializeField] private float radiusMin = 10f;
    [SerializeField] private float radiusMax = 30f;
    [SerializeField] private GameObject heartPrefab;

    [Header("Timing")]
    [SerializeField] private float spawnInterval = 15f; // Intervalle entre chaque spawn
    [SerializeField] private float spawnChance = 0.3f; // 30% de chance de spawn � chaque intervalle

    [Header("Spawn Area")]
    [SerializeField] private int maxAngle = 360; // Angle maximum pour la zone de spawn
    [SerializeField] private float spawnHeight = 1f; // Hauteur du spawn (Y)

    void Start()
    {
        StartCoroutine(SpawnLoop());
    }

    IEnumerator SpawnLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);

            // Spawn avec une probabilit�
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

    // Optionnel : Visualiser la zone de spawn dans l'�diteur
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, radiusMin);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radiusMax);
    }
}