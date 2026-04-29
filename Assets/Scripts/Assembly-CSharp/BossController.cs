using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BossController : MonoBehaviour
{
	[SerializeField]
	private GameObject[] projectileprefabs;

	[SerializeField]
	private GameObject[] AIPoints;

	public GameObject currentProjectile;

	public int maxHealth = 10;

	private int health;

	[Header("Projectile Spawner")]
	public float maxObjects = 15f;

	public float objects;

	public float spawnCooldown;

	[Header("MISC")]
	public bool BossFight;

	private bool realBossStart;

	public bool AntiDisable_Debug;

	[SerializeField]
	private Slider healthSlider;

	[SerializeField]
	private string onBossEnd;

	[SerializeField]
	private AudioClip aud_baldloonline;

	[SerializeField]
	private AudioClip[] NULL_Lines;

	[SerializeField]
	private AudioClip[] NULL_Music;

	[SerializeField]
	private GameControllerScript gc;

	public NullScript ns;

	[SerializeField]
	private AudioSource audioDevice;

	private void Awake()
	{
		health = maxHealth;
		healthSlider.maxValue = maxHealth - 1;
	}

	private void Update()
	{
		if (health < maxHealth)
		{
			healthSlider.value = health;
		}
		if (!BossFight)
		{
			return;
		}
		if (objects > maxObjects)
		{
			objects = maxObjects;
		}
		if (spawnCooldown > 0f)
		{
			spawnCooldown -= Time.deltaTime;
		}
		if (spawnCooldown <= 0f)
		{
			if (objects < maxObjects)
			{
				GameObject gameObject = AIPoints[Random.Range(0, AIPoints.Length)];
				Object.Instantiate(projectileprefabs[Random.Range(0, projectileprefabs.Length)], gameObject.transform.position, gameObject.transform.rotation).transform.position += Vector3.up * 4f;
				objects += 1f;
			}
			spawnCooldown = Random.Range(5f, 6f);
		}
	}

	public IEnumerator WaitForNULL()
	{
		if (PlayerPrefs.GetInt("NullDefeated") >= 1)
		{
			gc.baldiScrpt.baldiRenderer.enabled = true;
		}
		else
		{
			gc.baldiScrpt.Nsprite.SetActive(value: true);
		}
		ns.agent.speed = 100f;
		ns.target = gc.entrances[gc.lastestExit].bossSpawn.transform;
		while (ns.gameObject.transform.position.x != gc.entrances[gc.lastestExit].bossSpawn.transform.position.x && ns.gameObject.transform.position.z != gc.entrances[gc.lastestExit].bossSpawn.transform.position.z)
		{
			yield return null;
		}
		BossIntro(gc.lastestExit);
	}

	public void BossIntro(int ExitID)
	{
		audioDevice.PlayOneShot(gc.aud_Switch, 0.8f);
		BossFight = true;
		gc.debugMode = true;
		ns.agent.speed = 0f;
		gc.entrances[ExitID].Lower();
		PickupScript[] array = Resources.FindObjectsOfTypeAll<PickupScript>();
		for (int i = 0; i < array.Length; i++)
		{
			array[i].enabled = false;
		}
		gc.player.runSpeed = gc.player.walkSpeed;
		ns.gameObject.SetActive(value: true);
		ns.agent.isStopped = true;
		if (PlayerPrefs.GetInt("NullDefeated") == 0)
		{
			ns.audioMachine.PlayOneShot(NULL_Lines[0]);
		}
		audioDevice.clip = NULL_Music[0];
		audioDevice.loop = true;
		audioDevice.loop = true;
		audioDevice.Play();
		gc.player.hud.enabled = false;
		gc.LoseItem(0);
		gc.LoseItem(1);
		gc.LoseItem(2);
		if (PlayerPrefs.GetInt("NullDefeated") == 0)
		{
			StartCoroutine(WaitForPlayer());
		}
	}

	public IEnumerator WaitForPlayer()
	{
		float time = NULL_Lines[0].length;
		while ((time > 0f) & (health == maxHealth))
		{
			if (Time.timeScale != 0f)
			{
				time -= Time.deltaTime;
			}
			yield return null;
		}
		if (health == maxHealth)
		{
			ns.audioMachine.clip = NULL_Lines[1];
			ns.audioMachine.loop = true;
			ns.audioMachine.Play();
		}
	}

	private void BossBegin()
	{
		ns.enabled = true;
		ns.target = gc.player.gameObject.transform;
		ns.agent.isStopped = false;
		if (!AntiDisable_Debug)
		{
			gc.debugMode = false;
		}
		healthSlider.gameObject.SetActive(value: true);
		ns.agent.speed = 25f;
		BossIncrease(10f, 0f);
		audioDevice.clip = NULL_Music[2];
		audioDevice.loop = true;
		audioDevice.Play();
	}

	public void BossIncrease(float playerSpeed, float musicPitch)
	{
		if (realBossStart)
		{
			gc.player.walkSpeed += playerSpeed;
			gc.player.runSpeed = gc.player.walkSpeed;
			audioDevice.pitch += musicPitch;
		}
	}

	public IEnumerator Hit()
	{
		health--;
		if (health <= 0)
		{
			if (PlayerPrefs.GetInt("NullDefeated") >= 1)
			{
				PlayerPrefs.SetInt("UnlockNull", 1);
			}
			PlayerPrefs.SetInt("NullDefeated", 1);
			SceneManager.LoadScene(onBossEnd);
		}
		if (PlayerPrefs.GetInt("NullDefeated") == 0)
		{
			yield return new WaitForSeconds(ns.Aud_Hit[0].length);
		}
		else
		{
			yield return new WaitForSeconds(ns.Aud_Hit[1].length);
		}
		AfterHit();
		yield return null;
	}

	public void AfterHit()
	{
		if (!BossFight)
		{
			return;
		}
		StartCoroutine(ns.AfterHit());
		if ((health <= maxHealth - 1) & !realBossStart)
		{
			realBossStart = true;
			ns.agent.isStopped = true;
			spawnCooldown = NULL_Music[1].length;
			if (PlayerPrefs.GetInt("NullDefeated") >= 1)
			{
				ns.audioMachine.PlayOneShot(aud_baldloonline);
			}
			else
			{
				ns.audioMachine.PlayOneShot(NULL_Lines[2]);
			}
			audioDevice.clip = NULL_Music[1];
			audioDevice.loop = false;
			audioDevice.Play();
			DeleteProjectiles();
			StartCoroutine(BeforeBegin());
		}
		else if (health < maxHealth - 1)
		{
			BossIncrease(4f, 0.01f);
		}
		else if (health == 1)
		{
			DeleteProjectiles();
			maxObjects = 1f;
			spawnCooldown = 5f;
		}
	}

	private IEnumerator BeforeBegin()
	{
		yield return new WaitForSeconds(NULL_Music[1].length);
		BossBegin();
	}

	private void DeleteProjectiles()
	{
		GameObject[] array = GameObject.FindGameObjectsWithTag("Projectile");
		foreach (GameObject gameObject in array)
		{
			if (currentProjectile != gameObject && !gameObject.GetComponent<ProjectileScript>().Thrown)
			{
				Object.Destroy(gameObject);
				objects -= 1f;
			}
		}
	}
}
