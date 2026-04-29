using UnityEngine;
using UnityEngine.UI;

public class GlitchButtonScript : MonoBehaviour
{
	public Button button;

	public MouseoverScript moS;

	private void Start()
	{
        button.enabled = true;
        moS.enabled = true;
    }
}
