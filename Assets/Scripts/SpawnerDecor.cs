using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecorSpawner : MonoBehaviour
{
    public GameObject fence;
    private int fenceCount = 30;
    private float fenceRadius = 35f;

    public GameObject backgroundTree;
    private int treeCount = 50;
    private float treeRadius = 41f;

    public GameObject grass;
    private int grassCount = 100;


    void Start()
    {   // Instatiating fences
        SpawnInCircle(fence, fenceCount, fenceRadius);
        // Instantiating background tree
        SpawnInCircle(backgroundTree, treeCount, treeRadius);

        // Grass Spawn
        for (int i = 0; i < grassCount; i++)
        {
            float randomRadius = Random.Range(10, 34);
            int randomAngle = Random.Range(0, 360);
            float angle = 2 * Mathf.PI * randomAngle / 360f;

            Instantiate(grass, new Vector3(randomRadius * Mathf.Cos(angle), 0.35f, randomRadius * Mathf.Sin(angle)), Quaternion.identity);
        }
    }
    void SpawnInCircle(GameObject obj, int nb, float radius) {
        for (int i = 0; i < nb; i++)
        {
			// Calculate the position on the circle
			float angle = 2 * Mathf.PI * i / nb;
            Vector3 position = new Vector3(
                radius * Mathf.Cos(angle),
                0,
                radius * Mathf.Sin(angle)
            );

			// Create the rotation toward the center
			Quaternion rotation = Quaternion.LookRotation(-position.normalized, Vector3.up);

			// Instantiate the tree
			Instantiate(obj, position, rotation, transform);
        }
    }   

}
