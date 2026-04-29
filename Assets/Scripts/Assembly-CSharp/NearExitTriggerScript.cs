using UnityEngine;

public class NearExitTriggerScript : MonoBehaviour
{
	public GameControllerScript gc;

	public EntranceScript es;

	private void Update()
	{
		if ((gc.exitsReached >= 3) & (gc.style == "glitch"))
		{
			base.gameObject.transform.localScale = new Vector3(80f, 5f, 80f);
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if (PlayerPrefs.GetInt("FreeRun") != 0)
		{
			return;
		}
		if (gc.style != "glitch")
		{
			if (((gc.exitsReached < gc.entrances.Length - 1) & gc.finaleMode) && other.tag == "Player")
			{
				GetComponent<BoxCollider>().enabled = false;
				gc.lastestExit = es.EntranceID;
				gc.ExitReached();
				es.Lower();
				if (gc.baldiScrpt.isActiveAndEnabled)
				{
					gc.baldiScrpt.Hear(base.transform.position, 8f);
				}
			}
		}
		else if (((gc.exitsReached < gc.entrances.Length) & gc.finaleMode) && other.tag == "Player")
		{
			GetComponent<BoxCollider>().enabled = false;
			gc.lastestExit = es.EntranceID;
			gc.ExitReached();
			if (gc.exitsReached < gc.entrances.Length)
			{
				es.Lower();
			}
			if (gc.baldiScrpt.isActiveAndEnabled)
			{
				gc.baldiScrpt.Hear(base.transform.position, 8f);
			}
		}
	}
}
