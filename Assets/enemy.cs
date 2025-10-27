using UnityEngine;

public class Enemy : MonoBehaviour
{
    private float maxHealth = 30f;
    private float currentHealth;
    
    

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        Debug.Log(gameObject.name + " a pris " + amount + " dégâts. PV restants : " + currentHealth);

        if (currentHealth <= 0f)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log(gameObject.name + " est mort !");

        if (ScoreManager.instance != null)
        {
            ScoreManager.instance.AddScore(100);
        }

        Destroy(gameObject);
    }
}
