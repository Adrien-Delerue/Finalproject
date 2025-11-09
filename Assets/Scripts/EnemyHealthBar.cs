using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
	[SerializeField] private Slider healthSlider;

	void LateUpdate()
    {
		transform.rotation = Camera.main.transform.rotation;
	}

	public void SetHealthPercent(float percent)
	{
		healthSlider.value = percent;
	}
}