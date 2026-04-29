using UnityEngine;

public class ProjectileScript : MonoBehaviour
{
	private GameObject cm;

	private bool pickedUp;

	public bool Thrown;

	private GameControllerScript gc;

	private Rigidbody rb;

	public int spawnID;

	private static int nextID;

	private void Start()
	{
		rb = GetComponent<Rigidbody>();
		cm = GameObject.FindWithTag("MainCamera");
		gc = GameObject.FindWithTag("GameController").GetComponent<GameControllerScript>();
		GetComponent<BsodaSparyScript>().enabled = false;
		spawnID = nextID++;
	}

	private void Update()
	{
		if (pickedUp)
		{
			if (Input.GetMouseButtonDown(0) && Time.timeScale != 0f)
			{
				pickedUp = false;
				Thrown = true;
				base.transform.position += Vector3.up * 2f;
				gc.bossController.currentProjectile = null;
				gc.bossController.objects -= 1f;
			}
			if (Thrown)
			{
				GetComponent<BsodaSparyScript>().enabled = true;
			}
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player" && (!pickedUp & !Thrown) && gc.bossController.currentProjectile == null)
		{
			gc.bossController.currentProjectile = base.gameObject;
			pickedUp = true;
		}
	}

	private void OnTriggerStay(Collider other)
	{
		if (other.tag == "Projectile" && !other.GetComponent<ProjectileScript>().pickedUp && !other.GetComponent<ProjectileScript>().Thrown && !pickedUp && !Thrown && gc.bossController.currentProjectile != other.gameObject && other.GetComponent<ProjectileScript>().spawnID > spawnID)
		{
			Object.Destroy(other.gameObject, 0f);
			gc.bossController.objects -= 1f;
		}
	}

	private void LateUpdate()
	{
		if (pickedUp && !Thrown)
		{
			if (!Input.GetButton("Look Behind"))
			{
				base.transform.position = gc.player.transform.position + gc.player.transform.forward * 3f + Vector3.up * -2f;
				base.transform.rotation = cm.transform.rotation;
			}
			else
			{
				base.transform.position = gc.player.transform.position + gc.player.transform.forward * -3f + Vector3.up * -2f;
				base.transform.rotation = cm.transform.rotation;
			}
		}
	}
}
