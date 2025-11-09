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

		// If player is within attack range, switch to Attack state
		if (distToPlayer <= attackRadius && state != State.Attack)
		{
			agent.isStopped = true;
			state = State.Attack;
		}

		bool mustChase = distToPlayer <= chaseRadius && distToFlag >= focusFlagRadius;

		Debug.Log(state);
		switch (state)
		{
			case State.ToFlag:
				agent.isStopped = false;

				if (distToFlag > reachedFlagRadius)
				{
					if (!agent.pathPending) agent.SetDestination(flagTarget.position);
				} else
				{
					if (!agent.pathPending)
					{
						Vector3 randomDir = Random.insideUnitSphere * (reachedFlagRadius * 0.9f);
						randomDir.y = 0; // Keep on the same horizontal plane
						Vector3 randomPoint = flagTarget.position + randomDir;

						agent.SetDestination(randomPoint);
					}
				}

				// Player close enough and flag far enough -> chase player
				if (mustChase) state = State.Chase;

				break;

			case State.Chase:
				agent.isStopped = false;
				agent.SetDestination(player.transform.position);

				// Player out of chase range or flag too close -> go to flag
				if (!mustChase) state = State.ToFlag;
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
