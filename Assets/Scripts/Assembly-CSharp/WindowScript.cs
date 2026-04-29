using UnityEngine;
using UnityEngine.AI;

public class WindowScript : MonoBehaviour
{
	public bool broken;

	public MeshRenderer window_In;

	private MeshRenderer window_Out;

	private MeshCollider meshCollider_In;

	private MeshCollider meshCollider_Out;

	[SerializeField]
	private GameControllerScript gc;

	public Material window_Broken;

	public Material In_windowbroken;

	private void Awake()
	{
		window_Out = GetComponent<MeshRenderer>();
		meshCollider_In = window_In.gameObject.GetComponent<MeshCollider>();
		meshCollider_Out = window_Out.gameObject.GetComponent<MeshCollider>();
	}

	public void BreakWindow()
	{
		if (!broken)
		{
			base.gameObject.GetComponent<AudioSource>().Play();
			window_In.material = In_windowbroken;
			window_Out.material = window_Broken;
			meshCollider_In.enabled = false;
			meshCollider_Out.enabled = false;
			broken = true;
		}
	}

	public void Update()
	{
		if (broken || gc.style == "glitch")
		{
			base.gameObject.GetComponent<NavMeshObstacle>().enabled = false;
		}
	}
}
