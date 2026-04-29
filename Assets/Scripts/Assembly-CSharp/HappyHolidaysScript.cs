using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class HappyHolidaysScript : MonoBehaviour
{
	public AudioSource audioSource;

	public AudioClip itemSound;

	public bool db;

	public Transform player;

	public Transform wanderTarget;

	public AILocationSelectorScript wanderer;

	public float coolDown;

	private NavMeshAgent agent;

	public GameControllerScript gc;

	public float CooldownDuration = 40f;

	public bool IsAvailable = true;

	private void Start()
	{
		agent = GetComponent<NavMeshAgent>();
		Wander();
	}

	private void Update()
	{
		if (coolDown > 0f)
		{
			coolDown -= 1f * Time.deltaTime;
		}
	}

	private void FixedUpdate()
	{
		db = false;
		if ((agent.velocity.magnitude <= 1f) & (coolDown <= 0f))
		{
			Wander();
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if (IsAvailable && other.transform.tag == "Player")
		{
			int item_ID = Mathf.RoundToInt(Random.Range(1f, 9f));
			gc.CollectItem(item_ID);
			MonoBehaviour.print("hi it works");
			StartCoroutine(StartCooldown());
			PlayItemSound();
		}
	}

	private void Wander()
	{
		wanderer.GetNewTarget();
		agent.SetDestination(wanderTarget.position);
		coolDown = 1f;
	}

	private void TargetPlayer()
	{
		agent.SetDestination(player.position);
		coolDown = 1f;
	}

	public IEnumerator StartCooldown()
	{
		IsAvailable = false;
		yield return new WaitForSeconds(CooldownDuration);
		IsAvailable = true;
	}

	private void PlayItemSound()
	{
		if (audioSource != null && itemSound != null)
		{
			audioSource.PlayOneShot(itemSound);
		}
	}
}
