using UnityEngine;

public static class SpawnUtils
{
	public static Vector3 GetRandomPosition(float radiusMin, float radiusMax, int angleMax, float defaultY, float offsetY = 0.1f)
	{
		float randomRadius = Random.Range(radiusMin, radiusMax);
		int randomAngle = Random.Range(0, angleMax);
		float angle = 2 * Mathf.PI * randomAngle / 360f;

		float x = randomRadius * Mathf.Cos(angle);
		float z = randomRadius * Mathf.Sin(angle);
		float y = defaultY;

		Vector3 start = new(x, 50f, z);

		if (Physics.Raycast(start, Vector3.down, out RaycastHit hit))
		{
			if (hit.collider.CompareTag("Ground"))
			{
				y = hit.point.y + offsetY;
			}
		}

		return new Vector3(x, y, z);
	}
}