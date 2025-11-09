using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Enemy : MonoBehaviour
{
	private Transform targetTransform;
	private GameObject player;
	private NavMeshAgent agent;

	[SerializeField] private EnemyHealthBar enemyHealthBar;
	[SerializeField] private float focusFlagRadius = 10f;
	[SerializeField] private float reachedFlagRadius = 2.5f;
	[SerializeField] private float chaseRadius = 10f;
	[SerializeField] private float attackRadius = 2f;
	[SerializeField] private float attackCooldown = 1.25f;
	[SerializeField] private int playerDamage = 10;
	[SerializeField] private string playerTag = "Player";
	[SerializeField] private float maxHealth = 20f;
	[SerializeField] private int killPoints = 100;

	private float currentHealth;
	private bool isDead = false;
	private float lastAttackTime = -Mathf.Infinity;
	enum State { MoveToObjective, Patrol, Chase, Attack }
	private State state = State.MoveToObjective;

	private Animator animator;

	public AudioSource attackAudio;
	public AudioSource deathAudio;

	public void Init(Transform targetTransformParam)
	{
		currentHealth = maxHealth;
		agent = GetComponent<NavMeshAgent>();
		animator = GetComponent<Animator>(); // getting the Animator component
		player = PlayerController.instance?.gameObject;
		targetTransform = targetTransformParam;
		if (agent == null || targetTransform == null || player == null || transform == null)
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
		if (targetTransform == null)
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
		float distToObjective = Vector3.Distance(transform.position, targetTransform.position);

		// Priority : If player is within attack range, switch to Attack state
		if (distToPlayer <= attackRadius && state != State.Attack)
		{
			agent.isStopped = true;
			state = State.Attack;
		}

		bool mustChase = distToPlayer <= chaseRadius && distToObjective >= focusFlagRadius;

		switch (state)
		{
			case State.MoveToObjective:
				agent.isStopped = false;

				// Move toward the objective until close enough
				if (!agent.pathPending && distToObjective > reachedFlagRadius)
				{
					agent.SetDestination(targetTransform.position);
				}

				// Once reached, transition to Patrol state
				if (distToObjective <= reachedFlagRadius)
				{
					state = State.Patrol;
				}

				// Player detected while moving (but flag far enough) -> chase
				if (mustChase) state = State.Chase;
				break;

			case State.Patrol:
				agent.isStopped = false;

				// Generate random patrol points around the objective
				if (!agent.pathPending)
				{
					Vector3 randomDir = Random.insideUnitSphere * (reachedFlagRadius * 0.9f);
					randomDir.y = 0; // Keep on the same horizontal plane
					Vector3 randomPoint = targetTransform.position + randomDir;

					agent.SetDestination(randomPoint);
				}
				break;

			case State.Chase:
				agent.isStopped = false;

				// Continuously update destination to player's position
				agent.SetDestination(player.transform.position);

				// Player out of chase range or too close to objective -> return
				if (!mustChase) 
				{
					state = State.MoveToObjective;
				}
				break;

			case State.Attack:
				// Face the player (only rotate on Y axis)
				Vector3 lookPos = player.transform.position;
				lookPos.y = transform.position.y;
				transform.LookAt(lookPos);

				// Attack if cooldown has elapsed
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
		enemyHealthBar.SetHealthPercent(currentHealth / maxHealth);

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
		GetComponent<Collider>().enabled = false;
		enabled = false;
		if (ScoreManager.instance != null)
		{
			ScoreManager.instance.AddScore(killPoints);
		}
		StartCoroutine(WaitForAnimationAndDestroy());
	}

	private IEnumerator WaitForAnimationAndDestroy()
	{
		yield return new WaitForSeconds(0.7f);
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
