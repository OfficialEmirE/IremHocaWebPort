using UnityEngine;
using UnityEngine.AI;

public class BaldiScript : MonoBehaviour
{
	public GameControllerScript gc;

	public Sprite baldloonSprite;

	public GameObject Nsprite;

	[SerializeField]
	private Animator baldicator;

	public bool db;

	public float baseTime;

	public float speed;

	public float timeToMove;

	public float baldiAnger;

	public float baldiTempAnger;

	public float baldiWait;

	public float baldiSpeedScale;

	private float moveFrames;

	private float currentPriority;

	public bool antiHearing;

	public float antiHearingTime;

	public float vibrationDistance;

	public float angerRate;

	public float angerRateRate;

	public float angerFrequency;

	public float timeToAnger;

	public bool endless;

	public bool summon;

	public Transform player;

	public Transform wanderTarget;

	public AILocationSelectorScript wanderer;

	private AudioSource baldiAudio;

	public SpriteRenderer baldiRenderer;

	public AudioClip slap;

	public AudioClip[] speech = new AudioClip[3];

	public Animator baldiAnimator;

	public float coolDown;

	private Vector3 previous;

	private bool rumble;

	public NavMeshAgent agent;

	private float nullSpeechTimer = 60f;

	private void Start()
	{
		baldiAudio = GetComponent<AudioSource>();
		agent = GetComponent<NavMeshAgent>();
		baldiRenderer = GetComponentInChildren<SpriteRenderer>();
		timeToMove = baseTime;
		Wander();
		if (PlayerPrefs.GetInt("Rumble") == 1)
		{
			rumble = true;
		}
		if (gc.style == "glitch")
		{
			if (PlayerPrefs.GetInt("NullDefeated") >= 1)
			{
				baldiAnimator.enabled = false;
				baldiRenderer.sprite = baldloonSprite;
			}
			else
			{
				Nsprite.SetActive(value: true);
				baldiRenderer.enabled = false;
			}
		}
	}

	private void Update()
	{
		if (summon)
		{
			TargetPlayer();
		}
		if (timeToMove > 0f)
		{
			timeToMove -= 1f * Time.deltaTime;
		}
		else
		{
			Move();
		}
		if (coolDown > 0f)
		{
			coolDown -= 1f * Time.deltaTime;
		}
		if (baldiTempAnger > 0f)
		{
			baldiTempAnger -= 0.02f * Time.deltaTime;
		}
		else
		{
			baldiTempAnger = 0f;
		}
		if (antiHearingTime > 0f)
		{
			antiHearingTime -= Time.deltaTime;
		}
		else
		{
			antiHearing = false;
		}
		if (endless)
		{
			if (timeToAnger > 0f)
			{
				timeToAnger -= 1f * Time.deltaTime;
			}
			else
			{
				timeToAnger = angerFrequency;
				GetAngry(angerRate);
				angerRate += angerRateRate;
			}
		}
		if (!(gc.style == "glitch"))
		{
			return;
		}
		WindowScript[] array = Object.FindObjectsOfType<WindowScript>();
		foreach (WindowScript windowScript in array)
		{
			if (!windowScript.broken && Vector3.Distance(base.transform.position, windowScript.transform.position) < 5f)
			{
				windowScript.BreakWindow();
				if (Random.Range(0, 100) <= 3 && ((gc.style == "glitch") & (PlayerPrefs.GetInt("NullDefeated") == 0)) && !baldiAudio.isPlaying)
				{
					baldiAudio.PlayOneShot(speech[0]);
				}
			}
		}
		if (PlayerPrefs.GetInt("NullDefeated") != 0)
		{
			return;
		}
		if (nullSpeechTimer > 0f)
		{
			nullSpeechTimer -= Time.deltaTime;
			return;
		}
		nullSpeechTimer = Random.Range(30f, 60f);
		int num = Random.Range(1, speech.Length);
		if (num == 1 && ((Vector3.Distance(player.position, base.transform.position) > 120f) & !db) && !baldiAudio.isPlaying)
		{
			baldiAudio.PlayOneShot(speech[1]);
		}
		if (num == 2 && gc.gameTime > 250f && !baldiAudio.isPlaying)
		{
			baldiAudio.PlayOneShot(speech[2]);
		}
		if (num == 3 && ((gc.item[0] == 0) & (gc.item[1] == 0) & (gc.item[2] == 0) & (gc.player.stamina < 30f)) && !baldiAudio.isPlaying)
		{
			baldiAudio.PlayOneShot(speech[3]);
		}
		if (num >= 4 && !baldiAudio.isPlaying)
		{
			baldiAudio.PlayOneShot(speech[num]);
		}
	}

	private void FixedUpdate()
	{
		if (moveFrames > 0f)
		{
			moveFrames -= 1f;
			agent.speed = speed;
		}
		else
		{
			agent.speed = 0f;
		}
		Vector3 direction = player.position - base.transform.position;
		if (Physics.Raycast(base.transform.position + Vector3.up * 2f, direction, out var hitInfo, float.PositiveInfinity, 769, QueryTriggerInteraction.Ignore))
		{
			if (hitInfo.transform.tag == "Player")
			{
				db = true;
				TargetPlayer();
			}
			else
			{
				db = false;
			}
		}
	}

	private void Wander()
	{
		wanderer.GetNewTarget();
		agent.SetDestination(wanderTarget.position);
		coolDown = 1f;
		currentPriority = 0f;
	}

	public void TargetPlayer()
	{
		agent.SetDestination(player.position);
		coolDown = 1f;
		currentPriority = 0f;
	}

	private void Move()
	{
		if ((base.transform.position == previous) & (coolDown < 0f))
		{
			Wander();
		}
		if (PlayerPrefs.GetString("CurrentStyle") != "glitch")
		{
			moveFrames = 10f;
		}
		else
		{
			moveFrames = 13f;
		}
		timeToMove = baldiWait - baldiTempAnger;
		previous = base.transform.position;
		if (PlayerPrefs.GetString("CurrentStyle") != "glitch")
		{
			baldiAudio.PlayOneShot(slap);
			baldiAnimator.SetTrigger("slap");
		}
		if (rumble)
		{
			float num = Vector3.Distance(base.transform.position, player.position);
			if (num < vibrationDistance)
			{
				_ = num / vibrationDistance;
			}
		}
	}

	public void GetAngry(float value)
	{
		baldiAnger += value;
		if (baldiAnger < 0.5f)
		{
			baldiAnger = 0.5f;
		}
		baldiWait = -3f * baldiAnger / (baldiAnger + 2f / baldiSpeedScale) + 3f;
	}

	public void GetTempAngry(float value)
	{
		baldiTempAnger += value;
	}

	public void Hear(Vector3 soundLocation, float priority)
	{
		if (!antiHearing && priority >= currentPriority)
		{
			agent.SetDestination(soundLocation);
			currentPriority = priority;
			if (baldicator != null && gc.spoopMode)
			{
				if (gc.style == "glitch")
				{
					baldicator.Play("BALDIC_Confused", 0, 0f);
				}
				else
				{
					baldicator.Play("BALDIC_Hear", 0, 0f);
				}
			}
		}
		else
		{
			baldicator.Play("BALDIC_Confused", 0, 0f);
		}
	}

	public void ActivateAntiHearing(float t)
	{
		Wander();
		antiHearing = true;
		antiHearingTime = t;
	}
}
