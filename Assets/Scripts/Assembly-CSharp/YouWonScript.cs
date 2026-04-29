using UnityEngine;
using UnityEngine.SceneManagement;

public class YouWonScript : MonoBehaviour
{
	private float delay;

	public AudioSource device;

	private void Start()
	{
		delay = 10f;
		if (PlayerPrefs.GetInt("NullDefeated") == 0)
		{
			device.Play();
		}
	}

	private void Update()
	{
		delay -= Time.deltaTime;
		if (delay <= 0f)
		{
			SceneManager.LoadScene("MainMenu");
		}
	}
}
