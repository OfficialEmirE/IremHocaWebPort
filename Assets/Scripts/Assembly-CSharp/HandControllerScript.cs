using UnityEngine;

public class HandControllerScript : MonoBehaviour
{
	public Animator hands;

	public bool isWalking;

	private void Start()
	{
	}

	private void Update()
	{
		if (Input.GetKey(KeyCode.W) | Input.GetKey(KeyCode.D) | Input.GetKey(KeyCode.A) | Input.GetKey(KeyCode.S))
		{
			hands.SetTrigger("Walking");
			hands.ResetTrigger("Idle");
			isWalking = true;
		}
		else
		{
			hands.ResetTrigger("Walking");
			hands.SetTrigger("Idle");
			isWalking = false;
		}
	}
}
