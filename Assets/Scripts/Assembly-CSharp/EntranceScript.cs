using UnityEngine;

public class EntranceScript : MonoBehaviour
{
	public GameControllerScript gc;

	public GameObject bossSpawn;

	public BoxCollider barrier;

	public int EntranceID;

	public Material map;

	public MeshRenderer wall;

	private void Update()
	{
		if ((gc.style == "glitch") & (gc.exitsReached >= gc.entrances.Length - 1) & gc.finaleMode)
		{
			barrier.enabled = true;
		}
	}

	public void Lower()
	{
		base.transform.position = base.transform.position - new Vector3(0f, 10f, 0f);
		if (gc.finaleMode)
		{
			wall.material = map;
		}
	}

	public void Raise()
	{
		base.transform.position = base.transform.position + new Vector3(0f, 10f, 0f);
	}
}
