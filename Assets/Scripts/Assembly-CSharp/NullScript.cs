using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class NullScript : MonoBehaviour
{
	public NavMeshAgent agent;

	public Transform target;

	public AudioClip[] Aud_Hit;

	public AudioSource audioMachine;

	public bool Hit;

	public GameControllerScript gc;

	private void Start()
	{
		audioMachine = GetComponent<AudioSource>();
	}

	private void Update()
	{
		agent.SetDestination(target.position);
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
			}
		}
	}

	private void LateUpdate()
	{
		if (Hit)
		{
			if (PlayerPrefs.GetInt("NullDefeated") >= 1)
			{
				gc.baldiScrpt.baldiRenderer.color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
			}
			else
			{
				gc.baldiScrpt.Nsprite.GetComponent<SpriteRenderer>().color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
			}
		}
		else if (PlayerPrefs.GetInt("NullDefeated") >= 1)
		{
			gc.baldiScrpt.baldiRenderer.color = Color.white;
		}
		else
		{
			gc.baldiScrpt.Nsprite.GetComponent<SpriteRenderer>().color = Color.white;
		}
	}

	private void OnTriggerStay(Collider other)
	{
		if (other.tag == "Projectile" && other.GetComponent<ProjectileScript>() != null && other.GetComponent<ProjectileScript>().Thrown)
		{
			Object.Destroy(other.gameObject);
			gc.debugMode = true;
			Hit = true;
			audioMachine.Stop();
			if (PlayerPrefs.GetInt("NullDefeated") == 0)
			{
				audioMachine.PlayOneShot(Aud_Hit[0]);
			}
			else
			{
				audioMachine.PlayOneShot(Aud_Hit[1]);
			}
			agent.isStopped = true;
			StartCoroutine(gc.bossController.Hit());
		}
		if ((gc.exitsReached == 3) & (other.name == "Office Trigger"))
		{
			if (PlayerPrefs.GetInt("NullDefeated") >= 1)
			{
				gc.baldiScrpt.baldiRenderer.enabled = false;
			}
			else
			{
				gc.baldiScrpt.Nsprite.SetActive(value: false);
			}
		}
	}

	public IEnumerator AfterHit()
	{
		if (PlayerPrefs.GetInt("NullDefeated") == 0)
		{
			yield return new WaitForSeconds(Aud_Hit[0].length);
		}
		else
		{
			yield return new WaitForSeconds(Aud_Hit[1].length);
		}
		if (!gc.bossController.AntiDisable_Debug)
		{
			gc.debugMode = false;
		}
		Hit = false;
		agent.isStopped = false;
		agent.speed += 5f;
	}
}
