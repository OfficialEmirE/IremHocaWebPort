using UnityEngine;
using UnityEngine.SceneManagement;

public class Script : MonoBehaviour
{
	public AudioSource audioDevice;

	public AudioClip[] audios;

	private SpriteRenderer renderer;

	public GameObject fakebaldi;

	public GameObject banana;

	public GameObject apple;

	public GameObject baldiSprite;

	private bool played;

	private void Awake()
	{
		renderer = GetComponent<SpriteRenderer>();
		if (PlayerPrefs.GetInt("NullDefeated") >= 1)
		{
			fakebaldi.SetActive(value: false);
			renderer.enabled = false;
			baldiSprite.SetActive(value: true);
			banana.SetActive(value: false);
			apple.SetActive(value: true);
		}
	}

	private void Update()
	{
		if (!(!audioDevice.isPlaying & played))
		{
			return;
		}
		if (PlayerPrefs.GetInt("NullDefeated") == 0)
		{
			SceneManager.LoadScene("MainMenu");
		}
		else
		{
			SceneManager.LoadScene("MainMenu");
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if ((other.name == "Player") & !played)
		{
			PlayerPrefs.SetInt("ClassicSecret", 1);
			if (PlayerPrefs.GetInt("NullDefeated") == 0)
			{
				audioDevice.clip = audios[0];
			}
			else
			{
				audioDevice.clip = audios[1];
			}
			audioDevice.Play();
			played = true;
		}
	}
}
