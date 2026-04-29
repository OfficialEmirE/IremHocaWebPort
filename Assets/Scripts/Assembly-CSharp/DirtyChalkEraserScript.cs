using System.Collections;
using UnityEngine;

public class DirtyChalkEraserScript : MonoBehaviour
{
	[SerializeField]
	private ParticleSystem ps;

	private void Start()
	{
		StartCoroutine(CountDown());
	}

	private IEnumerator CountDown()
	{
		float t = ps.duration + 1f;
		while (t > 0f)
		{
			t -= Time.deltaTime;
			yield return null;
		}
		Object.Destroy(base.gameObject);
	}
}
