using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitTriggerScript : MonoBehaviour
{
	public GameControllerScript gc;

	private void OnTriggerEnter(Collider other)
	{
		if (PlayerPrefs.GetInt("FreeRun") == 0 && ((gc.notebooks >= 7) & (other.tag == "Player")))
		{
			PlayerPrefs.SetInt("ClassicWon", 1);
			if (gc.failedNotebooks >= 1)
			{
				SceneManager.LoadScene("Secret");
			}
			else
			{
				SceneManager.LoadScene("Results");
			}
		}
	}
}
