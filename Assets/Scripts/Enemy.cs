using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Enemy : MonoBehaviour
{
    private Transform flagTarget;
	private GameObject player;
	private NavMeshAgent agent;

	[SerializeField] private float focusFlagRadius = 10f;
	[SerializeField] private float chaseRadius = 10f;
	[SerializeField] private float attackRadius = 2f;
	[SerializeField] private float attackCooldown = 1.25f;
	[SerializeField] private int playerDamage = 10;
	[SerializeField] private string playerTag = "Player";
	[SerializeField] private float maxHealth = 20f;
    
    private float currentHealth;
    private bool isDead = false;
    private float lastAttackTime = -Mathf.Infinity;
	enum State { ToFlag, Chase, Attack }
	private State state = State.ToFlag;

	private Animator animator;

	public AudioSource attackAudio;
	public AudioSource deathAudio;

    public void Init(Transform flagTargetParam)
	{
		currentHealth = maxHealth;
		agent = GetComponent<NavMeshAgent>();
		animator = GetComponent<Animator>(); // getting the Animator component
        player = PlayerController.instance?.gameObject;
		flagTarget = flagTargetParam;
		if (agent == null || flagTarget == null || player == null || transform == null)
		{
			Debug.LogError("Enemy initialization error: missing components or references.");
			enabled = false;
		}
		else
		{
			enabled = true;
		}
		
    }

	void Update()
	{
		if (flagTarget == null)
		{
			Debug.LogError("Flag target is null. Enemy cannot operate.");
			return;
		}

		if (player == null)
		{
			Debug.Log("Player reference lost. Attempting to find player...");
			player = GameObject.FindWithTag(playerTag);
			if (player == null) return;
		}

		float distToPlayer = Vector3.Distance(transform.position, player.transform.position);
		float distToFlag = Vector3.Distance(transform.position, flagTarget.position);

		Debug.Log(state);
		switch (state)
		{
			case State.ToFlag:
				if (!agent.pathPending)
				{
					agent.SetDestination(flagTarget.position);
				}
				if (distToPlayer <= chaseRadius)
					state = State.Chase;
				break;

			case State.Chase:
				agent.isStopped = false;
				agent.SetDestination(player.transform.position);

				// Close enough -> attack
				if (distToPlayer <= attackRadius)
				{
					agent.isStopped = true;
					state = State.Attack;
				}

				// Player too far or flag too close -> go back to flag
				else if (distToPlayer > chaseRadius || distToFlag < focusFlagRadius)
				{
					state = State.ToFlag;
					agent.isStopped = false;
				}
				break;

			case State.Attack:
				// Face player (only rotate on Y)
				Vector3 lookPos = player.transform.position;
				lookPos.y = transform.position.y;
				transform.LookAt(lookPos);

				if (Time.time - lastAttackTime >= attackCooldown)
				{
					lastAttackTime = Time.time;
					AttackPlayer();
				}

				// If player moves out of attack range, chase again
				if (distToPlayer > attackRadius + 0.5f)
				{
					state = State.Chase;
					agent.isStopped = false;
				}
				break;
		}
	}

	public void TakeDamage(float amount)
    {
		if (isDead) return;
        currentHealth -= amount;
        if (currentHealth <= 0f)
        {
            Die();
        }
    }

    void Die()
    {
		//Show Death to player
        animator.SetTrigger("Death");
        deathAudio.Play();

		tag = "Untagged";

        isDead = true;
		agent.isStopped = true;
		agent.enabled = false;
		GetComponent<Collider>().enabled=false;
		enabled = false;
        if (ScoreManager.instance != null)
        {
            ScoreManager.instance.AddScore(100);
        }
        StartCoroutine(WaitForAnimationAndDestroy());
    }

    private IEnumerator WaitForAnimationAndDestroy()
    {
        yield return new WaitForSeconds(1.15f);
        Destroy(gameObject);
    }

    void AttackPlayer()
	{
		PlayerController playerController = player.GetComponent<PlayerController>();

        animator.SetTrigger("Attack");
        attackAudio.Play();

        if (playerController != null)
		{
			playerController.TakeDamage(playerDamage);
		}
	}
}
